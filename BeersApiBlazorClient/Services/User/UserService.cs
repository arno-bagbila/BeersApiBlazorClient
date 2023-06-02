using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;

namespace BeersApiBlazorClient.Services.User
{

   public class UserService : IUserService
   {

      #region Data

      private readonly IHttpClientFactory _factory;

      #endregion

      #region Constructors

      public UserService(IHttpClientFactory factory)
      {
         _factory = factory;
      }

      #endregion

      public async Task<Models.Common.Users.BeersApiUser> GetUser(string email)
      {
         var client = _factory.CreateClient("identity");
         var apiResponse = await client.GetAsync($"users/{email}").ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
         {
            var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new Exception(errorDetails.Message);
         }

         return JsonSerializer.Deserialize<Models.Common.Users.BeersApiUser>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      }
   }
}
