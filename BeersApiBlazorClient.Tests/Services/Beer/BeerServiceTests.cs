using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.Beer;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Services.Beer
{
   public class BeerServiceTests
   {
      #region Data

      private BeerService _sut;
      private Mock<IHttpClientFactory> _mockHtpClientFactory;

      #endregion

      [SetUp]
      public void BeforeEachTest()
      {
         _mockHtpClientFactory = new Mock<IHttpClientFactory>();
         _sut = new BeerService(_mockHtpClientFactory.Object);
      }

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
         _mockHtpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //assert
         var e = Assert.ThrowsAsync<Exception>(async () => await _sut.GetAll());
         Assert.IsNotNull(e);
      }

      [Test]
      public async Task GetAll_ApiResponseSuccessful_ReturnsListOfAllBeers()
      {
         //arrange
         var beers = new List<Models.Input.Beers.Beer>
         {
            new()
            {
               Description = "BeerTestDescription",
               Id = 1,
               Name = "BeerTest",
               UId = Guid.NewGuid()
            },
            new()
            {
               Description = "SecondBeerTestDescription",
               Id = 2,
               Name = "SecondBeerTest",
               UId = Guid.NewGuid()
            }
         };
         var beersJson = JsonSerializer.Serialize(beers);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(beersJson),
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("api"))
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
      public async Task AddBeer_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var unauthorizedResponse = await _sut.AddBeer(new Models.Output.Beers.Beer());

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddBeer_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var forbiddenResponse = await _sut.AddBeer(new Models.Output.Beers.Beer());

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddBeer_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var badRequestResponse = await _sut.AddBeer(new Models.Output.Beers.Beer());

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddBeer_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var internalServerResponse = await _sut.AddBeer(new Models.Output.Beers.Beer());

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddBeer_ApiResponseSuccessful_ReturnsBeer()
      {
         //arrange
         var beer = new Models.Input.Beers.Beer { Name = "test", Id = 1 };
         var beerJson = JsonSerializer.Serialize(beer);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(beerJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var successfulResponse = await _sut.AddBeer(new Models.Output.Beers.Beer { Name = "test" });

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }

      #endregion

      #region UPDATE

      [Test]
      public async Task UpdateBeer_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var forbiddenResponse = await _sut.Update(new Models.Output.Beers.Beer());

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateBeer_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var unauthorizedResponse = await _sut.Update(new Models.Output.Beers.Beer());

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateBeer_ApiResponseNotFound_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var notFoundResponse = await _sut.Update(new Models.Output.Beers.Beer { Id = 1 });

         //assert
         notFoundResponse.Should().NotBeNull();
         notFoundResponse.Success.Should().BeFalse();
         notFoundResponse.ErrorMessage.Contains("1");
      }

      [Test]
      public async Task UpdateBeer_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var badRequestResponse = await _sut.Update(new Models.Output.Beers.Beer { Id = 1 });

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }


      [Test]
      public async Task UpdateBeer_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var internalServerResponse = await _sut.Update(new Models.Output.Beers.Beer { Id = 1 });

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateBeer_ApiResponseSuccessful_ReturnsUpdatedBeer()
      {
         //arrange
         var updatedBeer = new Models.Input.Beers.Beer { Id = 1 };
         var updatedBeerJson = JsonSerializer.Serialize(updatedBeer);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(updatedBeerJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var successfulResponse = await _sut.Update(new Models.Output.Beers.Beer { Id = 1 });

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }

      #endregion
   }
}
