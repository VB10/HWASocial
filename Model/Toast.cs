using System;
namespace BoshokuDemo1.Model
{
    public class Toast
    {
        public string text
        {
            get;
            set;
        }
        public Show display
        {
            get;
            set;
        }
        public AnimationType type
        {
            get;
            set;
        }

    }
    public enum Show
    {
        SHORT,
        LONG,
        LOOP
    }
    public enum AnimationType
    {
        Success,
        False,
        Default
    }
}
