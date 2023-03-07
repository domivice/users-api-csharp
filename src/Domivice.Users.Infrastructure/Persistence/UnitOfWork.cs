using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Domain.Entities;

namespace Domivice.Users.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly UsersDbContext _context;
    private bool _disposed;
    public IGenericRepository<User, Guid> UserRepository { get; }

    public UnitOfWork(UsersDbContext context,
        IGenericRepository<User, Guid> userRepository)
    {
        _context = context;
        UserRepository = userRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}