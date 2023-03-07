namespace Domivice.Users.Application.Common.Interfaces;

public interface IGenericRepository<TEntity, in TId>
{
    public List<TEntity> GetList(ISpecification<TEntity> specification);
    public Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    public int GetCount(ISpecification<TEntity> specification);
    public Task<int> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    public TEntity? GetById(TId id);
    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    public void Insert(TEntity entity);
    public void Delete(TId id);
    public void Delete(TEntity entityToDelete);
    public void Update(TEntity entityToUpdate);
}