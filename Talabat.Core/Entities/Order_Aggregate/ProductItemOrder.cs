namespace Talabat.Core.Entities.Order_Aggregate
{
	public class ProductItemOrder
	{	
		private ProductItemOrder(){ } // this parameterless constructor for EF CORE [Migration]
		public ProductItemOrder(int productId, string productName, string pictureUrl)
		{
			ProductId = productId;
			ProductName = productName;
			PictureUrl = pictureUrl;
		}

		// This class is Composite attribute
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
	}
}
