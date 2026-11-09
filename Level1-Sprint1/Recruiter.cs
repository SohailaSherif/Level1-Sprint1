using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1
{
    public class Recruiter : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
    }
}
