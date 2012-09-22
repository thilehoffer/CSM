using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseloadManager.Data 
{
    public static class AccountRecovery 
    {
        private static void DeleteRecoveryRecords(Guid userId)
        {
            using (var context = DataContext.GetContext())
            {
                foreach (var item in context.Recoveries.Where(a => a.UserId == userId))
                {
                    context.Recoveries.DeleteObject(item);
                }
                context.SaveChanges();
            }
        }

        public static Guid CreateRecoveryRecord(Guid userId)
        {
            DeleteRecoveryRecords(userId);
                 var result = Guid.NewGuid();
                 using (var context = DataContext.GetContext())
                 {
                     var newObjext = new Entities.Recovery { RecoveryGuid = result, UserId = userId, TimeCreated = DateTime.Now};
                     context.Recoveries.AddObject(newObjext);
                     context.SaveChanges();
                 }
                 return result;
        }

        public static void FinishRecovery(Guid userId)
        {
            DeleteRecoveryRecords(userId);
        }

        public static Models.RecoverAccount GetRecoveryRecord(Guid recoveryGuid, ref bool? infoFound)
        {
            using (var context = DataContext.GetContext())
            {
                var recoveryItem = context.Recoveries.SingleOrDefault(a => a.RecoveryGuid == recoveryGuid);
                if (recoveryItem == null)
                {
                    infoFound = false;
                    return null;
                } 

                infoFound = true;
                var dataItem = context.vwUserDetails.Single(a => a.UserGuid == recoveryItem.UserId);
                var result = new Models.RecoverAccount { FullName = dataItem.FirstName + " " + dataItem.LastName, UserName = dataItem.UserName, UserGuid = dataItem.UserGuid };
                return result;
            }
        }
    }
}