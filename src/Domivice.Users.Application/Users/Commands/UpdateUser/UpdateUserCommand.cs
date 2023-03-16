using System.Text.Json;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Application.Users.ReadModels;
using Domivice.Users.Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domivice.Users.Application.Users.Commands.UpdateUser;

public record UpdateAddressCommand(
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode
);

public record UpdateSocialMediaUrlCommand(
    string Site,
    string Uri
);

public record UpdateUserCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneCountryCode,
    string? PhoneNumber,
    string? DisplayLanguage,
    string? UserBio,
    string? Website,
    string? EntryInstructions,
    UpdateAddressCommand? HomeAddress,
    List<string>? UserLanguages,
    List<UpdateSocialMediaUrlCommand>? UserSocialMediaUrls
) : IRequest<Result<UserRm>>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserRm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UserRm>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling command {Action} with request: {Request}",
            nameof(UpdateUserCommandHandler),
            JsonSerializer.Serialize(request)
        );

        var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user is null)
        {
            return Result.Fail<UserRm>(new NotFoundError()
            {
                Title = "User not found",
                Message = "We could not find a user with the provided id. Please verify the user id and try again."
            });
        }

        if (!string.IsNullOrEmpty(request.FirstName))
        {
            user.UpdateFirstName(FirstName.Create(request.FirstName).Value);
        }

        if (!string.IsNullOrEmpty(request.LastName))
        {
            user.UpdateLastName(LastName.Create(request.LastName).Value);
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            user.UpdateEmail(Email.Create(request.Email).Value);
        }

        if (!string.IsNullOrEmpty(request.PhoneCountryCode) && !string.IsNullOrEmpty(request.PhoneNumber))
        {
            user.UpdatePhoneNumber(PhoneNumber.Create(request.PhoneNumber, request.PhoneCountryCode).Value);
        }

        if (!string.IsNullOrEmpty(request.DisplayLanguage))
        {
            user.UpdateDisplayLanguage(CultureCode.Create(request.DisplayLanguage).Value);
        }

        if (!string.IsNullOrEmpty(request.UserBio))
        {
            user.UpdateUserBiography(Text.Create(request.UserBio).Value);
        }

        if (request.HomeAddress != null)
        {
            user.UpdateHomeAddress(new Address(
                request.HomeAddress.Street,
                request.HomeAddress.City,
                request.HomeAddress.State,
                request.HomeAddress.Country,
                request.HomeAddress.PostalCode
            ));
        }

        if (!string.IsNullOrEmpty(request.Website))
        {
            user.UpdateUserWebsite(new Uri(request.Website));
        }

        if (!string.IsNullOrEmpty(request.EntryInstructions))
        {
            user.UpdateEntryInstructions(Text.Create(request.EntryInstructions).Value);
        }

        if (request.UserLanguages != null)
        {
            user.SetUserLanguages(request.UserLanguages.ConvertAll(l => LanguageCode.Create(l).Value));
        }

        if (request.UserSocialMediaUrls != null)
        {
            user.SetUserSocialMediaUrls(
                request.UserSocialMediaUrls.ConvertAll(url => SocialMediaUrl.Create(url.Site, url.Uri).Value));
        }

        _unitOfWork.UserRepository.Update(user);

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (saveResult == 0)
        {
            return Result.Fail<UserRm>(new InternalServerError()
            {
                Title = "Error saving user",
                Message = "An error occured while updating the user. Please try again"
            });
        }

        return (UserRm) user;
    }
}