using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.ExtensionMethods;

namespace BeersApiBlazorClient.Services.Beer
{
   public class BeerService : IBeerService
   {
      #region Data

      private readonly IHttpClientFactory _factory;

      #endregion

      #region Constructors

      public BeerService(IHttpClientFactory factory)
      {
         _factory = factory;
      }

      #endregion


      public async Task<IEnumerable<Models.Input.Beers.Beer>> GetAll()
      {
         var client = _factory.CreateClient("api");
         var apiResponse = await client.GetAsync("beers").ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
         {
            var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new Exception(errorDetails.Message);
         }

         return JsonSerializer.Deserialize<IEnumerable<Models.Input.Beers.Beer>>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      }

      public async Task<Response> AddBeer(Models.Output.Beers.Beer beer)
      {
         var client = _factory.CreateClient("identity");
         var beerAsJson = JsonSerializer.Serialize(beer);
         var apiResponse = await client.PostAsync($"beers",
            new StringContent(beerAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("beer");


         var createdBeer = JsonSerializer.Deserialize<Models.Input.Beers.Beer>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return createdBeer.Id > 0
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }

      public async Task<Response> Update(Models.Output.Beers.Beer beer)
      {
         var client = _factory.CreateClient("identity");
         var beerAsJson = JsonSerializer.Serialize(beer);
         var apiResponse = await client.PutAsync($"beers/{beer.Id}",
            new StringContent(beerAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("beer");


         var updatedBeer = JsonSerializer.Deserialize<Models.Input.Beers.Beer>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return updatedBeer.Id == beer.Id
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }
   }
}
