using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApiBlazorClient.Services.User
{
   public interface IUserService
   {
      Task<Models.Common.Users.BeersApiUser> GetUser(string email);
   }
}
