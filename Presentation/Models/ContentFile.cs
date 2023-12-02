namespace Presentation.Models;

public class ContentFile
{
    public ContentFile(string path, string content)
    {
        Path = path;
        Content = content;
    }

    public string Path { get; }
    public string Content { get; }
}
