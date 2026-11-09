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

                            CompanyMenu(company, undoStack);

                            break;

                        case 2:

                            JobMenu(job, company, jobDictionary);

                            break;

                        case 3:

                            CandidateMenu(candidate, candidateEmails);

                            break;

                        case 4:

                            ApplyToJob(candidate, jobDictionary, company, application, applicationSet);

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


            static void CompanyMenu(DataStore<Company> companyStore, Stack<(int, CStatus)> undoStack)
            {
                while (true)
                {
                    
                    Console.WriteLine("==== Company Management ====");
                    Console.WriteLine("1. List Companies");
                    Console.WriteLine("2. Change Company Status (Pending/Approved/Rejected)");
                    Console.WriteLine("3. Undo Last Status Change");
                    Console.WriteLine("4. Add New Company");
                    Console.WriteLine("5. Update Company Information");
                    Console.WriteLine("6. Remove Company");
                    Console.WriteLine("7. Back to Main Menu");
                    if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 7) break;
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("==== Companies List ====");
                            foreach (var c in companyStore.GetAll())
                            {
                                Console.WriteLine($"ID: {c.Id} | Name: {c.Name} | Industry: {c.Industry} | Status: {c.Status} | City: {c.City}");
                            }
                            break;

                        case 2:
                            Console.Write("Enter Company ID: ");
                            if (int.TryParse(Console.ReadLine(), out int id))
                            {
                                var comp = companyStore.GetById(id);
                                if (comp != null)
                                {
                                    undoStack.Push((comp.Id, comp.Status));

                                    Console.Write("New Status (0: Pending, 1: Approved, 2: Rejected): ");
                                    if (int.TryParse(Console.ReadLine(), out int statusInt) && Enum.IsDefined(typeof(CStatus), statusInt))
                                    {
                                        comp.Status = (CStatus)statusInt;
                                        Console.WriteLine("Status updated!");
                                    }
                                    else { Console.WriteLine("Invalid status value."); }
                                }
                                else { Console.WriteLine("Company not found."); }
                            }
                            break;

                        case 3:
                            if (undoStack.Count == 0)
                            {
                                Console.WriteLine("Nothing to undo!");
                                break;
                            }

                            var (Id, oldStatus) = undoStack.Pop();
                            var company = companyStore.GetById(Id);
                            if (company != null)
                            {
                                company.Status = oldStatus;
                                Console.WriteLine($"Reverted '{company.Name}' back to '{oldStatus}'.");
                            }
                            break;
                        case 4:
                            Console.Write("Enter Company Name: ");
                            string name = Console.ReadLine();
                            Console.Write("Enter Industry: ");
                            string industry = Console.ReadLine();
                            Console.Write("Enter City: ");
                            string city = Console.ReadLine();
                            int maxId = 0;
                            foreach (var c in companyStore.GetAll()) if (c.Id > maxId) maxId = c.Id;
                            var newCompany = new Company { Id = maxId + 1, Name = name, Industry = industry, City = city, Status = CStatus.Pending };
                            companyStore.Add(newCompany);
                            Console.WriteLine($"[Success]: Added new company '{name}' with ID {newCompany.Id} and status 'Pending'.");
                            break;
                         case 5:
                            Console.Write("Enter Company ID to update: ");
                            if (int.TryParse(Console.ReadLine(), out int updateId))
                            {
                                var company1 = companyStore.GetById(updateId);
                                if (company1 != null)
                                {
                                    Console.Write("Enter new Company Name: ");
                                    string newName = Console.ReadLine();
                                    Console.Write("Enter new Industry: ");
                                    string newIndustry = Console.ReadLine();
                                    Console.Write("Enter new City: ");
                                    string newCity = Console.ReadLine();

                                    company1.Name = newName;
                                    company1.Industry = newIndustry;
                                    company1.City = newCity;
                                    Console.WriteLine($"Updated company '{company1.Name}' (ID: {company1.Id}).");
                                }
                                else
                                {
                                    Console.WriteLine("Company not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Company ID.");
                            }
                            break;
                         case 6:
                            Console.WriteLine("Enter Company ID to remove: ");
                            if (int.TryParse(Console.ReadLine(), out int removeId))
                            {
                                var company2 = companyStore.GetById(removeId);
                                if (company2 != null)
                                {
                                    companyStore.Remove(company2);
                                    Console.WriteLine($"Removed company '{company2.Name}' (ID: {company2.Id}).");
                                }
                                else
                                {
                                    Console.WriteLine("Company not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Company ID.");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
            static void JobMenu(DataStore<Job> jobStore, DataStore<Company> companyStore, Dictionary<int, Job> jobDict)
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

                static void CandidateMenu(DataStore<Candidate> candidateStore, HashSet<string> emails)
            {
                while (true)
                {
                    Console.WriteLine("==== Candidate Management ====");
                    Console.WriteLine("1. List Candidates");
                    Console.WriteLine("2. Add Candidate");
                    Console.WriteLine("3. Back");

                    if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 3) break;
                    switch (choice)
                    {
                        case 1:
                            foreach (var c in candidateStore.GetAll())
                                Console.WriteLine($"ID: {c.Id} | Full Name: {c.FullName} | Email: {c.Email} | Phone: {c.Phone} | Year of Experience: {c.YearsOfExperience}");
                            break;
                        case 2:
                            Console.Write("Enter Full Name: "); string name = Console.ReadLine();
                            Console.Write("Enter Email: "); string email = Console.ReadLine();
                            Console.Write("Enter Phone: "); string phone = Console.ReadLine();
                            Console.Write("Enter Year Of Experience: "); string yearOfExperience = Console.ReadLine();
                            if (emails.Contains(email))
                            {
                                Console.WriteLine("Email already exists! Must be unique.");
                                continue;
                            }
                            int maxId = 0;
                            foreach (var c in candidateStore.GetAll()) if (c.Id > maxId) maxId = c.Id;
                            candidateStore.Add(new Candidate { Id = maxId + 1, FullName = name, Email = email });
                            emails.Add(email);
                            Console.WriteLine("Candidate added!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
            
            

                    static void ApplyToJob(DataStore<Candidate> candStore, Dictionary<int, Job> jobDict, DataStore<Company> compStore, DataStore<Application> appStore, HashSet<(int JobId, int CandidateId)> appSet)
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
    
        