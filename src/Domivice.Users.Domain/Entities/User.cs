using Domivice.Domain.ValueObjects;

namespace Domivice.Users.Domain.Entities;

public class User : BaseEntity<Guid>
{
    public FirstName FirstName { get; }
    public LastName LastName { get; }
}