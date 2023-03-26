using Domivice.Domain.ValueObjects;
using Domivice.Users.Domain.ValueObjects;
using FluentValidation;

namespace Domivice.Users.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(command => command.UserId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("A user id is required.")
            .NotNull().WithMessage("A user id is required.")
            .Must(userId => Guid.TryParse(userId, out _))
            .WithMessage("We could not find a user with the provided id. Please verify the user id and try again.");

        When(command => !string.IsNullOrEmpty(command.FirstName), () =>
        {
            RuleFor(command => command.FirstName)
                .Custom((firstName, context) =>
                {
                    var result = FirstName.Create(firstName);
                    if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
                });
        });

        When(command => !string.IsNullOrEmpty(command.LastName), () =>
        {
            RuleFor(command => command.LastName)
                .Custom((lastName, context) =>
                {
                    var result = LastName.Create(lastName);
                    if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
                });
        });

        When(command => !string.IsNullOrEmpty(command.Email), () =>
        {
            RuleFor(command => command.Email)
                .Custom((email, context) =>
                {
                    var result = Email.Create(email);
                    if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
                });
        });

        When(command => !string.IsNullOrEmpty(command.PhoneNumber), () =>
        {
            RuleFor(command => command.PhoneCountryCode).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("A country code is required to update the phone number.")
                .NotNull()
                .WithMessage("A country code is required to update the phone number.")
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
        });

        When(command => !string.IsNullOrEmpty(command.DisplayLanguage), () =>
        {
            RuleFor(command => command.DisplayLanguage)
                .Custom((displayLanguage, context) =>
                {
                    var result = CultureCode.Create(displayLanguage);
                    if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
                });
        });

        When(command => !string.IsNullOrEmpty(command.UserBio), () =>
        {
            RuleFor(command => command.UserBio)
                .Custom((userBio, context) =>
                {
                    var result = Text.Create(userBio);
                    if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
                });
        });

        When(command => !string.IsNullOrEmpty(command.Website), () =>
        {
            RuleFor(command => command.Website).Must(webSite => Uri.TryCreate(webSite, UriKind.Absolute, out _))
                .WithMessage("The Url is not in the right format.");
        });

        When(command => !string.IsNullOrEmpty(command.EntryInstructions), () =>
        {
            RuleFor(command => command.EntryInstructions)
                .Custom((entryInstructions, context) =>
                {
                    var result = Text.Create(entryInstructions);
                    if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
                });
        });

        When(command => command.HomeAddress != null,
            () => { RuleFor(command => command.HomeAddress).SetValidator(new AddressValidator()!); });

        When(command => command.Languages.Any(), () =>
        {
            RuleForEach(command => command.Languages).Custom((languageCode, context) =>
            {
                var result = LanguageCode.Create(languageCode);
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });
        });

        When(command => command.Languages.All(languageCode => LanguageCode.Create(languageCode).IsSuccess), () =>
        {
            RuleFor(command => command.Languages).Custom((languages, context) =>
            {
                var result = UserLanguageList.Create(languages.ConvertAll(l => LanguageCode.Create(l).Value));
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });
        });

        When(command => command.SocialMediaUrls.Any(), () =>
        {
            RuleForEach(command => command.SocialMediaUrls).Custom((socialMediaUrl, context) =>
            {
                var result = SocialMediaUrl.Create(socialMediaUrl.Site, socialMediaUrl.Uri);
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });
        });

        When(command => command.SocialMediaUrls.All(url => SocialMediaUrl.Create(url.Site, url.Uri).IsSuccess), () =>
        {
            RuleFor(command => command.SocialMediaUrls).Custom((socialMediaUrls, context) =>
            {
                var result =
                    UserSocialMediaUrlList.Create(socialMediaUrls.ConvertAll(url =>
                        SocialMediaUrl.Create(url.Site, url.Uri).Value));
                if (result.IsFailed) result.Errors.ForEach(e => context.AddFailure(e.Message));
            });
        });
    }
}