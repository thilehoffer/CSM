using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CaseloadManager.Helpers;

namespace CaseloadManager.Models
{
    public class StudentParentModel
    {
        public int StudentParentId { get; set; }
        public int StudentId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Relationship to studnet")]
        public string Relationship { get; set; }

        [Display(Name = "Preferred Contact Method")]
        public string PreferredContactMethod { get; set; }

        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }

    public class EditStudentParentModel : Model.Base
    {
        //For display only
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public bool ShowCancelButton { get; set; }

        public IEnumerable<SelectListItem> PreferredContactMethodOptions
        {
            get
            {
                return new SelectListItem[] { new SelectListItem { Value = "", Text = "" }, new SelectListItem { Value = "Email", Text = "Email" }, new System.Web.Mvc.SelectListItem { Value = "Phone", Text = "Phone" } } as IEnumerable<SelectListItem>;
            }
        }

        public int StudentParentId { get; set; }
        public int StudentId { get; set; }

        [Display(Name = "First Name")] 
        public string FirstName { get; set; }


        [Display(Name = "Last Name")] 
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")] 
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")] 
        [DisplayFormat(DataFormatString = "{###-###-####}", ApplyFormatInEditMode = true)]
        public string Phone { get; set; }

        [Display(Name = "Relationship to student")]
        public string Relationship { get; set; }

        
        [Display(Name = "Preferred Contact Method")]
        public string PreferredContactMethod { get; set; }

        public override void DoValidation()
        {
            if (string.IsNullOrEmpty(this.FirstName))
                this.ValidationErrors.Add("First Name is required");

            if (string.IsNullOrEmpty(this.LastName))
                this.ValidationErrors.Add("Last Name is required");

            if (!string.IsNullOrEmpty(this.Email))
            {
                if (!Helpers.Validation.IsValidEmail(this.Email))
                    this.ValidationErrors.Add("Email Address is not valid");
            }

            if (!string.IsNullOrEmpty(this.Phone))
            {
                if (!Helpers.Validation.IsValidPhone(this.Phone.RemoveNonNumericCharacters()))
                    this.ValidationErrors.Add("Phone is not valid. 10 digits are required.");
            }
            if (!string.IsNullOrEmpty(this.PreferredContactMethod))
            {
                if (this.PreferredContactMethod.ToLower().Trim() == "email")
                {
                    if (string.IsNullOrEmpty(this.Email))
                        this.ValidationErrors.Add("Primary contact method is email, so email is required");
                }
                if (this.PreferredContactMethod.ToLower().Trim() == "phone")
                {
                    if (string.IsNullOrEmpty(this.Phone))
                        this.ValidationErrors.Add("Primary contact method is phone, so phone is required");
                }
            }

            if (string.IsNullOrEmpty(this.Relationship))
                this.ValidationErrors.Add("Relationship is required");


            base.DoValidation();
        }
      
    }
    
    
    
}

