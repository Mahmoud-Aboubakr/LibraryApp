using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public interface IEntitySpec <T> where T : BaseEntity
    {
        Expression <Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        IQueryable<T> GetQuery (IQueryable<T> inputQuery, IEntitySpec<T> spec);
    }
}
