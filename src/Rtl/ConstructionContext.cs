using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Aow.Rtl.Tests")]
[assembly: InternalsVisibleTo("Aow.Game")]

namespace Aow.Rtl;

// One context belongs to one allocation. Capture in the common root before semantic work.
// Managed field initializers and constructor arguments that run before capture stay outside
// this failure boundary. Independently allocated children use their own factory and context.
internal sealed class ConstructionContext
{
    private TObject? instance;
    private bool finished;

    internal static T Create<T>(Func<ConstructionContext, T> construct) where T : TObject
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

    internal static T Create<TArg, T>(TArg arg, Func<ConstructionContext, TArg, T> construct) where T : TObject
    {
        var context = new ConstructionContext();
        try
        {
            T result = construct(context, arg);
            context.Complete();
            return result;
        }
        catch
        {
            context.Fail();
            throw;
        }
    }

    internal void Capture(TObject value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (finished || instance is not null)
            throw new InvalidOperationException("A construction context captures one object once.");
        instance = value;
    }

    internal void Complete()
    {
        instance = null;
        finished = true;
    }

    internal void Fail()
    {
        if (finished) return;
        TObject? failed = instance;
        // Release and close before virtual dispatch, including a reentrant or throwing destructor.
        instance = null;
        finished = true;
        failed?.Destroy();
    }
}
