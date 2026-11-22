using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Practice
{
    internal class ActionPractice
    {
        public static void Run()
        {
            Action<string> printMessage = message =>
        {
            Console.WriteLine(message);
        };

            printMessage("Application submitted successfully");


            Action<string> logInfo = message => Console.WriteLine($"INFO: {DateTime.Now:HH:mm:ss} - {message}");

            logInfo("Company profile loaded.");
            Console.WriteLine("================================");
        }
    }
}
