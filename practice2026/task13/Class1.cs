using Newtonsoft.Json;
using System.IO;

namespace Task13;
public class Subject
{
    public string Name { get; set; }
    public int Grade { get; set; }
    public string NullField { get; set; }
}

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Subject> Grades { get; set; }
    public string NullField { get; set; }
}

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

public class Serialization
{
    readonly JsonSerializerSettings options = new JsonSerializerSettings
    { 
        NullValueHandling = NullValueHandling.Ignore,
        DateFormatString = "dd.MM.yyyy" 
    };

    public void SaveToJson(object obj, string path)
    {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented, options);
        File.WriteAllText(path, json);
    }

    public T LoadFromJson<T>(string path)
    {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json, options);
    }
}
