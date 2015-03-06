using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
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
        private readonly IRoleService roleService;
        public AccountController(IUserAuthCredentialService aUserAuthCredentialService, IRoleService aRoleService)
        {
            userAuthCredentialService = aUserAuthCredentialService;
            roleService = aRoleService;
        }

        public AccountController()
        {
            userAuthCredentialService = new UserAuthCredentialService(new UserAuthCredentialRepository(new StardomeEntitiesCS()));
            roleService = new RoleService(new RoleRepository(new StardomeEntitiesCS()));
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
            //TODO: Dont need this as password is stored in webpages_membership
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
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.showAdminMenu = true;
            ViewBag.Roles = roleService.GetRoles();

            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    try
                    {
                        string password = userAuthCredentialService.EncryptPassword(model.Password);

                        var userProfile = WebSecurity.CreateUserAndAccount(model.UserName, password, new
                        {

                            AccountCreatedOn = DateTime.Now,
                            RoleId = model.RoleId,
                            Password = password

                        });

                        if (userProfile == null) //This way Userdetail is only created if UserProfile exists so that it can retrieve the foreign key
                        {
                            UserInformation UserInformation = new UserInformation();
                            UserInformation.UserId = WebSecurity.GetUserId(model.UserName);
                            UserInformation.FirstName = model.FirstName;
                            UserInformation.LastName = model.LastName;
                            UserInformation.Email = model.EmailAddress;
                            UserInformation.CreatedOn = DateTime.Now;

                            using (var dbCtx = new StardomeEntitiesCS())
                            {
                                dbCtx.UserInformations.Add(UserInformation);
                                dbCtx.SaveChanges();
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                    // What happens here?
                    return RedirectToAction("Users", "Admin");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Manage

        public ActionResult Manage()
        {
            if (WebSecurity.IsAuthenticated)
            {
                UserAuthCredential usrInfo = userAuthCredentialService.GetByUsername(User.Identity.Name);
                int roleId = usrInfo.Role.Id;
                if (roleId == 1)
                    ViewBag.showAdminMenu = true;
                else
                {
                    ViewBag.showAdminMenu = false;
                }
                ViewBag.HasLocalPassword = true;
                ViewBag.Name = usrInfo.UserInformations.FirstOrDefault().FirstName + " " + usrInfo.UserInformations.FirstOrDefault().LastName;
                ViewBag.Email = usrInfo.UserInformations.FirstOrDefault().Email;
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

            if (ModelState.IsValid)
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
                    ViewBag.changePasswordSucceeded = "Your Password has been changed successfully";
                    return RedirectToAction("Manage");
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
                UserAuthCredential user;
                user = userAuthCredentialService.GetByEmail(model.Email);
                ViewBag.EmailSend = false;
                if (user != null)
                {
                    // Generae password token that will be used in the email link to authenticate user
                    var token = WebSecurity.GeneratePasswordResetToken(user.Username, 15);
                    // Generate the html link sent via email
                    string resetLink = "<a href='"
                       + Url.Action("ResetPassword", "Account", new { rt = token }, "http")
                       + "'>Reset Password Link</a>";

                    // Email stuff
                    string subject = "Reset your password for stardome.com";
                    string body = "You link: " + resetLink;
                    string from = "donotreply@stardome.com";

                    MailMessage message = new MailMessage(from, model.Email);
                    message.Subject = subject;
                    message.Body = body;
                    SmtpClient smtp = new SmtpClient();
                    //Add SMTP Server; Now runs on simulations
                    // Emails will b in c:\email

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
                else // Email not found
                {
                    ModelState.AddModelError("", "No user found by that email.");
                }
            }

            /* You may want to send the user to a "Success" page upon the successful
            * sending of the reset email link. Right now, if we are 100% successful
            * nothing happens on the page. :P
            */
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
                
                bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, userAuthCredentialService.EncryptPassword(model.Password));
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
                case 1: // Admin Users
                    return RedirectToAction("Users", "Admin");

                case 2: // Producers
                    return RedirectToAction("Index", "Producer");

                case 3: // Clients
                    return RedirectToAction("Index", "Clients");

                default:
                    return RedirectToAction("Login", "Account");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
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
