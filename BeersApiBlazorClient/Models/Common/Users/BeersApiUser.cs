using System;

namespace BeersApiBlazorClient.Models.Common.Users
{
   public class BeersApiUser
   {
      public Guid UId { get; set; }

      public int  Id { get; set; }

      public string Email { get; set; }

      public string RoleName { get; set; }

      public string UserFirstName { get; set; }
   }
}
