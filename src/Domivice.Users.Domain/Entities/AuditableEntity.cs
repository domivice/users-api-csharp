namespace Domivice.Users.Domain.Entities;

public class AuditableEntity<TId> : BaseEntity<TId>
{
    public TId CreatedBy { get; set; }
    public TId LastModifiedBy { get; set; }
}