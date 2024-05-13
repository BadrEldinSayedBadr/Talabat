using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFiltarationForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltarationForCountSpecification(ProductSpecParams productSpecParams)  //GetAll
           : base(P =>
                   (!productSpecParams.BrandId.HasValue || P.BrandId == productSpecParams.BrandId) &&
                   (!productSpecParams.TypeId.HasValue || P.TypeId == productSpecParams.TypeId))
        {


        }
    } 
}
