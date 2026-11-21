using System.Reflection;

namespace Aow.Game.Tests;

// The name a snapshot records for each handler of a delegate. A named method is its declaring
// type and its name. A lambda or a local function is `lambda in <Type>.<member>`: the member
// comes from the compiler's `<member>b__N_M` name without its ordinals, and a closure type
// (`<>c`, `<>c__DisplayClassN_M`) stands for the type that contains it. So a recording does not
// change when the compiler numbers its lambdas differently.
internal static class HandlerNames
{
    internal static string[] Of(Delegate handler) => [.. handler.GetInvocationList().Select(item => Name(item.Method))];

    private static string Name(MethodInfo method)
    {
        Type type = method.DeclaringType!;
        if (!method.Name.StartsWith('<'))
        {
            return $"{type.FullName}.{method.Name}";
        }

        string member = method.Name[1..method.Name.IndexOf('>', comparisonType: StringComparison.Ordinal)];
        while (type.Name.StartsWith('<') && type.DeclaringType is Type containing)
        {
            type = containing;
        }

        return $"lambda in {type.FullName}.{member}";
    }
}
