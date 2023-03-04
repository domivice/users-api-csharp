using System.Reflection;
using Domivice.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domivice.Users.Infrastructure.Persistence;

public class UsersDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}