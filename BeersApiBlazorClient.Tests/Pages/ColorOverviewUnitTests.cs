using System;
using System.Collections.Generic;
using BeersApiBlazorClient.Elements;
using BeersApiBlazorClient.Models.Input.Colors;
using BeersApiBlazorClient.Pages;
using BeersApiBlazorClient.Services.Color;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Pages
{
   public class ColorOverviewUnitTests : TestContextWrapper
   {
      private Mock<IColorService> _colorServiceMock;

      [SetUp]
      public void SetUp()
      {
         _colorServiceMock = new Mock<IColorService>();

         TestContext = new Bunit.TestContext();
         TestContext.Services.AddSingleton(_colorServiceMock.Object);
      }

      [Test]
      public void ItShouldDisplayError()
      {
         //arrange
         _colorServiceMock.Setup(x => x.GetAll()).Throws<Exception>();

         //act
         var renderedComponent = RenderComponent<ColorOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.ColorOverviewError}");
      }

      [Test]
      public void ItShouldNotDisplayError()
      {
         //arrange
         _colorServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Color> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<ColorOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.ColorOverviewError}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayPageTitle()
      {
         //act
         var renderedComponent = RenderComponent<ColorOverview>();

         //assert
         Assert.AreEqual("Colors", renderedComponent
            .Find($"#{ElementIds.ColorOverviewTitle}")
            .TextContent);
      }

      [Test]
      public void ItShouldDisplayLoading()
      {
         //arrange
         _colorServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Color>());

         //act
         var renderedComponent = RenderComponent<ColorOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.ColorOverviewLoading}");
      }

      [Test]
      public void ItShouldNotDisplayLoading()
      {
         //arrange
         _colorServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Color> { new() { Id = 1 } });

         //act
         var renderedComponent = RenderComponent<ColorOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.ColorOverviewLoading}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayColorsGrid()
      {
         //arrange
         _colorServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Color> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<ColorOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.ColorOverviewGrid}");
      }
   }
}
