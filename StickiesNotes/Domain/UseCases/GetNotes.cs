using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;

namespace StickiesNotes.Domain.UseCases;

public class GetNotes(NoteService service)
{
    public async Task<OperationResult<Note[]>> Execute() => await service.GetAll();
}