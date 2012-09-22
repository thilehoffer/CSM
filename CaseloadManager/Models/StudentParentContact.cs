using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CaseloadManager.Data;

namespace CaseloadManager.Models
{
    public class StudentParentContactModel
    {
        public int StudentId { get; set; }
        public int? StudentParentContactId { get; set; }
        public string StudentName { get; set; }
        public string ParentName { get; set; }
        public DateTime? DateTimeOfContact { get; set; } 

        [Display(Name = "Contact Made")]
        public Boolean ContactMade { get; set; }

        [Display(Name = "Contact Notes")]
        public string ContactNotes { get; set; }

        private List<Models.DocumentInfo> _attachments;
        public List<Models.DocumentInfo>GetAttachments()
        {
            if (_attachments == null)
                _attachments = Data.Lists.GetStudentParentContactDocuments(StudentParentContactId.Value);

            return _attachments;
        }
    }

    public class StudentParentContactModelDataCRUD : Models.Model.Base
    {
        public int StudentId { get; set; }
        public int? StudentParentContactId { get; set; }
        public string StudentName { get; set; }
        public bool ShowCancelButton { get; set; }

        [Display(Name = "Date of Contact")]
        public DateTime? DateOfContact { get; set; }

        [Display(Name = "Time of Contact")]
        public DateTime? TimeOfContact { get; set; }

        [Display(Name = "Contact Made")]
        public Boolean ContactMade { get; set; }

        [Display(Name = "Contact Notes")]
        public string ContactNotes { get; set; }

        [Display(Name = "Parent")]
        public int? StudentParentId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> StudentParents { get; set; }

        public override void DoValidation()
        {
            if (!this.DateOfContact.HasValue)
                this.ValidationErrors.Add("Date of Contact is required");

            if (!this.TimeOfContact.HasValue)
                this.ValidationErrors.Add("Time of Contact is required");

            if (!this.StudentParentId.HasValue)
                this.ValidationErrors.Add("Parent is required");


            base.DoValidation();
        }

    }

    public class StudentParentContactIndexModel
    {
        public int StudentParentContactId { get; set; }

        public DateTime DateOfContact { get; set; }

        public Boolean ContactMade { get; set; }

        public string ContactNotes { get; set; }

        public string ParentName { get; set; }
    }
}