using System;

namespace BeersApiBlazorClient.Models.Input.Colors
{
   public class Color
   {
      /// <summary>
      /// Color Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Color name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Unique identifier for the color
      /// </summary>
      public Guid UId { get; set; }
   }
}
