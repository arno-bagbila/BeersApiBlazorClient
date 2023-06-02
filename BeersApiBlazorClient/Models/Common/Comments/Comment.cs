using System;
using Radzen;

namespace BeersApiBlazorClient.Models.Common.Comments
{
   public class Comment
   {
      /// <summary>
      /// Comment text
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// username of user who set the comment
      /// </summary>
      public string UserFirstName { get; set; }

      /// <summary>
      /// User Id
      /// </summary>
      public Guid UserUId { get; set; }

      /// <summary>
      /// Commented beer id
      /// </summary>
      public int BeerId { get; set; }
   }
}
