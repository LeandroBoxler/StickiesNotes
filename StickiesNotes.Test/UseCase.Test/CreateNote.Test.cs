using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;
using StickiesNotes.Domain.UseCases;
namespace StickiesNotes.Tests;

public class CreateNoteTest
{
    [Fact]
    public async Task TestCreateNote_Success()
    {
        NoteServiceMock noteService = new NoteServiceMock();
        CreateNote useCase = new CreateNote(noteService);
        string noteContent = "This is a test note.";
        string noteTitle = "Test Title";

        var result = await useCase.Execute(noteContent, noteTitle);

        Assert.True(result.IsSuccess);
        Assert.Equal(noteContent, result.Value!.Content);
        Assert.Equal(noteTitle, result.Value!.Title);

        OperationResult<Note> dbResult = await noteService.GetById(result.Value!.Id);

        Assert.True(dbResult.IsSuccess);
    }

}