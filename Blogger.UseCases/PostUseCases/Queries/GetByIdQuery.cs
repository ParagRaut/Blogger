using Blogger.UseCases.PostUseCases.Entities;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Queries;

public record GetByIdQuery(PostId ID) : IRequest<Post?>;
