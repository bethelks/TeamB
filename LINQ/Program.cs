using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json;
namespace StudentGrades
{
    public class Student
    {
        public string Name { get; set; }
        public int Grade { get; set; }

        public Student(string name, int grade)
        {
            Name = name;
            Grade = grade;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = LoadStudentsFromFile("students.json") ?? new List<Student>();
            //Console.WriteLine("All Students:");
            //DisplayStudents(students);


            //List<Student> students = new List<Student>
            if (!students.Any())
            {
                students = new List<Student>
                {
                new Student("Alice", 85),
                new Student("Bob", 76),
                new Student("Charlie", 92),
                new Student("Diana", 88),
                new Student("Evan", 65),
                new Student("Fay", 95)
                };
            }
            Console.WriteLine("All Students:");
            DisplayStudents(students);

            //Search for a student by name
            bool searching = true;
            while (searching)
            {
                Console.WriteLine("\nEnter the name of the student to search for:");
                string studentName = Console.ReadLine();
                Student foundStudent = FindStudentByName(students, studentName);

                if (foundStudent != null)
                {
                    Console.WriteLine($"\nStudent found: {foundStudent.Name}, Grade: {foundStudent.Grade}");
                }
                else
                {
                    Console.WriteLine($"\nStudent with the name '{studentName}' not found");
                }

                //Ask if the user wants to search agian
                Console.WriteLine("Do you want to try again? (yes/no):");
                string response = Console.ReadLine().Trim().ToLower();

                if (response != "yes")
                {
                    searching = false; //Set searhing to false to exit the loop
                }

                //Option to save students to a file
                Console.WriteLine("Do you want to save the student data? (yes/no):");
                string saveResponse = Console.ReadLine();
                if (saveResponse?.Trim().ToLower() == "yes")
                {
                    SaveStudentsToFile(students, "students.json");
                    Console.WriteLine("Student data saved to file.");
                }
            }

                // Use LINQ to filter students with grades greater than 80
                var highAchievers = students.Where(s => s.Grade > 80);
            Console.WriteLine("\nStudents with grades greater than 80:");
            DisplayStudents(highAchievers);

            // Use LINQ to sort students by grade
            var sortedStudents = students.OrderByDescending(s => s.Grade);
            Console.WriteLine("\nStudents sorted by grades (highest to lowest):");
            DisplayStudents(sortedStudents);

            // Calculate average grade
            double averageGrade = students.Average(s => s.Grade);
            Console.WriteLine($"\nAverage grade of all students: {averageGrade}");

        }

        private static string ToLower()
        {
            throw new NotImplementedException();
        }

        //Method to Seach for student by name
        static Student FindStudentByName(List<Student> students, string name)
        {
            return students.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        static void DisplayStudents(IEnumerable<Student> students)
        {
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name}, Grade: {student.Grade}");
            }
        }

        static void SaveStudentsToFile(List<Student> students, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; // Pretty print
            string jsonString =
                JsonConvert.SerializeObject(students, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }
        static List<Student> LoadStudentsFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return 
                    JsonConvert.DeserializeObject<List<Student>>(jsonString);
            }
            return null;
        }

        private class JsonSerializerOptions
        {
            public bool WriteIndented { get; set; }
        }
    }
}

