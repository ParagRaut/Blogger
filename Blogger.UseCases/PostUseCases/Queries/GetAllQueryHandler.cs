using Blogger.UseCases.PostUseCases.Entities;
using Blogger.UseCases.PostUseCases.Repositories;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Queries;

public class GetAllQueryHandler(IPostRepository postRepository) : IRequestHandler<GetAllQuery, List<Post>>
{
    public async Task<List<Post>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        List<Post> posts = await postRepository.GetAllPostsAsync(
            cancellationToken,
            request.Page,
            request.PageSize);

        return posts;
    }
}
