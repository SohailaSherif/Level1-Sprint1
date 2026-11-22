using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Models
{
    public enum ApplicationStatus { Submitted, UnderReview, Accepted, Rejected }
    public class Application : IEntity
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int CandidateId { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
    }
}
