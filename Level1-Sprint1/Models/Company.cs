using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Models
{
    public enum CompanyStatus{ Pending, Approved, Rejected }
    public class Company : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string City { get; set; }
        public CompanyStatus Status { get; set; }= CompanyStatus.Pending;

        
    }
}
