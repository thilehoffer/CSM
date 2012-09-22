using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace CaseloadManager.Models
{

    public class Student
    {

        private List<object> _categories;
        private List<string> _years;

        public IEnumerable<object> Categories
        {
            get
            {
                //Resturns Disability categories from the database
                if (_categories == null)
                {
                    _categories = new List<object>();
                    _categories.Add(new { ID = "", Description = "" });
                    _categories.AddRange(DataLayer.GetActiveDisabilityCategories().Select(m => new { ID = m.ID, Description = m.Description }));
                }
                return _categories as IEnumerable<object>;
            }
        }
        public IEnumerable<string> Years
        {
            get
            {
                //Returns a an IEnumberable of years from now through 20 years in the future.
                if (_years == null)
                {
                    _years = new List<string>();
                    _years.Add(string.Empty);
                    int theYear = DateTime.Now.Year;
                    while (theYear <= (DateTime.Now.Year + 20))
                    {
                        _years.Add(theYear.ToString());
                        theYear++;
                    }
                }
                return _years as IEnumerable<string>;
            }
        }

        public Guid? StudentId { get; set; }
        public Guid UserID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(256, ErrorMessage = "Maximum number of characters is 256")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(256, ErrorMessage = "Maximum number of characters is 256")]
        public string LastName { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Date Of Birth is required")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Expected Graduation Year")]
        [Required(ErrorMessage = "Expected Graduation Year is required")]
        public int ExpectedGraduationYear { get; set; }

        [Required(ErrorMessage = "Disability Category is require")]
        [Display(Name = "Disability Category")]
        public int PrimaryDisabilityId { get; set; }

        [Display(Name = "Additional Disability Category")]
        public int? SecondaryDisabilityId { get; set; }
        

        [Display(ShortName="LEA", Name="Local Education Agency")]
        [Required(ErrorMessage = "LEA is required")]
        [StringLength(256, ErrorMessage = "Maximum number of characters is 256")]
        public string LocalEducationAgency { get; set; }

        [Required(ErrorMessage="IEP Date is required")]
        [Display(Name = "IEP Date")]
        [DataType(DataType.DateTime)]
        public DateTime? IEPDate { get; set; }

        [Display(Name = "Next IEP Due On")]
        [DataType(DataType.DateTime)]
        public DateTime? NextIEPDueDate { get; set; }

        [Display(Name = "Next Evaluation Due On")]
        [DataType(DataType.DateTime)]
        public DateTime? NextEvaluationDueOn   { get; set; }

        [Display(Name = "Date Of Entry")]
        [DataType(DataType.DateTime)]
        public DateTime? DateOfEntry   { get; set; }

    }

}