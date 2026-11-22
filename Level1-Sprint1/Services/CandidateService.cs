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
                Console.WriteLine("3. Update Candidate");
                Console.WriteLine("4. Remove Candidate");
                Console.WriteLine("5. Back to Main Menu");

                if (!int.TryParse(Console.ReadLine(), out int choice) || choice == 5) break;
                switch (choice)
                {
                    case 1:
                        foreach (var c in candidateStore.GetAll())
                            Console.WriteLine($"ID: {c.Id} | Full Name: {c.FullName} | Email: {c.Email} | Phone: {c.Phone} | Year of Experience: {c.YearsOfExperience}");
                        break;
                    case 2:
                        Console.Write("Enter Full Name: "); string name = Console.ReadLine();
                        Console.Write("Enter Email: "); string email = Console.ReadLine()?.Trim().ToLowerInvariant();

                        Console.Write("Enter Phone: "); string phone = Console.ReadLine();
                        Console.Write("Enter Year Of Experience: ");
                        int.TryParse(Console.ReadLine(), out int yearOfExperience);
                        if (emails.Contains(email))
                        {
                            Console.WriteLine("Email already exists! Must be unique.");
                            continue;
                        }
                        int maxId = 0;
                        foreach (var c in candidateStore.GetAll()) if (c.Id > maxId) maxId = c.Id;
                        candidateStore.Add(new Candidate { Id = maxId + 1, FullName = name, Email = email , Phone = phone , YearsOfExperience = yearOfExperience });
                        emails.Add(email);
                        Console.WriteLine("Candidate added!");
                        break;
                    case 3:
                        Console.Write("Enter Candidate ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int CandidateId))
                        {
                            var candidateToUpdate = candidateStore.GetById(CandidateId);
                            if (candidateToUpdate != null)
                            {
                                Console.Write("Enter new Full Name: ");
                                string newName = Console.ReadLine();
                                Console.Write("Enter new Email: ");
                                string newEmail = Console.ReadLine()?.Trim().ToLowerInvariant();
                                Console.Write("Enter new Phone: ");
                                string newPhone = Console.ReadLine();
                                Console.Write("Enter new Year Of Experience: ");
                                int.TryParse(Console.ReadLine(), out int newYearOfExperience);
                                candidateToUpdate.FullName = newName;
                                candidateToUpdate.Email = newEmail;
                                candidateToUpdate.Phone = newPhone;
                                candidateToUpdate.YearsOfExperience = newYearOfExperience;

                            }
                        }
                            break;
                    case 4:
                        Console.Write("Enter Candidate ID to remove: ");
                        if (!int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            Console.WriteLine("Invalid ID.");
                            continue;
                        }
                        var candidateToRemove = candidateStore.GetById(removeId);
                        if (candidateToRemove == null)
                        {
                            Console.WriteLine("Candidate not found.");
                            continue;
                        }
                        candidateStore.Remove(candidateToRemove);
                        emails.Remove(candidateToRemove.Email);
                        Console.WriteLine("Candidate removed!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
