namespace Blogger.UseCases.AuthorUseCases.Entities;

public class Author
{
    public Author(AuthorId id, string name, string surname)
    {
        this.Id = id;
        this.Name = name;
        this.Surname = surname;
    }

    private Author()
    {
    }

    public AuthorId Id { get; private set; }

    public string Name { get; private set; }

    public string Surname { get; private set; }
}

public record AuthorId(Guid Value);
