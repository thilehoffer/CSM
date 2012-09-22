using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaseloadManager.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SendTestMessage()
        {
            //using (var client = new SMSService.SendSMSWorldSoapClient())
            //{
            //    var result = client.sendSMS("CaseLoadManager@gmail.com", "11", "6107159128", "Test Message From CaseLoadManager");
            //}
            return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new string[] { "" }, success = true });
        }

    }
}
