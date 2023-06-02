using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeersApiBlazorClient.Models.Input.Colors;
using BeersApiBlazorClient.Services.Color;
using Microsoft.AspNetCore.Components;

namespace BeersApiBlazorClient.Pages.CodeBehind
{
   public class ColorOverviewBase : ComponentBase
   {
      [Inject] public IColorService ColorService { get; set; }

      public IEnumerable<Color> Colors { get; set; }

      protected bool ShowError;

      protected string Error;

      protected override async Task OnInitializedAsync()
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
   }
}
