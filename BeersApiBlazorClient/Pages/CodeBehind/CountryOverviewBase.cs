using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeersApiBlazorClient.Models.Input.Countries;
using BeersApiBlazorClient.Services.Country;
using Microsoft.AspNetCore.Components;

namespace BeersApiBlazorClient.Pages.CodeBehind
{
   public class CountryOverviewBase : ComponentBase
   {
      [Inject] public ICountryService CountryService { get; set; }

      public IEnumerable<Country> Countries { get; set; }

      protected bool ShowError;

      protected string Error;

      protected override async Task OnInitializedAsync()
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
   }
}
