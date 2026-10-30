using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.CMapCtrl;
using Aow.Units.MapCtrl;
using Aow.Units.HSEngine;
using Aow.Units.Engine;
using Aow.Units.HSLib;
using Aow.Units.AOWInterfaceImgLib;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

public sealed class UiConstructionFailureTests
{
    private static readonly List<string> Events = [];
    private sealed class TrackedEngine : THSEngine
    {
        public override void Destroy() { Events.Add("engine destroyed"); base.SetOwner(null!); }
    }
    private sealed class FailingEngineComponent : THexagonSpriteEngine
    {
        internal FailingEngineComponent(ConstructionContext context, TComponent owner) : base(context, owner) { }

        public override void CreateEngine() => SetHSEngine(new TrackedEngine());
        public override void SetHSEngine(THSEngine value)
        {
            base.SetHSEngine(value);
            Events.Add("engine acquired");
            throw new InvalidOperationException("after acquisition");
        }
        public override void Destroy() { Events.Add("component destroyed"); base.Destroy(); }
    }
    private sealed class FailingGameComponent : TAgeOfWondersEngine
    {
        internal FailingGameComponent(ConstructionContext context, TComponent owner) : base(context, owner) { }

        public override void SetHSEngine(THSEngine value)
        {
            base.SetHSEngine(value);
            Events.Add("game engine borrowed");
            throw new InvalidOperationException("after acquisition");
        }
        public override void Destroy() { Events.Add("component destroyed"); base.Destroy(); }
    }
    private sealed class FailingInterface : TInterfaceIL
    {
        internal FailingInterface(ConstructionContext context, TComponent owner) : base(context, owner) { }

        public override void Clear()
        {
            base.Clear();
            Events.Add("clear");
            if (Events.Count == 1) throw new InvalidOperationException("after clear");
        }
        public override void Destroy() { Events.Add("component destroyed"); base.Destroy(); }
    }
    private sealed class FailingLibrary : THSLibrary
    {
        internal FailingLibrary(ConstructionContext context, TComponent owner) : base(context, owner) { }

        public override void Clear()
        {
            base.Clear();
            Events.Add("clear");
            if (Events.Count == 1) throw new InvalidOperationException("after clear");
        }
        public override void Destroy() { Events.Add("component destroyed"); base.Destroy(); }
    }
    private static TComponent Create(int family, TComponent owner) => family switch
    {
        0 => TAoWMapControl.Create(owner),
        1 => TAoWCombatMapControl.Create(owner),
        2 => ConstructionContext.Create(owner, static (context, value) => new FailingEngineComponent(context, value)),
        3 => ConstructionContext.Create(owner, static (context, value) => new FailingGameComponent(context, value)),
        4 => ConstructionContext.Create(owner, static (context, value) => new FailingInterface(context, value)),
        5 => ConstructionContext.Create(owner, static (context, value) => new FailingLibrary(context, value)),
        _ => throw new ArgumentOutOfRangeException(nameof(family))
    };

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void FailureAfterPublicationOrAcquisitionMatchesBaseCommit(int family)
    {
        var previousEngine = Game.AoWEngine;
        var previousMap = Game.AoWHSMap;
        var previousCombat = Aow.Units.AoWTC.AoWTC.AoWCombatMap;
        var loaded = new TEventList();
        var owner = new TComponent();
        Events.Clear();
        Game.AoWEngine = new TAoWEngine();
        Game.AoWEngine.FOnHSMLoaded = loaded;
        Game.AoWEngine.FOnHSMClose = null!;
        Game.AoWHSMap = null!;
        Aow.Units.AoWTC.AoWTC.AoWCombatMap = null!;
        try
        {
            TComponent? result = null;
            System.Exception? failure = null;
            try { result = Create(family, owner); }
            catch (System.Exception error) { failure = error; }
            Assert.Null(result);
            Assert.NotNull(failure);
            string actual = JsonSerializer.Serialize(new
            {
                error = failure.GetType().FullName,
                failure.Message,
                owner = owner.ComponentCount,
                subscriptions = loaded.Count,
                events = Events.ToArray()
            });
            {
                using var expected = JsonDocument.Parse(typeof(UiConstructionFailureTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.UiConstructionFailureBaseline.json")!);
                Assert.Equal(expected.RootElement.GetProperty(family.ToString()).GetString(), actual);
            }
        }
        finally
        {
            // A failing legacy destructor leaves an owned partial object in three recorded cases.
            while (owner.ComponentCount > 0) owner.RemoveComponent(owner.Components[0]);
            owner.Destroy();
            loaded.Destroy();
            Game.AoWEngine = previousEngine;
            Game.AoWHSMap = previousMap;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap = previousCombat;
            Events.Clear();
        }
    }
}
