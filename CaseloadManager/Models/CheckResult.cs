using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseloadManager.Models
{
    public class CheckResult
    {
        public bool Exists { get; set; }
        public bool HasAccess { get; set; }
    }
}