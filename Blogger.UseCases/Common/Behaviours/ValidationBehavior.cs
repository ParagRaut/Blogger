using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = Blogger.UseCases.Common.Exceptions.ValidationException;

namespace Blogger.UseCases.Common.Behaviours;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this._validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!this._validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationFailures = await Task.WhenAll(
            this._validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        Dictionary<string, string[]> errorsDictionary = validationFailures
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName, Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);

        if (errorsDictionary.Any()) throw new ValidationException(errorsDictionary);

        return await next();
    }
}
