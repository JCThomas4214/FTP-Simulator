﻿namespace Stardome.DomainObjects
{
    public static class Enums
    {
        public enum Roles
        {
            Admin = 1,
            Producer = 2,
            User = 3,
            InActive = 4
        };

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        };

        public enum EmailType
        {
            ChangePassword,
            AccountVerify,
        };
    }
}