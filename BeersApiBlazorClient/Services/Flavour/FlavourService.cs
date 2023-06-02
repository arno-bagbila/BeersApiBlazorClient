using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Infrastructure.Apis.Extensions;
using BeersApiBlazorClient.Services.ExtensionMethods;

namespace BeersApiBlazorClient.Services.Flavour
{
   public class FlavourService : IFlavourService
   {
      #region Data

      private readonly IHttpClientFactory _factory;

      #endregion

      #region Constructors

      public FlavourService(IHttpClientFactory factory)
      {
         _factory = factory;
      }

      #endregion

      public async Task<IEnumerable<Models.Input.Flavours.Flavour>> GetAll()
      {
         var client = _factory.CreateClient("api");
         var apiResponse = await client.GetAsync("flavours").ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
         {
            var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new Exception(errorDetails.Message);
         }

         return JsonSerializer.Deserialize<IEnumerable<Models.Input.Flavours.Flavour>>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
      }

      public async Task<Response> AddFlavour(Models.Output.Flavours.Flavour flavour)
      {
         var client = _factory.CreateClient("identity");
         var flavourAsJson = JsonSerializer.Serialize(flavour);
         var apiResponse = await client.PostAsync("flavours",
            new StringContent(flavourAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("flavour");


         var createdFlavour = JsonSerializer.Deserialize<Models.Input.Flavours.Flavour>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return createdFlavour.Id > 0
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }

      public async Task<Response> Update(Models.Input.Flavours.Flavour flavour)
      {
         var client = _factory.CreateClient("identity");
         var flavourAsJson = JsonSerializer.Serialize(flavour);
         var apiResponse = await client.PutAsync($"flavours/{flavour.Id}",
            new StringContent(flavourAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("flavour", flavour.Id.ToString());


         var updatedFlavour = JsonSerializer.Deserialize<Models.Input.Flavours.Flavour>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return updatedFlavour.Id == flavour.Id
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }

      public async Task<Response> Delete(int flavourId)
      {
         var client = _factory.CreateClient("identity");
         var apiResponse = await client.DeleteAsync($"flavours/{flavourId}").ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("category", flavourId.ToString());


         var deletedFlavour = JsonSerializer.Deserialize<Models.Input.Flavours.Flavour>(
            await apiResponse.Content.ReadAsStringAsync()
               .ConfigureAwait(false), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return deletedFlavour.Id == flavourId
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }
   }
}
