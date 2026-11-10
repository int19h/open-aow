using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl.Tests.Generated;

public enum TFlag : byte { First, Second, Third }

[InlineArray(6)]
public struct FixedArray_byte_6 { private byte element0; }

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct TSampleRecord
{
    [FieldOffset(0)] private byte flags;
    [FieldOffset(1)] private FixedArray_byte_6 name;
    [FieldOffset(8)] public int Value;

    public Set<TFlag> Flags
    {
        readonly get => Set<TFlag>.FromMask(flags);
        set => flags = (byte)value.Image;
    }

    public string Name
    {
        get
        {
            string result = "";
            for (int i = 1; i <= name[0]; i++) result += Cp1252.Text(name[i]);
            return result;
        }
        set
        {
            name[0] = (byte)Math.Min(5, value.Length);
            for (int i = 1; i <= name[0]; i++) name[i] = Cp1252.Byte(value[i - 1]);
        }
    }
}

public class TSample(int value) : IDisposable
{
    public int Value = value;
    public bool Destroyed;

    public virtual int ClassID => 1;

    public virtual void AbstractCall() => throw AbstractErrorException.Create("Abstract Error");
    public virtual void Dispose() => Destroyed = true;
}

public sealed class TSampleChild(int value) : TSample(value + 10)
{
    public override int ClassID => 2;
}

public sealed class TFailingSample : TSample
{
    public static TFailingSample? LastInstance;

    private TFailingSample(ConstructionContext context, int value) : base(value)
    {
        context.Capture(this);
        LastInstance = this;
        throw new InvalidOperationException("Constructor failure");
    }

    public static TFailingSample Create(int value) =>
        ConstructionContext.Create(value, static (context, value) => new TFailingSample(context, value));
}

public static class GeneratedStyleSample
{
    public static int TypedConstant = 7;
    public static Func<int, TSample> ClassConstant = static value => new TSampleChild(value);

    public static TSampleRecord Run()
    {
        var instance = ClassConstant(TypedConstant);
        try
        {
            var records = new TSampleRecord[3];
            var pointer = Ptr<TSampleRecord>.To(records, 0);
            for (int i = 0; i < records.Length; i++)
            {
                pointer.Value.Value = instance.Value + i;
                pointer += 1;
            }
            ref var sampleRecord = ref records[1];
            sampleRecord.Flags = Set<TFlag>.Of(TFlag.First).Range(TFlag.Second, TFlag.Third);
            sampleRecord.Name = "abcdef";
            sampleRecord.Value += instance.ClassID;
            return records[1];
        }
        finally { instance?.Dispose(); }
    }
}
