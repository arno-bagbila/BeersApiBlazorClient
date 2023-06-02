using BeersApiBlazorClient.Infrastructure.Apis;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Services.Comment
{
   public interface ICommentService
   {
      Task<Response> AddComment(Models.Common.Comments.Comment comment);
   }
}
