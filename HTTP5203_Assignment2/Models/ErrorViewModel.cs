using System;

namespace HTTP5203_Assignment2.Models
{
    public class ErrorViewModel
    {
        public string RequestId {
            get; set;
        }

        public bool ShowRequestId => !string.IsNullOrEmpty( RequestId );
    }
}
