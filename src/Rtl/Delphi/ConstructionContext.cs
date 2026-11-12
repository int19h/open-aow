namespace Aow.Rtl;

// One context belongs to one allocation. Capture in the common root before semantic work.
// Managed field initializers and constructor arguments that run before capture stay outside
// this failure boundary. Independently allocated children use their own factory and context.
public sealed class ConstructionContext
{
    private object? _instance;
    private bool _finished;

    public static T Create<T>(Func<ConstructionContext, T> construct) where T : class
    {
        var context = new ConstructionContext();
        try
        {
            T result = construct(context);
            context.Complete();
            return result;
        }
        catch
        {
            context.Fail();
            throw;
        }
    }

    public static T Create<TArgument, T>(TArgument argument, Func<ConstructionContext, TArgument, T> construct) where T : class
    {
        var context = new ConstructionContext();
        try
        {
            T result = construct(context, argument);
            context.Complete();
            return result;
        }
        catch
        {
            context.Fail();
            throw;
        }
    }

    public void Capture(object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (_finished || _instance is not null)
            throw new InvalidOperationException("A construction context captures one object once.");
        _instance = value;
    }

    public void Complete()
    {
        _instance = null;
        _finished = true;
    }

    public void Fail()
    {
        if (_finished) return;
        object? failed = _instance;
        // Release and close before virtual dispatch, including a reentrant or throwing destructor.
        _instance = null;
        _finished = true;
        (failed as IDisposable)?.Dispose();
    }
}
