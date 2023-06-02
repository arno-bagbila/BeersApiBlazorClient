using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.Flavour;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Services.Flavour
{
   [TestFixture]
   public class FlavourServiceTests
   {
      #region Data

      private FlavourService _sut;
      private Mock<IHttpClientFactory> _mockHttpClientFactory;

      #endregion

      [SetUp]
      public void BeforeEachTest()
      {
         _mockHttpClientFactory = new Mock<IHttpClientFactory>();
         _sut = new FlavourService(_mockHttpClientFactory.Object);
      }

      #region TESTS

      #region GET ALL

      [Test]
      public void GetAll_ApiResponseNotSuccessful_ThrowsException()
      {
         //arrange
         var errorDetails = new ErrorDetails { Message = "Called failed", StatusCode = 400 };
         var errorJson = JsonSerializer.Serialize(errorDetails);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.BadRequest,
               Content = new StringContent(errorJson),
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //assert
         var e = Assert.ThrowsAsync<Exception>(async () => await _sut.GetAll());
         Assert.IsNotNull(e);
      }

      [Test]
      public async Task GetAll_ApiResponseSuccessful_ReturnsListOfCategories()
      {
         //arrange
         var categories = new List<Models.Input.Flavours.Flavour>
         {
            new()
            {
               Description = "FlavourTestDescription",
               Id = 1,
               Name = "FlavourTest",
               UId = Guid.NewGuid()
            },
            new()
            {
               Description = "SecondFlavourTestDescription",
               Id = 2,
               Name = "SecondFlavourTest",
               UId = Guid.NewGuid()
            }
         };
         var categoriesJson = JsonSerializer.Serialize(categories);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(categoriesJson),
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //act
         var returnedCategories = await _sut.GetAll();

         //assert
         Assert.IsNotNull(returnedCategories);
         returnedCategories.Count().Should().Be(2);
      }

      #endregion

      #region CREATE

      [Test]
      public async Task AddFlavour_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.Unauthorized
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var unauthorizedResponse = await _sut.AddFlavour(new Models.Output.Flavours.Flavour());

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddFlavour_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.Forbidden
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var forbiddenResponse = await _sut.AddFlavour(new Models.Output.Flavours.Flavour());

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddFlavour_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var errorDetails = new ErrorDetails { StatusCode = 400, Message = "Something wrong happens!" };
         var errorDetailsJson = JsonSerializer.Serialize(errorDetails);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.BadRequest,
               Content = new StringContent(errorDetailsJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var badRequestResponse = await _sut.AddFlavour(new Models.Output.Flavours.Flavour());

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddFlavour_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var errorDetails = new ErrorDetails { StatusCode = 500, Message = "Something wrong happens!" };
         var errorDetailsJson = JsonSerializer.Serialize(errorDetails);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.InternalServerError,
               Content = new StringContent(errorDetailsJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var internalServerResponse = await _sut.AddFlavour(new Models.Output.Flavours.Flavour());

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddFlavour_ApiResponseSuccessful_ReturnsCategory()
      {
         //arrange
         var flavour = new Models.Input.Flavours.Flavour { Name = "test", Id = 1 };
         var flavourJson = JsonSerializer.Serialize(flavour);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(flavourJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var successfulResponse = await _sut.AddFlavour(new Models.Output.Flavours.Flavour { Name = "test" });

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }

      #endregion

      #region UPDATE

      [Test]
      public async Task UpdateFlavour_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.Forbidden
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var forbiddenResponse = await _sut.Update(new Models.Input.Flavours.Flavour());

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateFlavour_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.Unauthorized
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var unauthorizedResponse = await _sut.Update(new Models.Input.Flavours.Flavour());

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateFlavour_ApiResponseNotFound_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.NotFound
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var notFoundResponse = await _sut.Update(new Models.Input.Flavours.Flavour { Id = 1 });

         //assert
         notFoundResponse.Should().NotBeNull();
         notFoundResponse.Success.Should().BeFalse();
         notFoundResponse.ErrorMessage.Contains("1");
      }

      [Test]
      public async Task UpdateFlavour_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var errorDetails = new ErrorDetails { StatusCode = 400, Message = "Something wrong happens!" };
         var errorDetailsJson = JsonSerializer.Serialize(errorDetails);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.BadRequest,
               Content = new StringContent(errorDetailsJson)
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var badRequestResponse = await _sut.Update(new Models.Input.Flavours.Flavour() { Id = 1 });

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }


      [Test]
      public async Task UpdateFlavour_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var errorDetails = new ErrorDetails { StatusCode = 500, Message = "Something wrong happens!" };
         var errorDetailsJson = JsonSerializer.Serialize(errorDetails);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.InternalServerError,
               Content = new StringContent(errorDetailsJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var internalServerResponse = await _sut.Update(new Models.Input.Flavours.Flavour { Id = 1 });

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateFlavour_ApiResponseSuccessful_ReturnsUpdatedCategory()
      {
         //arrange
         var updatedFlavour = new Models.Input.Flavours.Flavour { Id = 1 };
         var updatedFlavourJson = JsonSerializer.Serialize(updatedFlavour);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(updatedFlavourJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var successfulResponse = await _sut.Update(new Models.Input.Flavours.Flavour { Id = 1 });

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }

      #endregion

      #region DELETE

      [Test]
      public async Task DeleteFlavour_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.Forbidden
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var forbiddenResponse = await _sut.Delete(1);

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteFlavour_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.Unauthorized
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var unauthorizedResponse = await _sut.Delete(1);

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteFlavour_ApiResponseNotFound_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.NotFound
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var notFoundResponse = await _sut.Delete(1);

         //assert
         notFoundResponse.Should().NotBeNull();
         notFoundResponse.Success.Should().BeFalse();
         notFoundResponse.ErrorMessage.Contains("1");
      }

      [Test]
      public async Task DeleteFlavour_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var errorDetails = new ErrorDetails { StatusCode = 400, Message = "Something wrong happens!" };
         var errorDetailsJson = JsonSerializer.Serialize(errorDetails);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.BadRequest,
               Content = new StringContent(errorDetailsJson)
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var badRequestResponse = await _sut.Delete(1);

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteFlavour_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var errorDetails = new ErrorDetails { StatusCode = 500, Message = "Something wrong happens!" };
         var errorDetailsJson = JsonSerializer.Serialize(errorDetails);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.InternalServerError,
               Content = new StringContent(errorDetailsJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var internalServerResponse = await _sut.Delete(1);

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteFlavour_ApiResponseSuccessful_ReturnsDeletedCategory()
      {
         //arrange
         var deletedFlavour = new Models.Input.Categories.Category { Id = 1 };
         var deletedFlavourJson = JsonSerializer.Serialize(deletedFlavour);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(deletedFlavourJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var successfulResponse = await _sut.Delete(1);

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }

      #endregion

      #endregion
   }
}
