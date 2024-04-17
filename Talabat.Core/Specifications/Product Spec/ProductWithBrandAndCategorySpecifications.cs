using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Spec
{
    public class ProductWithBrandAndCategorySpecifications :BaseSpecifications<Product>
    {
        // this ctor use to create an Object  which Used to Get all products
        public ProductWithBrandAndCategorySpecifications():base()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
        public ProductWithBrandAndCategorySpecifications(Expression<Func<Product, bool>> criteria)
            : this()
        {
            Criteria = criteria;
        }
    }
}
