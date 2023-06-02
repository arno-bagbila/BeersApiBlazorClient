using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Models.Input.Categories
{
   public class Category
   {
      /// <summary>
      /// Category Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Category unique Id
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// Category Name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Category Description
      /// </summary>
      public string Description { get; set; }

      [JsonIgnore]
      public bool IsEditing { get; set; }
   }
}
