using System.Collections;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.Infrastructure
{
	internal class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;
		private Hashtable _repositories;

		public UnitOfWork(StoreContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}

		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var key = typeof(TEntity).Name;
			if (!_repositories.ContainsKey(key))
			{
				var repository = new GenericRepository<TEntity>(_dbContext);
				_repositories.Add(key, repository);
			}
			return _repositories[key] as IGenericRepository<TEntity>;
		}
		public async Task<int> CompleteAsync()
			=> await _dbContext.SaveChangesAsync();
		

		public async ValueTask DisposeAsync()
			=> await _dbContext.DisposeAsync();
		
	}
}
