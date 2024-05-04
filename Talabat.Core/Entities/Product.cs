namespace Talabat.Core.Entities
{
	public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        public int BrandId { get; set; } // FK : ProductBrand
        public virtual ProductBrand Brand { get; set; } = null!; // Navigational Property [ONE]

        public int CategoryId { get; set; } // FK : ProductCategory
        public virtual ProductCategory Category { get; set; } = null!; // Navigational Property [ONE]

    }
}
