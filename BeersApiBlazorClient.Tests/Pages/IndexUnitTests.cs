using BeersApiBlazorClient.Elements;
using BeersApiBlazorClient.Services.User;
using Blazored.LocalStorage;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Index = BeersApiBlazorClient.Pages.Index;

namespace BeersApiBlazorClient.Tests.Pages
{
   public class IndexUnitTests
   {
      [Test]
      public void ItShouldDisplayPageTitle()
      {
         //arrange
         var localStorageMock = new Mock<ILocalStorageService>();
         var userServiceMock = new Mock<IUserService>();

         using var ctx = new Bunit.TestContext();
         ctx.AddTestAuthorization();
         ctx.Services.AddSingleton(localStorageMock.Object);
         ctx.Services.AddSingleton(userServiceMock.Object);

         //act
         var renderedComponent = ctx.RenderComponent<Index>();

         //assert
         Assert.AreEqual("Beer's Notebook", renderedComponent
            .Find($"#{ElementIds.HomePageTitle}")
            .TextContent);
      }
   }
}
