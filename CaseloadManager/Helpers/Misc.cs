using System;
using System.Diagnostics;

namespace CaseloadManager.Helpers
{
    public class Misc
    {
        public static string GetMimeType(string fileName)
        {
            var mime = "application/octetstream";


            var extension = System.IO.Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(extension))
            {
                var ext = extension.ToLower();
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (rk != null && rk.GetValue("Content Type") != null)
                    mime = rk.GetValue("Content Type").ToString();
            }
            return mime;
        }

        public static string GetFileNameFromPath(string path)
        {
            try
            {
                return path.Substring(
                   path.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1);
            }
            catch
            {
                // error  
                return string.Empty;
            }

        }

        public static DateTime CombineDateAndTime(DateTime date, DateTime time)
        {
            return new DateTime(
           date.Year,
           date.Month,
           date.Day,
           time.Hour,
           time.Minute,
           time.Second);
        }

        public static DateTime CombineDateAndTime(DateTime date, DateTime? time)
        {
            if (time.HasValue)
                return CombineDateAndTime(date, time.Value);
            else
                return date;
        }

        public static DateTime? CombineDateAndTime(DateTime? date, DateTime? time)
        {
            if (!time.HasValue)
            {
                return date;
            }
            Debug.Assert(date != null, "date != null");
            return CombineDateAndTime(date.Value, time);

        }
    }
}