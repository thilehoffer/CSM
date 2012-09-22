using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseloadManager.Models 
{ 
  
    [Serializable]
    public class UserKey
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
    } 

}