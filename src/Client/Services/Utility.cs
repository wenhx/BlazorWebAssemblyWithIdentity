using BlazorWebAssemblyWithIdentity.Shared;
using System.Diagnostics;

namespace BlazorWebAssemblyWithIdentity.Client.Services
{
    internal static class Utility
    {
        internal static void EnsureResultNotNull(InvokedResult? result)
        {
            if (result == null)
                throw new InvalidOperationException(Constants.Messages.ServerErrorMessage);
        }

        /// <summary>
        /// The Debug.WriteLine method isn't working.
        /// </summary>
        /// <param name="text"></param>
        [Conditional("DEBUG")]
        public static void ConsoleDebug(string text)
        {
            Console.WriteLine(text);
        }
    }
}
