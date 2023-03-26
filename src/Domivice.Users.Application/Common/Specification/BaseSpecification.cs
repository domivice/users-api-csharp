using System.Linq.Expressions;
using Domivice.PagingSorting.Domain.Enumerations;
using Domivice.PagingSorting.Domain.Models;
using Domivice.Users.Application.Common.Interfaces;

namespace Domivice.Users.Application.Common.Specification;

public class BaseSpecification<TEntity> : ISpecification<TEntity>
{
    protected BaseSpecification(Expression<Func<TEntity, bool>>? where)
    {
        Where = where;
    }

    public Expression<Func<TEntity, bool>>? Where { get; }
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();
    public List<OrderExpression<TEntity>> OrderExpressions { get; } = new();
    public int? Page { get; private set; }
    public int? PageSize { get; private set; }

    public void AddInclude(Expression<Func<TEntity, object>> include)
    {
        IncludeExpressions.Add(include);
    }

    public void AddOrderExpressions(IEnumerable<OrderExpression<TEntity>> orderExpressions)
    {
        OrderExpressions.AddRange(orderExpressions);
    }

    public void SetPagination(int? page, int? pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public void AddSorting(Dictionary<string, SortOrder> sortFields)
    {
        foreach (var (key, value) in sortFields)
            OrderExpressions.Add(new OrderExpression<TEntity>(BuildLambdaExpression(key), value));
    }

    private static Expression<Func<TEntity, object>> BuildLambdaExpression(string propertyPath)
    {
        var parameterExpression = Expression.Parameter(typeof(TEntity), "p");
        var propertyNodes = propertyPath.Split(".");
        var memberExpression = Expression.Property(parameterExpression, propertyNodes.First());
        memberExpression = propertyNodes.Skip(1).Aggregate(memberExpression, Expression.Property);
        
        return Expression.Lambda<Func<TEntity, object>>(memberExpression, parameterExpression);
    }
}