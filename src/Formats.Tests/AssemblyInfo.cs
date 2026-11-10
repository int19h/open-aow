using System.Runtime.InteropServices;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Rtl;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Aow.Formats.Tests;

internal static class UnitMappings
{
    // The case mappings of TProductionItem and TDiplomaticAction are unit code
    // (own-formats.md 3.1). Loading Aow.Units runs ClassRegistry, which registers them
    // and the unit enum catalog. The two sample enums belong to this assembly.
    [System.Runtime.CompilerServices.ModuleInitializer]
    internal static void Load()
    {
        System.Runtime.CompilerServices.RuntimeHelpers.RunModuleConstructor(typeof(Aow.Game.Saves.FormatMapping).Module.ModuleHandle);
        EnumCatalog.Register(new DictionaryEnumCatalog(typeof(TSampleRank), typeof(TOtherRank)));
        RecordCatalog.Register(new DictionaryRecordCatalog(typeof(TSampleBehavior), typeof(TSamplePacked), typeof(TEveryWidth)));
    }
}

[StructLayout(LayoutKind.Explicit, Size = 8)]
internal struct TSampleBehavior
{
    [FieldOffset(0)] public byte PlayerType;
    [FieldOffset(4)] public int Flags;
}

[StructLayout(LayoutKind.Explicit, Size = 1)]
internal struct TSamplePacked
{
    [FieldOffset(0)] public byte Value__image;

    public Set<TSampleRank> Value
    {
        get => Set<TSampleRank>.FromMask(Value__image);
        set => Value__image = (byte)value.Image;
    }
}

[StructLayout(LayoutKind.Explicit, Size = 14)]
internal struct TEveryWidth
{
    [FieldOffset(0)] public byte U8;
    [FieldOffset(1)] public sbyte I8;
    [FieldOffset(2)] public ushort U16;
    [FieldOffset(4)] public short I16;
    [FieldOffset(6)] public uint U32;
    [FieldOffset(10)] public int I32;
}

enum TSampleRank : byte
{
    None = 0,
    Gold = 1,
}

enum TOtherRank : byte
{
    Low = 0,
}
