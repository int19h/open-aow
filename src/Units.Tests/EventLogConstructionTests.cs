using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class EventLogConstructionTests
{
    private sealed class EventListOwner : TEventLogList
    {
        internal TEventLog? Last;
        internal int Changes;
        internal int DestroyEntries;
        internal readonly List<string> Observations = [];

        internal EventListOwner() : base(null!)
        {
            FOnChanged = _ =>
            {
                Changes++;
                var child = GetEvents(GetCount() - 1);
                bool nil = true;
                for (Type? type = child.GetType(); type != typeof(TEventLog); type = type!.BaseType)
                    foreach (var field in type!.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                    {
                        if (field.Name == "captureOrdinal") continue;
                        if (!field.FieldType.IsValueType && field.FieldType != typeof(string) && field.GetValue(child) is not null) nil = false;
                    }
                Observations.Add(JsonSerializer.Serialize(new { Type = child.GetType().FullName, child.FDay, child.FPlayer, child.FID, child.FRefCount, AddRefReturn = child.FRefCount, DescendantReferencesNil = nil, State = OrderingRecording.Snapshot(child) }));
            };
        }

        public override int AddChild(TEObject child)
        {
            Last = (TEventLog)child;
            return base.AddChild(child);
        }

        public override int RemoveChild(TEObject child)
        {
            DestroyEntries = Math.Max(DestroyEntries, new StackTrace().GetFrames().Count(x => x.GetMethod() is { Name: "Dispose", DeclaringType: { } t } && t == typeof(TEObject)));
            // Record removal notifications without indexing an empty list.
            var changed = FOnChanged;
            FOnChanged = _ => Changes++;
            try { return base.RemoveChild(child); }
            finally { FOnChanged = changed; }
        }
    }

    private static readonly string[] Names =
    [
        "Aow.Game.Events.TArmyEventLog",
        "Aow.Game.Events.TArmyInfoEventLog",
        "Aow.Game.Events.TCityProductionEventLog",
        "Aow.Game.Events.TCityRebellionEventLog",
        "Aow.Game.Events.TCombatEventLog",
        "Aow.Game.Events.TCombatRestoreHotseatEventLog",
        "Aow.Game.Events.TCombatTypeRequestEventLog",
        "Aow.Game.Events.TDiplomaticActionEventLog",
        "Aow.Game.Events.TEndTurnEventLog",
        "Aow.Game.Events.TEnterArenaEventLog",
        "Aow.Game.Events.TEnterCaveEventLog",
        "Aow.Game.Events.TEnterWizardsTowerEventLog",
        "Aow.Game.Events.TEventLog",
        "Aow.Game.Events.TFlagMessageEventLog",
        "Aow.Game.Events.TGameOverEventLog",
        "Aow.Game.Events.TGlobalEnchantmentEventLog",
        "Aow.Game.Events.THelpMessageEventLog",
        "Aow.Game.Events.THeroEmergeEventLog",
        "Aow.Game.Events.THeroJoinEventLog",
        "Aow.Game.Events.THeroUpgradeEventLog",
        "Aow.Game.Events.TItemExplorationSiteEventLog",
        "Aow.Game.Events.TJoinEventLog",
        "Aow.Game.Events.TLocationTextEventLog",
        "Aow.Game.Events.TMessageEventLog",
        "Aow.Game.Events.TNegotiateRequestEventLog",
        "Aow.Game.Events.TNewDayEventLog",
        "Aow.Game.Events.TNewTurnEventLog",
        "Aow.Game.Events.TPasswordEventLog",
        "Aow.Game.Events.TPlayerConnectionEventLog",
        "Aow.Game.Events.TPlayerMagicEventLog",
        "Aow.Game.Events.TPrisonersJoinEventLog",
        "Aow.Game.Events.TProductionEventLog",
        "Aow.Game.Events.TProductionPlaceEventLog",
        "Aow.Game.Events.TRazeStructureEventLog",
        "Aow.Game.Events.TRestartGameEventLog",
        "Aow.Game.Events.TSMTPEventLog",
        "Aow.Game.Events.TSaveGameEventLog",
        "Aow.Game.Events.TSendEmailGameEventLog",
        "Aow.Game.Events.TShowBookEventLog",
        "Aow.Game.Events.TShowMessageEventLog",
        "Aow.Game.Events.TSpellCastEventLog",
        "Aow.Game.Events.TTextEventLog",
        "Aow.Game.Events.TTutorialMessageEventLog",
        "Aow.Game.Events.TUserEventLog",
    ];

    [Fact]
    public void StateInsertionAndOffsetsMatchBaseCommit()
    {
        var actual = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (string name in Names)
            for (int mapped = 0; mapped < 2; mapped++)
                for (int owned = 0; owned < 2; owned++)
                    for (int route = 0; route < 4; route++)
                    {
                        Type type = typeof(TEventLog).Assembly.GetType(name)!;
                        using var fixture = new OrderingRecording.Fixture(type);
                        if (mapped == 1) Aow.Game.World.Map.TAoWHSMap.AoWHSMap = EngineFixture.Configure(new TAoWHSMap(null!), value => { value.FDay = 7; value.FCurrentPlayer = unchecked((sbyte)200); });
                        var owner = owned == 1 ? new EventListOwner() : null;
                        TEventLog? value = null;
                        string state;
                        try
                        {
                            value = (TEventLog)OrderingRecording.Create(type, owner, route, fixture.Engine);
                            Assert.IsType(type, value);
                            state = OrderingRecording.Snapshot(value);
                        }
                        catch (System.Exception error) when (error is not Xunit.Sdk.XunitException) { state = error.GetType().FullName + ":" + error.Message; }
                        Assert.NotNull(value);
                        actual.Add(name + $".map{mapped}.owner{owned}.route{route}.fail0", JsonSerializer.Serialize(new { State = state, owner?.Observations, owner?.Changes, Count = owner?.GetCount(), owner?.DestroyEntries, RefCount = (value ?? owner?.Last)?.FRefCount }));
                        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDictionary = fixture.Dictionary;
                        Aow.Game.World.Map.TAoWHSMap.AoWHSMap = EngineFixture.Configure(new TAoWHSMap(null!), value => { value.FStatus = TSet<Aow.Game.World.Map.THSMapStatusElement>.Of(Aow.Game.World.Map.THSMapStatusElement.msDestroying); });
                        if (owner is not null) value?.AddRef();
                        value?.Dispose();
                        owner?.Dispose();
                    }
        OrderingRecording.Check("EventLogConstruction", actual);
    }
}
