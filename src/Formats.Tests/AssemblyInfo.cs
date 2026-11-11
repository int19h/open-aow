using System.Runtime.InteropServices;

using Aow.Formats.Tree;
using Aow.Rtl;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Aow.Formats.Tests;

internal static class UnitMappings
{
    // The case mappings of TProductionItem and TDiplomaticAction are unit code
    // (own-formats.md 3.1). Loading Aow.Game runs ClassRegistry, which registers them
    // and the unit enum catalog. The two sample enums belong to this assembly.
    [System.Runtime.CompilerServices.ModuleInitializer]
    internal static void Load()
    {
        System.Runtime.CompilerServices.RuntimeHelpers.RunModuleConstructor(typeof(Game.Saves.FormatMapping).Module.ModuleHandle);
        EnumCatalog.Register(new DictionaryEnumCatalog(typeof(SampleRank), typeof(OtherRank)));
        RecordCatalog.Register(new DictionaryRecordCatalog(typeof(SampleBehavior), typeof(SamplePacked), typeof(EveryWidth)));
    }
}

[StructLayout(LayoutKind.Explicit, Size = 8)]
internal struct SampleBehavior
{
    [FieldOffset(0)] public byte PlayerType;
    [FieldOffset(4)] public int Flags;
}

[StructLayout(LayoutKind.Explicit, Size = 1)]
internal struct SamplePacked
{
    [FieldOffset(0)] public byte Value__image;

    public Set<SampleRank> Value
    {
        get => Set<SampleRank>.FromMask(Value__image);
        set => Value__image = (byte)value.Image;
    }
}

[StructLayout(LayoutKind.Explicit, Size = 14)]
internal struct EveryWidth
{
    [FieldOffset(0)] public byte U8;
    [FieldOffset(1)] public sbyte I8;
    [FieldOffset(2)] public ushort U16;
    [FieldOffset(4)] public short I16;
    [FieldOffset(6)] public uint U32;
    [FieldOffset(10)] public int I32;
}

enum SampleRank : byte
{
    None = 0,
    Gold = 1,
}

enum OtherRank : byte
{
    Low = 0,
}
