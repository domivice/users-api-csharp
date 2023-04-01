namespace Domivice.Users.Infrastructure.Consumers.Exceptions;

public class CreateUserCommandException : Exception
{
    public CreateUserCommandException(string title, string message) : base(message)
    {
        Title = title;
    }

    public string Title { get; }
}