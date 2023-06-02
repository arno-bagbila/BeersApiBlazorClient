using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeersApiBlazorClient.Components;
using BeersApiBlazorClient.Models.Input.Categories;
using BeersApiBlazorClient.Services.Category;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BeersApiBlazorClient.Pages.CodeBehind
{
   public class CategoryOverviewBase : ComponentBase
   {
      [Inject] public ICategoryService CategoryService { get; set; }

      [Inject] public NavigationManager NavigationManager { get; set; }

      [Inject] public Blazored.LocalStorage.ILocalStorageService LocalStorageService { get; set; }

      [CascadingParameter]
      public IModalService Modal { get; set; }

      public IEnumerable<Category> Categories { get; set; } = new List<Category>();

      public AddCategoryDialog AddCategoryDialog { get; set; }

      protected DeleteCategoryDialog DeleteCategoryDialog { get; set; }

      protected bool ShowError;

      protected string Error;

      protected string BeersApiRole;

      protected override async Task OnInitializedAsync()
      {
         BeersApiRole = await LocalStorageService.GetItemAsStringAsync("beersapirole").ConfigureAwait(false);

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

      public void EnableEditing(bool flag, Category category)
      {
         category.IsEditing = flag;
      }

      public async Task Update(Category category)
      {
         try
         {
            var response = await CategoryService.Update(category).ConfigureAwait(false);

            if (!response.Success)
            {
               ShowError = true;
               Error = response.ErrorMessage;
            }
            else
            {
               category.IsEditing = true;
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

      public void AddCategoryDialog_OnDialogClose()
      {
         NavigateToOverview();
         StateHasChanged();
      }

      protected void AddCategory()
      {
         AddCategoryDialog.Show();
      }

      protected void ShowDeleteModal(Category category)
      {
         var parameters = new ModalParameters();
         parameters.Add(nameof(DeleteCategoryDialog.Name), category.Name);
         parameters.Add(nameof(DeleteCategoryDialog.Id), category.Id);

         Modal.Show<DeleteCategoryDialog>("Delete Category", parameters);
      }
   }
}
