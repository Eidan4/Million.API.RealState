using System.Linq;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;

namespace Million.API.RealEstate.Persistence.Repositories.CrossRepositories
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // Aplicar filtro (Criteria)
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Aplicar ordenamiento
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            // Aplicar agrupamiento
            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            // Aplicar paginación
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }
    }
}