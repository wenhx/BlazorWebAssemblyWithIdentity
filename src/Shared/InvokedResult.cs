using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWebAssemblyWithIdentity.Shared;

public class InvokedResult
{
    private static readonly InvokedResult _success = new InvokedResult { Succeeded = true };

    public bool Succeeded { get; set; }
    public string? Message { get; set; }

    public static InvokedResult Success => _success;

    public static InvokedResult<T> Ok<T>(T data)
    {
        return new InvokedResult<T> { Succeeded = true, Data = data };
    }

    public static InvokedResult Fail(string message)
    {
        return new InvokedResult { Succeeded = false, Message = message };
    }

    public static InvokedResult<T> Fail<T>(string message, T data)
    {
        return new InvokedResult<T> { Succeeded = false, Message = message, Data = data };
    }
}

public class InvokedResult<T> : InvokedResult
{
    public T Data { get; set; } = default(T)!;
}
