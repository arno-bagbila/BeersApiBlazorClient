using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;

namespace BeersApiBlazorClient.Services.Country
{
   public class CountryService : ICountryService
   {
      #region Data

      private readonly IHttpClientFactory _factory;

      #endregion

      #region Constructors

      public CountryService(IHttpClientFactory factory)
      {
         _factory = factory;
      }

      #endregion

      public async Task<IEnumerable<Models.Input.Countries.Country>> GetAll()
      {
         var client = _factory.CreateClient("api");
         var apiResponse = await client.GetAsync("countries").ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
         {
            var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new Exception(errorDetails.Message);
         }

         return JsonSerializer.Deserialize<IEnumerable<Models.Input.Countries.Country>>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      }
   }
}
