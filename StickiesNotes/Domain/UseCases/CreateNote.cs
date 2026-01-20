using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;

namespace StickiesNotes.Domain.UseCases;

public class CreateNote(NoteService service)
{
    public async Task<OperationResult<Note>> Execute(string content, string title)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return new OperationResult<Note>(new ArgumentException("Content cannot be empty."));
        }

        Note note = new Note(
            Guid.NewGuid(),
            title,
            content
        );
        
        OperationResult createRes = await service.Create(note);

        if (!createRes.IsSuccess) return new OperationResult<Note>(createRes.Error!);
        
        return new OperationResult<Note>(note);
    }
    
}