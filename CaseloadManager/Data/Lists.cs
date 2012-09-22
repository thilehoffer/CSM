using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CaseloadManager.Models;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace CaseloadManager.Data
{
    public class Lists
    {
        public static List<SelectListItem> DisabilityCategories(bool includeEmpty)
        {
            var result = new List<SelectListItem>();
            if (includeEmpty)
                result.Add(new SelectListItem { Text = string.Empty, Value = string.Empty });

            using (var context = DataContext.GetContext())
            {
                result.AddRange(context.DisabilityCategories.Select(item => new SelectListItem { Text = item.Description, Value = SqlFunctions.StringConvert((double)item.ID).Trim() }));
            }
            return result;

        }

        public static List<DisabilityCategoryModel> GetActiveDisabilityCategories()
        {
            using (var context = DataContext.GetContext())
            {
                return
                    context.DisabilityCategories.Where(a => a.Active).Select(
                        item =>
                        new DisabilityCategoryModel { ID = item.ID, Description = item.Description, Active = item.Active }).ToList();
            }
        }

        public static List<DisabilityCategoryModel> GetAllDisabilityCategories()
        {
            using (var context = DataContext.GetContext())
            {
                return
                    context.DisabilityCategories.Select(
                        item =>
                        new DisabilityCategoryModel { ID = item.ID, Description = item.Description, Active = item.Active }).ToList();
            }
        }

        public static List<StudentListItemModel> GetStudentList(int userId, bool includePrevious)
        {
            using (var context = DataContext.GetContext())
            {
                if (includePrevious)
                {
                    return context.Students.Include("PrimaryDisabilityCategory").Where(a => a.UserId == userId).Select(a => new
                    StudentListItemModel
                    {
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        ExpectedGraduationYear = a.ExpectedGraduationYear,
                        LocalEducationAgency = a.LocalEducationAgency,
                        PrimaryDisability = a.PrimaryDisabilityCategory.Description,
                        StudentId = a.StudentId,
                        CurrentStudent = a.Current
                    }).ToList();
                }
                return context.Students.Include("PrimaryDisabilityCategory").Where(a => a.UserId == userId && a.Current).Select(a => new
                                                                                                                                                 StudentListItemModel
                                                                                                                                                 {
                                                                                                                                                     FirstName = a.FirstName,
                                                                                                                                                     LastName = a.LastName,
                                                                                                                                                     ExpectedGraduationYear = a.ExpectedGraduationYear,
                                                                                                                                                     LocalEducationAgency = a.LocalEducationAgency,
                                                                                                                                                     PrimaryDisability = a.PrimaryDisabilityCategory.Description,
                                                                                                                                                     StudentId = a.StudentId,
                                                                                                                                                     CurrentStudent = a.Current
                                                                                                                                                 }).ToList();
            }
        }

        public static List<StudentIEPModel> GetStudentIEPs(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                var result = (from s in context.StudentIEPs.Include("Student")
                              where s.StudentId == studentId
                              select new StudentIEPModel
                              {
                                  IsComplete = s.IsComplete,
                                  DateOfMeeting = s.DateOfMeeting,
                                  DateOfMeetingTime = s.DateOfMeeting,
                                  ScheduledDate = s.ScheduledDate,
                                  ScheduledDateTime = s.ScheduledDate,
                                  StudentId = s.StudentId,
                                  StudentIEPId = s.StudentIEPId,
                                  StudentName = s.Student.FirstName + " " + s.Student.LastName,
                                  IsCurrent = s.IsCurrentIEP
                              }).ToList();

                return result;
            }
        }

        public static List<StudentParentModel> GetStudentParents(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                var dataItems = context.StudentParents.Where(a => a.StudentId == studentId).ToList();
                return dataItems.Select(item => new StudentParentModel { StudentId = item.StudentId, StudentParentId = item.StudentParentId, Email = item.Email, FirstName = item.FirstName, LastName = item.LastName, Phone = item.Phone, PreferredContactMethod = item.PreferredContactMethod, Relationship = item.Relationship }).ToList();
            }


        }

        public static List<StudentEvaluationModel> GetStudentEvaluations(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                return (from s in context.StudentEvaluations.Include("Student")
                        where s.StudentId == studentId
                        select new StudentEvaluationModel
                        {
                            StudentEvaluationId = s.StudentEvaluationId,
                            DateCompleted = s.DateCompleted,
                            ScheduledDate = s.ScheduledDate,
                            StudentId = s.StudentId,
                            StudentName = s.Student.FirstName + " " + s.Student.LastName
                        }).ToList();
            }
        }

        public static List<DocumentInfo> GetStudentIEPDocuments(int studentIEPId)
        {
            using (var context = DataContext.GetContext())
            {

                return context.StudentDocuments.Where(a => a.StudentIEPId == studentIEPId).Select(a =>
                        new DocumentInfo
                        {
                            Name = a.Name,
                            StudentDocumentId = a.StudentDocumentId,
                            StudentEvaluationId = a.StudentEvaluationId,
                            StudentId = a.StudentId,
                            StudentParentContactId = a.StudentParentContactId,
                            StudentIEPId = a.StudentIEPId,
                            CreatedOn = a.CreatedOn,
                            CreatedBy = a.CreatedBy
                        }
                        ).ToList();
            }
        }

        public static List<DocumentInfo> GetStudentEvaluationDocuments(int studentEvaluationId)
        {

            using (var context = DataContext.GetContext())
            {

                return context.StudentDocuments.Where(a => a.StudentEvaluationId == studentEvaluationId).Select(a =>
                        new DocumentInfo
                        {
                            Name = a.Name,
                            StudentDocumentId = a.StudentDocumentId,
                            StudentEvaluationId = a.StudentEvaluationId,
                            StudentId = a.StudentId,
                            StudentParentContactId = a.StudentParentContactId,
                            StudentIEPId = a.StudentIEPId,
                            CreatedOn = a.CreatedOn,
                            CreatedBy = a.CreatedBy
                        }
                        ).ToList();
            }
        }

        public static List<DocumentInfo> GetStudentParentContactDocuments(int studentParentContactId)
        {
            using (var context = DataContext.GetContext())
            {

                return context.StudentDocuments.Where(a => a.StudentParentContactId == studentParentContactId).Select(a =>
                        new DocumentInfo
                        {
                            Name = a.Name,
                            StudentDocumentId = a.StudentDocumentId,
                            StudentEvaluationId = a.StudentEvaluationId,
                            StudentId = a.StudentId,
                            StudentParentContactId = a.StudentParentContactId,
                            StudentIEPId = a.StudentIEPId,
                            CreatedOn = a.CreatedOn,
                            CreatedBy = a.CreatedBy
                        }
                        ).ToList();
            }
        }

        public static List<StudentParentContactIndexModel> GetStudentParentContacts(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                ObjectSet<Entities.StudentParent> parents = context.StudentParents;
                ObjectSet<Entities.StudentParentContact> parentContacts = context.StudentParentContacts;

                var query =
                from parent in parents
                join parentContact in parentContacts
                on parent.StudentParentId
                equals parentContact.StudentParentId
                where parent.StudentId == studentId
                orderby parentContact.ContactTime descending
                select new StudentParentContactIndexModel
                {
                    StudentParentContactId = parentContact.StudentParentContactId,
                    ContactMade = parentContact.ContactMade,
                    ContactNotes = parentContact.ContactNotes,
                    DateOfContact = parentContact.ContactTime,
                    ParentName = parent.FirstName + " " + parent.LastName
                };


                return query.ToList();


            }
        }

        public static List<EventListItem> GetUpcompintEvents(int userId, bool includeCompleted, int recordCount)
        {
            using (var context = DataContext.GetContext())
            {
                IQueryable<EventListItem> query;
                if (includeCompleted)
                {
                    query = from v in context.vwUpcomingEvents
                            
                            where v.UserId == userId
                            orderby v.ScheduledDate descending
                            select new EventListItem
                            {
                                EventType = v.EventType,
                                IsComplete = v.IsComplete.Value,
                                ScheduledOn = v.ScheduledDate,
                                StudentName = v.StudentName,
                                StudentId = v.StudentId.Value
                            };
                }
                else
                {
                    query = from v in context.vwUpcomingEvents
                            where v.UserId == userId && v.IsComplete == false
                            orderby v.ScheduledDate descending
                            select new EventListItem
                            {
                                EventType = v.EventType,
                                IsComplete = v.IsComplete.Value,
                                ScheduledOn = v.ScheduledDate,
                                StudentName = v.StudentName,
                                StudentId = v.StudentId.Value
                            };
                }

                return query.Take(recordCount).ToList();

            }
        }
    }
}