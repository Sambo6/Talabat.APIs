using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Order : BaseEntity
	{
		private Order() {} // Must Create Parameterliess ctor for EF Core [Migration]
		public Order(string buyerEmail, Address shippingAddress, int? deliveryMethodId, ICollection<OrderItem> items, decimal subTotal)
		{
			BuyerEmail = buyerEmail;
			ShippingAddress = shippingAddress;
			DeliveryMethodId= deliveryMethodId;
			Items = items;
			Subtotal = subTotal;
		}

		public string BuyerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public Address ShippingAddress { get; set; } = null!;

        public int? DeliveryMethodId { get; set; } //Forign Key
        public DeliveryMethod? DeliveryMethod { get; set; } = null!; //Navigational property[One]
		public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
		public decimal Subtotal { get; set; }

        public string PaymentIntentId { get; set; } = string.Empty;

		//1-
		//[NotMapped]
		//public decimal Total => Subtotal + DeliveryMethod.Cost;

		//2-
		public decimal GetTotal () => Subtotal + DeliveryMethod.Cost;

    }
}
