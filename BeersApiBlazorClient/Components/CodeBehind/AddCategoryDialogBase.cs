using System;
using System.Threading.Tasks;
using BeersApiBlazorClient.Models.Output.Categories;
using BeersApiBlazorClient.Services.Category;
using Microsoft.AspNetCore.Components;

namespace BeersApiBlazorClient.Components.CodeBehind
{
   public class AddCategoryDialogBase : ComponentBase
   {
      public bool ShowDialog { get; set; }

      [Parameter]
      public Category Category { get; set; }

      [Parameter]
      public EventCallback<bool> CloseEventCallback { get; set; }

      [Inject]
      public ICategoryService CategoryService { get; set; }

      [Inject] public NavigationManager NavigationManager { get; set; }

      protected bool ShowError;

      protected string Error;

      public void Close()
      {
         ShowDialog = false;
         StateHasChanged();
      }

      public void Show()
      {
         ResetDialog();
         ShowDialog = true;
         StateHasChanged();
      }

      private void ResetDialog()
      {
         ShowError = false;
         Category = new Category();
      }

      protected async Task HandleValidSubmit()
      {
         try
         {
            var response = await CategoryService.AddCategory(Category).ConfigureAwait(false);

            if (!response.Success)
            {
               ShowError = true;
               Error = response.ErrorMessage;
            }
            else
            {
               ShowDialog = false;
               await CloseEventCallback.InvokeAsync(true);
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
         NavigationManager.NavigateTo("/categories", true);
      }
   }
}
