using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaseloadManager.Helpers;
using CaseloadManager.Data;
using System.IO;
using CaseloadManager.Models;

namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class StudentEvaluationController : Controller
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

            var exsiting = Data.Queries.DoesStudentHaveEvaluations(id);
            var student = Data.Objects.GetStudent(id);

            if (exsiting)
                return (PartialView("_Index", student));
            else
                return (PartialView("_Create", new
                 StudentEvaluationModel { StudentId = id, StudentName = student.FirstName + " " + student.LastName }));
        }

        [Authorize]
        [HttpPost]
        public ActionResult AttachmentsGet(int id)
        {
            var check = Data.Security.CheckForStudentEvaluation(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            return PartialView("_Documents", Data.Objects.GetStudentEvaluation(id));
        }


        [Authorize]
        [HttpPost]
        public ActionResult CreateGet(int id)
        {
            var student = Data.Objects.GetStudent(id);
            return (PartialView("_Create", new
               StudentEvaluationModel { StudentId = id, StudentName = student.FirstName + " " + student.LastName }));
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteGet(int id)
        {
            var check = Data.Security.CheckForStudentEvaluation(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            var model = Data.Objects.GetEvaluationWstudent(id);
            return PartialView("_Delete", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditGet(int id)
        {
            var check = Data.Security.CheckForStudentEvaluation(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            return PartialView("_Edit", Data.Objects.GetStudentEvaluation(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var check = Data.Security.CheckForStudentEvaluation(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });

            Data.CRUD.DeleteStudentEvaluation(id);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Models.StudentEvaluationModel model)
        {
            model.DoValidation();
            if (!model.IsValid())
            {
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            }
            Data.CRUD.CreateStudentEvaluation(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Models.StudentEvaluationModel model)
        {
            var check = Data.Security.CheckForStudentEvaluation(model.StudentEvaluationId.Value, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });


            model.DoValidation();
            if (!model.IsValid())
            {
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            }

            Data.CRUD.UpdateStudentEvaluation(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddFile(int? studentId, int? studentEvaluationId, IEnumerable<HttpPostedFileBase> attachments)
        {
            var check = Data.Security.CheckForStudentEvaluation(studentEvaluationId.Value, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });


            byte[] fileData = null;
            var postedFile = attachments.First();
            using (var binaryReader = new BinaryReader(postedFile.InputStream))
            {
                fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
            }

            var document = Data.CRUD.CreateStudentEvaluationDocument(
                studentId.Value, studentEvaluationId.Value, Helpers.Misc.GetFileNameFromPath(postedFile.FileName), fileData, SessionItems.CurrentUser.UserId);
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
