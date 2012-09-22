using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaseloadManager.Data;
using CaseloadManager.Models;

namespace CaseloadManager.Helpers
{
    public class SessionItems
    {
        public static UserKey CurrentUser
        {
            get
            {
//This is for unit testing purposes only so we can unit test without a session state
#if DEBUG
                if (HttpContext.Current == null)
                {
                    return Data.UserKeys.GetCurrentUserKeys("thilehoffer");
                }
#endif

                if (HttpContext.Current.Session["CurrentUser"] == null)
                {

                    HttpContext.Current.Session["CurrentUser"] = Data.UserKeys.GetCurrentUserKeys(HttpContext.Current.User.Identity.Name);
                }
                return HttpContext.Current.Session["CurrentUser"] as UserKey;
            }

            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
        }
    }
}