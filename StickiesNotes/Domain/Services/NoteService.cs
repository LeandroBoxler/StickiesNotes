using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Types;


namespace StickiesNotes.Domain.Services;

public interface NoteService
{
    public Task<OperationResult<Note[]>> GetAll();
    public Task<OperationResult<Note>> GetById(Guid id);
    public Task<OperationResult> Create(Note note);
    public Task<OperationResult> Update(Note note);
    public Task<OperationResult> Delete(Guid id);
}