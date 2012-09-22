using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaseloadManager.Models;
 

namespace CaseloadManager.Data
{
    public class UserKeys
    {
        public static UserKey GetCurrentUserKeys(string userName)
        {
            using (var context = new CaseloadManager.Entities.Context())
            {
                var user = context.vwUserDetails.Single(a => a.UserName == userName);
                return new UserKey { UserId = user.UserId, UserGuid = user.UserGuid, UserName = user.UserName };
            }
        }

        public static UserKey FindAccount(string emailAddress)
        {
            using (var context = new CaseloadManager.Entities.Context())
            {
                var details = context.vwUserDetails.SingleOrDefault(a => a.Email == emailAddress);
                if (details == null)
                    return null;
                else
                    return new UserKey { UserId = details.UserId, UserGuid = details.UserGuid, UserName = details.UserName };
            }
        }

    }
}