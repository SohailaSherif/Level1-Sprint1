using System;
using System.Collections.Generic;
using System.Text;
using Level1_Sprint1.Events;
using Level1_Sprint1.Models;
namespace Level1_Sprint1.Events
{
    public class CompanyApproved : EventArgs
    {
        public Company ApprovedCompany { get; }
        public CompanyApproved(Company company)
        {
            ApprovedCompany = company;
        }
    }
}
