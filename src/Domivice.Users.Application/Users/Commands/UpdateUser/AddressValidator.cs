using FluentValidation;

namespace Domivice.Users.Application.Users.Commands.UpdateUser;

public class AddressValidator : AbstractValidator<UpdateAddressCommand>
{
    public AddressValidator()
    {
        RuleFor(address => address.PostalCode)
            .NotEmpty()
            .WithMessage("A postal code is required to update the address.")
            .NotNull()
            .WithMessage("A postal code is required to update the address.");
        RuleFor(address => address.Country)
            .NotEmpty()
            .WithMessage("A country is required to update the address.")
            .NotNull()
            .WithMessage("A country is required to update the address.");
        RuleFor(address => address.State)
            .NotEmpty()
            .WithMessage("A state is required to update the address.")
            .NotNull()
            .WithMessage("A state is required to update the address.");
        RuleFor(address => address.City)
            .NotEmpty()
            .WithMessage("A city is required to update the address.")
            .NotNull()
            .WithMessage("A city is required to update the address.");
        RuleFor(address => address.Street)
            .NotEmpty()
            .WithMessage("A street is required to update the address.")
            .NotNull()
            .WithMessage("A street is required to update the address.");
    }
}