using Domivice.Domain.ValueObjects;
using FluentValidation;

namespace Domivice.Users.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.UserId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("A user id is required.")
            .NotNull().WithMessage("A user id is required.")
            .Must(userId => Guid.TryParse(userId, out _))
            .WithMessage("We could not find a user with the provided id. Please verify the user id and try again.");

        RuleFor(command => command.FirstName)
            .Custom((firstName, context) =>
            {
                var result = FirstName.Create(firstName);
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });

        RuleFor(command => command.LastName)
            .Custom((lastName, context) =>
            {
                var result = LastName.Create(lastName);
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });

        RuleFor(command => command.Email)
            .Custom((email, context) =>
            {
                var result = Email.Create(email);
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });

        RuleFor(command => command.PhoneCountryCode).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("A country code is required to create user.")
            .NotNull()
            .WithMessage("A country code is required to create user.")
            .Must(phoneCountryCode => CountryCode.Create(phoneCountryCode).IsSuccess)
            .WithMessage(
                $"Please use one of the following supported country codes: {string.Join(", ", CountryCode.SupportedCountries)}.")
            .DependentRules(() =>
            {
                RuleFor(command => command).Custom((command, context) =>
                {
                    var result = PhoneNumber.Create(command.PhoneNumber, command.PhoneCountryCode);
                    if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure("PhoneNumber", e.Message));
                });
            });

        RuleFor(command => command.DisplayLanguage)
            .Custom((displayLanguage, context) =>
            {
                var result = CultureCode.Create(displayLanguage);
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });
    }
}