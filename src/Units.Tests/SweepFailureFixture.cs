using System.Reflection;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

internal static class SweepFailureFixture
{
    internal static readonly string[] Names =
    [
        "Aow.Units.City.TCity",
        "Aow.Units.HolyG.THolyGroundSpell",
        "Aow.Units.UnHolyG.TUnholyGroundSpell",
        "Aow.Units.Mountain.TRaiseTerrain",
        "Aow.Units.PoisonP.TPoisonPlantSpell",
        "Aow.Units.AoWE.TAoWMapLevel",
        "Aow.Units.AoWTC.TAoWCombatMapLevel",
    ];

    private sealed class FailureOwner(bool reject) : EngineFixture.Owner
    {
        internal readonly List<string> Events = [];
        internal TEObject? Last;

        public override int AddChild(TEObject child)
        {
            Last = child;
            Events.Add("insert:" + ReferenceEquals(this, child.Owner));
            base.AddChild(child);
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }

        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove:" + ReferenceEquals(Last, child));
            int index = Members.IndexOf(child);
            base.RemoveChild(child);
            return index;
        }
    }

    internal static void CheckPlainState(string name, int route)
    {
        var type = typeof(TSpell).Assembly.GetType(name)!;
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
