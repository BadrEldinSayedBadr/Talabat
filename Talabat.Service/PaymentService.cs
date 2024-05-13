using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Interfaces;
using Talabat.Core.Services;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null) return null;

            var shippingPrice = 0m; //Decimal

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                shippingPrice = deliveryMethod.Cost;

                basket.ShippingCost = deliveryMethod.Cost;

            }

            if (basket?.BasketItems?.Count > 0)
            {
                foreach (var item in basket.BasketItems)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))  // Create PaymentIntent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,  //Cent so multiply to 100 to convert into dollar
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else  //Update PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100  //Cent so multiply to 100 to convert into dollar
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }


            await _basketRepository.UpdateBasketAsync(basket);

            return basket;

        }
    }
}
