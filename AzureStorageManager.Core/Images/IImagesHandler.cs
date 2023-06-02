using System.IO;
using System.Threading.Tasks;

namespace AzureStorageManager.Core.Images
{
   public interface IImagesHandler
   {
      Task<string> GetImageLink(string imageName, string containerName, Stream stream);
   }
}
