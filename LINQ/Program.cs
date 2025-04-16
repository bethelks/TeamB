using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
        private static bool running = true; // Initialize to ture to start the main loop.

        static void Main(string[] args)
        {
            List<Student> students = LoadStudentsFromFile("students.json") ?? new List<Student>();
           
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

            //Main Loop
            while (running)
            {
                //User interatction options
                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("1. Search for a student");
                Console.WriteLine("2. Add a new student");
                Console.WriteLine("3. Remove a student");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SearchForStudent(students);
                        break;
                    case "2":
                        AddNewStudent(students);
                        break;
                    case "3":
                        RemoveStudent(students);
                        break;
                    case "4":
                        SaveStudentsToFile(students, "students.json");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }

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

            //List Students Bellow Average
            var studentsBelowAverage = students.Where(s => s.Grade < averageGrade);
            Console.WriteLine("\nStudents below average grade:");
            DisplayStudents(studentsBelowAverage);

            //List top n student
            var topStudent = sortedStudents.First();
            Console.WriteLine("\nThe Top Student is:");
            Console.WriteLine($"{topStudent.Name}, Grade: {topStudent.Grade}");

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

        static void SearchForStudent(List<Student> students)
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
        }

        static void AddNewStudent(List<Student> students)
        {
            Console.WriteLine("Enter the name of the new student:");
            string name = Console.ReadLine().Trim();

            //Check if the student already exists
            if (students.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Student with the name '{name}' already exists.");
                return;
            }
            Console.WriteLine("Enter the grade of the new student:");

            if (int.TryParse(Console.ReadLine(), out int grade))
            {
                students.Add(new Student(name, grade));
                Console.WriteLine($"new stuend '{name}' with grade {grade} added.");
            }
            else
            {
                Console.WriteLine("Invalid grade input. Please enter a numerical value.");
            }
        }

        static void RemoveStudent (List<Student> students)
        {
            Console.WriteLine("Enter the name of the student to remove:");
            string studentName = Console.ReadLine().Trim();

            //Check if the student exists and remove them
            var studentToRemove = students.FirstOrDefault(s => s.Name.Equals(studentName, StringComparison.OrdinalIgnoreCase));
            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                Console.WriteLine($"Student '{studentName}' has been removed.");

                //Save after removing
                SaveStudentsToFile(students, "students.json");
            }
            else
            {
                Console.WriteLine($"Student with the name '{studentName}' not found.");
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

