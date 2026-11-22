using System;
using System.Collections.Generic;
using System.Text;
using Level1_Sprint1.Events;
using Level1_Sprint1.Models;
namespace Level1_Sprint1.Events
{
    public class ApplicationSubmitted : EventArgs
    {
        public Application SubmittedApplication { get; }
        public Job SubmittedJob { get; }
        public Candidate SubmittedCandidate { get; }
        public ApplicationSubmitted(Application application, Job submittedJob, Candidate submittedCandidate)
        {
            SubmittedApplication = application;
            SubmittedJob = submittedJob;
            SubmittedCandidate = submittedCandidate;
        }
    }
}
