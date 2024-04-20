namespace Blogger.UseCases.Common.Exceptions;

public abstract class BadRequestException(string message) : ApplicationException("Bad Request", message);
