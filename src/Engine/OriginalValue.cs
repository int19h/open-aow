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

    /// <summary>
    /// The stored none value of the type at the unsigned 16-bit width, when the type names one
    /// that a present id also has. <see cref="PersistSupport"/> then stores and reads absence as
    /// this value, so a present id of the same value loads as absent. Null for the other types,
    /// whose stored none comes from <see cref="NoneValue"/>.
    /// </summary>
    static virtual int? StoredUInt16None => null;
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
