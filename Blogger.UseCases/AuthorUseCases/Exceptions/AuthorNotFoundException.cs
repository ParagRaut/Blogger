using Blogger.UseCases.Common.Exceptions;

namespace Blogger.UseCases.AuthorUseCases.Exceptions;

public sealed class AuthorNotFoundException(Guid authorId)
    : NotFoundException($"The author with the identifier {authorId} was not found.");
