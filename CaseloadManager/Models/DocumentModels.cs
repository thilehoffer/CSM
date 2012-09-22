using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseloadManager.Models
{
    public enum AttachmentType
    {
        IEP,
        Evaluation,
        ParentContact,
        None
    }

     public class DocumentInfo
     {
        public int StudentDocumentId {get; set;}
         public int StudentId {get; set; }
         public string Name {get; set; }
         public int? StudentIEPId {get; set; }
         public int? StudentEvaluationId {get; set;}
         public int? StudentParentContactId { get; set; }
         public DateTime CreatedOn { get; set; }
         public int CreatedBy { get; set; }
         public AttachmentType AttachmentType
         {
             get 
             {
                 if (StudentIEPId.HasValue)
                     return Models.AttachmentType.IEP;

                 if (StudentEvaluationId.HasValue)
                     return Models.AttachmentType.Evaluation;

                 if (StudentParentContactId.HasValue)
                     return Models.AttachmentType.ParentContact;

                 return Models.AttachmentType.None;
             }
         }
     }
}