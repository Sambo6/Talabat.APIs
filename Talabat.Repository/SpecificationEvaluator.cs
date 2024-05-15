using Microsoft.EntityFrameworkCore;
using Talabat.Core;
using Talabat.Core.Entities;

namespace Talabat.Infrastructure
{
	internal class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity>  GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc != null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query,(currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));

            return query;
        }
    }
}
