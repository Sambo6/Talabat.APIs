namespace Talabat.Core.Entities
{
    public class BasketItem
    {
        //Id OF PRODUCT
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int Quantity { get; set; }


    }
}