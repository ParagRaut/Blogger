using Blogger.UseCases.PostUseCases.Entities;
using MediatR;

namespace Blogger.UseCases.PostUseCases.Commands;

public record DeletePostByIdCommand(PostId Id) : IRequest;
