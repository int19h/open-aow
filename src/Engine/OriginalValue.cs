namespace Aow.Engine;

public interface IOriginalValue
{
    int OriginalValue { get; }

    int NoneValue { get; }
}

public interface IOriginalId<TSelf> : IOriginalValue where TSelf : struct, IOriginalId<TSelf>
{
    static abstract TSelf FromInt32(int value);

    static abstract TSelf? FromOriginal(int value);

    static new abstract int NoneValue { get; }
}

public interface IOptionalOriginalValue
{
}

public interface IOptionalOriginalId<TSelf> : IOriginalId<TSelf>, IOptionalOriginalValue where TSelf : struct, IOptionalOriginalId<TSelf>
{
}

public static class OriginalValueSnapshot
{
    public static bool TryRender(object? value, Type? declaredType, out object? rendered)
    {
        if (value is IOriginalValue original)
        {
            rendered = original.OriginalValue;
            return true;
        }

        Type? underlying = declaredType is null ? null : Nullable.GetUnderlyingType(declaredType);
        if (value is null && underlying is not null && typeof(IOptionalOriginalValue).IsAssignableFrom(underlying))
        {
            rendered = ((IOriginalValue)Activator.CreateInstance(underlying)!).NoneValue;
            return true;
        }

        rendered = null;
        return false;
    }
}
