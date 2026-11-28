using System.Diagnostics;
using Aow.Game.Net;
using Aow.Game.Recording;

namespace Aow.Headless;

/// <summary>
/// The site of a recorded add (<c>--record-sites</c>, issue 343): the first three game methods on
/// the stack outside the token layer and the recorder, and a last
/// <c>TokenExecutor.ExecuteTokenEvent</c> when an execution is on the stack. The site is the
/// evidence that the recorder's split between inputs and derived events holds: an add is derived
/// exactly when an execution is on its stack.
/// </summary>
internal static class TokenSites
{
    public static string Describe()
    {
        List<string> callers = [];
        bool executing = false;
        foreach (StackFrame frame in new StackTrace(1, fNeedFileInfo: false).GetFrames())
        {
            if (frame.GetMethod() is not { DeclaringType: { } type } method || type.Assembly != typeof(TokenRecorder).Assembly)
            {
                continue;
            }

            if (type == typeof(TokenExecutor) && method.Name == nameof(TokenExecutor.ExecuteTokenEvent))
            {
                executing = true;
            }

            if (type.Namespace is not ("Aow.Game.Net" or "Aow.Game.Recording") && callers.Count < 3)
            {
                callers.Add($"{type.Name}.{method.Name}");
            }
        }

        if (executing)
        {
            callers.Add("TokenExecutor.ExecuteTokenEvent");
        }

        return string.Join(" < ", values: callers);
    }
}
