using Blogger.Infrastructure.Data;
using Blogger.UseCases.AuthorUseCases.Entities;
using Blogger.UseCases.AuthorUseCases.Repositories;

namespace Blogger.Infrastructure.Repository;

public class AuthorRepository(ApplicationDbContext context) : IAuthorRepository
{
    public async Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken)
    {
        return await context.Authors.FindAsync(id, cancellationToken);
    }
}
