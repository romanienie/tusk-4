namespace TextFileEditorApp.Services;

// Сервис для индексации текстовых файлов по ключевым словам
public class TextFileIndexer
{
    // Создаёт индекс: для каждого ключевого слова - список файлов, где оно встречается
    public Dictionary<string, List<string>> IndexFiles(string directoryPath, List<string> keywords)
    {
        // Инициализируем словарь: каждому ключевому слову - пустой список
        Dictionary<string, List<string>> index = new();

        foreach (string keyword in keywords)
        {
            index[keyword] = new List<string>();
        }

        // Проверяем существование директории
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Директория не найдена.");
            return index; // Возвращаем пустой индекс
        }

        // Получаем все .txt файлы из директории и всех вложенных папок
        string[] files = Directory.GetFiles(directoryPath, "*.txt", SearchOption.AllDirectories);

        // Перебираем каждый файл
        foreach (string file in files)
        {
            // Читаем содержимое и приводим к нижнему регистру
            string content = File.ReadAllText(file).ToLower();

            // Проверяем каждое ключевое слово
            foreach (string keyword in keywords)
            {
                // Если файл содержит ключевое слово (регистронезависимо)
                if (content.Contains(keyword.ToLower()))
                {
                    // Добавляем путь к файлу в список для этого ключевого слова
                    index[keyword].Add(file);
                }
            }
        }

        return index;
    }

    // Выводит сформированный индекс в консоль в удобочитаемом виде
    public void PrintIndex(Dictionary<string, List<string>> index)
    {
        Console.WriteLine("\nРезультат индексации:");

        // Перебираем каждую пару ключевое слово -> список файлов
        foreach (var item in index)
        {
            Console.WriteLine($"\nКлючевое слово: {item.Key}");

            // Если список файлов пуст
            if (item.Value.Count == 0)
            {
                Console.WriteLine("  Файлы не найдены.");
            }
            else
            {
                // Выводим все найденные файлы для данного ключевого слова
                foreach (string file in item.Value)
                {
                    Console.WriteLine($"  {file}");
                }
            }
        }
    }
}
