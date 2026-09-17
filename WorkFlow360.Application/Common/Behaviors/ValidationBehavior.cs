using FluentValidation;
using MediatR;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>>
            _validators;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context =
                new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(
                    _validators.Select(
                        validator =>
                            validator.ValidateAsync(
                                context,
                                cancellationToken)));

            var failures =
                validationResults
                    .SelectMany(result => result.Errors)
                    .Where(failure => failure is not null)
                    .ToList();

            if (failures.Count == 0)
            {
                return await next();
            }

            var message =
                string.Join(
                    "; ",
                    failures.Select(
                        failure => failure.ErrorMessage));

            var error =
                Error.Validation(message);

            return CreateFailureResponse<TResponse>(
                error);
        }

        private static TResponse CreateFailureResponse<T>(
            Error error)
        {
            var responseType = typeof(T);

            if (responseType == typeof(Result))
            {
                return (TResponse)(object)
                    Result.Failure(error);
            }

            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition()
                    == typeof(Result<>))
            {
                var failureMethod =
                    responseType.GetMethod(
                        nameof(Result<object>.Failure));

                return (TResponse)failureMethod!
                    .Invoke(null, [error])!;
            }

            throw new InvalidOperationException(
                $"ValidationBehavior cannot create " +
                $"a failure response for {responseType.Name}.");
        }
    }
}
