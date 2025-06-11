using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BointApp.Models;
using Newtonsoft.Json;

namespace BointApp.Services;

// Klasa do przechowywania wszystkich danych w jednym obiekcie
public class DataContainer
{
    public List<User> Users { get; set; } = new();
    public List<Bike> Bikes { get; set; } = new();
    public List<Station> Stations { get; set; } = new();
    // Na razie pomijamy zapisywanie aktywnych wypożyczeń, aby uprościć proces.
    // Można to dodać później.
}

public class JsonDataService
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonDataService(string fileName)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolderPath = Path.Combine(appDataPath, "BointApp");
        Directory.CreateDirectory(appFolderPath);
        _filePath = Path.Combine(appFolderPath, fileName);

        _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            
            PreserveReferencesHandling = PreserveReferencesHandling.Objects, 
            
            Formatting = Newtonsoft.Json.Formatting.Indented
        };
    }

    public void SaveData(DataContainer data)
    {
        string json = JsonConvert.SerializeObject(data, _settings);
        File.WriteAllText(_filePath, json);
    }

    public DataContainer? LoadData()
    {
        if (!File.Exists(_filePath))
        {
            return null;
        }

        string json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<DataContainer>(json, _settings);
    }
}