namespace TextFileEditorApp.Models;

// Снимок (Memento) состояния текстового файла для реализации отката изменений (Undo)
public class TextFileMemento
{
    // Сохранённое содержимое файла (только для чтения, неизменяемое)
    public string Content { get; }

    // Создаёт снимок с указанным содержимым
    public TextFileMemento(string content)
    {
        Content = content;
    }
}
