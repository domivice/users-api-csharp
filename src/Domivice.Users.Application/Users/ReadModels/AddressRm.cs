using Domivice.Domain.ValueObjects;

namespace Domivice.Users.Application.Users.ReadModels;

public class AddressRm
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;

    public static explicit operator AddressRm(Address address)
    {
        return new AddressRm
        {
            City = address.City,
            Country = address.Country,
            PostalCode = address.PostalCode,
            State = address.State,
            Street = address.Street
        };
    }
}