﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [Authorize (Roles="Admin")]
         
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
