using System;
using System.Collections.Generic;
using System.Text;
using Level1_Sprint1.Events;
using Level1_Sprint1.Models;

namespace Level1_Sprint1.Events
{
    public class ApplicationStatusChanged : EventArgs
    {
        public Application Application { get; }
        public ApplicationStatus NewStatus { get; }
        public ApplicationStatus OldStatus { get; }
        public ApplicationStatusChanged(Application application, ApplicationStatus newStatus, ApplicationStatus oldStatus)
        {
            Application = application;
            NewStatus = newStatus;
            OldStatus = oldStatus;
        }
    }
}
