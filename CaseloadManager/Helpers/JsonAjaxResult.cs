using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseloadManager.Helpers.JsonAjaxResult
{
    [Serializable]
    public class result
    {
        public bool success { get; set; }
        public string[] errorList { get; set; }
        public bool notFound { get; set; }
        public bool noAccess { get; set; }


        public result() { }
        

    }

    [Serializable]
    public class DropdownDataSource
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}