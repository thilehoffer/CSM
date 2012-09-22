using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaseloadManager.Helpers;
using CaseloadManager.Data;
using CaseloadManager.Models;
using System.IO;

namespace CaseloadManager.Controllers 
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class StudentIEPController : Controller
    {
        [Authorize]
        [HttpPost]
        public ActionResult Get(int id)
        {
            var check = Data.Security.CheckForStudent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            var exsiting = Data.Queries.DoesStuentHaveIePs(id);
            var student = Data.Objects.GetStudent(id);

            if (exsiting)
                return (PartialView("_Index", student));
            else
                return (PartialView("_Create", new
                StudentIEPModel { StudentId = id, StudentName = student.FirstName + " " + student.LastName }));
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateGet(int id)
        {
            var check = Data.Security.CheckForStudent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            var student = Data.Objects.GetStudent(id);
            return PartialView("_Create", new Models.StudentIEPModel
            {
                StudentId = id,
                StudentName = student.FirstName + " " + student.LastName,
                IsComplete = false,
                IsCurrent = true
            });

        }

        [HttpPost]
        [Authorize]
        public ActionResult EditGet(int id)
        {
            var check = Data.Security.CheckForStudentIEP(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            return PartialView("_Edit", Data.Objects.GetStudentIEP(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Models.StudentIEPModel model)
        {
            var check = Data.Security.CheckForStudentIEP(model.StudentIEPId.Value, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });

            model.DoValidation();
            if (!model.IsValid())
            {
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            }

            Data.CRUD.UpdateStudentIep(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { errorList = new string[] { "" }, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AttachmentsGet(int id)
        {
            var check = Data.Security.CheckForStudentIEP(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");
            return PartialView("_Documents", Data.Objects.GetStudentIEP(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Models.StudentIEPModel model)
        {
            var check = Data.Security.CheckForStudent(model.StudentId, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });

            model.DoValidation();
            if (!model.IsValid())
            {
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            }
            Data.CRUD.CreateStudentIEP(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var check = Data.Security.CheckForStudentIEP(id, SessionItems.CurrentUser.UserId);

            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });


            Data.CRUD.DeleteStudentIEP(id);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteGet(int id)
        {
            var check = Data.Security.CheckForStudentIEP(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");
            var model = Data.Objects.GetIepWithStudent(id);
            return PartialView("_Delete", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddFile(int? studentId, int? studentIepId, IEnumerable<HttpPostedFileBase> attachments)
        {
            var check = Data.Security.CheckForStudentIEP(studentIepId.Value, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            byte[] fileData = null;
            var postedFile = attachments.First();
            using (var binaryReader = new BinaryReader(postedFile.InputStream))
            {
                fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
            }

            var document = Data.CRUD.CreateStudentIepDocument(
                studentId.Value, studentIepId.Value, Helpers.Misc.GetFileNameFromPath(postedFile.FileName), fileData, SessionItems.CurrentUser.UserId);
            return Json(new { studentDocumentId = document.StudentDocumentId }, "text/plain");


        }

        public ActionResult NoAccess()
        {
            return (View());
        }

        public ActionResult NotFound()
        {
            return (View());
        }

    }
}
