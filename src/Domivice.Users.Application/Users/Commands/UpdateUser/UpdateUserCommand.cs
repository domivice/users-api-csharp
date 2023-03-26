using System.Text.Json;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Application.Users.ReadModels;
using Domivice.Users.Domain.Errors;
using Domivice.Users.Domain.ValueObjects;
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

public record UpdateUserCommand : IRequest<Result<UserRm>>
{
    public string UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneCountryCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? DisplayLanguage { get; set; }
    public string? UserBio { get; set; }
    public string? Website { get; set; }
    public string? EntryInstructions { get; set; }
    public UpdateAddressCommand? HomeAddress { get; set; }
    public List<string> Languages { get; set; } = new();
    public List<UpdateSocialMediaUrlCommand> SocialMediaUrls { get; set; } = new();
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserRm>>
{
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

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
            return Result.Fail<UserRm>(new NotFoundError
            {
                Title = "User not found",
                Message = "We could not find a user with the provided id. Please verify the user id and try again."
            });

        if (!string.IsNullOrEmpty(request.FirstName)) user.UpdateFirstName(FirstName.Create(request.FirstName).Value);

        if (!string.IsNullOrEmpty(request.LastName)) user.UpdateLastName(LastName.Create(request.LastName).Value);

        if (!string.IsNullOrEmpty(request.Email)) user.UpdateEmail(Email.Create(request.Email).Value);

        if (!string.IsNullOrEmpty(request.PhoneCountryCode) && !string.IsNullOrEmpty(request.PhoneNumber))
            user.UpdatePhoneNumber(PhoneNumber.Create(request.PhoneNumber, request.PhoneCountryCode).Value);

        if (!string.IsNullOrEmpty(request.DisplayLanguage))
            user.UpdateDisplayLanguage(CultureCode.Create(request.DisplayLanguage).Value);

        if (!string.IsNullOrEmpty(request.UserBio)) user.UpdateUserBiography(Text.Create(request.UserBio).Value);

        if (request.HomeAddress != null)
            user.UpdateHomeAddress(new Address(
                request.HomeAddress.Street,
                request.HomeAddress.City,
                request.HomeAddress.State,
                request.HomeAddress.Country,
                request.HomeAddress.PostalCode
            ));

        if (!string.IsNullOrEmpty(request.Website)) user.UpdateUserWebsite(new Uri(request.Website));

        if (!string.IsNullOrEmpty(request.EntryInstructions))
            user.UpdateEntryInstructions(Text.Create(request.EntryInstructions).Value);

        user.SetUserLanguages(UserLanguageList.Create(request.Languages.ConvertAll(l => LanguageCode.Create(l).Value))
            .Value);

        var userSocialMediaUrls = request.SocialMediaUrls
            .ConvertAll(url => SocialMediaUrl.Create(url.Site, url.Uri).Value);

        user.SetUserSocialMediaUrls(UserSocialMediaUrlList.Create(userSocialMediaUrls).Value);

        _unitOfWork.UserRepository.Update(user);

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (saveResult == 0)
            return Result.Fail<UserRm>(new InternalServerError
            {
                Title = "Error saving user",
                Message = "An error occured while updating the user. Please try again"
            });

        return (UserRm) user;
    }
}