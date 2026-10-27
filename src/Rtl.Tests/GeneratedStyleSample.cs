using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Aow.Rtl.Rtl;

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
    public TSet<TFlag> Flags
    {
        readonly get => TSet<TFlag>.FromMask(flags);
        set => flags = (byte)value.Image;
    }
    public AnsiString Name
    {
        get
        {
            AnsiString result = default;
            for (int i = 1; i <= name[0]; i++) result += AnsiString.FromBytes(name[i]);
            return result;
        }
        set
        {
            name[0] = (byte)Math.Min(5, value.Length);
            for (int i = 1; i <= name[0]; i++) name[i] = value[i];
        }
    }
}

public partial class TSample : TObject
{
    public int Value;
    public bool Destroyed;
    public virtual void Create__init(int value) => Value = value;
    public virtual void AbstractCall() => throw EAbstractError.Create("Abstract Error");
    public override void Destroy() => Destroyed = true;
    public new Meta ClassType => (Meta)base.ClassType;
    public new class Meta : TObject.Meta
    {
        public virtual int ClassID() => 1;
        public TSample Create(int value)
        {
            var instance = (TSample)NewInstance();
            try { instance.Create__init(value); }
            catch { instance.Destroy(); throw; }
            return instance;
        }
        public static new Meta Of<T>() where T : TSample => (Meta)MetaRegistry.Of(typeof(T));
    }
}

public partial class TSampleChild : TSample
{
    public override void Create__init(int value)
    {
        base.Create__init(value);
        Value += 10;
    }
    public new class Meta : TSample.Meta { public override int ClassID() => 2; }
}

public partial class TFailingSample : TSample
{
    public static TFailingSample? LastInstance;
    public override void Create__init(int value)
    {
        LastInstance = this;
        throw EInvalidOperation.Create("Constructor failure");
    }
}

public static class GeneratedMetaRegistry
{
    [ModuleInitializer]
    public static void Register()
    {
        ProcessSafety.Initialize();
        MetaRegistry.Register(typeof(TSample), new TSample.Meta(), () => new TSample(), 12);
        MetaRegistry.Register(typeof(TSampleChild), new TSampleChild.Meta(), () => new TSampleChild(), 12);
        MetaRegistry.Register(typeof(TFailingSample), new TSample.Meta(), () => new TFailingSample(), 12);
    }
}

public static class GeneratedStyleSample
{
    public static int TypedConstant = 7;
    public static TSample.Meta ClassConstant = TSample.Meta.Of<TSampleChild>();
    public static TSampleRecord Run()
    {
        var instance = ClassConstant.Create(TypedConstant);
        try
        {
            var records = new TSampleRecord[3];
            var pointer = Ptr<TSampleRecord>.To(records, 0);
            for (int i = 0; i < records.Length; i++)
            {
                pointer.Value.Value = instance.Value + i;
                pointer += 1;
            }
            {
                ref var w__1 = ref records[1];
                w__1.Flags = TSet<TFlag>.Of(TFlag.First).Range(TFlag.Second, TFlag.Third);
                w__1.Name = "abcdef";
                w__1.Value += instance.ClassType.ClassID();
            }
            return records[1];
        }
        finally { instance.Free(); }
    }
}
