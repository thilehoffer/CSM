using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaseloadManager.Data;
using CaseloadManager.Helpers;
using System.IO;
using System.Web.Script.Serialization;
using CaseloadManager.Models;


namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        [Authorize]
        public ActionResult Index()
        {
            return View("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(new Models.StudentModel());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Models.StudentModel model)
        {
            model.UserId = Helpers.SessionItems.CurrentUser.UserId;
            model.DoValidation();
            if (!model.IsValid())
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            else
            {
                model.UserId = SessionItems.CurrentUser.UserId;
                Data.CRUD.InsertStudent(model, SessionItems.CurrentUser.UserId);
                return Json(new Helpers.JsonAjaxResult.result { errorList = null, success = true });
            }
            
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var check = Data.Security.CheckForStudent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            var student = Data.Objects.GetStudent(id);
            student.StudetDetailsType = Helpers.Enums.StudetDetailsTypeEnum.BasicInfo;
            return View("Details", student);
        }

        //This is the details section selected from the menu
        #region partial views

        #region demographics
        [Authorize]
        [HttpPost]
        public ActionResult BasicInfoGet(int id)
        {
            var check = Data.Security.CheckForStudent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return View("NotFound");
            if (!check.HasAccess)
                return View("NoAccess");

            var model = Data.Objects.GetStudent(id);
            //The details type will decide which patial view is loaded
            model.StudetDetailsType = Helpers.Enums.StudetDetailsTypeEnum.BasicInfo;
            return PartialView("_BasicInfoPartial", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BasicInfoPost(Models.StudentModel model)
        {
            var check = Data.Security.CheckForStudent(model.StudentId.Value, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });

            model.UserId = Helpers.SessionItems.CurrentUser.UserId;
            if (!model.IsValid())
            {
                return Json(new Helpers.JsonAjaxResult.result { errorList = model.ValidationErrors.ToArray(), success = false });
            }
            else
            {
                Data.CRUD.UpdateStudent(model, Helpers.SessionItems.CurrentUser.UserId);
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new string[] { "" }, success = true });
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult SetCurrent(int id, bool current)
        {

            var check = Data.Security.CheckForStudent(id, SessionItems.CurrentUser.UserId);
            if (!check.Exists)
                return Json(new Helpers.JsonAjaxResult.result { notFound = true, noAccess = false, errorList = new string[] { "" }, success = false });

            if (!check.HasAccess)
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = true, errorList = new string[] { "" }, success = false });

            Data.CRUD.UpdateStudent(id, current, Helpers.SessionItems.CurrentUser.UserId);
            return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new string[] { "" }, success = true });
         
        }
        #endregion

        #region IEP
        #endregion


        #region Evaluation


        #endregion

        public ActionResult NoAccess()
        {
            return (View());
        }

        public ActionResult NotFound()
        {
            return (View());
        }

        #endregion
    }
}
