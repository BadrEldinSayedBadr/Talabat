using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        //where(P=>P.BrandId == brandid && P => P.TypeId == typeid)
        //where(true && true)
        //where(P=>P.BrandId ==brandid && true)
        //where(true && P=> P.TypeId == typeid)
        public ProductSpecification(ProductSpecParams productSpecParams)  //GetAll
            :base(P => 
                    (string.IsNullOrEmpty(productSpecParams.Search) || P.Name.ToLower().Contains(productSpecParams.Search)) &&
                    (!productSpecParams.BrandId.HasValue || P.BrandId == productSpecParams.BrandId) &&  
                    (!productSpecParams.TypeId.HasValue || P.TypeId == productSpecParams.TypeId))
        {
            
            
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);

            if(!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch(productSpecParams.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            //TotalProducts = 100
            //PageSize = 10
            //PageIndex = 3
            ApplyPagination(productSpecParams.PageSize*(productSpecParams.PageIndex - 1),productSpecParams.PageSize);

        }

        public ProductSpecification(int id):base(P => P.Id == id) //GetById
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }

    }
}
