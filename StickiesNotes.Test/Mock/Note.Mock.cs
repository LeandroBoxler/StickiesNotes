namespace StickiesNotes.Test.Mock;

using StickiesNotes.Domain.Entities;
public class NoteMock
{
    public static Note GetValidNote()
    {
        return new Note(
            Guid.NewGuid(),
            "Sample Title",
            "Sample Content"
        );
    }
    
}