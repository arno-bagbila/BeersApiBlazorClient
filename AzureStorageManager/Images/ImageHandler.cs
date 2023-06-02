using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureStorageManager.Images
{
   public class ImageHandler : IImageHandler
   {
      public async Task<string> GetImageLink(string imageName, string containerName, Stream stream)
      {
         return await UploadBlobIntoContainer(imageName, containerName, stream).ConfigureAwait(false);
      }

      #region Private Methods

      private async Task<string> UploadBlobIntoContainer(string imageName, string containerName, Stream stream)
      {
         var blobContainerClient = GetBlobContainerClient(containerName);
         var blobClient = blobContainerClient.GetBlobClient(imageName);
         var blobUploadOptions = new BlobUploadOptions
         {
            HttpHeaders = new BlobHttpHeaders { ContentType = "image/jpg" }
         };
         await blobClient.UploadAsync(stream, blobUploadOptions).ConfigureAwait(false);
         return blobClient.Uri.AbsoluteUri;
      }

      private BlobContainerClient GetBlobContainerClient(string containerName)
      {
         var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
         var blobContainer = blobServiceClient.GetBlobContainerClient(containerName);
         return blobContainer;
      }

      #endregion
   }
}
