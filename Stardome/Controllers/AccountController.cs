using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Stardome.DomainObjects;
using Stardome.Repositories;
using Stardome.Services.Domain;
using WebMatrix.WebData;
using Stardome.Filters;
using Stardome.Models;
using System.Net.Mail;

namespace Stardome.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private readonly IUserAuthCredentialService userAuthCredentialService;
        private readonly ISiteSettingsService siteSettingsService;
        private readonly IUserInformationService userInformationService;
        private const string defaultPassword = "deFa8lt";

        public AccountController(IUserAuthCredentialService aUserAuthCredentialService, ISiteSettingsService aSiteSettingsService, IUserInformationService aUserInformationService)
        {
            userAuthCredentialService = aUserAuthCredentialService;
            siteSettingsService = aSiteSettingsService;
            userInformationService = aUserInformationService;
        }

        public AccountController()
        {
            userAuthCredentialService = new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
            siteSettingsService = new SiteSettingsService(new SiteSettingsRepository(new StardomeEntitiesCS()));
            userInformationService = new UserInformationService(new UserInformationRepository(new StardomeEntitiesCS()));
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.IsAuthenticated)
            {
                int roleId = userAuthCredentialService.GetByUsername(WebSecurity.CurrentUserName).Role.Id;
                return RedirectToLocal(roleId);
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {

            string password = userAuthCredentialService.EncryptPassword(model.Password);

            if (ModelState.IsValid && WebSecurity.Login(model.UserName, password, model.RememberMe))
            {
                int roleId = userAuthCredentialService.GetByUsername(model.UserName).Role.Id;
                return RedirectToLocal(roleId);
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Login", "Account");
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Attempt to register the user

                    string password = userAuthCredentialService.EncryptPassword(defaultPassword);
                    var userProfile = WebSecurity.CreateUserAndAccount(model.Username, password, new
                    {
                        AccountCreatedOn = DateTime.Now,
                        model.RoleId,
                    });

                    if (userProfile == null)
                        //This way Userdetail is only created if UserProfile exists so that it can retrieve the foreign key
                    {
                        UserInformation userInformation = new UserInformation
                        {
                            UserId = WebSecurity.GetUserId(model.Username),
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.EmailAddress,
                            CreatedOn = DateTime.Now
                        };

                        userInformationService.AddAUser(userInformation);
                        
                        GenerateAndSendEmail(model.Username, Enums.EmailType.AccountVerify);
                        return Json(new {Result = "OK", Record = model});
                    }
                    throw new Exception();
                }
            }
            catch (Exception)
            {
            }
            return Json(new { Result = "ERROR", Message = "Unable to create user" });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(Enums.ManageMessageId? message)
        {
            if (WebSecurity.IsAuthenticated)
            {
                ViewBag.StatusMessage =
                        message == Enums.ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                        : message == Enums.ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                        : "";
                UserAuthCredential userAuthCredential = userAuthCredentialService.GetByUsername(User.Identity.Name);

                ViewBag.showAdminMenu = userAuthCredential.RoleId == (int)Enums.Roles.Admin;

                ViewBag.HasLocalPassword = true;
                ViewBag.UpdateMessage = String.Empty;
                if (userAuthCredential.UserInformations.FirstOrDefault() != null)
                {
                    ViewBag.Name = userAuthCredential.UserInformations.First().FirstName + " " +
                                   userAuthCredential.UserInformations.First().LastName;
                    ViewBag.Email = userAuthCredential.UserInformations.First().Email;
                    
                }
                else
                {
                    ViewBag.Name = String.Empty;
                    ViewBag.Email = String.Empty;
                }
                ViewBag.Role = userAuthCredential.Role.Role1;
                return View();

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {

            if (ModelState.IsValid && WebSecurity.IsAuthenticated)
            {
                bool changePasswordSucceeded;
                try
                {
                    string OldPassword = userAuthCredentialService.EncryptPassword(model.OldPassword);
                    string newPassword = userAuthCredentialService.EncryptPassword(model.NewPassword);

                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, OldPassword, newPassword);
                    
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }
                
                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = Enums.ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            return View(model);
        }


        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            ViewBag.EmailSend = false;
            return View();
        }

        // POST: Account/LostPassword

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                UserAuthCredential user = userAuthCredentialService.GetByEmail(model.Email);
                ViewBag.EmailSend = false;
                if (user != null)
                {
                    GenerateAndSendEmail(user.Username, Enums.EmailType.ChangePassword);
                }
                else // Email not found
                {
                    ModelState.AddModelError("", "No user found by that email.");
                }
            }
            return View(model);
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.ReturnToken = rt;
            return View(model);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken,
                userAuthCredentialService.EncryptPassword(model.Password));
                if (resetResponse)
                {
                    ViewBag.Message = "Successfully Changed";
                }
                else
                {
                    ViewBag.Message = "Something went horribly wrong!";
                }
            }
            return View(model);
        }
        
        #region Helpers

        public ActionResult RedirectToLocal(int roleId)
        {
            switch (roleId)
            {
                case (int)Enums.Roles.Admin: // Admin Users
                    return RedirectToAction("Users", "Admin");

                case (int)Enums.Roles.Producer: // Producers
                    // Fall Through

                case (int)Enums.Roles.User: // Clients
                    return RedirectToAction("Actions", "Manage");

                default:
                    return RedirectToAction("Login", "Account");
            }
        }

        // This function Generate and Sends the email for User Register and Password Reset
        private void GenerateAndSendEmail(String userName, Enums.EmailType emailType)
        {
            // Generae password token that will be used in the email link to authenticate user
            var token = WebSecurity.GeneratePasswordResetToken(userName);
            // Generate the html link sent via email
            string resetLink = "<a href='"
               + Url.Action("ResetPassword", "Account", new { rt = token }, "http")
               + "'>Reset Password Link</a>";

            string subject="", body="";
            UserInformation aUser = userAuthCredentialService.GetByUsername(userName).UserInformations.FirstOrDefault();
            // Email stuff
            body = "Dear " + aUser.LastName + "<\br>";
            if (emailType == Enums.EmailType.AccountVerify)
            {
                 subject = "Welcome to stardome.com. Activate your Account";
                 body += siteSettingsService.GetById(6).Value;  
            }
            else if (emailType==Enums.EmailType.ChangePassword)
            {
                subject = "Reset your password for stardome.com";
                body += siteSettingsService.GetById(7).Value; 
            }
            body += "<\br> You link: " + resetLink;

            string from = "donotreply@stardome.com";
                        
            MailMessage message = new MailMessage(from, aUser.Email);
            message.Subject = subject;
            message.Body = body;
            SmtpClient smtp = new SmtpClient();

            //Add SMTP Server; Now runs on simulations
            // Emails will be in c:\email

            // Attempt to send the email
            try
            {
                smtp.Send(message);
                ViewBag.EmailSend = true;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Issue sending email: " + e.Message);
            }
        
        }

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
