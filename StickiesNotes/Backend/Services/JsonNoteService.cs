using Microsoft.VisualBasic;
using StickiesNotes.Domain.Entities;
using StickiesNotes.Domain.Exceptions;
using StickiesNotes.Domain.Services;
using StickiesNotes.Domain.Types;
using System.Data.Common;
using System.IO;
using System.Text.Json;

namespace StickiesNotes.Backend.Services;

public class JsonNoteService : NoteService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    public JsonNoteService(string filePath = "notes.json"){
        _filePath = filePath;
        _options = new JsonSerializerOptions{
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
            WriteIndented = true
        };

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize<Note[]>(new Note[] {}));
        }
    }

    public async Task<OperationResult<Note[]>> GetAll()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return new OperationResult<Note[]>(new UnexpectedException());
            }

            string json = await File.ReadAllTextAsync(_filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            return new OperationResult<Note[]>(notes);
          
            
        } catch (Exception ex)
        {
            return new OperationResult<Note[]>(new UnexpectedException(ex.Message));
        }
    }

    public async Task<OperationResult<Note>> GetById(Guid id)
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return new OperationResult<Note>(new UnexpectedException());
            }

            string json = await File.ReadAllTextAsync(_filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            Note? note = notes.FirstOrDefault((n => n.Id == id));
            if (note == null) return new OperationResult<Note>(new NotFoundException());
            
            return new OperationResult<Note>(note);
        } catch (Exception ex)
        {
            return new OperationResult<Note>(new UnexpectedException(ex.Message));
        }
    }

    public async Task<OperationResult> Create(Note newNote)
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return new OperationResult(new NotFoundException());
            }

            string json = await File.ReadAllTextAsync(_filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            notes = notes.Append(newNote).ToArray();
            string updatedJson = JsonSerializer.Serialize(notes,_options);
            await File.WriteAllTextAsync(_filePath, updatedJson);
            return new OperationResult();
        } catch (Exception ex)
        {
            return new OperationResult(new UnexpectedException(ex.Message));
        }
    }

    public async Task<OperationResult> Delete(Guid id)
    {
            try
        {
            if (!File.Exists(_filePath))
            {
                return new OperationResult(new UnexpectedException());
            }

            string json = await File.ReadAllTextAsync(_filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            Note? note = notes.FirstOrDefault((n => n.Id == id));
            if (note == null) return new OperationResult(new NotFoundException());
            Note[] updatedNotes = notes.Where(n => n.Id != id).ToArray();
            string updatedJson = JsonSerializer.Serialize(updatedNotes, _options);
            await File.WriteAllTextAsync(_filePath, updatedJson);
            return new OperationResult();
            
        } catch (Exception ex)
        {
            return new OperationResult(new UnexpectedException(ex.Message));
        }
        
    }

    public async Task<OperationResult> Update(Note note)
    {
         try
        {
            if (!File.Exists(_filePath))
            {
                return new OperationResult(new UnexpectedException());
            }

            string json = await File.ReadAllTextAsync(_filePath);
            Note[] notes = JsonSerializer.Deserialize<Note[]>(json, _options) ?? Array.Empty<Note>();
            Note? existingNote = notes.FirstOrDefault((n => n.Id == note.Id));
            
            if (existingNote == null) return new OperationResult(new NotFoundException());
            
            Note[] updatedNotes = notes.Select(n => n.Id == note.Id ? note : n).ToArray();
            string updatedJson = JsonSerializer.Serialize(updatedNotes, _options);
            await File.WriteAllTextAsync(_filePath, updatedJson);
            return new OperationResult();
            
        } catch (Exception ex)
        {
            return new OperationResult(new UnexpectedException(ex.Message));
        }
    }
}