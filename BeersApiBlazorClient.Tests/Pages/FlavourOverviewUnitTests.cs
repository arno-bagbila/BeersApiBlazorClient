using System;
using System.Collections.Generic;
using BeersApiBlazorClient.Elements;
using BeersApiBlazorClient.Models.Input.Flavours;
using BeersApiBlazorClient.Pages;
using BeersApiBlazorClient.Services.Flavour;
using Blazored.LocalStorage;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Radzen;

namespace BeersApiBlazorClient.Tests.Pages
{
   public class FlavourOverviewUnitTests : TestContextWrapper
   {
      private Mock<IFlavourService> _flavourServiceMock;
      private Mock<ILocalStorageService> _localStorageServiceMock;
      private Mock<IAuthorizationPolicyProvider> _authorizationPolicyProviderMock;

      [SetUp]
      public void SetUp()
      {
         _flavourServiceMock = new Mock<IFlavourService>();
         _localStorageServiceMock = new Mock<ILocalStorageService>();
         _authorizationPolicyProviderMock = new Mock<IAuthorizationPolicyProvider>();

         TestContext = new Bunit.TestContext();
         TestContext.Services.AddSingleton(_flavourServiceMock.Object);
         TestContext.Services.AddSingleton(_localStorageServiceMock.Object);
         TestContext.Services.AddScoped<DialogService>();
         TestContext.Services.AddSingleton(_authorizationPolicyProviderMock.Object);
      }

      #region TESTS

      [Test]
      public void ItShouldDisplayError()
      {
         //arrange
         _flavourServiceMock.Setup(x => x.GetAll()).Throws<Exception>();

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.FlavourOverviewError}");
      }

      [Test]
      public void ItShouldNotDisplayError()
      {
         //arrange
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewError}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayPageTitle()
      {
         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         Assert.AreEqual("Flavours", renderedComponent
            .Find($"#{ElementIds.FlavourOverviewTitle}")
            .TextContent);
      }

      [Test]
      public void ItShouldDisplayAddFlavourButton()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.FlavourOverviewAddFlavourButton}");
      }

      [Test]
      public void ItShouldNotDisplayAddFlavourButton()
      {
         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewAddFlavourButton}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayLoading()
      {
         //arrange
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour>());

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.FlavourOverviewLoading}");
      }

      [Test]
      public void ItShouldNotDisplayLoading()
      {
         //arrange
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> {new(){ Id = 1 }, new(){ Id = 2 }});

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewLoading}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayFlavoursGrid()
      {
         //arrange
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new() { Id = 1 } });

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.FlavourOverviewGrid}");
      }

      [Test]
      public void ItShouldNotDisplayFlavoursGrid()
      {
         //arrange
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> ());

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewGrid}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldNotDisplayAdminOptionsInFlavoursGrid()
      {
         //arrange
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new(){ Id = 1 }, new(){ Id = 2 }});

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewGridAdmin}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayAdminOptionsInFlavoursGrid()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewGridAdmin}");
         elements.Count.Should().BeGreaterThan(0);
      }

      [Test]
      public void ItShouldDisplayEditButtonInFlavoursGrid()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewGridAdminEditButton}");
         elements.Count.Should().BeGreaterThan(0);
      }

      [Test]
      public void ItShouldNotDisplaySaveButtonInFlavoursGrid()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewGridAdminSaveButton}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldNotDisplayCancelButtonInFlavoursGrid()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewGridAdminCancelButton}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayDeleteButtonInFlavoursGridForAdmin()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");
         _flavourServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Flavour> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<FlavourOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.FlavourOverviewGridAdminDeleteButton}");
         elements.Count.Should().BeGreaterThan(0);
      }

      #endregion
   }
}
