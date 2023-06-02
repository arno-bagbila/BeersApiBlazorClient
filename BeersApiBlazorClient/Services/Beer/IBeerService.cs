using System.Collections.Generic;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;

namespace BeersApiBlazorClient.Services.Beer
{
   public interface IBeerService
   {
      Task<IEnumerable<Models.Input.Beers.Beer>> GetAll();

      Task<Response> AddBeer(Models.Output.Beers.Beer beer);

      Task<Response> Update(Models.Output.Beers.Beer beer);
   }
}
