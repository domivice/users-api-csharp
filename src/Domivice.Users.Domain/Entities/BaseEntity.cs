namespace Domivice.Users.Domain.Entities;

public class BaseEntity<TId>
{
    public TId Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}