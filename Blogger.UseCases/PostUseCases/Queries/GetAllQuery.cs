using Blogger.UseCases.PostUseCases.Entities;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Queries;

public record GetAllQuery(int Page = 1, int PageSize = 10) : IRequest<List<Post>>;
