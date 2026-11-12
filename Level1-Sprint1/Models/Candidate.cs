using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Models
{
    public class Candidate : IEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int YearsOfExperience { get; set; }
    }
}
