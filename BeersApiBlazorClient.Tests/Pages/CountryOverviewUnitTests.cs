using System;
using System.Collections.Generic;
using BeersApiBlazorClient.Elements;
using BeersApiBlazorClient.Models.Input.Countries;
using BeersApiBlazorClient.Pages;
using BeersApiBlazorClient.Services.Country;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Pages
{
   public class CountryOverviewUnitTests : TestContextWrapper
   {
      private Mock<ICountryService> _countryServiceMock;

      [SetUp]
      public void SetUp()
      {
         _countryServiceMock = new Mock<ICountryService>();

         TestContext = new Bunit.TestContext();
         TestContext.Services.AddSingleton(_countryServiceMock.Object);
      }

      [Test]
      public void ItShouldDisplayError()
      {
         //arrange
         _countryServiceMock.Setup(x => x.GetAll()).Throws<Exception>();

         //act
         var renderedComponent = RenderComponent<CountryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CountryOverviewError}");
      }

      [Test]
      public void ItShouldNotDisplayError()
      {
         //arrange
         _countryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Country> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<CountryOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.CountryOverviewError}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayPageTitle()
      {
         //act
         var renderedComponent = RenderComponent<CountryOverview>();

         //assert
         Assert.AreEqual("Countries", renderedComponent
            .Find($"#{ElementIds.CountryOverviewTitle}")
            .TextContent);
      }

      [Test]
      public void ItShouldDisplayLoading()
      {
         //arrange
         _countryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Country>());

         //act
         var renderedComponent = RenderComponent<CountryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CountryOverviewLoading}");
      }

      [Test]
      public void ItShouldNotDisplayLoading()
      {
         //arrange
         _countryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Country> { new() { Id = 1 } });

         //act
         var renderedComponent = RenderComponent<CountryOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.CountryOverviewLoading}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayCountriesGrid()
      {
         //arrange
         _countryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Country> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<CountryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CountryOverviewGrid}");
      }
   }
}
