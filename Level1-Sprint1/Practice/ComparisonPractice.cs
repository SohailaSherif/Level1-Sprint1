using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1.Practice
{
    internal class ComparisonPractice
    {
        public static void Run()
        {
            Comparison<int> compareNumbers = (first, second) =>
    first.CompareTo(second);

            int result = compareNumbers(10, 20);

            Console.WriteLine(result); 

            Comparison<int> compareNum = (x, y) => x.CompareTo(y);
            List<int> numbers = new List<int> { 5, 2, 8, 1, 4 };
            numbers.Sort(compareNum);
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }
            Console.WriteLine("================================");
        }
    }
}
