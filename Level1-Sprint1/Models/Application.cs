using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Models
{ 
    public class Application : IEntity
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int CandidateId { get; set; }
        public DateTime AppliedDate { get; set; }
        public enum Status { Submitted, UnderReview, Interview, Accepted, Rejected }
    }
}
