using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Spec
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        // this ctor use to create an Object  which Used to Get all products
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams) : base(P =>

                                           (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                                           (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value))
        {
            AddIncludes();

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price); break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price); break;
                    default:
                        AddOrderBy(p => p.Name); break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
            //total products = 18 , pageSize = 5
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        // this ctor use to create an Specific Object  which Used to Get products ById
        public ProductWithBrandAndCategorySpecifications(int id)
                    : base(P => P.Id == id)
        {
            AddIncludes();
        }
        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);

        }
    }
}
