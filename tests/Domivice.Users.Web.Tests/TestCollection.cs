using Xunit;

namespace Domivice.Users.Web.Tests;

[CollectionDefinition("TestCollection")]
public class TestCollection : ICollectionFixture<TestFactory>
{
}