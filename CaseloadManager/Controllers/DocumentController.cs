using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class DocumentController : Controller
    {
         

        [Authorize]
        public ActionResult Get(int id)
        {

            var document = Data.Objects.GetStudentDocument(id);

            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.Name,

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.Contents.ToArray(), Helpers.Misc.GetMimeType(document.Name));
        }
 
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                Data.CRUD.DeleteStudentDocument(id);
                return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
            }
            catch (Exception ex)
            {
                string innerException = ex.InnerException == null ? string.Empty : ex.InnerException.Message;
                return Json(new Helpers.JsonAjaxResult.result { errorList = new string[1] {string.Format("{0} {1}", ex.Message, innerException) }, success = false });
               
            }
        }
    }
}
