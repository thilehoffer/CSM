using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaseloadManager.Helpers;

namespace CaseloadManager
{
    public static class DataLayer
    {
        private static DataInteractionsDataContext _context;
        private static DataInteractionsDataContext GetContext()
        {
            if (_context == null)
                _context = new DataInteractionsDataContext();

            return _context;
        }

        public static Guid GetUserGuid(string userName)
        {
            return GetContext().GetUserGuid(userName).Single().UserId;

        }

        #region Lists    

        public static List<Models.DisabilityCategoryModel> GetActiveDisabilityCategories()
        {
            var dataItems = GetContext().GetDisabilityCategories();
            var result = new List<Models.DisabilityCategoryModel>();
            foreach(var item in dataItems)
            {
                if (item.Active)
                result.Add(new Models.DisabilityCategoryModel { ID = item.ID, Description = item.Description, Active = item.Active });
            }
            return result;
        }

        public static List<Models.DisabilityCategoryModel> GetAllDisabilityCategories()
        {
            var dataItems = GetContext().GetDisabilityCategories();
            var result = new List<Models.DisabilityCategoryModel>();
            foreach (var item in dataItems)
            {
                    result.Add(new Models.DisabilityCategoryModel { ID = item.ID, Description = item.Description, Active = item.Active });
            }
            return result;
        }

        public static List<Models.StudentListItemModel> GetStudentList(Guid userId)
        {
            var dataItems = GetContext().GetStudents(userId);
            var result = new List<Models.StudentListItemModel>();
            foreach (var item in dataItems)
            {
                result.Add(new Models.StudentListItemModel { FirstName = item.FirstName, LastName = item.LastName, ExpectedGraduationYear = item.ExpectedGraduationYear, LocalEducationAgency = item.LocalEducationAgency, NextIEPDueDate = item.NextIEPDueDate, PrimaryDisability = item.PrimaryDisability, StudentId = item.StudentId });
            }
            return result;
        }

        #endregion

        #region View Model Items
             
        public static Models.AccountDetailsModel GetAccountDetails(Guid userId)
        {
            var dataItem = GetContext().GetAdditionalUserInfo(userId).Single();
            var result = new Models.AccountDetailsModel {  UserId = dataItem.UserId, Email = dataItem.Email, FirstName = dataItem.FirstName, LastName = dataItem.LastName, Phone = dataItem.Phone };
            return result;
        }
       
        #endregion

        #region Model Items
        
        public static Models.DisabilityCategoryModel GetDisabilityCategory(int disabilityCategoryID)
        {
            var dataItem = GetContext().GetDisabilityCategory(disabilityCategoryID).Single();
            var result = new Models.DisabilityCategoryModel { ID = dataItem.ID, Active = dataItem.Active, Description = dataItem.Description };
            return result;
        }

        public static Models.StudentModel GetStudent(Guid studentId)
        {
            var dataItem = GetContext().GetStudent(studentId).Single();
            var result = new Models.StudentModel { StudentId = studentId, UserID = dataItem.UserId, DateOfBirth = dataItem.DateOfBirth, DateOfEntry = dataItem.DateOfEntry, ExpectedGraduationYear = dataItem.ExpectedGraduationYear, FirstName = dataItem.FirstName, LastName = dataItem.LastName, IEPDate = dataItem.IEPDate, LocalEducationAgency = dataItem.LocalEducationAgency, NextEvaluationDueOn = dataItem.NextEvaluationDueOn, NextIEPDueDate = dataItem.NextIEPDueDate, PrimaryDisabilityId = dataItem.PrimaryDisabilityID, SecondaryDisabilityId = dataItem.SecondaryDisabilityID };
            return result;

        }

        #endregion

        #region Database Updates
       
        public static void SaveDisabilityCategory(Models.DisabilityCategoryModel disabilityCategory)
        {
            GetContext().SaveDisabilityCategory(disabilityCategory.ID, disabilityCategory.Description, disabilityCategory.Active);
        }

        public static void SaveAccountDetails(Models.AccountDetailsModel accountDetailsModel)
        {
            GetContext().SaveAdditionalUserInfo( accountDetailsModel.UserId, accountDetailsModel.Email, accountDetailsModel.FirstName, accountDetailsModel.LastName, accountDetailsModel.Phone);
        }

        public static void SaveStudent(Models.StudentModel student)
        {
            GetContext().SaveStudent(student.StudentId, student.UserID, student.DateOfBirth, student.FirstName, student.LastName, student.ExpectedGraduationYear, student.PrimaryDisabilityId, student.SecondaryDisabilityId, student.LocalEducationAgency, student.IEPDate, student.NextIEPDueDate, student.NextEvaluationDueOn, student.DateOfEntry);
        }

        #endregion

        #region Validation

        public static bool DoesDisabilityCategoryAlreadyExists(string disabilityCategory)
        {
            bool? result = null;
            GetContext().CheckIfDisabilityCategoryExists(disabilityCategory, ref result);
            return result.Value;

        }

        #endregion
        


    }
}