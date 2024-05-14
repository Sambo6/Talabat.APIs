using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data.Config.Order_Config
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
			builder.Property(order => order.Status).HasConversion(
				(OStatus) => OStatus.ToString(),
				(OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
				);
			//builder.HasOne(order => order.DeliveryMethod).WithMany();
			builder.Property(order => order.Subtotal).HasColumnType("decimal(12,2)");
			builder.HasOne(order => order.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
			builder.HasMany(order => order.Items).WithOne().OnDelete(DeleteBehavior.Cascade);
		}
	}
}
