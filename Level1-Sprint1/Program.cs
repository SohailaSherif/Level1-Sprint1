using Level1_Sprint1.Models;
using Level1_Sprint1.Services;
using System;
using System.Collections.Generic;

namespace Level1_Sprint1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataStore<Company> company = new DataStore<Company>();
            DataStore<Recruiter> recruiter = new DataStore<Recruiter>();
            DataStore<Job> job = new DataStore<Job>();
            DataStore<Candidate> candidate = new DataStore<Candidate>();
            DataStore<Application> application = new DataStore<Application>();
            Dictionary<int, Job> jobDictionary = new Dictionary<int, Job>();
            HashSet<string> candidateEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            HashSet<(int JobId, int CandidateId)> applicationSet = new HashSet<(int, int)>();
            Stack<(int CompanyId, CStatus OldStatus)> undoStack = new Stack<(int, CStatus)>();

            SeedData(company, recruiter, job, candidate, jobDictionary, candidateEmails);

            bool isRunning = true;
            while (isRunning)
            {
                try
                {
                    Console.WriteLine("\n=== Job Application Tracker ===");

                    Console.WriteLine("\nSelect an option:");

                    Console.WriteLine("1. Manage Companies");

                    Console.WriteLine("2. Manage Jobs");

                    Console.WriteLine("3. Manage Candidates");

                    Console.WriteLine("4. Apply for Jobs");

                    Console.WriteLine("5. Exit");

                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int choice))

                    {

                        Console.WriteLine("Invalid input Please enter a number.");

                        continue;

                    }

                    switch (choice)

                    {

                        case 1:

                            CompanyService.CompanyMenu(company, undoStack);

                            break;

                        case 2:

                            JobService.JobMenu(job, company, jobDictionary);

                            break;

                        case 3:

                            CandidateService.CandidateMenu(candidate, candidateEmails);

                            break;

                        case 4:

                            ApplicationService.ApplyToJob(candidate, jobDictionary, company, application, applicationSet);

                            break;

                        case 5:

                            isRunning = false;

                            Console.WriteLine("Done");

                            break;

                        default:

                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Error: {ex.Message}");
                }
            }
  
                  


           static void SeedData(DataStore<Company> comp, DataStore<Recruiter> rec, DataStore<Job> job, DataStore<Candidate> cand, Dictionary<int, Job> jobDict, HashSet<string> emails)

            {

               var companies = new[]{

                 new Company { Id = 1, Name = "Tech Solutions", Industry = "Software", City = "Cairo", Status = CStatus.Approved },

                 new Company { Id = 2, Name = "Data Corp", Industry = "Analytics", City = "Alexandria", Status = CStatus.Pending },

                 new Company { Id = 3, Name = "InnoSoft", Industry = "IT", City = "Giza", Status = CStatus.Rejected }
                };

                foreach (var c in companies)
                 {
                  comp.Add(c);
                  Console.WriteLine($"Company ID: {c.Id} | Company Name: {c.Name} | City: {c.City} | Status: {c.Status}");
                 }

                Console.WriteLine("--------------------------------------------------------------------------------------");


                var recruiters = new[]{
                  new Recruiter { Id = 1, Name = "Ahmed Mohamed", Email = "ahmed@tech.com", CompanyId = 1 },
                  new Recruiter { Id = 2, Name = "Mona Ali", Email = "mona@datacorp.com", CompanyId = 2 },
                  new Recruiter { Id = 3, Name = "Khaled Omar", Email = "khaled@innosoft.com", CompanyId = 3 } 
                 };

                foreach (var r in recruiters)
                 {
                    rec.Add(r);

                   Console.WriteLine($"Recruiter ID: {r.Id} | Name: {r.Name} | Email: {r.Email} | Company ID: {r.CompanyId}");
                 }

                Console.WriteLine("--------------------------------------------------------------------------------------");


               var jobs = new[]{
                 new Job { Id = 1, Title = "C# Developer", Description = "Backend engineer", Salary = 1500, Location = "Cairo", IsOpen = true, CompanyId = 1, RecruiterId = 1 },
                 new Job { Id = 2, Title = "React Dev", Description = "Frontend dev", Salary = 1200, Location = "Cairo", IsOpen = true, CompanyId = 1, RecruiterId = 1 },  
                 new Job { Id = 3, Title = "Data Analyst", Description = "SQL & BI", Salary = 1000, Location = "Alex", IsOpen = true, CompanyId = 2, RecruiterId = 2 },        
                 new Job { Id = 4, Title = "Data Engineer", Description = "Pipelines", Salary = 1800, Location = "Alex", IsOpen = false, CompanyId = 2, RecruiterId = 2 },
                 new Job { Id = 5, Title = "QA Engineer", Description = "Automation", Salary = 900, Location = "Giza", IsOpen = true, CompanyId = 3, RecruiterId = 3 },
                 new Job { Id = 6, Title = "DevOps", Description = "AWS & Docker", Salary = 2000, Location = "Giza", IsOpen = true, CompanyId = 3, RecruiterId = 3 } 
                };

               foreach (var j in jobs){
                job.Add(j);
                jobDict[j.Id] = j;
                 Console.WriteLine($"Job ID: {j.Id} | Title: {j.Title} | Description: {j.Description} | Salary: {j.Salary} | Location: {j.Location} | Is Open: {j.IsOpen} | Company ID: {j.CompanyId} | Recruiter ID: {j.RecruiterId}");

                }

              Console.WriteLine("--------------------------------------------------------------------------------------");


               var candidates = new[]{
                 new Candidate { Id = 1, FullName = "Tarek Ziad", Email = "tarek@gmail.com", Phone = "01011112222", YearsOfExperience = 3 },
                 new Candidate { Id = 2, FullName = "Sara Adel", Email = "sara@yahoo.com", Phone = "01122223333", YearsOfExperience = 5 },
                 new Candidate { Id = 3, FullName = "Youssef Nader", Email = "youssef@outlook.com", Phone = "01233334444", YearsOfExperience  = 1 },
                 new Candidate { Id = 4, FullName = "Nourhan Samy", Email = "nourhan@gmail.com", Phone = "01544445555", YearsOfExperience = 6 },
                 new Candidate { Id = 5, FullName = "Hassan Mostafa", Email = "hassan@hotmail.com", Phone = "01055556666", YearsOfExperience = 2 }
                };

               foreach (var cands in candidates)
                {
                  if (!emails.Add(cands.Email)){
                   Console.WriteLine($"[Refused in Seed]: Candidate '{cands.FullName}' has a duplicate email ({cands.Email}) and was NOT added.");
                   continue;
                   }
                cand.Add(cands);
                 Console.WriteLine($"Candidate ID: {cands.Id} | Name: {cands.FullName} | Email: {cands.Email} | Phone: {cands.Phone} | Experience: {cands.YearsOfExperience} years");
                }
            }
         }
      }
 }
    
        