using Aow.Rtl;

namespace Aow.Units.Engine;

public static partial class Engine
{
    // Engine.pas:3200, REP STOSD uses an unsigned count. Invalid negative counts fault.
    public static void FillInteger(Ptr<int> buffer, int count, int value) => Fill(buffer, count, value);
    // Engine.pas:3210, REP STOSW uses an unsigned count.
    public static void FillWord(Ptr<ushort> buffer, int count, ushort value) => Fill(buffer, count, value);
    // Engine.pas:3220, cache both halves before any store, including overlapping input.
    public static void FillLargeInteger(Ptr<TLargeInteger> buffer, int count, TLargeInteger value) => Fill(buffer, count, value);
    // Engine.pas:3240, cache the complete value before any store.
    public static void FillQuadInteger(Ptr<TQuadInteger> buffer, int count, in TQuadInteger value)
    {
        var copy = value;
        Fill(buffer, count, copy);
    }
    public static void FillLargeInteger(Ptr<TDoubleItem> buffer, int count, TDoubleItem value) => Fill(buffer, count, value);
    private static void Fill<T>(Ptr<T> buffer, int count, T value)
    {
        if (count < 0) throw new ERangeError("Negative fill count");
        Ptr<T>.Fill(buffer, count, value);
    }
}
