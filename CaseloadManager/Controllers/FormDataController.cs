using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaseloadManager.Data;

namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class FormDataController : Controller
    {
         
        [Authorize]
        [HttpGet]
        public ActionResult GetStudentList(bool includePrevious)
        {
            return Json(Data.Lists.GetStudentList(Helpers.SessionItems.CurrentUser.UserId, includePrevious), JsonRequestBehavior.AllowGet);
        }
         

        [Authorize]
        [HttpGet]
        public ActionResult GetUpcomingEvents()
        {
            return Json(Data.Lists.GetUpcompintEvents(Helpers.SessionItems.CurrentUser.UserId, false, 50), JsonRequestBehavior.AllowGet);
        }

    }
}
