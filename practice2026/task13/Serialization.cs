using Newtonsoft.Json;
using System.IO;

namespace Task13;

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
