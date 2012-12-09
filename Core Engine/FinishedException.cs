using System;
using System.Net;
using System.Windows;

namespace MB.Crammer
{
    public class FinishedException : System.Exception
    {
        // The default constructor needs to be defined
        // explicitly now since it would be gone otherwise.

        public FinishedException()
        {
        }

        public FinishedException(string message)
            : base(message)
        {
        }
    }
}
