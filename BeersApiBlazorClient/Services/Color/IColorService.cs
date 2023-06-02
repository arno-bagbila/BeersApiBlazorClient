using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Services.Color
{
   public interface IColorService
   {
      Task<IEnumerable<Models.Input.Colors.Color>> GetAll();
   }
}
