using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Application.OrderServices
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;
		public OrderService(IBasketRepository basketRepo,
							IUnitOfWork unitOfWork) 
		{
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
		}

		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From BasketsRepo
			var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From ProductsRepo

			var orderItems = new List<OrderItem>();
			if (basket?.Items?.Count > 0 )
			{
				var productRepository = _unitOfWork.Repository<Product>();
				foreach ( var item in basket.Items )
				{
					var product = await productRepository.GetAsync(item.Id);
					var productItemOrdered = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
					var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal

			var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

			// 4. Get Delivery Method From DeliveryMethodsRepo

			//var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);


			// 5. Create Order

			var order = new Order(buyerEmail,shippingAddress,deliveryMethodId,orderItems,subtotal);
			 _unitOfWork.Repository<Order>().Add(order);

			// 6. Save To Database [TODO]

			var result =  await _unitOfWork.CompleteAsync();
			if (result <= 0) return null;
			else return order;

		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			var ordersRepo = _unitOfWork.Repository<Order>();
			var spec = new OrderSpecifications(buyerEmail);

			var orders = await ordersRepo.GetAllWithSpecAsync(spec);

			return orders;
		}
	}
}
