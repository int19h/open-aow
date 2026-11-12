
using Aow.Engine.Nodes;

using Aow.Game.Magic.Spells;

namespace Aow.Game.Tests;

internal static class SweepFailureFixture
{
    internal static string[] Names { get; } = [
        "Aow.Game.World.Structures.City",
        "Aow.Game.Magic.Spells.HolyGroundSpell",
        "Aow.Game.Magic.Spells.UnholyGroundSpell",
        "Aow.Game.Magic.Spells.RaiseTerrain",
        "Aow.Game.Magic.Spells.PoisonPlantSpell",
        "Aow.Game.World.Map.AoWMapLevel",
        "Aow.Game.Combat.CombatMapLevel",
    ];

    private sealed class FailureOwner(bool reject) : EngineFixture.Owner
    {
        internal List<string> Events { get; } = [];

        private EngineObject? _last;

        public override int AddChild(EngineObject child)
        {
            _last = child;
            Events.Add("insert:" + EngineFixture.Forwards(child.Owner, this));
            base.AddChild(child);
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
        {
            Events.Add("remove:" + ReferenceEquals(_last, child));
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
        var given = EngineFixture.OwnerFor(type, owner);
        var value = OrderingRecording.Create(type, given, route, fixture.Engine);
        Assert.IsType(type, value);
        Assert.Same(given, value.Owner);
        Assert.Same(value, Assert.Single(owner.Members));
        Assert.Equal(["insert:True"], owner.Events);
        value.Dispose();
        Assert.Empty(owner.Members);
        Assert.Equal(["insert:True", "remove:True"], owner.Events);
    }
}
