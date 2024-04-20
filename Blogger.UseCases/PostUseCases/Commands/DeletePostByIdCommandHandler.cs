using Blogger.UseCases.PostUseCases.Entities;
using Blogger.UseCases.PostUseCases.Exceptions;
using Blogger.UseCases.PostUseCases.Repositories;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Commands;

public class DeletePostByIdCommandHandler(IPostRepository postRepository) : IRequestHandler<DeletePostByIdCommand>
{
    public async Task Handle(DeletePostByIdCommand request, CancellationToken cancellationToken)
    {
        Post? post = await postRepository.GetByIdAsync(request.Id, cancellationToken);

        if (post is null) throw new PostNotFoundException(request.Id.Value);

        await postRepository.DeletePostAsync(post, cancellationToken);
    }
}
