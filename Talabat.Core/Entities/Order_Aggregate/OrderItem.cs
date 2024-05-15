namespace Talabat.Core.Entities.Order_Aggregate
{
	public class OrderItem : BaseEntity
	{

		private OrderItem(){ }// this parameterless constructor for EF CORE [Migration]

		public OrderItem(ProductItemOrder product, decimal price, int quantity)
		{
			Product = product;
			Price = price;
			Quantity = quantity;
		}

		//Composite Attribute
		public ProductItemOrder Product { get; set; } = null!;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
