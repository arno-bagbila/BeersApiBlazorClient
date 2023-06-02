using System.IO;
using System.Threading.Tasks;

namespace AzureStorageManager.Images
{
   public interface IImageHandler
   {
      Task<string> GetImageLink(string imageName, string containerName, Stream stream);
   }
}
