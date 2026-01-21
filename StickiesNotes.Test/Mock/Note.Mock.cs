using Bogus;

namespace StickiesNotes.Tests.Mocks;

using StickiesNotes.Domain.Entities;
public class NoteMock
{
    public static Note GetValidNote()
    {
        var faker = new Faker<Note>().CustomInstantiator(f => new Note(
             f.Random.Guid(),
 f.Lorem.Sentence(3),
 f.Lorem.Paragraph()
        ));
        
        return faker.Generate();
    }

    public static Note GetEmptyIdNote()
    {
        var faker = new Faker<Note>().CustomInstantiator(f => new Note(
             Guid.Empty,
 f.Lorem.Sentence(3),
 f.Lorem.Paragraph()
        ));
        
        return faker.Generate();
    }
    
}