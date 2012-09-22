using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CaseloadManager.Entities;
using CaseloadManager.Models;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
namespace CaseloadManager.Data
{
    public class Objects
    {

        public static StudentIEPModel GetStudentIEP(int studentIepId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentIEPs.Include("Student").Where(s => s.StudentIEPId == studentIepId).Select(a =>
                            new StudentIEPModel
                            {
                                IsComplete = a.IsComplete,
                                DateOfMeeting = a.DateOfMeeting,
                                DateOfMeetingTime = a.DateOfMeeting,
                                ScheduledDate = a.ScheduledDate,
                                ScheduledDateTime = a.ScheduledDate,
                                StudentId = a.StudentId,
                                StudentIEPId = a.StudentIEPId,
                                StudentName = a.Student.FirstName + " " + a.Student.LastName,
                                IsCurrent = a.IsCurrentIEP
                            }).Single();
            }
        }

        public static StudentEvaluationModel GetStudentEvaluation(int studentEvaluationId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentEvaluations.Include("Student").Where(s => s.StudentEvaluationId == studentEvaluationId).Select(a =>
                  new StudentEvaluationModel
                  {
                      ScheduledDate = a.ScheduledDate,
                      StudentId = a.StudentId,
                      StudentEvaluationId = a.StudentEvaluationId,
                      StudentName = a.Student.FirstName + " " + a.Student.LastName,
                      DateCompleted = a.DateCompleted
                  }).Single();
            }
        }

        public static AccountDetailsModel GetAccountDetails(int userId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.vwUserDetails.Where(a => a.UserId == userId).Select(a =>
                    new AccountDetailsModel
                    {
                        UserId = a.UserId,
                        Email = a.Email,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Phone = a.Phone
                    }).Single();
            }
        }

        public static DisabilityCategoryModel GetDisabilityCategory(int disabilityCategoryId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.DisabilityCategories.Where(a => a.ID == disabilityCategoryId).Select(a =>
                 new DisabilityCategoryModel
                 {
                     ID = a.ID,
                     Active = a.Active,
                     Description = a.Description
                 }).Single();

            }

        }

        public static StudentModel GetStudent(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.Students.Include("PrimaryDisabilityCategory").Include("SecondaryDisabilityCategory").Where(a => a.StudentId == studentId).Select(a =>
                                                                                                                                                                new StudentModel
                                                                                                                                                                    {
                                                                                                                                                                        StudentId = a.StudentId,
                                                                                                                                                                        UserId = a.UserId,
                                                                                                                                                                        DateOfBirth = a.DateOfBirth,
                                                                                                                                                                        DateOfEntry = a.DateOfEntry,
                                                                                                                                                                        ExpectedGraduationYear = a.ExpectedGraduationYear,
                                                                                                                                                                        FirstName = a.FirstName,
                                                                                                                                                                        LastName = a.LastName,
                                                                                                                                                                        LocalEducationAgency = a.LocalEducationAgency,
                                                                                                                                                                        PrimaryDisabilityId = a.PrimaryDisabilityID,
                                                                                                                                                                        SecondaryDisabilityId = a.SecondaryDisabilityID,
                                                                                                                                                                        PrimaryDisabilty = a.PrimaryDisabilityCategory.Description,
                                                                                                                                                                        CurrentStudent = a.Current
                                                                                                                                                                    }).SingleOrDefault();

            }
        }

        public static StudentDocument GetStudentDocument(int studentDocumentId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentDocuments.Single(a => a.StudentDocumentId == studentDocumentId);
            }
        }

        public static StudentParentContactModelDataCRUD GetCreateStudentParentModel(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                var student = (from s in context.Students.Include("StudentParents")
                               where s.StudentId == studentId
                               select s).Single();

                var result = new StudentParentContactModelDataCRUD
                {
                    ContactMade = false,
                    ContactNotes = string.Empty,
                    StudentName = student.FirstName + " " + student.LastName,
                    StudentId = studentId
                };

                var parentList =
                    student.StudentParents.Select(
                        a =>
                        new SelectListItem
                            {
                                Text = a.FirstName + " " + a.LastName,
                                Value = a.StudentParentId.ToString(CultureInfo.InvariantCulture)
                            }).ToList(); 

                parentList.Insert(0, new SelectListItem { Text = "", Value = "" });
                result.StudentParents = parentList.AsEnumerable();

                return result;

            }
        }

        public static StudentParentContactModelDataCRUD GetEditStudentParentModel(int studentParentContactId)
        {
            using (var context = DataContext.GetContext())
            {
                var result = context.StudentParentContacts.Include("StudentParent").Include("Student").Where(a => a.StudentParentContactId == studentParentContactId).Select(a =>
                    new StudentParentContactModelDataCRUD
                    {
                        ContactMade = a.ContactMade,
                        ContactNotes = a.ContactNotes,
                        DateOfContact = a.ContactTime,
                        TimeOfContact = a.ContactTime,
                        ShowCancelButton = true,
                        StudentId = a.StudentParent.Student.StudentId,
                        StudentName = a.StudentParent.Student.FirstName + " " + a.StudentParent.Student.LastName,
                        StudentParentContactId = a.StudentParentContactId,
                        StudentParentId = a.StudentParentId
                    }).Single();

                var parentList = new List<SelectListItem>();
                context.StudentParents.Where(a => a.StudentId == result.StudentId).ToList().ForEach(a => parentList.Add(new SelectListItem { Text = a.FirstName + " " + a.LastName, Value = a.StudentParentId.ToString(CultureInfo.InvariantCulture) }));
                parentList.Insert(0, new SelectListItem { Text = "", Value = "" });
                result.StudentParents = parentList.AsEnumerable();
                return result;
            }
        }

        public static StudentParent GetStudentParentWStudent(int studentParentId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentParents.Include("Student").Single(a => a.StudentParentId == studentParentId);
            }
        }

        public static string GetStudentParentContactNotes(int studentParentContactId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentParentContacts.Single(a => a.StudentParentContactId == studentParentContactId).ContactNotes;
            }
        }

        public static StudentIEP GetIepWithStudent(int studentIepId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentIEPs.Include("Student").Single(a => a.StudentIEPId == studentIepId);
            }
        }

        public static StudentEvaluation GetEvaluationWstudent(int studentEvaluationId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentEvaluations.Include("Student").Single(a => a.StudentEvaluationId == studentEvaluationId);
            }
        }

        public static StudentParentContactModel GetStudentParentContact(int studentParentContactId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentParentContacts.Include("StudentParent").Include("Student").Where(a => a.StudentParentContactId == studentParentContactId).Select(a => new
                 StudentParentContactModel
                 {
                     ContactMade = a.ContactMade,
                     ContactNotes = a.ContactNotes,
                     DateTimeOfContact = a.ContactTime,
                     ParentName = a.StudentParent.FirstName + " " + a.StudentParent.LastName,
                     StudentId = a.StudentParent.StudentId,
                     StudentName = a.StudentParent.Student.FirstName + a.StudentParent.Student.LastName,
                     StudentParentContactId = a.StudentParentContactId
                 }).Single();
            }
        }
    }
}