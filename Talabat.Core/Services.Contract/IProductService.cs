﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.Product_Spec;

namespace Talabat.Core.Services.Contract
{
	public interface IProductService
	{
		Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams);

		Task<Product?> GetProductAsync(int id);

		Task<int> GetCountAsync(ProductSpecParams specParams);

		Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();

		Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
	}
}
