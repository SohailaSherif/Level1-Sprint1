using Level1_Sprint1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Services
{
    internal class CompanyService
    {
        public static void CompanyMenu(DataStore<Company> companyStore, Stack<(int, CStatus)> undoStack)
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
    }
}
