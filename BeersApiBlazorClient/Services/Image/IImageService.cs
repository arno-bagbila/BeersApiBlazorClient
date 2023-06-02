using System.Threading.Tasks;
using BeersApiBlazorClient.Models.Input.Images;

namespace BeersApiBlazorClient.Services.Image
{
   public interface IImageService
   {
      Task<ImageUrl> GetImageLink(string dataUri, string imageName);
   }
}
