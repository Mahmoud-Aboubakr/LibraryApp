using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class EntitySpec<T> : IEntitySpec<T> where T : BaseEntity
    {
        public EntitySpec()
        {

        }
        public EntitySpec(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>> (); 

        protected void AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }

        public IQueryable<T> GetQuery(IQueryable<T> inputQuery, IEntitySpec<T> spec)
        {
            var query = inputQuery;
            if(spec.Criteria != null)
                query = query.Where(spec.Criteria);
            if (spec.Includes.Count > 0)
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
