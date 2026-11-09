using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1
{
    public enum CStatus { Pending, Approved, Rejected }
    public class Company : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string City { get; set; }
        public CStatus Status { get; set; }= CStatus.Pending;

        
    }
}
