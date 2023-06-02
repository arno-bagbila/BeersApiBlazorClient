using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Infrastructure.Apis.Extensions;

namespace BeersApiBlazorClient.Services.ExtensionMethods
{
   public static class HttpResponseMessageExtensions
   {
      public static async Task<Response> CheckUnsuccessfulResponse(this HttpResponseMessage apiResponse, string entityTypeName, string entityId = null)
      {
         if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
            return new Response
               { Success = false, ErrorMessage = $"You do not have the authorization to update this {entityTypeName}" };

         if (apiResponse.StatusCode == HttpStatusCode.Unauthorized)
            return new Response
               { Success = false, ErrorMessage = $"You are not logging, you cannot update a {entityTypeName}" };

         if (apiResponse.StatusCode == HttpStatusCode.NotFound)
            return new Response { Success = false, ErrorMessage = $"Could not find {entityTypeName} with id {entityId}" };

         var resultError = JsonSerializer.Deserialize<BeersApiValidationErrorResult>(apiResponse.Content.ReadAsStringAsync().Result);

         var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
         return new Response { Success = false, ErrorMessage = errorDetails.Message ?? resultError.BeersApiErrorResultToString() };

      }
   }
}
