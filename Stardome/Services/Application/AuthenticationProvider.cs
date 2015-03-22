using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace Stardome.Services.Application
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
       public bool IsAuthenticated() {
            return WebSecurity.IsAuthenticated;
        }
       public bool ResetPassword(string passwordResetToken, string newPassword)
       {
            return WebSecurity.ResetPassword(passwordResetToken, newPassword);
        }
       public string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440)
       {
            return WebSecurity.GeneratePasswordResetToken(userName);
        }
       public string CurrentUserName()
       {
            return WebSecurity.CurrentUserName;
        }
       public bool Login(string userName, string password, bool persistCookie = false)
       {
            return WebSecurity.Login(userName, password, persistCookie);
        }
       public void Logout()
       {
            WebSecurity.Logout();
        }
       public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
       {
          return  WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);
        }
       public int GetUserId(string userName)
       {
            return WebSecurity.GetUserId(userName);
        }
       public bool ChangePassword(string userName, string currentPassword, string newPassword)
       { 
            return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
        }
    }
}