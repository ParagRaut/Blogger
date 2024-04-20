using Blogger.UseCases.AuthorUseCases.Entities;
using Blogger.UseCases.PostUseCases.Entities;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Commands;

public record CreatePostCommand(AuthorId AuthorId, string Title, string Description, string Content) : IRequest<PostId>;
