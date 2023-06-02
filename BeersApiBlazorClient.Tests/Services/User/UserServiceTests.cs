using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.User;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Services.User
{
   [TestFixture]
   public class UserServiceTests
   {
      #region Data

      private UserService _sut;
      private Mock<IHttpClientFactory> _mockHtpClientFactory;

      #endregion

      [SetUp]
      public void BeforeEachTest()
      {
         _mockHtpClientFactory = new Mock<IHttpClientFactory>();
         _sut = new UserService(_mockHtpClientFactory.Object);
      }

      [Test]
      public void GetUser_ApiResponseNotSuccessful_ThrowsException()
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //assert
         var e = Assert.ThrowsAsync<Exception>(async () => await _sut.GetUser("test"));
         Assert.IsNotNull(e);
      }

      [Test]
      public async Task GetUser_ApiResponseSuccessful_ReturnsUser()
      {
         //arrange
         var beersApiUser = new Models.Common.Users.BeersApiUser { UserFirstName = "Test" };
         var beersApiUserJson = JsonSerializer.Serialize(beersApiUser);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(beersApiUserJson),
            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var beersApiUserRetrieved = await _sut.GetUser("test");

         //assert
         Assert.IsNotNull(beersApiUserRetrieved);
      }
   }
}
