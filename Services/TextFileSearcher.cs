namespace TextFileEditorApp.Services;

// Сервис для поиска текстовых файлов по ключевым словам
public class TextFileSearcher
{
    // Ищет все .txt файлы в указанной директории и поддиректориях,
    // содержащие хотя бы одно из ключевых слов
    public List<string> SearchFilesByKeywords(string directoryPath, List<string> keywords)
    {
        // Список для хранения путей к найденным файлам
        List<string> foundFiles = new();

        // Проверяем существование директории
        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine("Директория не найдена.");
            return foundFiles; // Возвращаем пустой список
        }

        // Получаем все .txt файлы из директории и всех вложенных папок
        string[] files = Directory.GetFiles(directoryPath, "*.txt", SearchOption.AllDirectories);

        // Перебираем каждый найденный файл
        foreach (string file in files)
        {
            // Читаем содержимое файла и приводим к нижнему регистру
            // (для регистронезависимого поиска)
            string content = File.ReadAllText(file).ToLower();

            // Проверяем каждое ключевое слово
            foreach (string keyword in keywords)
            {
                // Если содержимое содержит ключевое слово (регистронезависимо)
                if (content.Contains(keyword.ToLower()))
                {
                    foundFiles.Add(file); // Добавляем путь к файлу в результат
                    break; // Выходим из цикла по ключевым словам (файл уже найден)
                }
            }
        }

        // Возвращаем список найденных файлов
        return foundFiles;
    }
}
