using System;
using System.Collections.Generic;
using BeersApiBlazorClient.Elements;
using BeersApiBlazorClient.Models.Input.Beers;
using BeersApiBlazorClient.Models.Input.Categories;
using BeersApiBlazorClient.Models.Input.Colors;
using BeersApiBlazorClient.Models.Input.Countries;
using BeersApiBlazorClient.Models.Input.Flavours;
using BeersApiBlazorClient.Pages;
using BeersApiBlazorClient.Services.Beer;
using Blazored.LocalStorage;
using NUnit.Framework;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Radzen;

namespace BeersApiBlazorClient.Tests.Pages
{
   public class BeerOverviewUnitTests : TestContextWrapper
   {
      private Mock<IBeerService> _beerServiceMock;
      private Mock<ILocalStorageService> _localStorageServiceMock;

      [SetUp]
      public void SetUp()
      {
         _beerServiceMock = new Mock<IBeerService>();
         _localStorageServiceMock = new Mock<ILocalStorageService>();

         TestContext = new Bunit.TestContext();
         TestContext.Services.AddSingleton(_beerServiceMock.Object);
         TestContext.Services.AddSingleton(_localStorageServiceMock.Object);
         TestContext.Services.AddScoped<DialogService>();
      }

      #region TESTS

      [Test]
      public void ItShouldDisplayPageTitle()
      {
         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         Assert.AreEqual("Beers", renderedComponent
            .Find($"#{ElementIds.BeerOverviewTitle}")
            .TextContent);
      }

      [Test]
      public void ItShouldDisplayAddBeerButton()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.BeerOverviewAddBeerButton}");
      }

      [Test]
      public void ItShouldNotDisplayAddBeerButton()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.BeerOverviewAddBeerButton}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayLoading()
      {
         //arrange
         _beerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Beer>());

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.BeerLoadingId}");
      }

      [Test]
      public void ItShouldNotDisplayLoading()
      {
         //arrange
         _beerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Beer>{ new() {Id = 1}});

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.BeerLoadingId}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayError()
      {
         //arrange
         _beerServiceMock.Setup(x => x.GetAll()).Throws<Exception>();

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.BeerOverviewError}");
      }

      [Test]
      public void ItShouldNotDisplayError()
      {
         //arrange
         _beerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Beer> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.BeerOverviewError}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayBeerListing()
      {
         //arrange
         _beerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Beer>{ new() { Id = 1}, new(){ Id = 2}});

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.BeerOverviewBeersListing}");
      }

      [Test]
      public void ItShouldDisplayAllBeersReceivedFromApiCall()
      {
         //arrange
         _beerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Beer> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<BeerOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.BeerOverviewBeer}");
         elements.Count.Should().Be(2);
      }

      [Test]
      public void ItShouldDisplayClickableBeers()
      {
         //arrange
         _beerServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Beer>
         {
            new()
            {
               Id = 1,
               Category = new Category { Id = 1, Name = "beerCategoryTest"},
               Color = new Color { Id = 1, Name = "beerColorTest"},
               Flavours = new List<Flavour>{ new() { Name = "beerFlavourTest"}},
               Country = new Country { Id = 1, Name = "beerCountryTest"}
            }, new()
            {
               Id = 2,
               Category = new Category { Id = 2, Name = "beerCategoryTest2"},
               Color = new Color { Id = 2, Name = "beerColorTest2"},
               Flavours = new List<Flavour>{ new() { Name = "beerFlavourTest2"}},
               Country = new Country { Id = 2, Name = "beerCountryTest2"}
            } 

         });
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");

         //act
         var renderedComponent = RenderComponent<BeerOverview>();
         var firstBeerElement = renderedComponent.FindAll($"#{ElementIds.BeerOverviewBeer}")[0];

         //assert
         firstBeerElement.Click();
      }

      #endregion

      [TearDown]
      public void TearDown() => TestContext?.Dispose();
   }
}
