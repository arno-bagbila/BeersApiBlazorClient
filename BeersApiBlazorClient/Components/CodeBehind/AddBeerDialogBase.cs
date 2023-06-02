using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureStorageManager.Core.Images;
using BeersApiBlazorClient.Models.Input.Categories;
using BeersApiBlazorClient.Models.Input.Colors;
using BeersApiBlazorClient.Models.Input.Countries;
using BeersApiBlazorClient.Models.Input.Flavours;
using BeersApiBlazorClient.Models.Output.Beers;
using BeersApiBlazorClient.Services.Beer;
using BeersApiBlazorClient.Services.Category;
using BeersApiBlazorClient.Services.Color;
using BeersApiBlazorClient.Services.Country;
using BeersApiBlazorClient.Services.Flavour;
using BeersApiBlazorClient.Services.Image;
using Microsoft.AspNetCore.Components;
using Radzen;
using Exception = System.Exception;

namespace BeersApiBlazorClient.Components.CodeBehind
{
   public class AddBeerDialogBase : ComponentBase
   {
      [ParameterAttribute]
      public int BeerId { get; set; }

      [ParameterAttribute]
      public string BeerName { get; set; }

      [ParameterAttribute]
      public string BeerCategoryName { get; set; }

      [ParameterAttribute]
      public string BeerLogoUrl { get; set; }

      [ParameterAttribute]
      public string BeerColorName { get; set; }

      [ParameterAttribute]
      public IEnumerable<string> BeerFlavourNames { get; set; }

      [ParameterAttribute]
      public string BeerCountryName { get; set; }

      [ParameterAttribute]
      public double BeerAlcoholLevel { get; set; }

      [ParameterAttribute]
      public double BeerTiwooRating { get; set; }

      [ParameterAttribute]
      public string BeerDescription { get; set; }

      [ParameterAttribute] 
      public int BeerCategoryId { get; set; }

      [ParameterAttribute]
      public int BeerColorId { get; set; }

      [ParameterAttribute]
      public int BeerCountryId { get; set; }

      [ParameterAttribute]
      public IEnumerable<int> BeerFlavourIds { get; set; }

      public Beer Beer { get; set; } = new Beer();

      [Inject] public DialogService DialogService { get; set; }

      [Inject] public ICategoryService CategoryService { get; set; }

      [Inject] public IColorService ColorService { get; set; }

      [Inject] public ICountryService CountryService { get; set; }

      [Inject] public IFlavourService FlavourService { get; set; }

      [Inject] public IBeerService BeerService { get; set; }

      [Inject] public IImageService ImageService { get; set; }

      [Inject] public NavigationManager NavigationManager { get; set; }

      public IEnumerable<Category> Categories { get; set; }

      public IEnumerable<Color> Colors { get; set; }

      public IEnumerable<Country> Countries { get; set; }

      public IEnumerable<Flavour> Flavours { get; set; }

      public string LogoUrlString { get; set; }

      protected bool ShowError;

      protected string Error;


      protected override async Task OnInitializedAsync()
      {
         await GetAllCategories().ConfigureAwait(false);
         await GetAllColors().ConfigureAwait(false);
         await GetAllCountries().ConfigureAwait(false);
         await GetAllFlavours();

         if (BeerId != 0)
         {
            Beer.Id = BeerId;
            Beer.Name = BeerName;
            Beer.AlcoholLevel = BeerAlcoholLevel;
            Beer.TiwooRating = BeerTiwooRating;
            Beer.CategoryId = BeerCategoryId;
            Beer.CountryId = BeerCountryId;
            Beer.ColorId = BeerColorId;
            Beer.Description = BeerDescription;
            Beer.LogoUrl = BeerLogoUrl;
            Beer.FlavourIds = BeerFlavourIds;
         }
      }

      public async Task Submit(Beer beer)
      {
         try
         {
            if (string.IsNullOrWhiteSpace(LogoUrlString))
            {
               beer.LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/default_beer.jpg";
            }
            else
            {
               var imageUrl = await ImageService.GetImageLink(LogoUrlString, beer.Name).ConfigureAwait(false);
               beer.LogoUrl = imageUrl.Url;
            }

            var response = BeerId == 0
               ? await BeerService.AddBeer(beer).ConfigureAwait(false)
               : await BeerService.Update(beer).ConfigureAwait(false);

            if (!response.Success)
            {
               ShowError = true;
               Error = response.ErrorMessage;
            }
            else
            {
               DialogService.Close();
               StateHasChanged();
               NavigateToOverview();
            }
         }
         catch (Exception e)
         {
            ShowError = true;
            Error = $"Something wrong happens - {e.Message}";
         }
      }

      protected void NavigateToOverview()
      {
         NavigationManager.NavigateTo("/beers", true);
      }

      #region Private Methods

      private async Task GetAllCategories()
      {
         try
         {
            Categories = await CategoryService.GetAll().ConfigureAwait(false);
         }
         catch (Exception e)
         {
            ShowError = true;
            Error = $"Something wrong happens - {e.Message}";
         }
      }

      private async Task GetAllColors()
      {
         try
         {
            Colors = await ColorService.GetAll().ConfigureAwait(false);
         }
         catch (Exception e)
         {
            ShowError = true;
            Error = $"Something wrong happens - {e.Message}";
         }
      }

      private async Task GetAllCountries()
      {
         try
         {
            Countries = await CountryService.GetAll().ConfigureAwait(false);
         }
         catch (Exception e)
         {
            ShowError = true;
            Error = $"Something wrong happens - {e.Message}";
         }
      }

      private async Task GetAllFlavours()
      {
         try
         {
            Flavours = await FlavourService.GetAll().ConfigureAwait(false);
         }
         catch (Exception e)
         {
            ShowError = true;
            Error = $"Something wrong happens - {e.Message}";
         }
      }

      //private async Task<string> GetImageLink(string dataUri, string imageName)
      //{
      //   var position = dataUri.IndexOf(',');
      //   var data = dataUri.Substring(position + 1);
      //   var bytes = Convert.FromBase64String(data);
      //   var stream = new MemoryStream(bytes);
      //   var imageLink = await ImageHandler
      //      .GetImageLink(imageName, "beersapilogourls", stream)
      //      .ConfigureAwait(false);

      //   if (string.IsNullOrWhiteSpace(imageLink))
      //      throw new Exception("Something wrong happened when uploading image to the AzureStorage");

      //   return imageLink;
      //}

      #endregion

   }
}
