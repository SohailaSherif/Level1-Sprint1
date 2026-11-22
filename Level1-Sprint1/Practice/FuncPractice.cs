using System;
using Level1_Sprint1.Practice;
using System.Collections.Generic;
using System.Text;
namespace Level1_Sprint1.Practice
{
    public class FuncPractice
    {
        public static void Run()
        {
            Func<int, int> square = number => number * number;

            int result = square(5);

            Console.WriteLine(result);

            Func<decimal, string> formatSalary = salary => $"{salary:N0} EGP";

            string display = formatSalary(15000);
            Console.WriteLine(display);
            Console.WriteLine("================================");
        }
    }
}
