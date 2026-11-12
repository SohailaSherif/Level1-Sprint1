using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Models
{
    public class Job : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Salary { get; set; } 
        public string Location { get; set; }
        public bool IsOpen { get; set; } = true;
        public DateTime PostedDate { get; set; }
        public int CompanyId { get; set; }
        public int RecruiterId { get; set; }

    }
}
