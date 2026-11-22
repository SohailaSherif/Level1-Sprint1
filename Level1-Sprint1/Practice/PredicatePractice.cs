using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Practice
{
    internal class PredicatePractice
    {
        public static void Run()
        {
            Predicate<int> isEligible = age => age >= 18;

            bool res = isEligible(21);

            Console.WriteLine(res);

            Predicate<int> isEven = number => number % 2 == 0;
            bool result = isEven(5);
            Console.WriteLine(result);
            Console.WriteLine("================================");
        }
    }
}
