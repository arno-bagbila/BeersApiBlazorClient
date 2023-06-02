using System;

namespace BeersApiBlazorClient.Models.Input.Countries
{
   public class Country
   {
      /// <summary>
      /// Country Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Unique identifier for the country
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// Color name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Code of the Country
      /// </summary>
      public string Code { get; set; }
   }
}
