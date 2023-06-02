using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.Comment;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Services.Comment
{

   [TestFixture]
   public class CommentServiceTests
   {
      #region Data

      private CommentService _sut;
      private Mock<IHttpClientFactory> _mockHtpClientFactory;

      #endregion

      [SetUp]
      public void BeforeEachTest()
      {
         _mockHtpClientFactory = new Mock<IHttpClientFactory>();
         _sut = new CommentService(_mockHtpClientFactory.Object);

      }

      [Test]
      public async Task AddComment_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
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
         var unauthorizedResponse = await _sut.AddComment(new Models.Common.Comments.Comment());

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddComment_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
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
         var forbiddenResponse = await _sut.AddComment(new Models.Common.Comments.Comment());

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddComment_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
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
         var badRequestResponse = await _sut.AddComment(new Models.Common.Comments.Comment());

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddComment_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
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
         var internalServerResponse = await _sut.AddComment(new Models.Common.Comments.Comment());

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddComment_ApiResponseSuccessful_ReturnsBeer()
      {
         //arrange
         var beer = new Models.Input.Beers.Beer { Name = "test", Id = 1, Comments  = new List<Models.Input.Comments.Comment> {new() { Body = "Body"}}};
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
         var successfulResponse = await _sut.AddComment(new Models.Common.Comments.Comment() { Body = "test" });

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }
   }
}
