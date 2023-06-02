using System;
using System.Collections.Generic;
using BeersApiBlazorClient.Elements;
using BeersApiBlazorClient.Models.Input.Categories;
using BeersApiBlazorClient.Pages;
using BeersApiBlazorClient.Services.Category;
using Blazored.LocalStorage;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace BeersApiBlazorClient.Tests.Pages
{
   public class CategoryOverviewUnitTests : TestContextWrapper
   {
      private Mock<ICategoryService> _categoryServiceMock;
      private Mock<ILocalStorageService> _localStorageServiceMock;

      [SetUp]
      public void SetUp()
      {
         _categoryServiceMock = new Mock<ICategoryService>();
         _localStorageServiceMock = new Mock<ILocalStorageService>();

         TestContext = new Bunit.TestContext();
         TestContext.Services.AddSingleton(_categoryServiceMock.Object);
         TestContext.Services.AddSingleton(_localStorageServiceMock.Object);
      }

      #region TESTS

      [Test]
      public void ItShouldDisplayError()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).Throws<Exception>();

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CategoryOverviewError}");
      }

      [Test]
      public void ItShouldNotDisplayError()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.CategoryOverviewError}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayPageTitle()
      {
         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         Assert.AreEqual("Categories", renderedComponent
            .Find($"#{ElementIds.CategoryOverviewTitle}")
            .TextContent);
      }

      [Test]
      public void ItShouldDisplayAddCategoryButton()
      {
         //arrange
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CategoryOverviewAddCategoryButton}");
      }

      [Test]
      public void ItShouldDisplayLoading()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category>());

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CategoryOverviewCategoriesLoading}");
      }

      [Test]
      public void ItShouldNotDisplayLoading()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category> { new() { Id = 1 } });

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.CategoryOverviewCategoriesLoading}");
         elements.Count.Should().Be(0);
      }

      [Test]
      public void ItShouldDisplayCategoriesTable()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category> { new() { Id = 1 } });

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CategoryOverviewCategoriesTable}");
      }

      [Test]
      public void ItShouldDisplayAllCategoriesReceivedFromApiCall()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category> { new() { Id = 1 }, new() { Id = 2 } });

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.CategoryOverviewCategoryTableRow}");
         elements.Count.Should().Be(2);
      }

      [Test]
      public void ItShouldDisplayEditAndDeleteColumn()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category> { new() { Id = 1 }, new() { Id = 2 } });
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         renderedComponent.Find($"#{ElementIds.CategoryOverviewCategoryTableEditAndDeleteColumn}");
      }

      [Test]
      public void ItShouldDisplayEditButtons()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category> { new() { Id = 1 }, new() { Id = 2 } });
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.CategoryOverviewEditButton}");
         elements.Count.Should().Be(2);
      }

      [Test]
      public void ItShouldDisplayDeleteButtons()
      {
         //arrange
         _categoryServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Category> { new() { Id = 1 }, new() { Id = 2 } });
         _localStorageServiceMock.Setup(x => x.GetItemAsStringAsync(It.IsAny<string>())).ReturnsAsync("BeersApiAdmin");

         //act
         var renderedComponent = RenderComponent<CategoryOverview>();

         //assert
         var elements = renderedComponent.FindAll($"#{ElementIds.CategoryOverviewDeleteButton}");
         elements.Count.Should().Be(2);
      }

      #endregion

      [TearDown]
      public void TearDown() => TestContext?.Dispose();
   }
}
