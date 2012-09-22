using System.Linq;
using CaseloadManager.Models;

namespace CaseloadManager.Data
{
    public static class Security
    {
        public static CheckResult CheckForStudentIEP(int studentIepId, int userId)
        {
            var result = new CheckResult { Exists = false, HasAccess = false };
            using (var context = DataContext.GetContext())
            {
                dynamic item = context.StudentIEPs.Include("Student").Where(a => a.StudentIEPId == studentIepId).Select(a => new
                {
                    a.Student.UserId
                }).SingleOrDefault();



                if (item == null)
                    return result;

                result.Exists = true;

                if (item.UserId != userId)
                    return result;

                result.HasAccess = true;
                return result;
            }
        }

        public static CheckResult CheckForStudent(int studentId, int userId)
        {
            var result = new CheckResult { Exists = false, HasAccess = false };

            using (var conext = DataContext.GetContext())
            {
                dynamic item = conext.Students.Where(a => a.StudentId == studentId).Select(a => new
                    {
                        a.UserId
                    }).SingleOrDefault();

                if (item == null)
                {
                    return result;
                }
                result.Exists = true;

                if (item.UserId != userId)
                    return result;

                result.HasAccess = true;
                return result;
            }
        }

        public static CheckResult CheckForStudentEvaluation(int studentEvaluationId, int userId)
        {
            var result = new CheckResult { Exists = false, HasAccess = false };

            using (var conext = DataContext.GetContext())
            {
                dynamic item = conext.StudentEvaluations.Include("Student").Where(a => a.StudentEvaluationId == studentEvaluationId).Select(a => new
                {
                    a.Student.UserId
                }).SingleOrDefault();

                if (item == null)
                {
                    return result;
                }

                result.Exists = true;

                if (item.UserId != userId)
                {
                    return result;
                }

                result.HasAccess = true;

                return result;
            }

        }

        public static CheckResult CheckForStudentParent(int studentParentId, int userId)
        {
            var result = new CheckResult { Exists = false, HasAccess = false };
            using (var context = DataContext.GetContext())
            {


                dynamic item = context.StudentParents.Include("Student").Where(a => a.StudentParentId == studentParentId).Select(a => new
                {
                    a.Student.UserId
                }).SingleOrDefault();
                if (item == null)
                {
                    return result;
                }
                result.Exists = true;

                if (item.UserId != userId)
                {
                    return result;
                }

                result.HasAccess = true;

                return result;


            }
        }

        public static CheckResult CheckForStudentParentContact(int studentParentContactId, int userId)
        {
            var result = new CheckResult { Exists = false, HasAccess = false };
            using (var context = DataContext.GetContext())
            {
                dynamic item = context.StudentParentContacts.Include("StudentParent").Include("Student").Where(a => a.StudentParentContactId == studentParentContactId).Select(a => new
                {
                    a.StudentParent.Student.UserId
                }).SingleOrDefault();

                if (item == null)
                {
                    return result;
                }
                result.Exists = true;

                if (item.UserId != userId)
                {
                    return result;
                }

                result.HasAccess = true;
                return result;
            }
        }
    }
}