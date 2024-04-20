using Blogger.UseCases.AuthorUseCases.Entities;
using Blogger.UseCases.AuthorUseCases.Exceptions;
using Blogger.UseCases.AuthorUseCases.Repositories;
using Blogger.UseCases.PostUseCases.Entities;
using Blogger.UseCases.PostUseCases.Repositories;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Commands;

public class CreatePostCommandHandler(IPostRepository postRepository, IAuthorRepository authorRepository)
    : IRequestHandler<CreatePostCommand, PostId>
{
    public async Task<PostId> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        Author? author = await authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);

        if (author is null) throw new AuthorNotFoundException(request.AuthorId.Value);

        var post = new Post(
            new PostId(Guid.NewGuid()),
            request.AuthorId,
            request.Title,
            request.Description,
            request.Content);

        await postRepository.CreatePostAsync(post, cancellationToken);

        return post.Id;
    }
}
