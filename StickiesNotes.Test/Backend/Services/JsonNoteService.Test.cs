using StickiesNotes.Backend.Services;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Entities;
using System.Text.Json;

using Xunit;
using StickiesNotes.Tests.Mocks;
using StickiesNotes.Domain.Exceptions;

namespace StickiesNotes.Tests;

public class JsonNoteServiceTest: IDisposable
{
    private readonly string filePath;
    private readonly NoteService service;

    private readonly JsonSerializerOptions _options;

    public JsonNoteServiceTest() // BeforeEach
    {
        _options = new JsonSerializerOptions{
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
            WriteIndented = true
        };
        filePath = "./NotesTest.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        service = new JsonNoteService(filePath);
    }
    public void Dispose() // AfterEach
    {
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    [Fact]
    public async Task TestFileCreation()
    {
        Assert.True(File.Exists(filePath));
    }
    
    [Fact]
    public async Task TestNoteCreation()
    {
        await service.Create(new Note(
            Guid.NewGuid(),
            "Test Title",
            "Test Content"
        ));
        
            string json = await File.ReadAllTextAsync(filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            Assert.True(notes.Length == 1);
            Assert.Equal("Test Content", notes[0].Content);
            Assert.Equal("Test Title", notes[0].Title);
    }

    [Fact]
    public async Task TestNoteDelete()
    {
        Note newNote = NoteMock.GetValidNote();
        string json = await File.ReadAllTextAsync(filePath);
        Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
        notes = notes.Append(newNote).ToArray();
        string updatedJson = JsonSerializer.Serialize(notes,_options);
        await File.WriteAllTextAsync(filePath, updatedJson);

        var deletionResult = await service.Delete(newNote.Id);
        Assert.True(deletionResult.IsSuccess);

        Note[] postDeleteNotes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
        Assert.Empty(postDeleteNotes);

        var deletionResultPostDelete = await service.Delete(newNote.Id);
        Assert.False(deletionResultPostDelete.IsSuccess);
        Assert.IsType<NotFoundException>(deletionResultPostDelete.Error);
    }
    [Fact]
        public async Task TestNoteGetAll()
    {
            Note newNote = NoteMock.GetValidNote();
            string json = await File.ReadAllTextAsync(filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            notes = notes.Append(newNote).ToArray();
            string updatedJson = JsonSerializer.Serialize(notes,_options);
            await File.WriteAllTextAsync(filePath, updatedJson);

            var getAllResult = await service.GetAll();

            Assert.True(getAllResult.IsSuccess);
            Assert.Single(getAllResult.Value!);
    }
        [Fact]
        public async Task TestNoteUpdate()
    {
            Note note = NoteMock.GetValidNote();
            string json = await File.ReadAllTextAsync(filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            notes = notes.Append(note).ToArray();
            string updatedJson = JsonSerializer.Serialize(notes,_options);

            await File.WriteAllTextAsync(filePath, updatedJson);

            var getAllResult = await service.Update(new Note(
                note.Id,
                "Updated Title",
                "Updated Content"
            ));

            Assert.True(getAllResult.IsSuccess);

            string postUpdate = await File.ReadAllTextAsync(filePath);
            notes = JsonSerializer.Deserialize<Note[]>(postUpdate, _options) ?? Array.Empty<Note>();
            Note? existingNote = notes.FirstOrDefault((n => n.Id == note.Id));

            Assert.NotNull(existingNote);
            Assert.Equal("Updated Title", existingNote!.Title);
            Assert.Equal("Updated Content", existingNote!.Content);
    }
}