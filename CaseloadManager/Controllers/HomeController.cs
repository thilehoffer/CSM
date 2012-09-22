using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaseloadManager.Helpers;

namespace CaseloadManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(Request.IsAuthenticated)
            { 
                return RedirectToAction("Index", "Student"); 
            }

            ViewBag.Message = "Welcome to Caseload Manager";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet] 
        [Authorize]
        #if !DEBUG
        [RequireHttps]
        #endif
        public ActionResult Feedback()
        {
            return View("Feedback", new Models.FeedbackModel());
        }


         [HttpPost] 
        [Authorize]
        #if !DEBUG
        [RequireHttps]
        #endif
        public ActionResult Feedback(Models.FeedbackModel model)
        {
            var s = model.comments;
            Data.CRUD.CreateUserFeedback(model, SessionItems.CurrentUser.UserId);
            return View("FeedbackSubmitted");
        }

        [HttpGet] 
        [Authorize]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }


        [HttpGet]
        [Authorize]
        public ActionResult NoAccess()
        {
            return View("NoAccess");
        }

    }
}
