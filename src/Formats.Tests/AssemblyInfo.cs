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

[StoredLayout, StoredPadding(offset: 1, length: 3)]
[StructLayout(LayoutKind.Explicit, Size = 8)]
internal struct SampleBehavior
{
    [FieldOffset(0)] public byte PlayerType;
    [FieldOffset(4)] public int Flags;
}

[StoredLayout]
[StructLayout(LayoutKind.Explicit, Size = 1)]
internal struct SamplePacked
{
    [FieldOffset(0)] public byte ValueImage;

    public SampleRanks Value
    {
        readonly get => (SampleRanks)ValueImage;
        init => ValueImage = (byte)value;
    }
}

[StoredLayout]
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

internal enum SampleRank : byte
{
    None = 0,
    Gold = 1,
}

internal enum OtherRank : byte
{
    Low = 0,
}

/// <summary>A set of <see cref="SampleRank"/> values: bit n is the member of ordinal n, as in the stored Delphi set.</summary>
[Flags, FlagsOf(typeof(SampleRank))]
internal enum SampleRanks : byte
{
    None = 0,
    Gold = 1 << 1,
}

/// <summary>The conversions between a single <see cref="SampleRank"/> and its flag in <see cref="SampleRanks"/>.</summary>
internal static class SampleRankConversions
{
    extension(SampleRank value)
    {
        /// <summary>The flag of the value. A value outside the members' ordinals has no flag, as it is in no Delphi set.</summary>
        public SampleRanks ToFlag() => (int)value is >= 0 and <= 1 ? (SampleRanks)(1U << (int)value) : SampleRanks.None;
    }

    extension(SampleRanks flags)
    {
        /// <summary>The member of a value that holds exactly one member.</summary>
        public SampleRank ToSampleRank() => System.Numerics.BitOperations.IsPow2((uint)flags)
            ? (SampleRank)System.Numerics.BitOperations.TrailingZeroCount((uint)flags)
            : throw new ArgumentOutOfRangeException(paramName: nameof(flags), message: "The value does not hold exactly one member.");
    }
}
