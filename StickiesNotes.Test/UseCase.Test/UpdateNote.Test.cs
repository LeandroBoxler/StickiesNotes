using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;
using StickiesNotes.Domain.Exceptions;
using StickiesNotes.Domain.UseCases;
using StickiesNotes.Tests.Mocks;
namespace StickiesNotes.Tests;

public class UpdateNoteTest
{
    [Fact]
    public async Task TestUpdateNote_Success()
    {
        Note baseNote = NoteMock.GetValidNote();
        NoteServiceMock noteService = new NoteServiceMock(new Note[] {new (baseNote.Id,baseNote.Title, baseNote.Content)});
        UpdateNote useCase = new UpdateNote(noteService);

        OperationResult<Note> existingRes = await noteService.GetById(baseNote.Id);

        Assert.True(existingRes.IsSuccess);

        Note existingNote = existingRes.Value!;

        Assert.Equal(baseNote.Id, existingNote.Id);
        Assert.Equal( baseNote.Title,existingNote.Title);
        Assert.Equal(baseNote.Content,existingNote.Content);

        string updatedTitle = "UpdatedTitle";
        string updatedContent = "UpdatedContent";

        Note updatingNote = new (baseNote.Id, updatedTitle, updatedContent);
        OperationResult updateResult = await useCase.Execute(updatingNote);
        Assert.True(updateResult.IsSuccess);

        OperationResult<Note> updatedNoteRes = await noteService.GetById(baseNote.Id);
        Assert.True(updatedNoteRes.IsSuccess);

        Note updatedNote = updatedNoteRes.Value!;
        Assert.Equal(baseNote.Id,updatedNote.Id);
        Assert.Equal(updatedTitle,updatedNote.Title);
        Assert.Equal(updatedContent,updatedNote.Content);
    }

    [Fact]
    public async Task TestUpdateNote_EmptyId()
    {
        Note baseNote = NoteMock.GetEmptyIdNote();
        NoteServiceMock noteService = new NoteServiceMock(new Note[] {new (baseNote.Id,baseNote.Title, baseNote.Content)});
        UpdateNote useCase = new UpdateNote(noteService);

        Assert.Equal(Guid.Empty, baseNote.Id);

        string updatedTitle = "UpdatedTitle";
        string updatedContent = "UpdatedContent";

        Note updatingNote = new (baseNote.Id, updatedTitle, updatedContent);
        OperationResult updateResult = await useCase.Execute(updatingNote);

        Assert.False(updateResult.IsSuccess);
        Assert.IsType<ArgumentException>(updateResult.Error);
    }

    [Fact]
    public async Task TestUpdateNote_NonexistentNote()
    {
        Note baseNote = NoteMock.GetValidNote();
        NoteServiceMock noteService = new NoteServiceMock(new Note[] {});
        UpdateNote useCase = new UpdateNote(noteService);

        Assert.NotEqual(baseNote.Id, Guid.Empty);

        string updatedTitle = "UpdatedTitle";
        string updatedContent = "UpdatedContent";

        Note updatingNote = new (baseNote.Id, updatedTitle, updatedContent);
        OperationResult updateResult = await useCase.Execute(updatingNote);

        Assert.False(updateResult.IsSuccess);
        Assert.IsType<NotFoundException>(updateResult.Error);
    }
}