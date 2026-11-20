using static Aow.Rtl.RtlHost;

namespace Aow.Rtl;

public static class DelphiRandom
{
    private static int randomSeed;

    public static Action<RandomTraceEvent>? RandomTrace { get; set; }

    public static int RandSeed
    {
        get => randomSeed;
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        set
        {
            int before = randomSeed;
            randomSeed = value;
            RandomTrace?.Invoke(new("seed", Argument: null, Result: value, before, After: value));
        }
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    public static int Random(int range)
    {
        int before = randomSeed;
        randomSeed = unchecked((randomSeed * 0x08088405) + 1);
        int result = (int)(((ulong)(uint)randomSeed * (uint)range) >> 32);
        RandomTrace?.Invoke(new("random", Argument: range, result, before, After: randomSeed));
        return result;
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    public static double Random()
    {
        int before = randomSeed;
        randomSeed = unchecked((randomSeed * 0x08088405) + 1);
        double result = (uint)randomSeed * (1.0 / 4294967296.0);
        RandomTrace?.Invoke(new("random", Argument: null, result, before, After: randomSeed));
        return result;
    }

    // SYSTEM.PAS:1647 uses GetSystemTime, which returns UTC.
    public static void Randomize()
    {
        if (RandomizeSeed is int seed)
        {
            RandSeed = seed;
        }
        else if (!SuppressRandomize)
        {
            RandSeed = (int)Clock.UtcNow.TimeOfDay.TotalMilliseconds;
        }
    }

    public static int? RandomizeSeed { get; set; }

    public static bool SuppressRandomize { get; set; }
}
