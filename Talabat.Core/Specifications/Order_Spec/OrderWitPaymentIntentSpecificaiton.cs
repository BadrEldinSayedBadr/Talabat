using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderWitPaymentIntentSpecificaiton : BaseSpecification<Order>
    {
        public OrderWitPaymentIntentSpecificaiton(string intentId) : base(O => O.PaymentIntendId == intentId)
        {

        }
    }
}
