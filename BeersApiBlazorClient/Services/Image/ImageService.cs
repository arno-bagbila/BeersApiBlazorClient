using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Models.Input.Images;

namespace BeersApiBlazorClient.Services.Image
{
   public class ImageService : IImageService
   {
      #region Data

      private readonly IHttpClientFactory _factory;

      #endregion

      public ImageService(IHttpClientFactory factory)
      {
         _factory = factory;
      }

      public async Task<ImageUrl> GetImageLink(string dataUri, string imageName)
      {
         var image = new Models.Input.Images.Image
         {
            DataUri = dataUri,
            Name = imageName
         };

         var client = _factory.CreateClient("api");
         var imageAsJson = JsonSerializer.Serialize(image);
         var apiResponse = await client.PostAsync($"images",
            new StringContent(imageAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
         {
            var resultError = JsonSerializer.Deserialize<BeersApiValidationErrorResult>(apiResponse.Content.ReadAsStringAsync().Result);

            var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(
               await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            throw new Exception(resultError.Name != null? 
               $"{errorDetails.Message} - {string.Join(";", resultError.Description)}" :
               $"{errorDetails.Message}");
         }

         var imageUrl = JsonSerializer.Deserialize<ImageUrl>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         if (imageUrl != null && string.IsNullOrWhiteSpace(imageUrl.Url))
            throw new Exception("Something wrong happened when uploading image to the AzureStorage");

         return imageUrl;
      }
   }
}
