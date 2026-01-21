using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;

namespace StickiesNotes.Tests.Mocks;

public class NoteServiceMock : NoteService
{
    public Note[] notes;

    public NoteServiceMock(Note[] notes)
    {
        this.notes = notes;
    }
    public NoteServiceMock()
    {
        this.notes = Array.Empty<Note>();
    }

    public async Task<OperationResult> Create(Note note)
    {
        notes = notes.Append(note).ToArray();
        return new OperationResult();
    }

    public async Task<OperationResult> Delete(Guid id)
    {
        notes = notes.Where(n => n.Id != id).ToArray();
        return new OperationResult();
    }

    public async Task<OperationResult> Update(Note note)
    {
        notes = notes.Select(n => n.Id == note.Id ? note : n).ToArray();
        return new OperationResult();
    }

    public async Task<OperationResult<Note[]>> GetAll()
    {
        return new OperationResult<Note[]>(notes);
    }

    public async Task<OperationResult<Note>> GetById(Guid id)
    {
        Note? note = notes.FirstOrDefault(n => n.Id == id);

        if (note == null)
            return new OperationResult<Note>(new Exception("Note not found"));
        
        return new OperationResult<Note>(note);
    }
}