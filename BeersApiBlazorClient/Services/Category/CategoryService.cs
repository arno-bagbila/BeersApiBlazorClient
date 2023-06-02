using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.ExtensionMethods;

namespace BeersApiBlazorClient.Services.Category
{
   public class CategoryService : ICategoryService
   {
      #region Data

      private readonly IHttpClientFactory _factory;


      #endregion

      #region Constructors

      public CategoryService(IHttpClientFactory factory)
      {
         _factory = factory;
      }

      #endregion

      public async Task<IEnumerable<Models.Input.Categories.Category>> GetAll()
      {
         var client = _factory.CreateClient("api");
         var apiResponse = await client.GetAsync("categories").ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
         {
            var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
               new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
            throw new Exception(errorDetails.Message);
         }

         return JsonSerializer.Deserialize<IEnumerable<Models.Input.Categories.Category>>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false), new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
      }


      public async Task<Response> Update(Models.Input.Categories.Category category)
      {
         var client = _factory.CreateClient("identity");
         var categoryAsJson = JsonSerializer.Serialize(category);
         var apiResponse = await client.PutAsync($"categories/{category.Id}", 
            new StringContent(categoryAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("category", category.Id.ToString());


         var updatedCategory = JsonSerializer.Deserialize<Models.Input.Categories.Category>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

         return updatedCategory.Id == category.Id
            ? new Response {Success = true}
            : new Response {Success = false, ErrorMessage = "Something wrong happened!"};
      }
        

      public async Task<Response> AddCategory(Models.Output.Categories.Category category)
      {
         var client = _factory.CreateClient("identity");
         var categoryAsJson = JsonSerializer.Serialize(category);
         var apiResponse = await client.PostAsync($"categories",
            new StringContent(categoryAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("category");

         var createdCategory = JsonSerializer.Deserialize<Models.Input.Categories.Category>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return createdCategory.Id > 0
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }

      public async Task<Response> Delete(int categoryId)
      {
         var client = _factory.CreateClient("identity");
         var apiResponse = await client.DeleteAsync($"categories/{categoryId}").ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("category", categoryId.ToString());
         

         var deletedCategory = JsonSerializer.Deserialize<Models.Input.Categories.Category>(
            await apiResponse.Content.ReadAsStringAsync()
               .ConfigureAwait(false), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return deletedCategory.Id == categoryId
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }
   }
}
