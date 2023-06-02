using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Models.Input.Flavours
{
   public class Flavour
   {
      public int Id { get; set; }

      public Guid UId { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }
   }
}
