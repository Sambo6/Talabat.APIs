namespace Talabat.Core.Entities.Order_Aggregate
{
	public class OrderItem : BaseEntity
	{
		//Composite Attribute
		public ProductItemOrder Product { get; set; } = null!;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
