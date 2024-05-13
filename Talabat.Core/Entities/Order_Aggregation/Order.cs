using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{
    public class Order : BaseEntity
    {
        //Must Be Parameter Less Constructor
        public Order()
        {
            
        }

        public Order(string buyerEmail, OrderAddress shippingAddress ,string intentId, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            PaymentIntendId = intentId;
            Items = items;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;         // In Egypt Time is 8pm , but In America is 10Am
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; }
        //public int DeliveryMethodId { get; set; } //Foriegn Key        mesh m7tagha
        public DeliveryMethod DeliveryMethod { get; set; } //Navigational Property
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal {  get; set; } // Store in Database

        //[NotMapped]
        //public decimal Total => SubTotal + DeliveryMethod.Cost; // SubTotal + DevileryMethod.Cost      ===>    Deriverd Attribute  (Static)

        public decimal GetToal() => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntendId { get; set; } 

    }
}
