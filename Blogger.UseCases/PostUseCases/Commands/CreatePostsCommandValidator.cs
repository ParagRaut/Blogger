using FluentValidation;

namespace Blogger.UseCases.PostUseCases.Commands;

public class CreatePostsCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostsCommandValidator()
    {
        this.RuleFor(command => command.AuthorId).NotNull();
        this.RuleFor(command => command.Title).NotEmpty().MinimumLength(8).MaximumLength(80);
        this.RuleFor(command => command.Description).NotEmpty().MinimumLength(20);
        this.RuleFor(command => command.Content).NotEmpty().MinimumLength(20);
    }
}
