using System;
using System.Collections.Generic;
using BeersApiBlazorClient.Models.Input.Categories;
using BeersApiBlazorClient.Models.Input.Colors;
using BeersApiBlazorClient.Models.Input.Comments;
using BeersApiBlazorClient.Models.Input.Countries;
using BeersApiBlazorClient.Models.Input.Flavours;
using BeersApiBlazorClient.Models.Input.Images;

namespace BeersApiBlazorClient.Models.Input.Beers
{
   public class Beer
   {
      /// <summary>
      /// Beer Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Beer Unique Id
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// Beer Name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Beer Description
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// The alcohol level in the Beer
      /// </summary>
      public double AlcoholLevel { get; set; }

      /// <summary>
      /// Rating of Beer by Tiwoo
      /// </summary>
      public double TiwooRating { get; set; }

      /// <summary>
      /// Category to which the beer belongs
      /// </summary>
      public Category Category { get; set; }

      /// <summary>
      /// Color of the beer
      /// </summary>
      public Color Color { get; set; }

      /// <summary>
      /// Country of origin of the beer
      /// </summary>
      public Country Country { get; set; }

      /// <summary>
      /// Flavours of the beer
      /// </summary>
      public IEnumerable<Flavour> Flavours { get; set; }

      /// <summary>
      /// Images of the beer
      /// </summary>
      public IEnumerable<Image> Images { get; set; }

      /// <summary>
      /// Beer logo
      /// </summary>
      public string LogoUrl { get; set; }

      /// <summary>
      /// Beer comments
      /// </summary>
      public IEnumerable<Comment> Comments { get; set; }

      /// <summary>
      /// Beer registration date
      /// </summary>
      public DateTime DateRegistered { get; set; }
   }
}
