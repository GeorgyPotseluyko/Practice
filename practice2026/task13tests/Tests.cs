using Xunit;

namespace Task13.Tests;

public class Tests
{
    [Fact]
    public void SerializationShouldIgnoreNullValues()
    {
        var s = new Serialization();
        string testFilePath = "test1.json";

        var obj = new Student() 
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

        try
        {
            s.SaveToJson(obj, testFilePath);
            var loadedObj = s.LoadFromJson<Student>(testFilePath);

            string json = File.ReadAllText(testFilePath);

            Assert.DoesNotContain("NullField", json); 

            Assert.Equal(obj.FirstName, loadedObj.FirstName);

            Assert.Null(loadedObj.NullField);
        }
        finally
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);  
            }
        }
    }

    [Fact]
    public void SerializationShouldFormatDateCorrectly()
    {
        var s = new Serialization();
        string testFilePath = "test2.json";

        var obj = new Student() 
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

        try
        {
            s.SaveToJson(obj, testFilePath);
            var loadedObj = s.LoadFromJson<Student>(testFilePath);

            string json = File.ReadAllText(testFilePath);

            Assert.Contains("27.01.2007", json); 

            Assert.Equal(obj.BirthDate, loadedObj.BirthDate);

            Assert.Equal(obj.FirstName, loadedObj.FirstName);
        }
        finally
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }
    }
}
