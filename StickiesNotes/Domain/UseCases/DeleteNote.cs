using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;
using StickiesNotes.Domain.Exceptions;

namespace StickiesNotes.Domain.UseCases;

public class DeleteNote(NoteService service)
{
    public async Task<OperationResult> Execute(Guid id)
    {
        if (id == Guid.Empty)
        {
            return new OperationResult(new ArgumentException("Id cannot be empty."));
        }

        OperationResult<Note> existingNoteResult = await service.GetById(id);
        if (!existingNoteResult.IsSuccess)
        {
            return new OperationResult(new NotFoundException());
        }

        return await service.Delete(id);        
    }
    
}