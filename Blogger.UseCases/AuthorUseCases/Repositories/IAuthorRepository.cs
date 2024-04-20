using Blogger.UseCases.AuthorUseCases.Entities;

namespace Blogger.UseCases.AuthorUseCases.Repositories;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken);
}
