namespace VY.Hackaton.Entities
{
    public static class OperationResultHelper
    {
        public static OperationResult<T> GenerateOperationResult<T>(T result) where T : class => new(result, Array.Empty<Error>());

        public static OperationResult<T> GenerateOperationResultWithError<T>(Exception ex) where T : class =>
            new(null!, new[] {new Error(ex.Message)});
    }
}