namespace DataCollector.Shared;

public readonly struct Result<T>
{
    public bool IsSuccess { get; }

    public T? Value { get; }

    public string Error { get; }

    public Result(bool success, T? value = default, string? error = null)
    {
        Value = value;
        IsSuccess = success;
        Error = error ?? string.Empty;
    }
}

public readonly struct Result
{
    public bool IsSuccess { get; }

    public string Error { get; }

    public Result(bool isSuccess, string? error = null)
    {
        IsSuccess = isSuccess;
        Error = error ?? string.Empty;
    }

    public static Result Failed(string? error = null)
    {
        return new Result(false, error);
    }

    public static Result Success()
    {
        return new(true);
    }

    public static Result<T> Failed<T>(string? error = null)
    {
        return new Result<T>(false, default, error);
    }

    public static Result<T> Success<T>(T value)
    {
        return new(true, value);
    }
}
