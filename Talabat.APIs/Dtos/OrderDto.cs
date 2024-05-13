using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.APIs.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }

    }
}
