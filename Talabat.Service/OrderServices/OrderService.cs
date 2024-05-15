using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.OrderServices
{
	public class OrderService : IOrderService
	{
		

		
		private readonly IBasketRepository _basketRepo;
		private readonly IGenericRepository<Product>_productRepo;
		private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
		private readonly IGenericRepository<Order> _orderRepo;
		public OrderService(IBasketRepository basketRepo,
							IGenericRepository<Product> productRepo,
							IGenericRepository<DeliveryMethod> deliveryMethodRepo,
							IGenericRepository<Order> orderRepo) 
		{
			_basketRepo = basketRepo;
			_productRepo = productRepo;
			_deliveryMethodRepo = deliveryMethodRepo;
			_orderRepo = orderRepo;
		}

		public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From BasketsRepo
			var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From ProductsRepo

			var orderItems = new List<OrderItem>();
			if (basket?.Items?.Count > 0 )
			{
				foreach ( var item in basket.Items )
				{
					var product = await _productRepo.GetAsync(item.Id);
					var productItemOrdered = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
					var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal

			var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

			// 4. Get Delivery Method From DeliveryMethodsRepo

			//var deliveryMethod = await _deliveryMethodRepo.GetAsync(deliveryMethodId);


			// 5. Create Order

			var order = new Order(

				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				deliveryMethodId: deliveryMethodId,
				items: orderItems,
				subtotal: subtotal

				);
			_orderRepo.Add(order);

			// 6. Save To Database [TODO]

		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
