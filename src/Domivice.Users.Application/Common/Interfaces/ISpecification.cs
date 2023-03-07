using System.Linq.Expressions;
using Domivice.PagingSorting.Domain.Models;

namespace Domivice.Users.Application.Common.Interfaces;

public interface ISpecification<TEntity>
{
    public Expression<Func<TEntity, bool>>? Where { get; }
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
    public List<OrderExpression<TEntity>> OrderExpressions { get; }
    public int? Page { get; }
    public int? PageSize { get; }
    public void AddInclude(Expression<Func<TEntity, object>> include);
    public void AddOrderExpressions(IEnumerable<OrderExpression<TEntity>> orderExpressions);
    public void SetPagination(int? page, int? pageSize);
}