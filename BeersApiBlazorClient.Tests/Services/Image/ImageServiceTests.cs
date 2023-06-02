using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Models.Input.Images;
using BeersApiBlazorClient.Services.Image;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Services.Image
{

   [TestFixture]
   public class ImageServiceTests
   {
      #region Data

      private ImageService _sut;
      private Mock<IHttpClientFactory> _mockHttpClientFactory;

      #endregion

      [SetUp]
      public void BeforeEachTest()
      {
         _mockHttpClientFactory = new Mock<IHttpClientFactory>();
         _sut = new ImageService(_mockHttpClientFactory.Object);
      }

      [Test]
      public void GetImageLink_ApiResponseNotSuccessfulBadRequest_ThrowsException()
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
               Content = new StringContent(errorJson)
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //assert
         var e = Assert.ThrowsAsync<Exception>(async () => await _sut.GetImageLink(null, null));
         Assert.IsNotNull(e);
      }

      [Test]
      public void GetImageLink_ApiResponseNotSuccessfulValidationError_ThrowsException()
      {
         //arrange
         var beersApiValidationErrorResult = new BeersApiValidationErrorResult
         {
            Name = new List<string> {"Name"}, 
            Description = new List<string>{"Name is not valid"}

         };
         var errorJson = JsonSerializer.Serialize(beersApiValidationErrorResult);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.BadRequest,
               Content = new StringContent(errorJson)
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //assert
         var e = Assert.ThrowsAsync<Exception>(async () => await _sut.GetImageLink(null, null));
         Assert.IsNotNull(e);
      }

      [Test]
      public void GetImageLink_ApiResponseSuccessfulWithEmptyImageLink_ThrowsException()
      {
         //arrange
         var imageUrl = new ImageUrl { Url = string.Empty };
         var imageUrlJson = JsonSerializer.Serialize(imageUrl);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(imageUrlJson)
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //assert
         var e = Assert.ThrowsAsync<Exception>(async () => await _sut.GetImageLink(null, null));
         Assert.IsNotNull(e);
      }

      [Test]
      public async Task GetImageLink_ApiResponseSuccessful_ReturnsImageLink()
      {
         //arrange
         var imageUrl = new ImageUrl { Url = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg" };
         var imageUrlJson = JsonSerializer.Serialize(imageUrl);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(imageUrlJson)
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHttpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //act
         var imageUrlReturned =   await _sut.GetImageLink(null, null);

         //assert
         imageUrlReturned.Should().NotBeNull();
      }
   }
}
