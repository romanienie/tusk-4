using TextFileEditorApp.Models;
using TextFileEditorApp.Services;

namespace TextFileEditorApp;

// Главный класс программы - точка входа в приложение
internal class Program
{
    // Точка входа в приложение
    static void Main()
    {
        bool running = true;

        // Главный цикл программы
        while (running)
        {
            // Отображение главного меню
            Console.WriteLine("\n=== Главное меню ===");
            Console.WriteLine("1. Создать текстовый файл и открыть редактор");
            Console.WriteLine("2. Загрузить файл из XML");
            Console.WriteLine("3. Загрузить файл из Binary");
            Console.WriteLine("4. Поиск файлов по ключевым словам");
            Console.WriteLine("5. Индексация файлов в директории");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");

            string? choice = Console.ReadLine();

            // Обработка выбора пользователя
            switch (choice)
            {
                case "1":
                    CreateAndEditFile();     // Создание нового файла
                    break;

                case "2":
                    LoadXmlAndEdit();        // Загрузка из XML
                    break;

                case "3":
                    LoadBinaryAndEdit();     // Загрузка из Binary (JSON)
                    break;

                case "4":
                    SearchFiles();           // Поиск файлов по ключевым словам
                    break;

                case "5":
                    IndexFiles();            // Индексация файлов в директории
                    break;

                case "0":
                    running = false;         // Выход из программы
                    break;

                default:
                    Console.WriteLine("Неверный пункт меню.");
                    break;
            }
        }
    }

    // 1. Создание нового текстового файла и открытие редактора
    private static void CreateAndEditFile()
    {
        Console.Write("Введите имя файла: ");
        string fileName = Console.ReadLine() ?? "new_file.txt";

        Console.Write("Введите начальный текст: ");
        string content = Console.ReadLine() ?? string.Empty;

        // Создаём объект TextFile с введёнными данными
        TextFile textFile = new TextFile(fileName, content);
        
        // Запускаем консольный редактор для этого файла
        ConsoleTextEditor editor = new ConsoleTextEditor(textFile);
        editor.Start();
    }

    // 2. Загрузка файла из XML и открытие редактора
    private static void LoadXmlAndEdit()
    {
        Console.Write("Введите путь к XML-файлу: ");
        string? path = Console.ReadLine();

        // Проверяем, что путь указан и файл существует
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        // Десериализуем XML в объект TextFile
        TextFile textFile = TextFileSerializer.LoadFromXml(path);
        
        // Запускаем редактор
        ConsoleTextEditor editor = new ConsoleTextEditor(textFile);
        editor.Start();
    }

    // 3. Загрузка файла из Binary (JSON) и открытие редактора
    private static void LoadBinaryAndEdit()
    {
        Console.Write("Введите путь к бинарному файлу: ");
        string? path = Console.ReadLine();

        // Проверяем существование файла
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            Console.WriteLine("Файл не найден.");
            return;
        }

        // Десериализуем JSON в объект TextFile
        TextFile textFile = TextFileSerializer.LoadFromBinary(path);
        
        // Запускаем редактор
        ConsoleTextEditor editor = new ConsoleTextEditor(textFile);
        editor.Start();
    }

    // 4. Поиск файлов по ключевым словам
    private static void SearchFiles()
    {
        Console.Write("Введите путь к директории: ");
        string directoryPath = Console.ReadLine() ?? string.Empty;

        // Получаем список ключевых слов от пользователя
        List<string> keywords = ReadKeywords();

        // Выполняем поиск
        TextFileSearcher searcher = new TextFileSearcher();
        List<string> foundFiles = searcher.SearchFilesByKeywords(directoryPath, keywords);

        // Выводим результаты
        Console.WriteLine("\nНайденные файлы:");

        if (foundFiles.Count == 0)
        {
            Console.WriteLine("Файлы не найдены.");
        }
        else
        {
            foreach (string file in foundFiles)
            {
                Console.WriteLine(file);
            }
        }
    }

    // 5. Индексация файлов в директории
    private static void IndexFiles()
    {
        Console.Write("Введите путь к директории: ");
        string directoryPath = Console.ReadLine() ?? string.Empty;

        // Получаем ключевые слова
        List<string> keywords = ReadKeywords();

        // Создаём индекс
        TextFileIndexer indexer = new TextFileIndexer();
        Dictionary<string, List<string>> index = indexer.IndexFiles(directoryPath, keywords);

        // Выводим индекс в консоль
        indexer.PrintIndex(index);
    }

    // Вспомогательный метод: читает ключевые слова, введённые пользователем через запятую
    private static List<string> ReadKeywords()
    {
        Console.Write("Введите ключевые слова через запятую: ");
        string input = Console.ReadLine() ?? string.Empty;

        // Разделяем строку по запятым, удаляем лишние пробелы и пустые элементы
        return input
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.Trim())
            .Where(word => word.Length > 0)
            .ToList();
    }
}
