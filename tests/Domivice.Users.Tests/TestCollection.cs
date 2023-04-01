using Xunit;

namespace Domivice.Users.Tests;

[CollectionDefinition("TestCollection")]
public class TestCollection : ICollectionFixture<TestServer>
{
}