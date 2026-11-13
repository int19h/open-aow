using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic;
using Aow.Game.Net;
using Aow.Game.Units;

namespace Aow.Game.Tests;

/// <summary>
/// A class that stores through MainReadWrite or ReadWriteChildren and has no MainPersist is
/// refused. A class that stores through ReadWrite and has no PersistFields of its own is refused.
/// The pinned names are every product engine class in either state.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class PersistRefusalTests
{
    private static readonly string[] s_refused =
    [
        "AnimationTokenEvent",
        "ArenaTokenEvent",
        "ArmyCombatMoveTokenEvent",
        "ArmyCombineMoveTokenEvent",
        "ArmyDefaultMoveTokenEvent",
        "ArmyNegotiateMoveTokenEvent",
        "CombatAbilityRangedTokenEvent",
        "CombatAbilityTouchMoveTokenEvent",
        "TacticalBattleMeleeMoveTokenEvent",
        "ChangePlayerMagicTokenEvent",
        "ChangePlayerSettingsTokenEvent",
        "CheatTokenEvent",
        "CityEnchantmentSpellTokenEvent",
        "CityTargetSpellTokenEvent",
        "CombatFlowControlTokenEvent",
        "CombatMoveTokenEvent",
        "ConstructStructureTokenEvent",
        "DataPackage",
        "DiplomaticActionTokenEvent",
        "DisbandUnitTokenEvent",
        "DisjunctionTokenEvent",
        "DispelMagicAbilityTokenEvent",
        "EndTurnTokenEvent",
        "FireBarrierTokenEvent",
        "FreezeWaterTokenEvent",
        "GameOverTokenEvent",
        "GlobalEnchantmentSpellTokenEvent",
        "GlobalTargetSpellTokenEvent",
        "HealingShowersTokenEvent",
        "HealingTokenEvent",
        "HeroUpdateTokenEvent",
        "HeroUpgradeTokenEvent",
        "HolyGroundSpellTokenEvent",
        "ItemTokenEvent",
        "LevelTerrainTokenEvent",
        "MoveArmyTokenEvent",
        "MoveTokenEvent",
        "MultiMoveTokenEvent",
        "NegotiateTokenEvent",
        "PlayerCommunicationSettings",
        "PoisonPlantSpellTokenEvent",
        "ProductionTokenEvent",
        "RaiseTerrainTokenEvent",
        "RejuvenateTokenEvent",
        "SaveGameTokenEvent",
        "SpellTokenEvent",
        "StormSpellTokenEvent",
        "StructureTokenEvent",
        "SummonSpellTokenEvent",
        "SyncTokenEvent",
        "TerminatePlayerTokenEvent",
        "TokenEventList",
        "TokenPackage",
        "TornadoTokenEvent",
        "TownGateTokenEvent",
        "UnholyGroundSpellTokenEvent",
        "UnitSpellTokenEvent",
        "UpdateSignTokenEvent",
        "VortexTokenEvent",
        "WarpPartyTokenEvent",
        "WatcherTokenEvent",
    ];

    [Fact]
    public void EngineClassesOutsideThePersistenceTreeAreRefused()
    {
        var found = new List<string>();
        foreach (var engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(EngineObject).Assembly) continue;
            if (EngineObject.TreePersistenceRefusal(engineClass.Type) is string) found.Add(engineClass.Type.Name);
        }
        found.Sort(static (a, b) => string.CompareOrdinal(a, b));
        var expected = s_refused.ToList();
        expected.Sort(static (a, b) => string.CompareOrdinal(a, b));
        var missing = expected.Except(found).ToArray();
        var extra = found.Except(expected).ToArray();
        Assert.True(missing.Length == 0 && extra.Length == 0,
            "Refused list is missing " + string.Join(", ", missing) + " and has extra " + string.Join(", ", extra) + ".");
        Assert.Equal(expected, found);
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(Enchantment)));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(EChangeNotifyNode)));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(UnitList)));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(ArmyView)));
        using var package = new DataPackage(null!) { Holder = EngineFixture.Engine };
        var error = Assert.Throws<InvalidOperationException>(() => TreeWriter.Write(package, new EnginePersistableFactory(EngineFixture.Engine)));
        Assert.Equal("Class DataPackage overrides MainReadWrite and has no MainPersist.", error.Message);
    }
}
