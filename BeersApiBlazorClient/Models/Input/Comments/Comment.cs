using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Models.Input.Comments
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
      /// Time when the comment was posted
      /// </summary>
      public DateTime DatePosted { get; set; }
   }
}
