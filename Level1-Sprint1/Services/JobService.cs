using Level1_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Services
{
    internal class JobService
    {
        public static void JobMenu(DataStore<Job> jobStore, DataStore<Company> companyStore, Dictionary<int, Job> jobDict)
        {
            while (true)
            {

                Console.WriteLine("==== Job Management ====");
                Console.WriteLine("1. List Jobs");
                Console.WriteLine("2. Add Job");
                Console.WriteLine("3. Update Job");
                Console.WriteLine("4. Remove Job");
                Console.WriteLine("5. Back");

                if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 5) break;
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
                            if (comp == null || comp.Status != CStatus.Approved)
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
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
