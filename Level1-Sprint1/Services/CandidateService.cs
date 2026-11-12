using Level1_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Services
{
    internal class CandidateService
    {
        public static void CandidateMenu(DataStore<Candidate> candidateStore, HashSet<string> emails)
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
    }
}
