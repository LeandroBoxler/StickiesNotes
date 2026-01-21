namespace StickiesNotes.Domain.Entities;
public class Note
{
    public Guid Id { get; private set; }
    public string Title { get; set; }   
    public string Content { get; set; }
    public Note(Guid id, string title, string content)
    {
        Id = id;
        Title = title;
        Content = content;
    }
    
}
