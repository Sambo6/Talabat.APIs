using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeeding
    {
        public async static Task SeedAsync(StoreContext _dbContext)
        {

            #region ProductBrands Data Seeding
            if (!_dbContext.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var Brand in brands)
                    {
                        Brand.Id = 0;
                        await _dbContext.AddAsync(Brand);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            } 
            #endregion

            #region ProductCategory Data Seeding
            if (!_dbContext.ProductCategories.Any())
            {
                var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count > 0)
                {

                    foreach (var Category in categories)
                    {
                        Category.Id = 0;
                        await _dbContext.AddAsync(Category);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            } 
            #endregion

            #region Product Data Seeding
            if (!_dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {
                    foreach (var Product in products)
                    {
                        Product.Id = 0;
                        await _dbContext.AddAsync(Product);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            } 
            #endregion

            #region DeliveryMethod Data Seeding
            if (!_dbContext.DeliveryMethods.Any())
            {
                var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        deliveryMethod.Id = 0;
                        await _dbContext.AddAsync(deliveryMethod);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            }

            #endregion



        }
    }
}
