using System.Collections.Generic;

namespace BeersApiBlazorClient.Models.Output.Beers
{
   public class Beer
   {
      /// <summary>
      /// Beer Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Beer's name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Beer's description
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Beer's alcohol level in percentage
      /// </summary>
      public double AlcoholLevel { get; set; }

      /// <summary>
      /// Rating cannot be more than 5
      /// </summary>
      public double TiwooRating { get; set; }

      /// <summary>
      /// Id of the category to which the beer belong
      /// </summary>
      public int CategoryId { get; set; }

      /// <summary>
      /// Id of the color of the beer
      /// </summary>
      public int ColorId { get; set; }

      /// <summary>
      /// Id of the country of the beer
      /// </summary>
      public int CountryId { get; set; }

      /// <summary>
      /// Ids of the beer different flavours
      /// </summary>
      public IEnumerable<int> FlavourIds { get; set; }

      /// <summary>
      /// url of the logo of the beer
      /// </summary>
      public string LogoUrl { get; set; }
   }
}
