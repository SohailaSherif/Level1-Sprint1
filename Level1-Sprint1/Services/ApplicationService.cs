using Level1_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Services
{
    internal class ApplicationService
    {
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
            if (company == null || company.Status != CStatus.Approved)
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
    }
}
