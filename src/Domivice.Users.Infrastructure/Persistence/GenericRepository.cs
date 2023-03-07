using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Domivice.Users.Infrastructure.Persistence;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
{
    private readonly UsersDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(UsersDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public List<TEntity> GetList(ISpecification<TEntity> specification)
    {
        IQueryable<TEntity> query = _dbSet;
        return query.ApplySpecifications(specification).ToList();
    }

    public async Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbSet;
        return await query.ApplySpecifications(specification)
            .ToListAsync(cancellationToken);
    }

    public int GetCount(ISpecification<TEntity> specification)
    {
        IQueryable<TEntity> query = _dbSet;
        return query.ApplySpecifications(specification, true).Count();
    }

    public async Task<int> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = _dbSet;
        return await query.ApplySpecifications(specification, true)
            .CountAsync(cancellationToken);
    }

    public TEntity? GetById(TId id)
    {
        return _dbSet.Find(id);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(new object?[] { id }, cancellationToken);
    }

    public void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(TId id)
    {
        var entityToDelete = _dbSet.Find(id);
        if (entityToDelete != null) Delete(entityToDelete);
    }

    public void Delete(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached) _dbSet.Attach(entityToDelete);

        _dbSet.Remove(entityToDelete);
    }

    public void Update(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}