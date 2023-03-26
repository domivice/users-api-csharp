using Domivice.Users.Domain.Entities;

namespace Domivice.Users.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IGenericRepository<User, Guid> UserRepository { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}