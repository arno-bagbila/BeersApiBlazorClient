using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.Category;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Services.Category
{
   [TestFixture]
   public class CategoryServiceTests
   {
      #region Data

      private CategoryService _sut;
      private Mock<IHttpClientFactory> _mockHtpClientFactory;

      #endregion

      [SetUp]
      public void BeforeEachTest()
      {
         _mockHtpClientFactory = new Mock<IHttpClientFactory>();
         _sut = new CategoryService(_mockHtpClientFactory.Object);
      }

      #region Tests

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
      public async Task GetAll_ApiResponseSuccessful_ReturnsListOfCategories()
      {
         //arrange
         var categories = new List<Models.Input.Categories.Category>
         {
            new()
            {
               Description = "CategoryTestDescription",
               Id = 1,
               IsEditing = false,
               Name = "CategoryTest",
               UId = Guid.NewGuid()
            },
            new()
            {
               Description = "SecondCategoryTestDescription",
               Id = 2,
               IsEditing = false,
               Name = "SecondCategoryTest",
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
         _mockHtpClientFactory.Setup(x => x.CreateClient("api"))
            .Returns(client);

         //act
         var returnedCategories = await _sut.GetAll();

         //assert
         Assert.IsNotNull(returnedCategories);
         returnedCategories.Count().Should().Be(2);
      }

      #endregion

      #region UPDATE

      [Test]
      public async Task UpdateCategory_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
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
         var forbiddenResponse = await _sut.Update(new Models.Input.Categories.Category());

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateCategory_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
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
         var unauthorizedResponse = await _sut.Update(new Models.Input.Categories.Category());

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateCategory_ApiResponseNotFound_ReturnsResponseWithSuccessFalse()
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
         var notFoundResponse = await _sut.Update(new Models.Input.Categories.Category { Id = 1 });

         //assert
         notFoundResponse.Should().NotBeNull();
         notFoundResponse.Success.Should().BeFalse();
         notFoundResponse.ErrorMessage.Contains("1");
      }

      [Test]
      public async Task UpdateCategory_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
      {
         //arrange
         var errorDetails = new ErrorDetails {StatusCode = 400, Message = "Something wrong happens!"};
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
         var badRequestResponse = await _sut.Update(new Models.Input.Categories.Category {Id = 1});

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }


      [Test]
      public async Task UpdateCategory_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
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
         var internalServerResponse = await _sut.Update(new Models.Input.Categories.Category { Id = 1 });

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task UpdateCategory_ApiResponseSuccessful_ReturnsUpdatedCategory()
      {
         //arrange
         var updatedCategory = new Models.Input.Categories.Category {Id = 1};
         var updatedCategoryJson = JsonSerializer.Serialize(updatedCategory);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(updatedCategoryJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var successfulResponse = await _sut.Update(new Models.Input.Categories.Category { Id = 1 });

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }

      #endregion

      #region CREATE

      [Test]
      public async Task AddCategory_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
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
         var unauthorizedResponse = await _sut.AddCategory(new Models.Output.Categories.Category());

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddCategory_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
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
         var forbiddenResponse = await _sut.AddCategory(new Models.Output.Categories.Category());

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddCategory_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
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
         var badRequestResponse = await _sut.AddCategory(new Models.Output.Categories.Category());

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddCategory_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
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
         var internalServerResponse = await _sut.AddCategory(new Models.Output.Categories.Category());

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task AddCategory_ApiResponseSuccessful_ReturnsCategory()
      {
         //arrange
         var category = new Models.Input.Categories.Category { Name = "test", Id = 1 };
         var categoryJson = JsonSerializer.Serialize(category);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(categoryJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
            .Returns(client);

         //act
         var successfulResponse = await _sut.AddCategory(new Models.Output.Categories.Category { Name = "test" });

         //assert
         successfulResponse.Should().NotBeNull();
         successfulResponse.Success.Should().BeTrue();
      }

      #endregion

      #region DELETE

      [Test]
      public async Task DeleteCategory_ApiResponseForbidden_ReturnsResponseWithSuccessFalse()
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
         var forbiddenResponse = await _sut.Delete(1);

         //assert
         forbiddenResponse.Should().NotBeNull();
         forbiddenResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteCategory_ApiResponseUnauthorized_ReturnsResponseWithSuccessFalse()
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
         var unauthorizedResponse = await _sut.Delete(1);

         //assert
         unauthorizedResponse.Should().NotBeNull();
         unauthorizedResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteCategory_ApiResponseNotFound_ReturnsResponseWithSuccessFalse()
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
         var notFoundResponse = await _sut.Delete(1);

         //assert
         notFoundResponse.Should().NotBeNull();
         notFoundResponse.Success.Should().BeFalse();
         notFoundResponse.ErrorMessage.Contains("1");
      }

      [Test]
      public async Task DeleteCategory_ApiResponseBadRequest_ReturnsResponseWithSuccessFalse()
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
         var badRequestResponse = await _sut.Delete(1);

         //assert
         badRequestResponse.Should().NotBeNull();
         badRequestResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteCategory_ApiResponseInternalServerError_ReturnsResponseWithSuccessFalse()
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
         var internalServerResponse = await _sut.Delete(1);

         //assert
         internalServerResponse.Should().NotBeNull();
         internalServerResponse.Success.Should().BeFalse();
      }

      [Test]
      public async Task DeleteCategory_ApiResponseSuccessful_ReturnsDeletedCategory()
      {
         //arrange
         var deletedCategory = new Models.Input.Categories.Category { Id = 1 };
         var deletedCategoryJson = JsonSerializer.Serialize(deletedCategory);
         var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
         mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(deletedCategoryJson)

            });
         var client = new HttpClient(mockHttpMessageHandler.Object)
         {
            BaseAddress = new Uri("https://localhost:44346")
         };
         _mockHtpClientFactory.Setup(x => x.CreateClient("identity"))
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
