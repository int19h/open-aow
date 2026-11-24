using Aow.Engine;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

[Collection("Engine")]
public sealed class IdPersistenceTests
{
    private static EnginePersistableFactory Factory => field ??= new(EngineFixture.Engine);

    [Fact]
    public void PlayerBytesRoundTripEveryRecordedWidth()
    {
        foreach (IdWidth width in new[] { IdWidth.UnsignedByte, IdWidth.Byte, IdWidth.SByte })
        {
            for (int stored = 0; stored <= 255; stored++)
            {
                RoundTrip(PlayerId.FromOriginal(stored), width);
            }
        }
    }

    [Fact]
    public void CombatPlayerAndRaceBytesRoundTripBothRecordedWidths()
    {
        foreach (IdWidth width in new[] { IdWidth.UnsignedByte, IdWidth.Byte })
        {
            for (int stored = 0; stored <= 255; stored++)
            {
                RoundTrip(CombatPlayerId.FromOriginal(stored), width);
                RoundTrip(RaceId.FromOriginal(stored), width);
            }
        }
    }

    [Fact]
    public void CombatObjectBytesRoundTripAsRequiredIds()
    {
        for (int stored = 0; stored <= 255; stored++)
        {
            RoundTripRequired(CombatObjectId.FromInt32(stored), width: IdWidth.Byte);
        }
    }

    [Fact]
    public void Int32IdsRoundTripTheirNoneAndEdgeValues()
    {
        foreach (int stored in new[] { int.MinValue, -2, -1, 0, 1, 127, 128, 255, 268_435_455, int.MaxValue })
        {
            RoundTrip(UnitId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(GroupId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTripRequired(ItemId.FromInt32(stored), width: IdWidth.Int32);
            RoundTrip(StructureId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(EnchantmentId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(EventLogId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(LibraryId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(ScenarioId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(SpellId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(AbilityId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(UnitTypeId.FromOriginal(stored), width: IdWidth.Int32);
            RoundTrip(CpuId.FromOriginal(stored), width: IdWidth.Int32);
        }
        foreach (int stored in new[] { -2, -1, 0, 1, 12, 127, 128, 200, 255 })
        {
            RoundTrip(PlayerId.FromOriginal(stored), width: IdWidth.Int32);
        }
    }

    [Fact]
    public void UInt16AbilitiesRoundTripEveryLowByteAndTheSentinel()
    {
        for (int stored = 0; stored <= 255; stored++)
        {
            AbilityId? ability = AbilityId.FromInt32(stored);
            RoundTrip(ability, width: IdWidth.UInt16, none: -1);
        }
        RoundTrip<AbilityId>(null, width: IdWidth.UInt16, none: -1);
        AbilityId? edge = AbilityId.FromInt32(65_534);
        RoundTrip(edge, width: IdWidth.UInt16, none: -1);
    }

    [Fact]
    public void OptionalFallbackKeepsMissingMemberBehavior()
    {
        OptionalProbe<PlayerId> before = new(PlayerId.FromInt32(7), width: IdWidth.Byte, fallback: 7);
        ObjectNode tree = TreeWriter.Write(before, factory: Factory);
        Assert.Null(tree.Find("Value"));
        OptionalProbe<PlayerId> after = new(null, width: IdWidth.Byte, fallback: 7);
        TreeReader.Read(tree, objectValue: after, factory: Factory);
        Assert.Equal(PlayerId.FromInt32(7), after.Value);
    }

    [Fact]
    public void RequiredNoneLoadIsRefused()
    {
        RequiredProbe<PlayerId> before = new(PlayerId.FromInt32(-1), width: IdWidth.Byte);
        ObjectNode tree = TreeWriter.Write(before, factory: Factory);
        RequiredProbe<PlayerId> after = new(default, width: IdWidth.Byte);
        _ = Assert.Throws<InvalidOperationException>(() => TreeReader.Read(tree, objectValue: after, factory: Factory));
    }

    [Fact]
    public void OptionalByteWithoutDistinctNoneIsRefused()
    {
        OptionalProbe<CombatObjectId> before = new(null, width: IdWidth.Byte);
        _ = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(before, factory: Factory));
    }

    private static void RoundTrip<T>(T? expected, IdWidth width, int? none = null) where T : struct, IOptionalOriginalId<T>
    {
        OptionalProbe<T> before = new(expected, width);
        ObjectNode tree = TreeWriter.Write(before, factory: Factory);
        if (expected is null && tree.Find("Value") is null)
        {
            OptionalProbe<T> missing = new(default, width);
            TreeReader.Read(tree, objectValue: missing, factory: Factory);
            Assert.Equal(expected, missing.Value);
            return;
        }

        IntegerNode stored = Assert.IsType<IntegerNode>(tree.Find("Value"));
        int original = expected?.OriginalValue ?? none ?? T.NoneValue;
        long expectedStored = width switch
        {
            IdWidth.UnsignedByte or IdWidth.Byte => unchecked((byte)original),
            IdWidth.SByte => unchecked((sbyte)original),
            IdWidth.UInt16 => unchecked((ushort)original),
            IdWidth.Int32 => original,
            _ => throw new ArgumentOutOfRangeException(nameof(width)),
        };
        Assert.Equal(expectedStored, stored.Value);
        OptionalProbe<T> after = new(default, width);
        TreeReader.Read(tree, objectValue: after, factory: Factory);
        Assert.Equal(expected, after.Value);
    }

    private static void RoundTripRequired<T>(T expected, IdWidth width) where T : struct, IOriginalId<T>
    {
        RequiredProbe<T> before = new(expected, width);
        ObjectNode tree = TreeWriter.Write(before, factory: Factory);
        if (expected.OriginalValue == 0 && tree.Find("Value") is null)
        {
            RequiredProbe<T> zero = new(default, width);
            TreeReader.Read(tree, objectValue: zero, factory: Factory);
            Assert.Equal(expected, zero.Value);
            return;
        }
        IntegerNode stored = Assert.IsType<IntegerNode>(tree.Find("Value"));
        long expectedStored = width switch
        {
            IdWidth.UnsignedByte or IdWidth.Byte => unchecked((byte)expected.OriginalValue),
            IdWidth.SByte => unchecked((sbyte)expected.OriginalValue),
            IdWidth.UInt16 => unchecked((ushort)expected.OriginalValue),
            IdWidth.Int32 => expected.OriginalValue,
            _ => throw new ArgumentOutOfRangeException(nameof(width)),
        };
        Assert.Equal(expectedStored, stored.Value);
        RequiredProbe<T> after = new(default, width);
        TreeReader.Read(tree, objectValue: after, factory: Factory);
        Assert.Equal(expected, after.Value);
    }

    private sealed class OptionalProbe<T>(T? value, IdWidth width, int? fallback = null) : IPersistable where T : struct, IOptionalOriginalId<T>
    {
        public T? Value { get; private set; } = value;

        public void Persist(IPersister persister) => Value = persister.OptionalId("Value", value: Value, width, fallback: fallback);
    }

    private sealed class RequiredProbe<T>(T value, IdWidth width) : IPersistable where T : struct, IOriginalId<T>
    {
        public T Value { get; private set; } = value;

        public void Persist(IPersister persister) => Value = persister.Id("Value", value: Value, width);
    }
}
