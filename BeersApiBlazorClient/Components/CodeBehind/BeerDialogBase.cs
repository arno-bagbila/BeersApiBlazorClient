using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApiBlazorClient.Models.Common.Users;
using BeersApiBlazorClient.Models.Input.Comments;
using BeersApiBlazorClient.Services.Comment;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BeersApiBlazorClient.Components.CodeBehind
{
   public class BeerDialogBase : ComponentBase
   {
      [ParameterAttribute]
      public int BeerId { get; set; }

      [ParameterAttribute]
      public string BeerLogoUrl { get; set; }

      [ParameterAttribute]
      public string BeerCategoryName { get; set; }

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
      public string BeerName { get; set; }

      [ParameterAttribute]
      public int BeerCategoryId { get; set; }

      [ParameterAttribute]
      public int BeerColorId { get; set; }

      [ParameterAttribute]
      public int BeerCountryId { get; set; }

      [ParameterAttribute]
      public IEnumerable<int> BeerFlavourIds { get; set; }

      [ParameterAttribute]
      public IEnumerable<Comment> BeerComments { get; set; }

      [ParameterAttribute]
      public string BeersApiRole { get; set; }

      [ParameterAttribute]
      public BeersApiUser BeersApiUser { get; set; }

      [Inject] public DialogService DialogService { get; set; }

      [Inject] public ICommentService CommentService { get; set; }

      [Inject] public NavigationManager NavigationManager { get; set; }

      [CascadingParameter]
      public Task<AuthenticationState> AuthenticationStateTask { get; set; }

      public Models.Common.Comments.Comment Comment { get; set; } = new Models.Common.Comments.Comment();

      protected async Task ShowEditBeerDialog()
      {
         await DialogService.OpenAsync<AddBeerDialog>(BeerName, new Dictionary<string, object>
         {
            {"BeerId", BeerId},
            {"BeerName", BeerName},
            {"BeerLogoUrl", BeerLogoUrl},
            {"BeerCategoryName", BeerCategoryName},
            {"BeerColorName", BeerColorName},
            {"BeerFlavourNames", BeerFlavourNames},
            {"BeerCountryName", BeerCountryName},
            {"BeerAlcoholLevel", BeerAlcoholLevel},
            {"BeerTiwooRating", BeerTiwooRating},
            {"BeerDescription", BeerDescription},
            {"BeerCategoryId", BeerCategoryId},
            {"BeerColorId", BeerColorId},
            {"BeerCountryId", BeerCountryId},
            {"BeerFlavourIds", BeerFlavourIds}
         });
      }

      public async Task Submit(Models.Common.Comments.Comment comment)
      {
         if (BeersApiUser != null)
         {
            comment.BeerId = BeerId;
            comment.UserFirstName = BeersApiUser.UserFirstName;
            comment.UserUId = BeersApiUser.UId;

            await CommentService.AddComment(comment).ConfigureAwait(false);

            NavigateToBeersOverview();
         }
      }

      protected void NavigateToBeersOverview()
      {
         NavigationManager.NavigateTo("/beers", true);
      }
   }
}
