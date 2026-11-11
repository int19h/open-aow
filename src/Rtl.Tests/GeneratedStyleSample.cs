using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl.Tests.Generated;

public enum Flag : byte { First, Second, Third }

[InlineArray(6)]
public struct ByteArray6 { private byte element0; }

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct SampleRecord
{
    [FieldOffset(0)] private byte flags;
    [FieldOffset(1)] private ByteArray6 name;
    [FieldOffset(8)] public int Value;

    public Set<Flag> Flags
    {
        readonly get => Set<Flag>.FromMask(flags);
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

public class Sample(int value) : IDisposable
{
    public int Value = value;
    public bool Destroyed;

    public virtual int ClassID => 1;

    public virtual void AbstractCall() => throw AbstractErrorException.Create("Abstract Error");
    public virtual void Dispose() => Destroyed = true;
}

public sealed class SampleChild(int value) : Sample(value + 10)
{
    public override int ClassID => 2;
}

public sealed class FailingSample : Sample
{
    public static FailingSample? LastInstance;

    private FailingSample(ConstructionContext context, int value) : base(value)
    {
        context.Capture(this);
        LastInstance = this;
        throw new InvalidOperationException("Constructor failure");
    }

    public static FailingSample Create(int value) =>
        ConstructionContext.Create(value, static (context, value) => new FailingSample(context, value));
}

public static class GeneratedStyleSample
{
    public static int TypedConstant = 7;
    public static Func<int, Sample> ClassConstant = static value => new SampleChild(value);

    public static SampleRecord Run()
    {
        var instance = ClassConstant(TypedConstant);
        try
        {
            var records = new SampleRecord[3];
            var pointer = Ptr<SampleRecord>.To(records, 0);
            for (int i = 0; i < records.Length; i++)
            {
                pointer.Value.Value = instance.Value + i;
                pointer += 1;
            }
            ref var sampleRecord = ref records[1];
            sampleRecord.Flags = Set<Flag>.Of(Flag.First).WithRange(Flag.Second, Flag.Third);
            sampleRecord.Name = "abcdef";
            sampleRecord.Value += instance.ClassID;
            return records[1];
        }
        finally { instance?.Dispose(); }
    }
}
