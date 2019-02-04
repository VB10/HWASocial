using System;
using System.Collections.Generic;

namespace BoshokuDemo1.Model
{
    public class Users
    {
        public string fullName { get; set; }
        public string regNumber { get; set; }
        public string password { get; set; }
        public UserSecurity security { get; set; }


    }
    public class SecurityQuestions
    {
        public List<Qs> questions
        {
            get;
            set;
        }
    }

    public class Qs
    {
        public string q
        {
            get;
            set;
        }
    }
}
