using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CaseloadManager.Models
{
    public class DisabilityCategoryModel : Model.Base
    {

        public int ID { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public override void DoValidation()
        {
            if (string.IsNullOrEmpty(Description))
                this.ValidationErrors.Add("Description is required");
            base.DoValidation();
        }
    }
}