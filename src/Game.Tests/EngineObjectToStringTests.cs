using System.Diagnostics;
using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

public sealed class EngineObjectToStringTests
{
    [Fact]
    public void BaseUsesTheTypeAndClassIdAndAnUnknownTypeDoesNotThrow()
    {
        EngineObject value = new(null);
        Assert.Equal("EngineObject class=80", value.ToString());

        UnregisteredObject unregistered = new();
        Assert.Equal("UnregisteredObject class=?", unregistered.ToString());
        _ = Assert.Single(typeof(EngineObject).GetCustomAttributes(typeof(DebuggerDisplayAttribute), inherit: false));
    }

    [Fact]
    public void UnitAndArmyUseStableIdsAndGuardMissingResources()
    {
        Unit unit = new(null) { UnitId = 7, Player = PlayerId.Player2 };
        Assert.Equal("Unit#7 P2 no-resource", unit.ToString());
        unit.Player = null;
        Assert.Equal("Unit#7 P-1 no-resource", unit.ToString());
        unit.Player = PlayerId.Independent;
        Assert.Equal("Unit#7 P0 no-resource", unit.ToString());

        Army army = new(null) { Player = PlayerId.Player1, Status = ArmyStatuses.Selected };
        Assert.Equal("Army P1 status=Selected", army.ToString());
        army.Player = null;
        Assert.Equal("Army P-1 status=Selected", army.ToString());
        army.Player = PlayerId.Independent;
        Assert.Equal("Army P0 status=Selected", army.ToString());
    }

    [Fact]
    public void SpellAndPlayerHandleEmptyNames()
    {
        Spell spell = (Spell)RuntimeHelpers.GetUninitializedObject(typeof(Spell));
        Assert.Equal("<no name> id=0 Cosmos L0", spell.ToString());

        Player player = (Player)RuntimeHelpers.GetUninitializedObject(typeof(Player));
        player.Name = "Merlin";
        player.PlayerIndex = PlayerId.Independent;
        player.PlayerType = PlayerType.Human;
        Assert.Equal("Player Merlin idx=0 Human", player.ToString());
        player.PlayerIndex = null;
        Assert.Equal("Player Merlin idx=-1 Human", player.ToString());
    }

    [Fact]
    public void CityAndMapObjectGuardAnAbsentOwner()
    {
        City city = new(null!, holder: EngineFixture.Engine);
        Assert.Equal("City Noname P0 no-owner", city.ToString());
        city.Player = null;
        Assert.Equal("City Noname P-1 no-owner", city.ToString());

        MapObject mapObject = new(null);
        string text = mapObject.ToString();
        Assert.StartsWith("MapObject res=0 status=", text);
        Assert.EndsWith(" no-owner", text);
    }

    [Fact]
    public void DescriptionHelperReturnsTheBaseFallbackOnFailure()
    {
        ThrowingDescriptionObject value = new();
        Assert.Equal("ThrowingDescriptionObject class=?", value.ToString());
    }

    private sealed class UnregisteredObject() : EngineObject(null);

    private sealed class ThrowingDescriptionObject() : EngineObject(null)
    {
        public override string ToString() => Describe(() => throw new InvalidOperationException("injected"));
    }
}
