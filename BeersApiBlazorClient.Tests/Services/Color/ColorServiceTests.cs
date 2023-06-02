using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.Color;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Services.Color
{

   [TestFixture]
   public class ColorServiceTests
   {
      #region Data

      private ColorService _sut;
      private Mock<IHttpClientFactory> _mockHtpClientFactory;

      #endregion


      [SetUp]
      public void BeforeEachTest()
      {
         _mockHtpClientFactory = new Mock<IHttpClientFactory>();
         _sut = new ColorService(_mockHtpClientFactory.Object);

      }

      #region TESTS

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
      public async Task GetAll_ApiResponseSuccessful_ReturnsListOfColors()
      {
         //arrange
         var colors = new List<Models.Input.Colors.Color>
         {
            new()
            {
               Id = 1,
               Name = "ColorTest",
               UId = Guid.NewGuid()
            },
            new()
            {
               Id = 2,
               Name = "SecondColorTest",
               UId = Guid.NewGuid()
            }
         };
         var colorsJson = JsonSerializer.Serialize(colors);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(colorsJson),
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //act
         var returnedColors = await _sut.GetAll();

         //assert
         Assert.IsNotNull(returnedColors);
         returnedColors.Count().Should().Be(2);
      }

      #endregion
   }
}
