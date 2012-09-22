using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace CaseloadManager.Models
{
    public class StudentEvaluationModel : Model.Base
    {
        public int? StudentEvaluationId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }


        [Display(Name = "Evaluation Due On")]
        public DateTime? ScheduledDate { get; set; }

        [Display(Name = "Evalution Completed On")]
        public DateTime? DateCompleted { get; set; }

        private List<DocumentInfo> _documents;
        public List<DocumentInfo> GetEvaluationDocuments()
        {
            if (_documents == null)
            {
                if (StudentEvaluationId.HasValue)
                    _documents = Data.Lists.GetStudentEvaluationDocuments(StudentEvaluationId.Value);
            }
            return _documents;
        }

        public bool IsComplete
        {
            get { return DateCompleted.HasValue; }
        }

        public override void DoValidation()
        {
            if (!ScheduledDate.HasValue)
            {
                ValidationErrors.Add("Evaluation Due On is required");
            }
            base.DoValidation();
        }
    }
}