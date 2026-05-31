using TextFileEditorApp.Models;

namespace TextFileEditorApp.Services;

// Консольный текстовый редактор с поддержкой отката изменений (Undo) и сохранения в файлы
public class ConsoleTextEditor
{
    // Текущий редактируемый текстовый файл
    private TextFile _textFile;
    
    // История изменений для возможности отката (паттерн Memento)
    private readonly TextFileHistory _history = new();

    // Конструктор: принимает объект TextFile для редактирования
    public ConsoleTextEditor(TextFile textFile)
    {
        _textFile = textFile;
    }

    // Запускает главный цикл консольного редактора с меню
    public void Start()
    {
        bool running = true;

        while (running)
        {
            // Отображаем главное меню
            Console.WriteLine("\n--- Консольный редактор ---");
            Console.WriteLine("1. Показать текст");
            Console.WriteLine("2. Добавить текст");
            Console.WriteLine("3. Заменить весь текст");
            Console.WriteLine("4. Откатить изменение");
            Console.WriteLine("5. Сохранить как XML");
            Console.WriteLine("6. Сохранить как Binary");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");

            string? choice = Console.ReadLine();

            // Обработка выбора пользователя
            switch (choice)
            {
                case "1":
                    ShowText();      // Отображение содержимого
                    break;

                case "2":
                    AddText();       // Добавление текста в конец
                    break;

                case "3":
                    ReplaceText();   // Полная замена текста
                    break;

                case "4":
                    Undo();          // Отмена последнего изменения
                    break;

                case "5":
                    SaveXml();       // Сохранение в XML-файл
                    break;

                case "6":
                    SaveBinary();    // Сохранение в бинарный (JSON) файл
                    break;

                case "0":
                    running = false; // Выход из программы
                    break;

                default:
                    Console.WriteLine("Неверный пункт меню.");
                    break;
            }
        }
    }

    // 1. Показать текущее содержимое файла
    private void ShowText()
    {
        Console.WriteLine("\n--- Содержимое файла ---");
        Console.WriteLine(_textFile.Content);
    }

    // 2. Добавить текст (сохраняем состояние перед изменением)
    private void AddText()
    {
        // Сохраняем текущее состояние для возможности отката
        _history.Save(_textFile.SaveState());

        Console.Write("Введите текст для добавления: ");
        string? text = Console.ReadLine();

        // Добавляем введённый текст (если null, то пустую строку)
        _textFile.AddText(text ?? string.Empty);
    }

    // 3. Полностью заменить текст (сохраняем состояние перед изменением)
    private void ReplaceText()
    {
        // Сохраняем текущее состояние для возможности отката
        _history.Save(_textFile.SaveState());

        Console.Write("Введите новый текст: ");
        string? text = Console.ReadLine();

        // Заменяем содержимое
        _textFile.ReplaceContent(text ?? string.Empty);
    }

    // 4. Откатить последнее изменение (Undo)
    private void Undo()
    {
        // Получаем предыдущее состояние из истории
        TextFileMemento? previousState = _history.Undo();

        if (previousState == null)
        {
            Console.WriteLine("Нет изменений для отката.");
            return;
        }

        // Восстанавливаем сохранённое состояние
        _textFile.RestoreState(previousState);
        Console.WriteLine("Изменение отменено.");
    }

    // 5. Сохранить в XML-файл
    private void SaveXml()
    {
        Console.Write("Введите путь для XML-файла: ");
        string? path = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(path))
        {
            TextFileSerializer.SaveAsXml(_textFile, path);
            Console.WriteLine("Файл сохранён в XML.");
        }
    }

    // 6. Сохранить в бинарный (JSON) файл
    private void SaveBinary()
    {
        Console.Write("Введите путь для бинарного файла: ");
        string? path = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(path))
        {
            TextFileSerializer.SaveAsBinary(_textFile, path);
            Console.WriteLine("Файл сохранён в бинарном виде.");
        }
    }
}
