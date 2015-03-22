using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stardome.Services.Application
{
    public interface IAuthenticationProvider
    {
        bool IsAuthenticated();
        bool ResetPassword(string passwordResetToken, string newPassword);
        string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440);
        string CurrentUserName();
        bool Login(string userName, string password, bool persistCookie = false);
        void Logout();
        string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false);
        int GetUserId(string userName);
        bool ChangePassword(string userName, string currentPassword, string newPassword);

    }
}
