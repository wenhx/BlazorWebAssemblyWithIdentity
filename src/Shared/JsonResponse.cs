namespace BlazorWebAssemblyWithIdentity.Shared;

public class JsonResponse
{
    private static readonly JsonResponse _ok = new();

    public JsonResponseStatus Status { get; set; } = JsonResponseStatus.Success;
    public string? Message { get; set; }
    public object? Data { get; set; }

    public static JsonResponse Fail(string message, object? data = null)
    {
        return new JsonResponse { Status = JsonResponseStatus.Fail, Message = message, Data = data };
    }

    public static JsonResponse Ok => _ok;
}

public enum JsonResponseStatus
{ 
    Success,
    Fail,
    Error
}