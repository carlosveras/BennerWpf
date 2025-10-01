using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable disable

namespace BennerWpf.Services;

public class DataService<T>
{
    private readonly string _filePath;

    public DataService(string fileName)
    {
        var folder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, fileName);
    }

    public List<T> Load()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true
        }) ?? new List<T>();
    }

    public void Save(List<T> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        File.WriteAllText(_filePath, json);
    }
}
