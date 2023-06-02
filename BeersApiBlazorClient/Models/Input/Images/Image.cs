using System;
using System.IO;

namespace BeersApiBlazorClient.Models.Input.Images
{
   public class Image
   {
      /// <summary>
      /// Image data uri to be send
      /// </summary>
      public string DataUri { get; set; }

      /// <summary>
      /// Image name to be set in Azure storage blob container
      /// </summary>
      public string Name { get; set; }
   }
}
