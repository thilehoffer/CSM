using System;
using System.Diagnostics;
using System.Linq;
using CaseloadManager.Models;
using CaseloadManager.Helpers;

namespace CaseloadManager.Data
{
    public class CRUD
    {

        public static void CreateUser(string userName, string email)
        {
            using (var context = DataContext.GetContext())
            {
                var aspNetUser = context.aspnet_Users.Single(a => a.UserName == userName);
                var newUser = new Entities.User
                {
                    aspNetUserId = aspNetUser.Id,
                    Email = email,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now
                };

                context.Users.AddObject(newUser);
                newUser.CreatedBy = newUser.UserId;
                newUser.LastModifiedBy = newUser.UserId;
                context.SaveChanges();

            }
        }

        public static Entities.StudentDocument CreateStudentDocument(int studentId, string fileName, byte[] contents, int userId)
        {
            var result = new Entities.StudentDocument
            {
                Contents = contents,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                Name = fileName,
                StudentId = studentId
            };
            using (var context = DataContext.GetContext())
            {
                context.StudentDocuments.AddObject(result);
                context.SaveChanges();
            }
            return result;
        }

        public static Entities.StudentDocument CreateStudentIepDocument(int studentId, int studentIepId, string fileName, byte[] contents, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var result = new Entities.StudentDocument
                {
                    Contents = contents,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Name = fileName,
                    StudentId = studentId,
                    StudentIEPId = studentIepId
                };

                context.StudentDocuments.AddObject(result);
                context.SaveChanges();
                return result;
            }

        }

        public static Entities.StudentIEP CreateStudentIEP(StudentIEPModel model, int userId)
        {
            Debug.Assert(model.ScheduledDate != null, "model.ScheduledDate != null");
            Debug.Assert(model.ScheduledDateTime != null, "model.ScheduledDateTime != null");

            var result = new Entities.StudentIEP
            {
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                LastModifiedBy = userId,
                LastModifiedOn = DateTime.Now,
                DateOfMeeting = Misc.CombineDateAndTime(model.DateOfMeeting, model.DateOfMeetingTime),
                IsComplete = model.IsComplete,
                IsCurrentIEP = model.IsCurrent,
                ScheduledDate = Misc.CombineDateAndTime(model.ScheduledDate.Value, model.ScheduledDateTime.Value),
                StudentId = model.StudentId
            };
            using (var context = DataContext.GetContext())
            {
                context.StudentIEPs.AddObject(result);
                context.SaveChanges();

                if (model.StudentDocumentId.HasValue)
                {
                    var studentDocument = context.StudentDocuments.Single(a => a.StudentDocumentId == model.StudentDocumentId.Value);
                    studentDocument.StudentIEPId = result.StudentIEPId;
                    context.SaveChanges();
                }
            }

            return result;
        }

        public static Entities.StudentEvaluation CreateStudentEvaluation(StudentEvaluationModel model, int userId)
        {
            Debug.Assert(model.ScheduledDate != null, "model.ScheduledDate != null");
            var result = new Entities.StudentEvaluation
            {
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                LastModifiedBy = userId,
                LastModifiedOn = DateTime.Now,
                ScheduledDate = model.ScheduledDate.Value,
                DateCompleted = model.DateCompleted,
                StudentId = model.StudentId
            };

            using (var context = DataContext.GetContext())
            {
                context.StudentEvaluations.AddObject(result);
                context.SaveChanges();
            }

            return result;
        }

