using System.Diagnostics;
using System.Web.Mvc;
using CaseloadManager.Helpers;
using CaseloadManager.Models;

namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class StudentParentController : Controller
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

            var exsiting = Data.Queries.DoesStudentHaveParents(id);
            var student = Data.Objects.GetStudent(id);
            Debug.Assert(student.StudentId != null, "student.StudentId != null");
            if (exsiting)
                return (PartialView("_Index", student));
            

            var model = new EditStudentParentModel { StudentId = student.StudentId.Value, StudentFirstName = student.FirstName, StudentLastName = student.LastName, ShowCancelButton = false };
            return PartialView("_Create", model);
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
            Debug.Assert(student.StudentId != null, "student.StudentId != null");
            var model = new EditStudentParentModel {StudentId = student.StudentId.Value, StudentFirstName = student.FirstName, StudentLastName = student.LastName, ShowCancelButton = true };
            
            return PartialView("_Create", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditGet(int id)
        {
            var studentParent = Data.Objects.GetStudentParentWStudent(id);

            var check = Data.Security.CheckForStudent(studentParent.StudentId, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            var model = new EditStudentParentModel
            {
                Email = studentParent.Email,
                FirstName = studentParent.FirstName,
                LastName = studentParent.LastName,
                Phone = studentParent.Phone,
                PreferredContactMethod = studentParent.PreferredContactMethod,
                Relationship = studentParent.Relationship,
                StudentFirstName = studentParent.Student.FirstName,
                StudentId = studentParent.StudentId,
                StudentLastName = studentParent.Student.LastName,
                StudentParentId = studentParent.StudentParentId
            };

            return PartialView("_Edit", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditStudentParentModel model)
        {
            var check = Data.Security.CheckForStudent(model.StudentId, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new[] { "" }, success = false });

            model.DoValidation();
            if (!model.IsValid())
            {
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            }
            Data.CRUD.UpdateStudentParent(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(EditStudentParentModel model)
        {
            var check = Data.Security.CheckForStudent(model.StudentId, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new[] { "" }, success = false });

            model.DoValidation();
            if (!model.IsValid())
            {
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            }
            Data.CRUD.CreateStudentParent(model, SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteGet(int id)
        {
            var check = Data.Security.CheckForStudentParent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return PartialView("NotFound");

            if (!check.HasAccess)
                return PartialView("NoAccess");

            return PartialView("_Delete", Data.Objects.GetStudentParentWStudent(id));
        }


        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var check = Data.Security.CheckForStudentParent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new[] { "" }, success = false });

            Data.CRUD.DeleteStudentParent(id);
            return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });

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
