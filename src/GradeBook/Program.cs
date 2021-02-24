using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {
            // InMemoryBook book = new InMemoryBook("Josiah's GradeBook");
            IBook book = new DiskBook("Josiah's Disk GradeBook");
            book.GradeAdded += OnGradeAdded;

            EnterGrades(book);

            // .. end loop

            // book.ShowAllGrades();
            Console.WriteLine();
            Console.WriteLine();

            Statistics stats = book.GetStatistics();

            Console.WriteLine($"For the book named {book.Name}");
            Console.WriteLine($"Highest Grade: {stats.High:N2}");
            Console.WriteLine($"Lowest Grade: {stats.Low:N2}");
            Console.WriteLine($"Average Grade: {stats.Average:N2}");
            Console.WriteLine($"Letter Grade: {stats.Letter}");
        }

        private static void EnterGrades(IBook book)
        {
            while (true)
            {
                Console.WriteLine("Enter a grade, or 'q' to quit");
                var input = Console.ReadLine();

                if (input == "q")
                {
                    break;
                }

                try
                {
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("**");
                }

            }
        }

        static void OnGradeAdded(object sender, EventArgs e)
        {
            Console.WriteLine("A Grade was Added!");
        }
    }
}
