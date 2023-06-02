using System;
using System.Threading.Tasks;
using BeersApiBlazorClient.Services.Category;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BeersApiBlazorClient.Components.CodeBehind
{
   public class DeleteCategoryDialogBase : ComponentBase
   {
      [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

      [Inject] public ICategoryService CategoryService { get; set; }

      [Inject] public NavigationManager NavigationManager { get; set; }

      [Parameter]
      public string Name { get; set; }

      [Parameter]
      public int Id { get; set; }

      public string ErrorMessage { get; set; }

      public bool ShowErrorMessage { get; set; }

      public bool ShowDeleteDialog { get; set; } = true;

      public async Task DeleteCategory(int categoryId)
      {
         try
         {
            var response = await CategoryService.Delete(categoryId).ConfigureAwait(false);
            if (!response.Success)
            {
               ShowErrorMessage = true;
               ErrorMessage = response.ErrorMessage;
            }
            else
            {
               ShowDeleteDialog = false;
            }
         }
         catch (Exception e)
         {
            ShowErrorMessage = true;
            ErrorMessage = $"Something wrong happens - {e.Message}";
         }

      }

      protected void NavigateToOverview()
      {
         NavigationManager.NavigateTo("/categories", true);
      }

      public void Cancel() => BlazoredModal.CancelAsync();

      public void Done()
      {
         BlazoredModal.CancelAsync();
         StateHasChanged();
         NavigateToOverview();
      }
   }
}
