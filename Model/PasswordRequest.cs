using System;
namespace BoshokuDemo1.Model
{
    public class PasswordRequest
    {
        public string idToken
        {
            get;
            set;
        }
        public string password
        {
            get;
            set;
        }

    }

    public struct AutoLogin
    {
        public string refresh_token
        {
            get;
            set;
        }
        //  The refresh token's grant type, always "refresh_token".
        public string grant_type
        {
            get
            {
                return "refresh_token";
            }
        }
    }
}
