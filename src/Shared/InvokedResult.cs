using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWebAssemblyWithIdentity.Shared;

public class InvokedResult
{
    private static readonly InvokedResult _success = new InvokedResult { Succeeded = true };
    private readonly List<string> _errors = new List<string>();

    public bool Succeeded { get; private set; }
    public IReadOnlyCollection<string> Errors => _errors;

    public static InvokedResult Success => _success;

    public static InvokedResult Failed(params string[] errors)
    {
        var result = new InvokedResult { Succeeded = false };
        if (errors != null)
        {
            result._errors.AddRange(errors);
        }
        return result;
    }
}
