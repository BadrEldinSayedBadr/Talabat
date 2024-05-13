using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Interfaces;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly PaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork ,IBasketRepository basketRepository, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _paymentService = (PaymentService?)paymentService;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, OrderAddress address, string basketId, int deliveryMethod)
        {
            //1. Get Basket From Basket Repository
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //2. Get Selected Items at Basket From Product Repository
            var orderItems = new List<OrderItem>();
            if(basket?.BasketItems?.Count > 0)
            {
                foreach(var item in basket.BasketItems)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }


            //3. Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);


            //4. Get Delivery Method From Delivery Method Repository
            var DelivertyMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethod);


            //5. Create Order
            var spec = new OrderWitPaymentIntentSpecificaiton(basket.PaymentIntentId);

            var existOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (existOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            var order = new Order(buyerEmail, address , basket.PaymentIntentId ,DelivertyMethod, orderItems, subTotal);

                
            //6. Add Order Locally
            await _unitOfWork.Repository<Order>().Add(order);


            //7. Save Order To Database (Orders)
            var result = await _unitOfWork.Complete();

            if (result <= 0)
                return null;

            return order;

        }


        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return (IReadOnlyList<Order>) orders;
        }


        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpecification(orderId, buyerEmail);

            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return (IReadOnlyList<DeliveryMethod>) deliveryMethods;
        }


    }
}
