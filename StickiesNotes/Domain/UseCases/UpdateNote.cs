using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;
using StickiesNotes.Domain.Exceptions;

namespace StickiesNotes.Domain.UseCases;

public class UpdateNote(NoteService service)
{
    public async Task<OperationResult> Execute(Note updatedNote)
    {
        if (updatedNote.Id == Guid.Empty)
        {
            return new OperationResult(new ArgumentException("Id cannot be empty."));
        }

        OperationResult<Note> existingNoteResult = await service.GetById(updatedNote.Id);
        if (!existingNoteResult.IsSuccess)
        {
            return new OperationResult(new NotFoundException());
        }

        return await service.Update(updatedNote);
    }
    
}