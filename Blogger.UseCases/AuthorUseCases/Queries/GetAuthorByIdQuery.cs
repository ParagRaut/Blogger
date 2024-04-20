using Blogger.UseCases.AuthorUseCases.Entities;
using MediatR;

namespace Blogger.UseCases.AuthorUseCases.Queries;

public record GetAuthorByIdQuery(AuthorId ID) : IRequest<Author?>;
