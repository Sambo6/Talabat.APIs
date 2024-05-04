namespace Talabat.Core.Entities.Order_Aggregate
{
	public class ProductItemOrder
	{
		// This class is Composite attribute
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
	}
}
