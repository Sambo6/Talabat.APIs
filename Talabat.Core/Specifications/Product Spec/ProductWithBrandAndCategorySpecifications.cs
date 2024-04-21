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
        public ProductWithBrandAndCategorySpecifications(string sort) : base()
        {
            AddIncludes();

            if (!string.IsNullOrEmpty(sort))
            {
                switch(sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price); break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price); break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
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
