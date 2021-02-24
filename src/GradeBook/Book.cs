using System;
using System.Collections.Generic;
using System.IO;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);

    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        protected Book(string name) : base(name)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;
        public abstract void AddGrade(double grade);
        public abstract Statistics GetStatistics();
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            using(var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
        }

        public override Statistics GetStatistics()
        {
            Statistics result = new Statistics();

            using(var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while(line != null)
                {
                    double number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }

            return result;
        }
    }

    public class InMemoryBook : Book
    {
        public InMemoryBook(string name) : base(name)
        {
            grades = new List<double>();
            Name = name;
        }

        public void AddGrade(char letter)
        {
            switch(letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                case 'D':
                    AddGrade(60);
                    break;
                case 'F':
                    AddGrade(50);
                    break;
                default:
                    AddGrade(0);
                    break;
            }
        }

        public override void AddGrade(double grade)
        {
            if(grade <= 100 && grade >= 0)
            {
                grades.Add(grade);

                if(GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        public List<double> getAllGrades()
        {
            return grades;
        }

        public void ShowAllGrades()
        {
            Console.WriteLine("All Grades:");
            grades.ForEach(grade => Console.Write("{0}\t", grade));
        }


        public int getGradeCount()
        {
            return grades.Count;
        }

        public double getGradeSum()
        {
            double result = 0.0;

            foreach(double grade in grades)
            {
                result += grade;
            }

            return result;
        }

        public Dictionary<string, double> getMaxMinGrades()
        {
            Dictionary<string, double> maxMinGrades = new Dictionary<string, double>();


            double highestGrade = double.MinValue;
            double lowestGrade = double.MaxValue;

            foreach(double grade in grades)
            {
                highestGrade = Math.Max(grade, highestGrade);
                lowestGrade = Math.Min(grade, lowestGrade);
            }

            maxMinGrades.Add("highest", highestGrade);
            maxMinGrades.Add("lowest", lowestGrade);

            return maxMinGrades;
        }

        public double getHighestGrade()
        {
            return getMaxMinGrades()["highest"];
        }

        public double getLowestGrade()
        {
            return getMaxMinGrades()["lowest"];
        }

        public double getGradeAverage()
        {
            return getGradeSum() / getGradeCount();
        }

        public override Statistics GetStatistics()
        {
            Statistics result = new Statistics();

            for(var i = 0; i < grades.Count; i++)
            {
                result.Add(grades[i]);
            }

            return result;
        }

        private List<double> grades;

        public const string CATEGORY = "Science";
    }
}