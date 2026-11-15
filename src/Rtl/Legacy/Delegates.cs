using System.Linq.Expressions;
using System.Reflection;

namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static T? RebindDelegate<T>(Delegate? value) where T : Delegate
    {
        if (value is null)
        {
            return null;
        }

        if (value is T same)
        {
            return same;
        }

        Delegate[] subscribers = value.GetInvocationList();
        if (subscribers.Length > 1)
        {
            return (T)Delegate.Combine(subscribers.Select(RebindDelegate<T>).Cast<Delegate>().ToArray())!;
        }

        if (Delegate.CreateDelegate(typeof(T), value.Target, value.Method, false) is T compatible)
        {
            return compatible;
        }

        MethodInfo target = typeof(T).GetMethod("Invoke")!;
        MethodInfo source = value.GetType().GetMethod("Invoke")!;
        ParameterExpression[] parameters = target.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
        ParameterInfo[] inputs = source.GetParameters();
        if (inputs.Length != parameters.Length)
        {
            throw new InvalidCastException("Method pointer argument count differs");
        }
        // Delphi method-pointer casts retain the call and its object argument. Adapt the
        // managed reference type at invocation, including every multicast subscriber.
        IEnumerable<Expression> arguments = parameters.Select((p, i) => (Expression)Expression.Convert(p, inputs[i].ParameterType));
        return Expression.Lambda<T>(Expression.Invoke(Expression.Constant(value), arguments), parameters).Compile();
    }

    public static MethodInfo? MethodCode(Delegate? value) => value?.Method;
    public static object? MethodTarget(Delegate? value) => value?.Target;
}
