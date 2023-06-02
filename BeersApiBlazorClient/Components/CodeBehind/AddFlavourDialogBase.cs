using System;
using System.Threading.Tasks;
using BeersApiBlazorClient.Models.Output.Flavours;
using BeersApiBlazorClient.Services.Flavour;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BeersApiBlazorClient.Components.CodeBehind
{
   public class AddFlavourDialogBase : ComponentBase
   {
      public Flavour Flavour { get; set; } = new Flavour();

      [Inject]
      public DialogService DialogService { get; set; }

      [Inject]
      public IFlavourService FlavourService { get; set; }

      [Inject] public NavigationManager NavigationManager { get; set; }

      protected bool ShowError;

      protected string Error;

      public async Task Submit(Flavour flavour)
      {
         try
         {
            var response = await FlavourService.AddFlavour(flavour).ConfigureAwait(false);

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
         NavigationManager.NavigateTo("/flavours", true);
      }
   }
}
