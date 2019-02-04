using System;
using System.Collections.Generic;
using BoshokuDemo1.Helper;

namespace BoshokuDemo1.Model
{
    public class Surveys
    {
        public DateTime createdDate
        {
            get;
            set;
        }

        public string creator
        {
            get;
            set;
        }
        public DateTime startDate
        {
            get;
            set;
        }
        public string surveyName
        {
            get;
            set;
        }

        public List<Question> questions
        {
            get;
            set;
        }
        public string sKey
        {
            get;
            set;
        }
        public DateTime endDate
        {
            get;
            set;
        }
    }


    public class Question
    {


        public string questionTitle
        {
            get;
            set;
        }

        public List<Replys> replies
        {
            get;
            set;
        }


    }

    public class Replys
    {
        public string reply
        {
            get;
            set;
        }
    }


    public class surveyUserResponse
    {
        public string reply
        {
            get;
            set;
        }

    }

    public class eventUserResponse
    {
        public string userKey
        {
            get {
                return SaveUserData.userKey;
            }
        }
        public DateTime createdDate
        {
            get {
                return DateTime.Now;
            }
        }
        public int index
        {
            get;
            set;
        } = -1;

    }
}
