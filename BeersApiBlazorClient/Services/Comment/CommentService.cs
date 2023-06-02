using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;
using BeersApiBlazorClient.Services.ExtensionMethods;

namespace BeersApiBlazorClient.Services.Comment
{
   public class CommentService : ICommentService
   {

      #region Data

      private readonly IHttpClientFactory _factory;

      #endregion

      #region Constructors

      public CommentService(IHttpClientFactory factory)
      {
         _factory = factory;
      }

      #endregion

      public async Task<Response> AddComment(Models.Common.Comments.Comment comment)
      {
         var client = _factory.CreateClient("identity");
         var commentAsJson = JsonSerializer.Serialize(comment);
         var apiResponse = await client.PostAsync("beers/comment",
            new StringContent(commentAsJson, Encoding.UTF8, "application/json")).ConfigureAwait(false);
         if (!apiResponse.IsSuccessStatusCode)
            return await apiResponse.CheckUnsuccessfulResponse("comment");


         var updatedBeer = JsonSerializer.Deserialize<Models.Input.Beers.Beer>(
            await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return updatedBeer.Comments.Any()
            ? new Response { Success = true }
            : new Response { Success = false, ErrorMessage = "Something wrong happened!" };
      }
   }
}
