using Level1_Sprint1.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Level1_Sprint1.Models;
using Level1_Sprint1.Services;

namespace Level1_Sprint1.Services
{
    internal class NotificationSystem
    {
        public static void OnCompanyApproved(object? sender, CompanyApproved e)
        {
            Console.WriteLine($"Notification: Company '{e.ApprovedCompany.Name}' has been approved.");
        }
        public static void OnApplicationSubmitted(object? sender, ApplicationSubmitted e)
        {
            Console.WriteLine($"Notification: Candidate '{e.SubmittedCandidate.FullName}' has applied for the job '{e.SubmittedJob.Title}'.");
        }
        public static void OnApplicationStatusChanged(object? sender, ApplicationStatusChanged e)
        {
            Console.WriteLine($"Notification: Application #{e.Application.Id} status changed from '{e.OldStatus}' to '{e.NewStatus}'.");
        }
        public static void SafeInvoke<TEventArgs>(EventHandler<TEventArgs>? eventHandler, object? sender, TEventArgs e) where TEventArgs : EventArgs
        {
            if (eventHandler != null)
            {
                foreach (EventHandler<TEventArgs> handler in eventHandler.GetInvocationList())
                {
                    try
                    {
                        handler(sender, e);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error invoking event handler: {ex.Message}");
                    }
                }
            }
        }
    }
}
