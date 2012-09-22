using System.Linq;
using System.Data.Objects;

namespace CaseloadManager.Data 
{
    public class Queries
    {

        public static bool DoesDisabilityCategoryAlreadyExists(string disabilityCategory)
        {
            using (var context = DataContext.GetContext())
            {
                return context.DisabilityCategories.Any(a => a.Description == disabilityCategory);
            } 
        }

        public static bool DoesStuentHaveIePs(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentIEPs.Count(a => a.StudentId == studentId) >= 1;
            }
        }

        public static bool DoesStudentHaveAnotherCurrentIEP(int studentId, int? studentIepId)
        {
            using (var context = DataContext.GetContext())
            {
                if (studentIepId.HasValue)
                    return context.StudentIEPs.Count(a => a.StudentId == studentId && a.StudentIEPId != studentIepId.Value && a.IsCurrentIEP) >= 1;
                
                return context.StudentIEPs.Count(a => a.StudentId == studentId && a.IsCurrentIEP) >= 1;
            }
        }

        public static bool DoesStudentHaveEvaluations(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentEvaluations.Count(a => a.StudentId == studentId) >= 1;
            }
        }

        public static bool DoesStudentHaveParents(int studentId)
        {
            using (var context = DataContext.GetContext())
            {
                return context.StudentParents.Count(a => a.StudentId == studentId) >= 1 ;
            }
        }

        public static bool DoesStudentHaveParentContacts(int studentId)
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
                select new { id = parentContact.StudentParentId };

                return query.Any();
            }
        }

    }
}