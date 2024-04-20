using Blogger.UseCases.AuthorUseCases.Entities;
using Blogger.UseCases.AuthorUseCases.Repositories;
using MediatR;

namespace Blogger.UseCases.AuthorUseCases.Queries;

public class GetAuthorByIdQueryHandler(IAuthorRepository authorRepository)
    : IRequestHandler<GetAuthorByIdQuery, Author?>
{
    public async Task<Author?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        Author? author = await authorRepository.GetByIdAsync(request.ID, cancellationToken);

        return author;
    }
}
