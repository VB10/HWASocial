using System;
namespace BoshokuDemo1.Model
{
    public class Notify
    {
        public string Title
        {
            get;
            set;

        }
        public string Detail
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }

        public bool Likex
        {
            get;
            set;
        }

        public DateTime CreatorDate
        {
            get;
            set;
        }
        public string LikeButton
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }


    }

    public class Likes
    {

        public bool reply
        {
            get;
            set;
        }
    }
}
