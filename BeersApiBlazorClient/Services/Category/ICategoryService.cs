using System.Collections.Generic;
using System.Threading.Tasks;
using BeersApiBlazorClient.Infrastructure.Apis;

namespace BeersApiBlazorClient.Services.Category
{
   public interface ICategoryService
   {
      Task<IEnumerable<Models.Input.Categories.Category>> GetAll();

      Task<Response> Update(Models.Input.Categories.Category category);

      Task<Response> AddCategory(Models.Output.Categories.Category category);

      Task<Response> Delete(int categoryId);
   }
}
