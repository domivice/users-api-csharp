namespace Domivice.Users.Application.Common.Interfaces;

public interface IUsersDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}