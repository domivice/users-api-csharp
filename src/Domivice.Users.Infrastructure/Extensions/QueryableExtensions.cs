using Domivice.PagingSorting.Domain.Enumerations;
using Domivice.Users.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domivice.Users.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplySpecifications<TEntity>(this IQueryable<TEntity> queryable,
        ISpecification<TEntity> specification, bool isCount = false) where TEntity : class
    {
        if (specification.Where is not null) queryable = queryable.Where(specification.Where);

        if (isCount) return queryable;

        queryable = specification.IncludeExpressions.Aggregate(queryable,
            (current, includeExpression) => current.Include(includeExpression));

        foreach (var orderExpression in specification.OrderExpressions)
            if (orderExpression.Order == SortOrder.Asc)
                queryable = queryable.Expression.Type == typeof(IOrderedQueryable<TEntity>)
                    ? (queryable as IOrderedQueryable<TEntity>)!.ThenBy(orderExpression.Expression)
                    : queryable.OrderBy(orderExpression.Expression);
            else
                queryable = queryable.Expression.Type == typeof(IOrderedQueryable<TEntity>)
                    ? (queryable as IOrderedQueryable<TEntity>)!.ThenByDescending(orderExpression.Expression)
                    : queryable.OrderByDescending(orderExpression.Expression);

        if (specification is { Page: { }, PageSize: { } })
            queryable = queryable.Skip((specification.Page.Value - 1) * specification.PageSize.Value)
                .Take(specification.PageSize.Value);

        return queryable;
    }
}