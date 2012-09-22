using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace CaseloadManager.Helpers
{
    public class Validation
    {
        public static bool IsValidEmail(string email)
        {
            var match = Regex.Match(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
	        RegexOptions.IgnoreCase);

            return match.Success;
            
        }

        public static bool IsValidPhone(string phone)
        {
            if (phone.Length != 10)
                return false;

            var match = Regex.Match(phone, @"\d{10}",
	        RegexOptions.IgnoreCase);

             return match.Success;
        }


        
    }
}