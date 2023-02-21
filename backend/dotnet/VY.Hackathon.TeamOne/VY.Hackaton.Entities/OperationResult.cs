namespace VY.Hackaton.Entities;

public record OperationResult<T>(T? Result, IEnumerable<Error> Errors) where T : class;
