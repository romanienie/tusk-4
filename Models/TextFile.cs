using System.Xml.Serialization;

namespace TextFileEditorApp.Models;

[Serializable]
public class TextFile
{
    // Имя файла (путь или название)
    public string FileName { get; set; } = string.Empty;
    
    // Содержимое текстового файла
    public string Content { get; set; } = string.Empty;

    // Конструктор по умолчанию (требуется для сериализации)
    public TextFile()
    {
    }

    // Конструктор для инициализации файла с именем и содержимым
    public TextFile(string fileName, string content)
    {
        FileName = fileName;
        Content = content;
    }

    // Добавляет текст в конец текущего содержимого
    public void AddText(string text)
    {
        Content += text;
    }

    // Полностью заменяет содержимое файла
    public void ReplaceContent(string newContent)
    {
        Content = newContent;
    }

    // Сохраняет текущее состояние (снимок) для возможности отката (Memento pattern)
    public TextFileMemento SaveState()
    {
        return new TextFileMemento(Content);
    }

    // Восстанавливает состояние из сохранённого снимка
    public void RestoreState(TextFileMemento memento)
    {
        Content = memento.Content;
    }
}
