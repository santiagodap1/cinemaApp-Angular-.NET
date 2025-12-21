namespace Cinema.Application.Common;

public sealed record ServiceResult<T>(T? Value, ServiceResultStatus Status, string? Error = null)
{
    public static ServiceResult<T> Success(T value) => new(value, ServiceResultStatus.Success);
    public static ServiceResult<T> NotFound(string? error = null) => new(default, ServiceResultStatus.NotFound, error);
    public static ServiceResult<T> Conflict(string? error = null) => new(default, ServiceResultStatus.Conflict, error);
    public static ServiceResult<T> Validation(string? error = null) => new(default, ServiceResultStatus.Validation, error);
}
