using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.IO;
using CaseloadManager.Helpers;
namespace CaseloadManager.KendoUI.HtmlExtenstions
{
    public static class HtmlExtension
    {

        public static MvcHtmlString ValidationSpan(this HtmlHelper helper, string controlName)
        {
            return new MvcHtmlString(string.Format("<span class='k-invalid-msg' data-for='{0}'></span>", controlName));
        }

 
    }
}