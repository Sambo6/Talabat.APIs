using Talabat.Core.Entities;

namespace Talabat.APIs.Dto
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        public int BrandId { get; set; } 
        public  string Brand { get; set; } // Navigational Property [ONE]

        public int CategoryId { get; set; } 
        public  string Category { get; set; } // Navigational Property [ONE]

    }
}
