using Level1_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Services
{
    internal class JobService
    {
        public static List<Job> SearchJobs(DataStore<Job> jobStore, string? keyword,decimal? minSalary)
        {
            Predicate<Job> predicate = job => {
                bool match = true;
                if (!string.IsNullOrEmpty(keyword))
                    match = match && (job.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || job.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                if (minSalary.HasValue)
                    match = match && job.Salary >= minSalary.Value;
                return match;
            };

            return jobStore.Find(predicate);
        }
        public static List<Job> SortByAcsendingSalary(DataStore<Job> jobStore) =>
             jobStore.Sort((job1, job2) => job1.Salary.CompareTo(job2.Salary));
        public static List<Job> SortByDescendingSalary(DataStore<Job> jobStore) =>
             jobStore.Sort((job1, job2) => job2.Salary.CompareTo(job1.Salary));
        public static List<Job> SortByAlphabeticalTitle(DataStore<Job> jobStore) =>
             jobStore.Sort((job1, job2) => string.Compare(job1.Title, job2.Title, StringComparison.OrdinalIgnoreCase));
        public static void JobMenu(DataStore<Job> jobStore, DataStore<Company> companyStore, Dictionary<int, Job> jobDict, DataStore<Application> applicationStore)
        {
            while (true)
            {

                Console.WriteLine("==== Job Management ====");
                Console.WriteLine("1. List Jobs");
                Console.WriteLine("2. Add Job");
                Console.WriteLine("3. Update Job");
                Console.WriteLine("4. Remove Job");
                Console.WriteLine("5. Search Jobs (Title & Salary)");
                Console.WriteLine("6. Sort Jobs");
                Console.WriteLine("7. Back");

                if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 7) break;
                switch (choice)
                {
                    case 1:

                        foreach (var j in jobStore.GetAll())
                            Console.WriteLine($"ID: {j.Id} | Title: {j.Title} | Open: {j.IsOpen} | Company ID: {j.CompanyId} | Posted Date: {j.PostedDate} | Location: {j.Location} | Salary: {j.Salary} ");
                        break;
                    case 2:
                        Console.Write("Enter Company ID: ");
                        if (int.TryParse(Console.ReadLine(), out int compId))
                        {
                            var comp = companyStore.GetById(compId);
                            if (comp == null || comp.Status != CompanyStatus.Approved)
                            {
                                Console.WriteLine($" Company must be Approved to post jobs (Current: {comp?.Status.ToString() ?? "NotFound"})");
                                continue;
                            }
                            Console.Write("Enter Title: "); string title = Console.ReadLine();
                            Console.Write("Enter Salary: "); decimal.TryParse(Console.ReadLine(), out decimal sal);
                            Console.Write("Enter Location: "); string loc = Console.ReadLine();

                            int maxId = 0;
                            foreach (var j in jobStore.GetAll()) if (j.Id > maxId) maxId = j.Id;
                            var newJob = new Job { Id = maxId + 1, Title = title, Salary = sal, CompanyId = compId, IsOpen = true, PostedDate = DateTime.Now, Location = loc };
                            jobStore.Add(newJob);
                            jobDict[newJob.Id] = newJob;
                            Console.WriteLine("Job added!");
                        }
                        break;
                    case 3:
                        Console.Write("Enter Job ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int jobId))
                        {
                            var jobToUpdate = jobStore.GetById(jobId);
                            if (jobToUpdate != null)
                            {
                                Console.Write("Enter new Title: ");
                                string newTitle = Console.ReadLine();
                                Console.Write("Enter new Salary: ");
                                decimal.TryParse(Console.ReadLine(), out decimal newSalary);
                                Console.Write("Enter new Location: ");
                                string newLocation = Console.ReadLine();
                                jobToUpdate.Title = newTitle;
                                jobToUpdate.Salary = newSalary;
                                jobToUpdate.Location = newLocation;
                                Console.WriteLine($"Updated job '{jobToUpdate.Title}' (ID: {jobToUpdate.Id}).");
                            }
                            else
                            {
                                Console.WriteLine("Job not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Job ID.");
                        }
                        break;
                    case 4:
                        Console.Write("Enter Job ID to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int removeJobId))
                        {
                            var jobToRemove = jobStore.GetById(removeJobId);
                            if (jobToRemove != null)
                            {
                                List<Application> applicationsToRemove = applicationStore.Find(app => app.JobId == jobToRemove.Id);
                                if (applicationsToRemove.Count > 0)
                                {
                                    Console.WriteLine($"Cannot remove job '{jobToRemove.Title}' (ID: {jobToRemove.Id}) because it has associated applications.");
                                    break;
                                }
                                jobStore.Remove(jobToRemove);
                                jobDict.Remove(jobToRemove.Id);
                                Console.WriteLine($"Removed job '{jobToRemove.Title}' (ID: {jobToRemove.Id}).");
                            }
                            else
                            {
                                Console.WriteLine("Job not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Job ID.");
                        }
                        break;
                    case 5:
                        Console.Write("Enter keyword for Title or Location (or leave empty): ");
                        string keyword = Console.ReadLine();
                        Console.Write("Enter minimum Salary (or leave empty): ");
                        decimal? minSalary = null;
                        string salaryInput = Console.ReadLine();
                        if (decimal.TryParse(salaryInput, out decimal parsedSalary))
                        {
                            minSalary = parsedSalary;
                        }
                        var searchResults = SearchJobs(jobStore, keyword, minSalary);
                        if (searchResults.Count > 0)
                        {
                            Console.WriteLine("Search Results:");
                            foreach (var job in searchResults)
                            {
                                Console.WriteLine($"ID: {job.Id} | Title: {job.Title} | Open: {job.IsOpen} | Company ID: {job.CompanyId} | Posted Date: {job.PostedDate} | Location: {job.Location} | Salary: {job.Salary}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No jobs found matching the criteria.");
                        }
                        break;
                        case 6:
                        Console.WriteLine("1. Salary Ascending | 2. Salary Descending | 3. Title Alphabetical | 4. Back");
                        if (!int.TryParse(Console.ReadLine(), out int SortChoice) || SortChoice == 4) break;
                        switch (SortChoice)
                        {
                            case 1:
                                var sortedJobsAsc = SortByAcsendingSalary(jobStore);
                                DisplayJobs(sortedJobsAsc);

                                break;
                            case 2:
                                var sortedJobsDesc = SortByDescendingSalary(jobStore);
                                DisplayJobs(sortedJobsDesc);
                                break;
                            case 3:
                                var sortedJobsAlpha = SortByAlphabeticalTitle(jobStore);
                                DisplayJobs(sortedJobsAlpha);
                                break;
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
        public static void DisplayJobs(List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                Console.WriteLine($"ID: {job.Id} | Title: {job.Title} | Open: {job.IsOpen} | Company ID: {job.CompanyId} | Posted Date: {job.PostedDate} | Location: {job.Location} | Salary: {job.Salary}");
            }
        }
    }
}
