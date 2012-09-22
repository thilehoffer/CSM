using System.Web.Mvc;
using CaseloadManager.Helpers; 

namespace CaseloadManager.Controllers.Admin
{
    public class DisabilityCategoryController : Controller
    {
        

        //
        // GET: /DisabilityCategory/

        [Authorize(Roles = "Admin")]
        public ViewResult Index()
        {
            return View(Data.Lists.GetAllDisabilityCategories());
        }

        //
        // GET: /DisabilityCategory/Details/5
        [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            return View(Data.Objects.GetDisabilityCategory(id));
        }

        //
        // GET: /DisabilityCategory/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DisabilityCategory/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Models.DisabilityCategoryModel disabilitycategory)
        {
             
                if (Data.Queries.DoesDisabilityCategoryAlreadyExists(disabilitycategory.Description))
                {
                    return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new[] { disabilitycategory.Description  + " already exists" }, success = true });
                }

                disabilitycategory.ID = 0; 
                if (disabilitycategory.IsValid())
                {
                    Data.CRUD.InsertDisabilityCategory(disabilitycategory, SessionItems.CurrentUser.UserId);
                    return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new[]{""}, success = true }); 
                }

            return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = disabilitycategory.ValidationErrors.ToArray(), success = false });
       

            
        }

        //
        // GET: /DisabilityCategory/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Models.DisabilityCategoryModel disabilitycategory = Data.Objects.GetDisabilityCategory(id);
            return View(disabilitycategory);
        }

        //
        // POST: /DisabilityCategory/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Models.DisabilityCategoryModel disabilitycategory)
        {
            if (disabilitycategory.IsValid())
            {
                Data.CRUD.UpdateDisabilityCategory(disabilitycategory, SessionItems.CurrentUser.UserId);
                return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList = new[] { "" }, success = true });
            }
            return Json(new Helpers.JsonAjaxResult.result { notFound = false, noAccess = false, errorList =  disabilitycategory.ValidationErrors.ToArray(), success = false });
        }

         
        protected override void Dispose(bool disposing)
        {
          
            base.Dispose(disposing);
        }
    }
}