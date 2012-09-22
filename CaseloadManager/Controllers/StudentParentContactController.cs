using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaseloadManager.Helpers;
using System.IO;

namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class StudentParentContactController : Controller
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



            if (!Data.Queries.DoesStudentHaveParents(id))
            {
                return PartialView("_NoParents", Data.Objects.GetStudent(id));
            }
            if (!Data.Queries.DoesStudentHaveParentContacts(id))
            {
                var model = Data.Objects.GetCreateStudentParentModel(id);
                model.ShowCancelButton = false;
                return PartialView("_Create", model);
            }

            return PartialView("_index", Data.Objects.GetStudent(id));

        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateGet(int id)
        {
            var check = Data.Security.CheckForStudent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            var model = Data.Objects.GetCreateStudentParentModel(id);
            model.ShowCancelButton = true;
            return PartialView("_Create", model); 
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditGet(int id)
        {
            var check = Data.Security.CheckForStudentParentContact(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new[] { "" }, success = false });

            return PartialView("_Edit", Data.Objects.GetEditStudentParentModel(id));
        }

        [HttpPost]
        [Authorize]
        public string GetNotes(int id)
        {
            return Data.Objects.GetStudentParentContactNotes(id);
        }

       
        [Authorize]
        [HttpPost]
        public ActionResult Create(Models.StudentParentContactModelDataCRUD model)
        {
            var check = Data.Security.CheckForStudent(model.StudentId, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new[] { "" }, success = false });

            model.DoValidation();
            if (!model.IsValid())
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = model.ValidationErrors.ToArray(), success = false });
             
            Data.CRUD.CreateStudentParentContact(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new[] { "" }, success = true });

        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Models.StudentParentContactModelDataCRUD model)
        {
            var check = Data.Security.CheckForStudent(model.StudentId, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new[] { "" }, success = false });

            model.DoValidation();
            if (!model.IsValid())
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = model.ValidationErrors.ToArray(), success = false });

            Data.CRUD.UpdateStudentParentContact(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new[] { "" }, success = true });

        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteGet(int id)
        {
            var check = Data.Security.CheckForStudentParentContact(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            return PartialView("_Delete", Data.Objects.GetStudentParentContact(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var check = Data.Security.CheckForStudentParentContact(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });

            Data.CRUD.DeleteStudentParentContact(id);

            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }


       

        [Authorize]
        [HttpPost]
        public ActionResult AddFile(int? studentId, int? studentParentContactId, IEnumerable<HttpPostedFileBase> attachments)
        {
            Debug.Assert(studentParentContactId != null, "studentParentContactId != null");
            Debug.Assert(studentId != null, "studentId != null");
            Debug.Assert(Request != null, "Request != null");
            Debug.Assert(attachments != null, "attachments != null");
            var files = attachments.ToList();
            Debug.Assert(files.Any(), "attachments contains at leas one file");

            var check = Data.Security.CheckForStudentParentContact(studentParentContactId.Value, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new[] { "" }, success = false });

            byte[] fileData;
            var postedFile = files.First();
            using (var binaryReader = new BinaryReader(postedFile.InputStream))
            {
                var httpPostedFileBase = Request.Files[0];
                Debug.Assert(httpPostedFileBase != null, "Posted File != null");
                fileData = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
            }
            
            var document = Data.CRUD.CreateStudentParentContactDocument(
                studentId.Value, studentParentContactId.Value, Misc.GetFileNameFromPath(postedFile.FileName), fileData, SessionItems.CurrentUser.UserId);
            
            return Json(new { studentDocumentId = document.StudentDocumentId }, "text/plain");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AttachmentsGet(int id)
        {
            var check = Data.Security.CheckForStudentParentContact(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");
            return PartialView("_Documents", Data.Objects.GetStudentParentContact(id));
        }
    }
}
