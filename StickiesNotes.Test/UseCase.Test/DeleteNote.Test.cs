using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;
using StickiesNotes.Domain.UseCases;
using StickiesNotes.Tests.Mocks;
namespace StickiesNotes.Tests;

public class DeleteNoteTest
{
    [Fact]
    public async Task TestDeleteNote_Success()
    {
        Note note = NoteMock.GetValidNote();
        Note note2 = NoteMock.GetValidNote();
        NoteServiceMock noteService = new NoteServiceMock(new Note[] {new(note.Id, note.Title, note.Content), new(note2.Id, note2.Title, note2.Content)});
        DeleteNote useCase = new DeleteNote(noteService);

        await useCase.Execute(note.Id);
        

        OperationResult<Note> getResult = await noteService.GetById(note.Id);
        OperationResult<Note[]> getAllResult = await noteService.GetAll();

        Assert.Equal(1, getAllResult.Value!.Length);
        Assert.False(getResult.IsSuccess); 
    }

     [Fact]
    public async Task TestDeleteNote_InexistentId()
    {
        Note note = NoteMock.GetValidNote();
        Note note2 = NoteMock.GetValidNote();
        NoteServiceMock noteService = new NoteServiceMock(new Note[] {new(note.Id, note.Title, note.Content), new(note2.Id, note2.Title, note2.Content)});
        DeleteNote useCase = new DeleteNote(noteService);

        var result = await useCase.Execute(Guid.Empty);

        Assert.False(result.IsSuccess);
        Assert.IsType<ArgumentException>(result.Error);
    }
}