using System.Collections.Generic;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;

namespace BeersApiBlazorClient.Services.Flavour
{
   public interface IFlavourService
   {
      Task<IEnumerable<Models.Input.Flavours.Flavour>> GetAll();

      Task<Response> AddFlavour(Models.Output.Flavours.Flavour flavour);

      Task<Response> Update(Models.Input.Flavours.Flavour flavour);

      Task<Response> Delete(int flavourId);
   }
}