        public static Entities.StudentDocument CreateStudentEvaluationDocument(int studentId, int studentEvaluationId, string fileName, byte[] contents, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var result = new Entities.StudentDocument
                {
                    Contents = contents,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Name = fileName,
                    StudentId = studentId,
                    StudentEvaluationId = studentEvaluationId
                };

                context.StudentDocuments.AddObject(result);
                context.SaveChanges();
                return result;
            }

        }

        public static Entities.StudentDocument CreateStudentParentContactDocument(int studentId, int studentParentContactId, string fileName, byte[] contents, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var result = new Entities.StudentDocument
                {
                    Contents = contents,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Name = fileName,
                    StudentId = studentId,
                    StudentParentContactId = studentParentContactId
                };

                context.StudentDocuments.AddObject(result);
                context.SaveChanges();
                return result;
            }

        }

        public static void UpdateStudentParent(EditStudentParentModel model, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var existingItem = context.StudentParents.Single(a => a.StudentParentId == model.StudentParentId);

                existingItem.LastModifiedOn = DateTime.Now;
                existingItem.LastModifiedBy = userId;
                existingItem.Email = model.Email;
                existingItem.Phone = model.Phone.RemoveNonNumericCharacters();
                existingItem.FirstName = model.FirstName;
                existingItem.LastName = model.LastName;
                existingItem.StudentId = model.StudentId;
                existingItem.Relationship = model.Relationship;
                existingItem.PreferredContactMethod = model.PreferredContactMethod;

                context.SaveChanges();
            }
        }

        public static Entities.StudentParent CreateStudentParent(EditStudentParentModel model, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var result = new Entities.StudentParent
                {
                    CreatedOn = DateTime.Now,
                    CreatedBy = userId,
                    LastModifiedOn = DateTime.Now,
                    LastModifiedBy = userId,
                    Email = model.Email,
                    Phone = model.Phone.RemoveNonNumericCharacters(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StudentId = model.StudentId,
                    Relationship = model.Relationship,
                    PreferredContactMethod = model.PreferredContactMethod
                };
                context.StudentParents.AddObject(result);
                context.SaveChanges();
                return result;
            }
        }

        public static Entities.StudentParentContact CreateStudentParentContact(StudentParentContactModelDataCRUD model, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                Debug.Assert(model.DateOfContact != null, "model.DateOfContact != null");
                Debug.Assert(model.TimeOfContact != null, "model.TimeOfContact != null");
                Debug.Assert(model.StudentParentId != null, "model.StudentParentId != null");
                var result = new Entities.StudentParentContact
                {
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                    LastModifiedBy = userId,
                    ContactMade = model.ContactMade,
                    ContactNotes = string.IsNullOrEmpty(model.ContactNotes) ? string.Empty : model.ContactNotes ,
                    ContactTime = Misc.CombineDateAndTime(model.DateOfContact.Value, model.TimeOfContact.Value),
                    StudentParentId = model.StudentParentId.Value
                };
                context.StudentParentContacts.AddObject(result);
                context.SaveChanges();
                return result;
            }
        }

        public static Entities.UserFeedback CreateUserFeedback (FeedbackModel model, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var result = new Entities.UserFeedback
                {
                    Comments = model.comments,
                    CreatedOn = DateTime.Now,
                    UserId = userId
                };
                context.UserFeedbacks.AddObject(result);
                context.SaveChanges();
                return result;
            }
        }

        public static void UpdateStudentParentContact(StudentParentContactModelDataCRUD model, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var existingItem = context.StudentParentContacts.Single(a => a.StudentParentContactId == model.StudentParentContactId);
                existingItem.LastModifiedOn = DateTime.Now;
                existingItem.LastModifiedBy = userId;
                existingItem.ContactMade = model.ContactMade;
                existingItem.ContactNotes = model.ContactNotes;
                Debug.Assert(model.DateOfContact != null, "model.DateOfContact != null");
                Debug.Assert(model.TimeOfContact != null, "model.TimeOfContact != null");
                existingItem.ContactTime = Misc.CombineDateAndTime(model.DateOfContact.Value, model.TimeOfContact.Value);
                context.SaveChanges();
            }  
        }

        public static void InsertDisabilityCategory(DisabilityCategoryModel disabilityCategory, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                context.DisabilityCategories.AddObject(new Entities.DisabilityCategory
                {
                    Active = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Description = disabilityCategory.Description,
                    LastModifiedBy = userId,
                    LastModifiedOn = DateTime.Now
                });
                context.SaveChanges();
            }
        }

        public static void UpdateDisabilityCategory(DisabilityCategoryModel disabilityCategory, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var existing = context.DisabilityCategories.Single(a => a.ID == disabilityCategory.ID);
                existing.Description = disabilityCategory.Description;
                existing.Active = disabilityCategory.Active;
                existing.LastModifiedOn = DateTime.Now;
                existing.LastModifiedBy = userId;
                context.SaveChanges();

            }
        }

        
        public static void UpdateUser(AccountDetailsModel accountDetailsModel, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var existingItem = context.Users.Single(a => a.UserId == accountDetailsModel.UserId);
                existingItem.Email = accountDetailsModel.Email;
                existingItem.FirstName = accountDetailsModel.FirstName;
                existingItem.LastName = accountDetailsModel.LastName;
                existingItem.Phone = accountDetailsModel.Phone;
                existingItem.LastModifiedBy = userId;
                existingItem.LastModifiedOn = DateTime.Now;
                context.SaveChanges();
            }
        }

        public static void InsertStudent(StudentModel student, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                Debug.Assert(student.UserId != null, "student.UserId != null");
                Debug.Assert(student.ExpectedGraduationYear != null, "student.ExpectedGraduationYear != null");
                Debug.Assert(student.PrimaryDisabilityId != null, "student.PrimaryDisabilityId != null");
                Debug.Assert(student.DateOfBirth != null, "student.DateOfBirth != null");

                var newItem = new Entities.Student
                {
                    UserId = student.UserId.Value,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    ExpectedGraduationYear = student.ExpectedGraduationYear.Value,
                    PrimaryDisabilityID = student.PrimaryDisabilityId.Value,
                    LocalEducationAgency = student.LocalEducationAgency,
                    DateOfEntry = student.DateOfEntry,
                    DateOfBirth = student.DateOfBirth.Value,
                    SecondaryDisabilityID = student.SecondaryDisabilityId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    LastModifiedBy = userId,
                    LastModifiedOn = DateTime.Now,
                    Current = true
                };

                context.Students.AddObject(newItem);
                context.SaveChanges();
            }
        }

        public static void UpdateStudent(int studentId, bool current, int userId)
        {
            using (var context = DataContext.GetContext())
            {
                var existingItem = context.Students.Single(a => a.StudentId == studentId);
                 
                existingItem.LastModifiedBy = userId;
                existingItem.LastModifiedOn = DateTime.Now;
                existingItem.Current = current;
                context.SaveChanges();

            }
        }

        public static void UpdateStudent(StudentModel student, int userId)
        {
            Debug.Assert(student.DateOfBirth != null, "student.DateOfBirth != null");
            Debug.Assert(student.ExpectedGraduationYear != null, "student.ExpectedGraduationYear != null");
            Debug.Assert(student.PrimaryDisabilityId != null, "student.PrimaryDisabilityId != null");
                

            using (var context = DataContext.GetContext())
            {
                var existingItem = context.Students.Single(a => a.StudentId == student.StudentId);
                existingItem.DateOfBirth = student.DateOfBirth.Value;
                existingItem.FirstName = student.FirstName;
                existingItem.LastName = student.LastName;
                existingItem.ExpectedGraduationYear = student.ExpectedGraduationYear.Value;
                existingItem.PrimaryDisabilityID = student.PrimaryDisabilityId.Value;
                existingItem.SecondaryDisabilityID = student.SecondaryDisabilityId;
                existingItem.LocalEducationAgency = student.LocalEducationAgency;
                existingItem.DateOfEntry = student.DateOfEntry;
                existingItem.LastModifiedBy = userId;
                existingItem.LastModifiedOn = DateTime.Now;
                existingItem.Current = student.CurrentStudent;
                context.SaveChanges();

            }
        }

        public static void UpdateStudentIep(StudentIEPModel model, int userId)
        {
            Debug.Assert(model.ScheduledDate != null, "model.ScheduledDate != null");
            Debug.Assert(model.ScheduledDateTime != null, "model.ScheduledDateTime != null");
                
            using (var context = DataContext.GetContext())
            {
                var existing = context.StudentIEPs.Single(a => a.StudentIEPId == model.StudentIEPId);
                existing.DateOfMeeting = Misc.CombineDateAndTime(model.DateOfMeeting, model.DateOfMeetingTime);
                existing.IsComplete = model.IsComplete;
                existing.IsCurrentIEP = model.IsCurrent;
                existing.LastModifiedBy = userId;
                existing.LastModifiedOn = DateTime.Now;
                existing.ScheduledDate = Misc.CombineDateAndTime(model.ScheduledDate.Value, model.ScheduledDateTime.Value);
                context.SaveChanges();
            }
        }

        public static void UpdateStudentEvaluation(StudentEvaluationModel model, int userId)
        {
            Debug.Assert(model.ScheduledDate != null, "model.ScheduledDate != null");
            using (var context = DataContext.GetContext())
            {
                var existing = context.StudentEvaluations.Single(a => a.StudentEvaluationId == model.StudentEvaluationId);
                existing.DateCompleted = model.DateCompleted;
                existing.LastModifiedBy = userId;
                existing.LastModifiedOn = DateTime.Now;
                existing.ScheduledDate = model.ScheduledDate.Value;
                context.SaveChanges();
            }
        }

        #region Deletes
        public static void DeleteStudentIEP(int studentIEPId)
        {
            using (var context = DataContext.GetContext())
            {
                var existingIEP = context.StudentIEPs.Include("StudentDocuments").Single(a => a.StudentIEPId == studentIEPId);
                if (existingIEP.StudentDocuments.SingleOrDefault() != null)
                {
                    context.StudentDocuments.DeleteObject(existingIEP.StudentDocuments.Single());
                    context.SaveChanges();
                }
                context.DeleteObject(existingIEP);
                context.SaveChanges();

            }
        }

        public static void DeleteStudentEvaluation(int studentEvaluationId)
        {
            using (var context = DataContext.GetContext())
            {
                var existingEvaluation = context.StudentEvaluations.Include("StudentDocuments").Single(a => a.StudentEvaluationId == studentEvaluationId);
                if (existingEvaluation.StudentDocuments.SingleOrDefault() != null)
                {
                    context.StudentDocuments.DeleteObject(existingEvaluation.StudentDocuments.Single());
                    context.SaveChanges();
                }
                context.DeleteObject(existingEvaluation);
                context.SaveChanges();

            }
        }

        public static void DeleteStudentDocument(int studentDocumentId)
        {
            using (var context = DataContext.GetContext())
            {
                var document = context.StudentDocuments.Single(a => a.StudentDocumentId == studentDocumentId);
                context.StudentDocuments.DeleteObject(document);
                context.SaveChanges();
            }
        }

        public static void DeleteStudentParent(int studentParentId)
        {
            using (var context = DataContext.GetContext())
            {
                var studentParent = context.StudentParents.Single(i => i.StudentParentId == studentParentId);
                context.DeleteObject(studentParent);
                context.SaveChanges();
            }
        }

        public static void DeleteStudentParentContact(int studentParentContactId)
        {
            using (var context = DataContext.GetContext())
            {
                var studentParentContact =
                    context.StudentParentContacts.Single(i => i.StudentParentContactId == studentParentContactId);
                context.DeleteObject(studentParentContact);
                context.SaveChanges();
            }
        }

        #endregion

        public static void LogException(string exception)
        {
            using (var context = DataContext.GetContext())
            {
                context.ExceptionLogs.AddObject(new Entities.ExceptionLog { exception = exception, timeOf = DateTime.Now });
                context.SaveChanges();
            }
        }

    }
}