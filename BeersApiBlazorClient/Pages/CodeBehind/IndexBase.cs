using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApiBlazorClient.Models.Common.Users;
using BeersApiBlazorClient.Services.User;
using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BeersApiBlazorClient.Pages.CodeBehind
{
   public class IndexBase : ComponentBase
   {
      [CascadingParameter]
      public Task<AuthenticationState> AuthenticationStateTask { get; set; }

      [Inject] public Blazored.LocalStorage.ILocalStorageService LocalStorageService { get; set; }

      [Inject] public IUserService UserService { get; set; }

      public string Message { get; set; }

      protected override async Task OnInitializedAsync()
      {
         await LocalStorageService.ClearAsync();

         //TODO: Add role claim from the BeersApi to the list of claims we get from UserStorage
         Message = "Hello there!";
        
         var authState = await AuthenticationStateTask.ConfigureAwait(false);
         var user = authState.User;

         if (user.Identity != null && user.Identity.IsAuthenticated)
         {
            var emailClaim = user.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email);

            if (emailClaim != null && !string.IsNullOrWhiteSpace(emailClaim.Value))
            {
               var beersApiUser = await UserService.GetUser(emailClaim.Value).ConfigureAwait(false);

               if (beersApiUser != null)
               {
                  var givenNameClaim = user.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName);
                  if (givenNameClaim != null && !string.IsNullOrWhiteSpace(givenNameClaim.Value))
                  {
                     beersApiUser.UserFirstName = givenNameClaim.Value;
                     await LocalStorageService.SetItemAsync("beersApiUser", beersApiUser);
                  }

                  await LocalStorageService.SetItemAsync("beersapirole", beersApiUser.RoleName).ConfigureAwait(false);
                  var beersapirole = await LocalStorageService.GetItemAsStringAsync("beersapirole");

                  if (!string.IsNullOrWhiteSpace(beersapirole))
                        Message = beersapirole;
               }

            }
         }
      }
   }
}
