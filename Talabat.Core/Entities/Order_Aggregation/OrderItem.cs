using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            
        }

        public OrderItem(ProductOrderItem productOrderItem, decimal price, int quantity)
        {
            ProductOrderItem = productOrderItem;
            Price = price;
            Quantity = quantity;
        }

        //From BasketItem
        //OI => POI   One To One Total
        public ProductOrderItem ProductOrderItem { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
