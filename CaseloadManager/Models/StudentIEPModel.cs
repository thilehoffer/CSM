using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CaseloadManager.Models
{
    public class StudentIEPModel : Model.Base
    {
        public int? StudentIEPId { get; set; }
        public int StudentId { get; set; }
        public int? StudentDocumentId { get; set; }
        public string StudentName { get; set; }


        [Display(Name = "Scheduled Date")]
        public DateTime? ScheduledDate { get; set; }

        [Display(Name = "Scheduled Time")]
        public DateTime? ScheduledDateTime { get; set; }


        [Display(Name = "Meeting Held On Date")]
        public DateTime? DateOfMeeting { get; set; }

        [Display(Name = "Meeting Held On Time")]
        public DateTime? DateOfMeetingTime { get; set; }


        [Display(Name = "Completed")]
        public bool IsComplete { get; set; }

        [Display(Name = "Current IEP")]
        public bool IsCurrent { get; set; }

        private List<DocumentInfo> _documents;
        public List<DocumentInfo> GetIepDocuments()
        { 
            if (_documents == null)
            {
                Debug.Assert(StudentIEPId != null, "StudentIEPId != null");
                _documents = Data.Lists.GetStudentIEPDocuments(StudentIEPId.Value);
            }

            return _documents;
        }
        


        public override void DoValidation()
        {
            if (!ScheduledDate.HasValue)
                ValidationErrors.Add("Scheduled Date is required");

            if (!ScheduledDateTime.HasValue)
                ValidationErrors.Add("Scheduled Time is required");


            if (DateOfMeetingTime.HasValue && !DateOfMeeting.HasValue)
            {
                ValidationErrors.Add("The date of the meeting is required because the time was set.");
            }

            if (IsCurrent && Data.Queries.DoesStudentHaveAnotherCurrentIEP(StudentId, StudentIEPId))
            {
                ValidationErrors.Add("This student already has current IEP. Uncheck Current IEP");
            } 
            base.DoValidation();
        }

        
    }

    public class AttachmentData
    {
        public int StudentId {get; set;}
        public int StudentIepId { get; set; }
    }
}