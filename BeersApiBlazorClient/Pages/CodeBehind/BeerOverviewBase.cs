using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeersApiBlazorClient.Components;
using BeersApiBlazorClient.Models.Common.Users;
using BeersApiBlazorClient.Models.Input.Beers;
using BeersApiBlazorClient.Services.Beer;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BeersApiBlazorClient.Pages.CodeBehind
{
   public class BeerOverviewBase : ComponentBase
   {
      [Inject] public IBeerService BeerService { get; set; }

      [Inject] public DialogService DialogService { get; set; }

      [Inject] public Blazored.LocalStorage.ILocalStorageService LocalStorageService { get; set; }

      public IEnumerable<Beer> Beers { get; set; }


      protected bool ShowError;

      protected string Error;

      [Parameter]
      public string BeersApiRole { get; set; }

      [Parameter]
      public int Numbers { get; set; }

      public BeersApiUser BeersApiUser { get; set; }

      protected override async Task OnInitializedAsync()
      {
         BeersApiRole = await LocalStorageService.GetItemAsStringAsync("beersapirole");
         BeersApiUser = await LocalStorageService.GetItemAsync<BeersApiUser>("beersApiUser") ?? new BeersApiUser();

         try
         {
            Beers = await BeerService.GetAll().ConfigureAwait(false);
         }
         catch (Exception e)
         {
            ShowError = true;
            Error = $"Something wrong happens - {e.Message}";
         }
      }

      public async Task ShowBeerDialog(Beer beer, string beersApiRole)
      {

         await DialogService.OpenAsync<BeerDialog>(beer.Name, new Dictionary<string, object>
            {
               {"BeerId", beer.Id},
               {"BeerLogoUrl", beer.LogoUrl},
               {"BeerCategoryName", beer.Category.Name},
               {"BeerColorName", beer.Color.Name},
               {"BeerFlavourNames", beer.Flavours.Select(f => f.Name)},
               {"BeerCountryName", beer.Country.Name},
               {"BeerAlcoholLevel", beer.AlcoholLevel},
               {"BeerTiwooRating", beer.TiwooRating},
               {"BeerDescription", beer.Description},
               {"BeerName", beer.Name},
               {"BeerCategoryId", beer.Category.Id},
               {"BeerColorId", beer.Color.Id},
               {"BeerCountryId", beer.Country.Id},
               {"BeerFlavourIds", beer.Flavours.Select(f => f.Id)},
               {"BeersApiRole", beersApiRole },
               {"BeerComments", beer.Comments },
               {"BeersApiUser", BeersApiUser }
            },
            new DialogOptions());
      }
   }
}
