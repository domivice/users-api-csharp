using Domivice.Users.Application.Common.Interfaces;

namespace Domivice.Users.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now.ToUniversalTime();
}