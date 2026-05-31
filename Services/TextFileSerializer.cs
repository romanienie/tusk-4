using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;
using TextFileEditorApp.Models;

namespace TextFileEditorApp.Services;

// Статический класс для сериализации/десериализации TextFile в различных форматах
public static class TextFileSerializer
{
    // Сохраняет TextFile в XML-файл по указанному пути
    public static void SaveAsXml(TextFile textFile, string path)
    {
        // Создаём XML-сериализатор для типа TextFile
        XmlSerializer serializer = new XmlSerializer(typeof(TextFile));

        // Открываем поток для записи файла (создаст или перезапишет)
        using FileStream stream = new FileStream(path, FileMode.Create);
        // Сериализуем объект в XML и записываем в поток
        serializer.Serialize(stream, textFile);
    }

    // Загружает TextFile из XML-файла
    public static TextFile LoadFromXml(string path)
    {
        // Создаём XML-сериализатор для типа TextFile
        XmlSerializer serializer = new XmlSerializer(typeof(TextFile));

        // Открываем поток для чтения файла
        using FileStream stream = new FileStream(path, FileMode.Open);
        // Десериализуем XML обратно в объект TextFile
        return (TextFile)serializer.Deserialize(stream)!;
    }

    // Сохраняет TextFile в JSON-файл (метод назван Binary, но использует JSON)
    public static void SaveAsBinary(TextFile textFile, string path)
    {
        // Преобразуем объект в JSON-строку
        string json = JsonSerializer.Serialize(textFile);
        // Записываем JSON-строку в файл
        File.WriteAllText(path, json);
    }

    // Загружает TextFile из JSON-файла
    public static TextFile LoadFromBinary(string path)
    {
        // Читаем весь текст из файла
        string json = File.ReadAllText(path);
        // Десериализуем JSON обратно в объект TextFile
        return JsonSerializer.Deserialize<TextFile>(json)!;
    }
}
