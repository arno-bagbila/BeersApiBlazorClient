using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeersApiBlazorClient.Components;
using BeersApiBlazorClient.Models.Input.Flavours;
using BeersApiBlazorClient.Services.Flavour;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace BeersApiBlazorClient.Pages.CodeBehind
{
   public class FlavourOverviewBase : ComponentBase
   {
      [Inject] 
      public IFlavourService FlavourService { get; set; }

      [Inject] 
      public NavigationManager NavigationManager { get; set; }

      [Inject]
      public DialogService DialogService { get; set; }

      [Inject] public ILocalStorageService LocalStorageService { get; set; }

      public IEnumerable<Flavour> Flavours { get; set; } = new List<Flavour>();

      protected bool ShowError;

      protected string Error;

      protected string BeersApiRole;

      protected RadzenGrid<Flavour> FlavoursGrid;

      protected override async Task OnInitializedAsync()
      {
         BeersApiRole = await LocalStorageService.GetItemAsStringAsync("beersapirole").ConfigureAwait(false);

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

      protected override void OnInitialized()
      {
         DialogService.OnOpen += Open;
         DialogService.OnClose += Close;
      }

      void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
      {
         StateHasChanged();
      }

      void Close(dynamic result)
      {
         StateHasChanged();
      }

      protected void EditRow(Flavour flavour)
      {
         FlavoursGrid.EditRow(flavour);
      }


      protected void SaveRow(Flavour flavour)
      {
         FlavoursGrid.UpdateRow(flavour);
      }

      protected async Task OnUpdateRow(Flavour flavour)
      {
         try
         {
            var response = await FlavourService.Update(flavour).ConfigureAwait(false);

            if (!response.Success)
            {
               ShowError = true;
               Error = response.ErrorMessage;
            }
            else
            {
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

      protected void CancelEdit(Flavour flavour)
      {
         FlavoursGrid.CancelEditRow(flavour);
         NavigateToOverview();
      }

      protected void NavigateToOverview()
      {
         NavigationManager.NavigateTo("/flavours", true);
      }

      protected async Task ShowConfirmDeleteFlavourDialog(Flavour flavour)
      {
         var result = await DialogService.OpenAsync<ConfirmDialog>("Delete Flavour",
            new Dictionary<string, object>() {{"Message", $"Are you sure you want to delete {flavour.Name}"}},
            new DialogOptions() {Width = "400px", Height = "200px"});
         var resultBool = (bool) result;

         if (resultBool)
            await DeleteFlavour(flavour.Id).ConfigureAwait(false);
      }

      private async Task DeleteFlavour(int flavourId)
      {
         try
         {
            var response = await FlavourService.Delete(flavourId).ConfigureAwait(false);
            if (!response.Success)
            {
               ShowError = true;
               Error = response.ErrorMessage;
            }
            else
            {
               NavigateToOverview();
            }
         }
         catch (Exception e)
         {
            ShowError = true;
            Error = $"Something wrong happens - {e.Message}";
         }
      }
   }
}
