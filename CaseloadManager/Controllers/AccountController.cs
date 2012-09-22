using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Security;
using CaseloadManager.Models;
using System.Net.Mail;
using CaseloadManager.Helpers;

namespace CaseloadManager.Controllers
{
    #if !DEBUG
    [RequireHttps]
    #endif
    public class AccountController : Controller
    {
        
        [HttpGet]
        public ActionResult RecoverAccount(Guid id)
        {
            bool? found = false;
            var model = Data.AccountRecovery.GetRecoveryRecord(id, ref found);

            Debug.Assert(found != null, "found != null");
            if (!found.Value)
                return View("RecoveryInfoNotFound");

            return View(model);
        }

       
        [HttpPost]
        public ActionResult RecoverAccount(RecoverAccount model)
        {
            if (ModelState.IsValid)
            {

                var user = Membership.GetUser(model.UserName);
                Debug.Assert(user != null, "user != null");
                string oldPassword = user.ResetPassword();

                var createStatus = user.ChangePassword(oldPassword, model.NewPassword);

                if (createStatus)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    Data.AccountRecovery.FinishRecovery(model.UserGuid);
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("fail", "Unable to change password");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        public ActionResult LogOn()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        public ActionResult LogOff()
        {
            SessionItems.CurrentUser = null;

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
 
        public ActionResult Register()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
               
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);
                Data.CRUD.CreateUser(model.UserName, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword 
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    Debug.Assert(currentUser != null, "currentUser != null");
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        public ActionResult AttemptToSendRecovery(string emailAddress)
        {
            var result = Data.UserKeys.FindAccount(emailAddress);
            if (result == null)
                return Json(new { Message = "Eamil " + emailAddress + " not found!", Success = false });

            SendRecoveryEmail(result.UserGuid, emailAddress);
            return Json(new { Message = "Instructions sent to " + emailAddress, Success = true });
        }

        private void SendRecoveryEmail(Guid userGuid, string emailAddress)
        {
            var recoveryGuid = Data.AccountRecovery.CreateRecoveryRecord(userGuid);
            var message = new MailMessage(new MailAddress("CaseLoadManager@gmail.com"), new MailAddress(emailAddress))
                              {Subject = "Account Recovery", IsBodyHtml = true};
            message.Body += @"<br/><br/> <a href='" + AppSettings.SiteURL + "/Account/RecoverAccount/" + recoveryGuid.ToString() + @"'>Click Here to reset your password</a>";

            var client = new SmtpClient("smtp.gmail.com")
                             {
                                 Port = 587,
                                 EnableSsl = true,
                                 Credentials = new System.Net.NetworkCredential("CaseLoadManager@gmail.com", "7ujm,ki8")
                             };
            client.Send(message);
        }


        //
        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details()
        {
            var model = Data.Objects.GetAccountDetails(SessionItems.CurrentUser.UserId);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public RedirectResult Details(AccountDetailsModel model)
        {

            Data.CRUD.UpdateUser(model, SessionItems.CurrentUser.UserId);
            return new RedirectResult("../Home");
        }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

    }
}
