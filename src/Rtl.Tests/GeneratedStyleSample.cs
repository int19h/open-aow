using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl.Tests.Generated;

public enum Flag : byte { First, Second, Third }

[InlineArray(6)]
public struct ByteArray6 { private byte _element0; }

[StoredLayout]
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct SampleRecord
{
    [FieldOffset(0)] private byte _flags;
    [FieldOffset(1)] private ByteArray6 _name;
    [FieldOffset(8)] public int Value;

    public Flags Flags
    {
        readonly get => (Flags)_flags;
        set => _flags = (byte)value;
    }

    public string Name
    {
        readonly get
        {
            string result = "";
            for (int i = 1; i <= _name[0]; i++)
            {
                result += Cp1252.Text(_name[i]);
            }

            return result;
        }
        set
        {
            _name[0] = (byte)Math.Min(5, value.Length);
            for (int i = 1; i <= _name[0]; i++)
            {
                _name[i] = Cp1252.Byte(value[i - 1]);
            }
        }
    }
}

public class Sample(int value) : IDisposable
{
    public int Value { get; } = value;
    public bool Destroyed { get; private set; }

    public virtual int ClassId => 1;

    public virtual void AbstractCall() => throw AbstractErrorException.Create("Abstract Error");
    public virtual void Dispose() => Destroyed = true;
}

public sealed class SampleChild(int value) : Sample(value + 10)
{
    public override int ClassId => 2;
}

public sealed class FailingSample : Sample
{
    public static FailingSample? LastInstance { get; private set; }

    private FailingSample(ConstructionContext context, int value) : base(value)
    {
        context.Capture(this);
        LastInstance = this;
        throw new InvalidOperationException("Constructor failure");
    }

    public static FailingSample Create(int value)
        => ConstructionContext.Create(value, static (context, value) => new FailingSample(context, value));
}

public static class GeneratedStyleSample
{
    private const Flags SecondThroughThird = Flags.Second | Flags.Third;

    private static readonly int s_typedConstant = 7;
    private static readonly Func<int, Sample> s_classConstant = static value => new SampleChild(value);

    public static SampleRecord Run()
    {
        Sample instance = s_classConstant(s_typedConstant);
        try
        {
            SampleRecord[] records = new SampleRecord[3];
            Ptr<SampleRecord> pointer = Ptr<SampleRecord>.To(records, index: 0);
            for (int i = 0; i < records.Length; i++)
            {
                pointer.Value.Value = instance.Value + i;
                pointer += 1;
            }
            ref SampleRecord sampleRecord = ref records[1];
            sampleRecord.Flags = Flags.First | SecondThroughThird;
            sampleRecord.Name = "abcdef";
            sampleRecord.Value += instance.ClassId;
            return records[1];
        }
        finally
        {
            instance?.Dispose();
        }
    }
}

/// <summary>A set of <see cref="Flag"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(Flag))]
public enum Flags : byte
{
    None = 0,
    First = 1 << 0,
    Second = 1 << 1,
    Third = 1 << 2,
}

/// <summary>The conversions between a single <see cref="Flag"/> and its flag in <see cref="Flags"/>.</summary>
public static class FlagConversions
{
    extension(Flag value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public Flags ToFlag() => (int)value is >= 0 and <= 2 ? (Flags)(1U << (int)value) : Flags.None;
    }

    extension(Flags flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public Flag ToFlag() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (Flag)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
