using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public static class ValidateRequest
    {
        internal static bool AddUser(UserCR req)
        {
            if (req != null || req.User != null || string.IsNullOrEmpty(req.User.email_id) || string.IsNullOrEmpty(req.User.first_name) || string.IsNullOrEmpty(req.User.last_name) || string.IsNullOrEmpty(req.User.password))
                return true;
            else
                return false;
        }

        internal static bool UpdateUser(UserCR req)
        {
            if (req != null || req.User != null || string.IsNullOrEmpty(req.Token) || string.IsNullOrEmpty(req.User.email_id) || string.IsNullOrEmpty(req.User.first_name) || string.IsNullOrEmpty(req.User.last_name) || string.IsNullOrEmpty(req.User.password))
                return true;
            else
                return false;
        }

        internal static bool Authenticate(AuthenticateCR req)
        {
            if (req != null || string.IsNullOrEmpty(req.EmailID) || string.IsNullOrEmpty(req.Password) )
                return true;
            else
                return false;
        }

        internal static bool Profile(BaseResquest req)
        {
            if (req != null || string.IsNullOrEmpty(req.Token))
                return true;
            else
                return false;
        }
    }
}