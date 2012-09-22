using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CaseloadManager.Models
{

    public class StudentModel : Model.Base
    {
        #region Lookup Lists

        private List<SelectListItem> _years;
        public IEnumerable<SelectListItem> Years
        {
            get
            {
                if (_years == null)
                {
                    _years = new List<SelectListItem>();
                    int year = DateTime.Now.AddYears(-2).Year;
                    int stop = year + 12;
                    _years.Add(new SelectListItem { Value = string.Empty, Text = string.Empty });
                    while (year <= stop)
                    {
                        _years.Add(new SelectListItem { Value = year.ToString(CultureInfo.InvariantCulture), Text = year.ToString(CultureInfo.InvariantCulture) });
                        year++;
                    }
                }
                return _years;
            }

        }

        private List<SelectListItem> _disabilityCategories;
        public IEnumerable<SelectListItem> DisabilityCategories
        {
            get
            {
                if (_disabilityCategories == null)
                {
                    _disabilityCategories = Data.Lists.DisabilityCategories(true);
                }
                return _disabilityCategories;
            }
        }

        private List<SelectListItem> _currentStudentOptions;
        public IEnumerable<SelectListItem> CurrentStudentOptions
        {
            get
            {
                if (_currentStudentOptions == null)
                {
                    _currentStudentOptions = new List<SelectListItem>
                                                 {
                                                     new SelectListItem {Text = "Yes", Value = true.ToString()},
                                                     new SelectListItem {Text = "No", Value = false.ToString()}
                                                 };
                }
                return _currentStudentOptions;
            }

        }


        #endregion

        #region Fields

        public int? StudentId { get; set; }
        public int? UserId { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }


        [Display(Name = "Expected Graduation Year")]
        public int? ExpectedGraduationYear { get; set; }


        [Display(Name = "Disability Category")]
        public int? PrimaryDisabilityId { get; set; }

        [Display(Name = "Additonal Category")]
        public int? SecondaryDisabilityId { get; set; }




        [Required]
        [Display(Name = "Local Education Agency")]
        public string LocalEducationAgency { get; set; }

        [Display(Name = "Date Of Entry")]
        public DateTime? DateOfEntry { get; set; }

        [Display(Name = "Is Current Student")]
        public Boolean CurrentStudent { get; set; }

        //For display only don't need these returned from the client
        public string PrimaryDisabilty { get; set; }
        public string SecondaryDisabilty { get; set; }


        #endregion

        public Helpers.Enums.StudetDetailsTypeEnum StudetDetailsType { get; set; }

        public override void DoValidation()
        {
            if (!UserId.HasValue)
                ValidationErrors.Add("User Id is required");

            if (string.IsNullOrEmpty(FirstName))
                ValidationErrors.Add("First Name is required");
            else
                if (FirstName.Length > 256)
                    ValidationErrors.Add("First Name can not be more than 256 characters");

            if (string.IsNullOrEmpty(LastName))
                ValidationErrors.Add("Last Name is required");
            else
                if (LastName.Length > 256)
                    ValidationErrors.Add("Last Name can not be more than 256 characters");

            if (!DateOfBirth.HasValue)
                ValidationErrors.Add("Date of birth is required");
            else
                if (DateOfBirth.Value > DateTime.Now.AddYears(-3))
                    ValidationErrors.Add("Student's date of birth must be before " + DateTime.Now.AddYears(-3).ToShortDateString());

            if (!ExpectedGraduationYear.HasValue)
                ValidationErrors.Add("Expected Graduation Year is required");

            if (!PrimaryDisabilityId.HasValue)
                ValidationErrors.Add("Primary Disability is required");

            base.DoValidation();
        }

        #region Child Objects

        public List<StudentIEPModel> GetIeps()
        {
            Debug.Assert(StudentId != null, "StudentId != null");
            return Data.Lists.GetStudentIEPs(StudentId.Value).OrderByDescending(a => a.ScheduledDate).ToList();
        }

        public List<StudentEvaluationModel> GetEvaluations()
        {
            Debug.Assert(StudentId != null, "StudentId != null");
            return Data.Lists.GetStudentEvaluations(StudentId.Value).OrderByDescending(a => a.ScheduledDate).ToList();
        }

        public List<StudentParentModel> GetStudentParents()
        {
            Debug.Assert(StudentId != null, "StudentId != null");
            return Data.Lists.GetStudentParents(StudentId.Value).OrderBy(a => a.FullName).ToList();
        }

        public List<StudentParentContactIndexModel> GetStudentParentContacts()
        {
            Debug.Assert(StudentId != null, "StudentId != null");
            return Data.Lists.GetStudentParentContacts(StudentId.Value);
        }

        #endregion

    }

    public class StudentListItemModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExpectedGraduationYear { get; set; }
        public string PrimaryDisability { get; set; }
        public string LocalEducationAgency { get; set; }
        public int StudentId { get; set; }
        public Boolean CurrentStudent { get; set; }
        public string CurrentCheckBox
        {
            get
            {
                return CurrentStudent ? @"checked=""checked""" : "";
            }
        }

    }



}