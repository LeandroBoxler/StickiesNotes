using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;
using StickiesNotes.Domain.UseCases;
using StickiesNotes.Tests.Mocks;
namespace StickiesNotes.Tests;

public class GetNotesTest
{
    [Fact]
    public async Task TestGetNotes_Success()
    {
        NoteServiceMock noteService = new NoteServiceMock(new Note[]{
            NoteMock.GetValidNote(),
            NoteMock.GetValidNote(),
            NoteMock.GetValidNote()
        });
        GetNotes useCase = new GetNotes(noteService);

        var result = await useCase.Execute();

        Assert.True(result.IsSuccess);
        Assert.IsType<Note[]>(result.Value);
        Assert.Equal(3,result.Value!.Length);
    }
}