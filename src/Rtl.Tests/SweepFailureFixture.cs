using System.Reflection;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

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

    private sealed class Owner(bool reject) : TECustomNode(null!)
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<string> Events = [];
        internal TEObject? Last;
        public override int AddChild(TEObject child)
        {
            Last = child;
            Events.Add("insert:" + ReferenceEquals(this, child.Owner));
            Members.Add(child);
            if (reject) throw new InvalidDataException("owner rejects after insertion");
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove:" + ReferenceEquals(Last, child));
            int index = Members.IndexOf(child);
            Members.Remove(child);
            return index;
        }
    }

    internal static void CheckPlainState(string name, int route)
    {
        var type = typeof(TSpell).Assembly.GetType(name)!;
        using var fixture = new OrderingRecording.Fixture(type);
        var owner = new Owner(false);
        var value = OrderingRecording.Create(type, owner, route, fixture.Engine);
        Assert.IsType(type, value);
        Assert.Same(owner, value.Owner);
        Assert.Same(value, Assert.Single(owner.Members));
        Assert.Equal(["insert:True"], owner.Events);
        value.Destroy();
        Assert.Empty(owner.Members);
        Assert.Equal(["insert:True", "remove:True"], owner.Events);
    }
}
