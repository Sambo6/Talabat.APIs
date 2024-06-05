﻿using System.Drawing.Drawing2D;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbContext)
        {

            #region ProductBrands Data Seeding

            if (_dbContext.ProductBrands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            #endregion

            #region ProductCategory Data Seeding

            if (_dbContext.ProductCategories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count > 0)
                {

                    foreach (var category in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(category);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            #endregion

            #region Product Data Seeding
            if (_dbContext.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {

                    foreach (var product in products)
                    {
                        _dbContext.Set<Product>().Add(product);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            } 
            #endregion

            #region DeliveryMethod Data Seeding
            if (!_dbContext.DeliveryMethods.Any())
            {
                var deliveryMethodsData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/delivery.json");
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
