using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Infrastructure.Apis
{
   public class BeersApiValidationErrorResult
   {
      public IEnumerable<string> Name { get; set; }

      public IEnumerable<string> Description { get; set; }
   }
}
