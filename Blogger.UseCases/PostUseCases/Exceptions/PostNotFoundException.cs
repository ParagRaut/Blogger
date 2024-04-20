using Blogger.UseCases.Common.Exceptions;

namespace Blogger.UseCases.PostUseCases.Exceptions;

public class PostNotFoundException(Guid postId)
    : NotFoundException($"The post with the identifier {postId} was not found.");
