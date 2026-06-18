namespace StudentList
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double AverageGrade { get; set; }

        public Student(string name, int age, double averageGrade)
        {
            Name = name;
            Age = age;
            AverageGrade = averageGrade;
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<Student> students = new List<Student>
            {
                new Student("Петров Иван", 20, 85.5),
                new Student("Сидорова Анна", 23, 78.1),
                new Student("Кузнецов Олег", 19, 74.9),
                new Student("Васильева Мария", 26, 72.0),
                new Student("Смирнов Алексей", 22, 95.2)
            };
            
            Console.WriteLine("--- Список студентов-хорошистов (балл от 75 до 90) ---");
            var goodStudents = students.Where(s => s.AverageGrade >= 75 && s.AverageGrade <= 90);
            foreach (var s in goodStudents)
            {
                Console.WriteLine($"{s.Name} - {s.AverageGrade:F1}");
            }
            
            Console.WriteLine("\n--- Все студенты ---");
            List<string> studentNames = students.Select(s => s.Name).ToList();
            foreach (var name in studentNames)
            {
                Console.WriteLine(name);
            }
            
            Console.WriteLine("\n--- Сортировка по возрасту ---");
            var sortedByAge = students.OrderBy(s => s.Age);
            foreach (var s in sortedByAge)
            {
                string ageWord = GetAgeString(s.Age);
                Console.WriteLine($"{s.Name} - {s.Age} {ageWord}");
            }
            
            Console.WriteLine("\n--- Рейтинг студентов младше 25 лет (по убыванию балла) ---");
            var rating = students
                .Where(s => s.Age < 25)
                .OrderByDescending(s => s.AverageGrade)
                .Select(s => $"{s.Name} - {s.AverageGrade:F1}");

            foreach (string row in rating)
            {
                Console.WriteLine(row);
            }
        }
        
        public static string GetAgeString(int age)
        {
            int lastTwoDigits = age % 100;
            int lastDigit = age % 10;
            
            if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            {
                return "лет";
            }
            
            return lastDigit switch
            {
                1 => "год",
                >= 2 and <= 4 => "года",
                _ => "лет"
            };
        }
    }
}
