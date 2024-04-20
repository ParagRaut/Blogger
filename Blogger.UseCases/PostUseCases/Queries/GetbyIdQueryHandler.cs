using Blogger.UseCases.PostUseCases.Entities;
using Blogger.UseCases.PostUseCases.Repositories;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Queries;

public class GetbyIdQueryHandler(IPostRepository postRepository) : IRequestHandler<GetByIdQuery, Post?>
{
    public async Task<Post?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        Post? post = await postRepository.GetByIdAsync(request.ID, cancellationToken);

        return post;
    }
}
