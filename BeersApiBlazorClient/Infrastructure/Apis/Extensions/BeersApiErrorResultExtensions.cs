using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace BeersApiBlazorClient.Infrastructure.Apis.Extensions
{
   public static class BeersApiErrorResultExtensions
   {
      public static string BeersApiErrorResultToString(this BeersApiValidationErrorResult beersApiValidationErrorResult)
      {
         var beersApiErrorResultString = new StringBuilder();

         foreach (var p in beersApiValidationErrorResult.GetType().GetProperties())
         {
            var value = p.GetValue(beersApiValidationErrorResult, null);
            if (value != null)
            {
               beersApiErrorResultString.AppendJoin(Environment.NewLine, ((IEnumerable)value).Cast<string>().ToList());
            }
         }

         return beersApiErrorResultString.ToString();
      }
   }
}
