namespace Task13;

class Program
{
    public static void Main()
    {
        var student = new Student
        {
            FirstName = "Георгий",
            LastName = "Поцелуйко",
            BirthDate = new DateTime(2007, 01, 27),
            NullField = null,
            Grades = new List<Subject>
            {
                new Subject {Name = "Algebra", Grade = 5},
                new Subject {Name = "Analysis", Grade = 4},
                new Subject {Name = "DiscreteMath", Grade = 5},
                new Subject {Name = "Geometry", Grade = 4},
                new Subject {Name = "Programming", Grade = 5}
            }
        };

        var s = new Serialization();
        string filePath = "main.json";

        s.SaveToJson(student, filePath);
        s.LoadFromJson<Student>(filePath);
    }
}
