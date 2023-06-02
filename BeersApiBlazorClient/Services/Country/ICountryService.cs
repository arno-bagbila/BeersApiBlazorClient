using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Services.Country
{
   public interface ICountryService
   {
      Task<IEnumerable<Models.Input.Countries.Country>> GetAll();
   }
}
