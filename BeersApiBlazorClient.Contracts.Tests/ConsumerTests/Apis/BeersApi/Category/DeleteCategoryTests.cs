using System.Net;
using BeersApiBlazorClient.Services.Category;
using FluentAssertions;
using Moq;

namespace BeersApiBlazorClient.Contracts.Tests.ConsumerTests.Apis.BeersApi.Category;

[TestFixture]
public class DeleteCategoryTests : ConsumerApiTestBase
{
    private readonly Mock<IHttpClientFactory> _mockHtpClientFactory;
    
    public DeleteCategoryTests() : base(Module.BeersApiBlazorClient, Module.BeersApi, "GetCategory")
    {
        _mockHtpClientFactory = new Mock<IHttpClientFactory>();
    }

    [Test]
    public async Task GetCategory_WhenAWrongIdIsPassed_ShouldReturnNotFound()
    {
        Pact.UponReceiving("A request to get a category")
            .Given("Wrong Id was provided")
            .WithRequest(HttpMethod.Delete, "categories")
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WillRespond()
            .WithStatus(HttpStatusCode.NotFound);

        await Pact.VerifyAsync(async ctx =>
        {
            var client = new CategoryService(_mockHtpClientFactory.Object);

            var response = await client.Delete(99999);
            response.Should().NotBeNull();
        });
    }
}
    