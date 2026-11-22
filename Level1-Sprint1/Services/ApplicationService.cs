using Level1_Sprint1.Events;
using Level1_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Services
{
    internal class ApplicationService
    {
        public static event EventHandler<ApplicationStatusChanged>? ApplicationStatusChanged;
        public static event EventHandler<ApplicationSubmitted>? ApplicationSubmitted;
        
        public static void ApplyToJob(DataStore<Candidate> candStore, Dictionary<int, Job> jobDict, DataStore<Company> compStore, DataStore<Application> appStore, HashSet<(int JobId, int CandidateId)> appSet)
        {
            Console.WriteLine("==== Apply to Job =====");
            Console.Write("Enter Candidate ID: ");
            if (!int.TryParse(Console.ReadLine(), out int candId) || candStore.GetById(candId) == null)
            {
                Console.WriteLine("Invalid Candidate ID.");
                return;
            }

            Console.Write("Enter Job ID: ");
            if (!int.TryParse(Console.ReadLine(), out int jobId) || !jobDict.TryGetValue(jobId, out Job job))
            {
                Console.WriteLine("Job not found.");
                return;
            }

            var company = compStore.GetById(job.CompanyId);
            if (company == null || company.Status != CompanyStatus.Approved)
            {
                Console.WriteLine($"Company ({company?.Name}) is not Approved.");
                return;
            }

            if (!job.IsOpen)
            {
                Console.WriteLine("Job is closed.");
                return;
            }

            var pair = (JobId: jobId, CandidateId: candId);
            if (!appSet.Add(pair))
            {
                Console.WriteLine("Duplicate application! Candidate already applied to this job.");
                return;
            }

            int maxId = 0;
            foreach (var a in appStore.GetAll()) if (a.Id > maxId) maxId = a.Id;

            appStore.Add(new Application { Id = maxId + 1, JobId = jobId, CandidateId = candId });
            Console.WriteLine(" Application submitted successfully!");
            ;
        }
        public static bool ChangeApplicationStatus(DataStore<Application> appStore, int applicationId, ApplicationStatus newStatus)
        {
            var app = appStore.GetById(applicationId);
            if (app == null || app.Status == newStatus) return false;

            ApplicationStatus oldStatus = app.Status;
            app.Status = newStatus;

            
            NotificationSystem.SafeInvoke(
                ApplicationStatusChanged,
                null,
                new ApplicationStatusChanged(app, newStatus, oldStatus));

            return true;
        }
        public static void ChangeApplicationStatus(DataStore<Application> appStore)
        {
            Console.WriteLine("==== Change Application Status ====");
            Console.Write("Enter Application ID: ");
            if (!int.TryParse(Console.ReadLine(), out int appId) || appStore.GetById(appId) == null)
            {
                Console.WriteLine("Invalid Application ID.");
                return;
            }
            Console.WriteLine("Select new status:");
            foreach (var status in Enum.GetValues(typeof(ApplicationStatus)))
            {
                Console.WriteLine($"{(int)status}. {status}");
            }
            if (!int.TryParse(Console.ReadLine(), out int newStatusInt) || !Enum.IsDefined(typeof(ApplicationStatus), newStatusInt))
            {
                Console.WriteLine("Invalid status selection.");
                return;
            }
            var newStatus = (ApplicationStatus)newStatusInt;
            if (ChangeApplicationStatus(appStore, appId, newStatus))
            {
                Console.WriteLine("Application status updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update application status.");
            }
        }
        
    }
}
   
