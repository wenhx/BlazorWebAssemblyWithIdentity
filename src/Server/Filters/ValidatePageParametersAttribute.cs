using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorWebAssemblyWithIdentity.Server.Filters;

public class ValidatePageParametersAttribute : ActionFilterAttribute
{
    private const string s_PageName = "page";
    private const string s_PageSizeName = "pageSize";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var defaultPage = 1;
        ValidateIntValue(context, s_PageName, defaultPage, defaultPage, Constants.Models.MaxPage);
        var defaultPageSize = 10;
        ValidateIntValue(context, s_PageSizeName, defaultPageSize, defaultPageSize, Constants.Models.MaxPageSize);
    }

    private static void ValidateIntValue(ActionExecutingContext context, string keyName, int defaultValue, int minValue, int maxValue)
    {
        if (context.ActionArguments.TryGetValue(keyName, out object? objectValue))
        {
            var value = (int?)objectValue;
            if (!value.HasValue || value < minValue || value > maxValue)
            {
                value = defaultValue;
            }
            context.ActionArguments[keyName] = value;
        }
        else
        {
            context.ActionArguments[keyName] = defaultValue;
        }
    }
}