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

public sealed class MsgArgIdTests
{
    [Fact]
    public void KindIdsRoundTripPresentAndAbsentValues()
    {
        Assert.Equal(CombatPlayerId.FromInt32(3), MsgArg.FromCombatPlayer(CombatPlayerId.FromInt32(3)).CombatPlayer);
        Assert.Null(MsgArg.FromCombatPlayer(null).CombatPlayer);
        Assert.Equal(RaceId.FromInt32(4), MsgArg.FromRace(RaceId.FromInt32(4)).Race);
        Assert.Null(MsgArg.FromRace(null).Race);
        Assert.Equal(UnitTypeId.FromInt32(5), MsgArg.FromUnitType(UnitTypeId.FromInt32(5)).UnitType);
        Assert.Null(MsgArg.FromUnitType(null).UnitType);
        Assert.Equal(SpellId.FromInt32(6), MsgArg.FromSpell(SpellId.FromInt32(6)).Spell);
        Assert.Null(MsgArg.FromSpell(null).Spell);
        Assert.Equal(AbilityId.FromInt32(7), MsgArg.FromAbility(AbilityId.FromInt32(7)).Ability);
        Assert.Null(MsgArg.FromAbility(null).Ability);
    }

    [Fact]
    public void BoundaryIdsRoundTripPresentAndAbsentValues()
    {
        Assert.Equal(UnitId.FromInt32(8), MsgArg.FromUnit(UnitId.FromInt32(8)).Unit);
        Assert.Equal(GroupId.FromInt32(9), MsgArg.FromGroup(GroupId.FromInt32(9)).Group);
        Assert.Equal(ItemId.FromInt32(0), MsgArg.FromItem(ItemId.FromInt32(0)).Item);
        Assert.Equal(ItemId.FromInt32(10), MsgArg.FromItem(ItemId.FromInt32(10)).Item);
        Assert.Equal(StructureId.FromInt32(11), MsgArg.FromStructure(StructureId.FromInt32(11)).Structure);
        Assert.Equal(EnchantmentId.FromInt32(12), MsgArg.FromEnchantment(EnchantmentId.FromInt32(12)).Enchantment);
        Assert.Equal(EventLogId.FromInt32(13), MsgArg.FromEventLog(EventLogId.FromInt32(13)).EventLog);
        Assert.Equal(LibraryId.FromInt32(14), MsgArg.FromLibrary(LibraryId.FromInt32(14)).Library);
        Assert.Equal(ScenarioId.FromInt32(15), MsgArg.FromScenario(ScenarioId.FromInt32(15)).Scenario);
        Assert.Equal(CombatObjectId.FromInt32(16), MsgArg.FromCombatObject(CombatObjectId.FromInt32(16)).CombatObject);
        Assert.Equal(CombatObjectId.FromInt32(255), MsgArg.FromCombatObject(CombatObjectId.FromInt32(255)).CombatObject);
        Assert.Equal(CpuId.FromInt32(17), MsgArg.FromCpu(CpuId.FromInt32(17)).Cpu);
        Assert.Equal(0, MsgArg.FromGroup(null).Int);
        Assert.Null(MsgArg.FromUnit(null).Unit);
        Assert.Null(MsgArg.FromGroup(null).Group);
        Assert.Null(MsgArg.FromStructure(null).Structure);
        Assert.Null(MsgArg.FromEnchantment(null).Enchantment);
        Assert.Null(MsgArg.FromEventLog(null).EventLog);
        Assert.Null(MsgArg.FromLibrary(null).Library);
        Assert.Null(MsgArg.FromScenario(null).Scenario);
        Assert.Null(MsgArg.FromCombatObject(null).CombatObject);
        Assert.Null(MsgArg.FromCpu(null).Cpu);
    }
}
