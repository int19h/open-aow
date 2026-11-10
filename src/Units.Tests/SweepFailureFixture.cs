using System.Reflection;
using System.Text.Json;
using Aow.Engine.Nodes;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Spells;

namespace Aow.Units.Tests;

internal static class SweepFailureFixture
{
    internal static readonly string[] Names =
    [
        "Aow.Game.World.Structures.City",
        "Aow.Game.Magic.Spells.HolyGroundSpell",
        "Aow.Game.Magic.Spells.UnholyGroundSpell",
        "Aow.Game.Magic.Spells.RaiseTerrain",
        "Aow.Game.Magic.Spells.PoisonPlantSpell",
        "Aow.Game.World.Map.AoWMapLevel",
        "Aow.Game.Combat.AoWCombatMapLevel",
    ];

    private sealed class FailureOwner(bool reject) : EngineFixture.Owner
    {
        internal readonly List<string> Events = [];
        internal EObject? Last;

        public override int AddChild(EObject child)
        {
            Last = child;
            Events.Add("insert:" + ReferenceEquals(this, child.Owner));
            base.AddChild(child);
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }

        public override int RemoveChild(EObject child)
        {
            Events.Add("remove:" + ReferenceEquals(Last, child));
            int index = Members.IndexOf(child);
            base.RemoveChild(child);
            return index;
        }
    }

    internal static void CheckPlainState(string name, int route)
    {
        var type = typeof(Spell).Assembly.GetType(name)!;
        using var fixture = new OrderingRecording.Fixture(type);
        var owner = new FailureOwner(false);
        var value = OrderingRecording.Create(type, owner, route, fixture.Engine);
        Assert.IsType(type, value);
        Assert.Same(owner, value.Owner);
        Assert.Same(value, Assert.Single(owner.Members));
        Assert.Equal(["insert:True"], owner.Events);
        value.Dispose();
        Assert.Empty(owner.Members);
        Assert.Equal(["insert:True", "remove:True"], owner.Events);
    }
}
