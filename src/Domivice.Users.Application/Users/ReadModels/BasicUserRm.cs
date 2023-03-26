using Domivice.Users.Domain.Entities;

namespace Domivice.Users.Application.Users.ReadModels;

public class BasicUserRm
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    
    public static explicit operator BasicUserRm(User user)
    {
        return new BasicUserRm()
        {
            Id = user.Id.ToString(),
            Email = user.Email.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber.Number
        };
    }
}