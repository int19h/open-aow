using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

public sealed class EventLogConstructionTests
{
    private sealed class Owner : TEventLogList
    {
        internal TEventLog? Last;
        internal int Changes;
        internal int DestroyEntries;
        internal readonly List<string> Observations = [];
        internal int Origin;
        internal Owner() : base(null!)
        {
            FOnChanged = _ =>
            {
                Changes++;
                var child = GetEvents(GetCount() - 1);
                bool nil = true;
                for (Type? type = child.GetType(); type != typeof(TEventLog); type = type!.BaseType)
                    foreach (var field in type!.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                        if (!field.FieldType.IsValueType && field.GetValue(child) is not null) nil = false;
                Observations.Add(JsonSerializer.Serialize(new { Type = child.GetType().FullName, child.FDay, child.FPlayer, child.FID, child.FRefCount, AddRefReturn = child.FRefCount, DescendantReferencesNil = nil, Hash = OrderingRecording.Hash(OrderingRecording.Snapshot(child, Origin)) }));
            };
        }
        public override int AddChild(TEObject child)
        {
            Last = (TEventLog)child;
            return base.AddChild(child);
        }
        public override int RemoveChild(TEObject child)
        {
            DestroyEntries = Math.Max(DestroyEntries, new StackTrace().GetFrames().Count(x => x.GetMethod() is { Name: "Destroy", DeclaringType: { } t } && t == typeof(TEObject)));
            // Record removal notifications without indexing an empty list.
            var changed = FOnChanged;
            FOnChanged = _ => Changes++;
            try { return base.RemoveChild(child); }
            finally { FOnChanged = changed; }
        }
    }
    private static readonly string[] Names =
    [
        "Aow.Units.AoWE.TArmyEventLog",
        "Aow.Units.AoWE.TArmyInfoEventLog",
        "Aow.Units.City.TCityProductionEventLog",
        "Aow.Units.City.TCityRebellionEventLog",
        "Aow.Units.AoWE.TCombatEventLog",
        "Aow.Units.AoWE.TCombatRestoreHotseatEventLog",
        "Aow.Units.AoWE.TCombatTypeRequestEventLog",
        "Aow.Units.AoWE.TDiplomaticActionEventLog",
        "Aow.Units.EventLog.TEndTurnEventLog",
        "Aow.Units.Arena.TEnterArenaEventLog",
        "Aow.Units.Cave.TEnterCaveEventLog",
        "Aow.Units.WizardsTower.TEnterWizardsTowerEventLog",
        "Aow.Units.EventLog.TEventLog",
        "Aow.Units.FlagEvents.TFlagMessageEventLog",
        "Aow.Units.EventLog.TGameOverEventLog",
        "Aow.Units.AoWE.TGlobalEnchantmentEventLog",
        "Aow.Units.EventLog.THelpMessageEventLog",
        "Aow.Units.AoWE.THeroEmergeEventLog",
        "Aow.Units.AoWE.THeroJoinEventLog",
        "Aow.Units.AoWE.THeroUpgradeEventLog",
        "Aow.Units.ItemExpS.TItemExplorationSiteEventLog",
        "Aow.Units.AoWE.TJoinEventLog",
        "Aow.Units.EventLog.TLocationTextEventLog",
        "Aow.Units.EventLog.TMessageEventLog",
        "Aow.Units.AoWE.TNegotiateRequestEventLog",
        "Aow.Units.EventLog.TNewDayEventLog",
        "Aow.Units.AoWE.TNewTurnEventLog",
        "Aow.Units.EventLog.TPasswordEventLog",
        "Aow.Units.AoWE.TPlayerConnectionEventLog",
        "Aow.Units.AoWE.TPlayerMagicEventLog",
        "Aow.Units.Dungeon.TPrisonersJoinEventLog",
        "Aow.Units.AoWE.TProductionEventLog",
        "Aow.Units.PrdPlace.TProductionPlaceEventLog",
        "Aow.Units.AoWE.TRazeStructureEventLog",
        "Aow.Units.AoWE.TRestartGameEventLog",
        "Aow.Units.AoWE.TSMTPEventLog",
        "Aow.Units.EventLog.TSaveGameEventLog",
        "Aow.Units.AoWE.TSendEmailGameEventLog",
        "Aow.Units.EventLog.TShowBookEventLog",
        "Aow.Units.EventLog.TShowMessageEventLog",
        "Aow.Units.AoWE.TSpellCastEventLog",
        "Aow.Units.EventLog.TTextEventLog",
        "Aow.Units.EventLog.TTutorialMessageEventLog",
        "Aow.Units.EventLog.TUserEventLog",
    ];
    [Fact]
    public void StateInsertionOffsetsAndFailureMatchBaseCommit()
    {
        var actual = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (string name in Names)
        for (int mapped = 0; mapped < 2; mapped++)
        for (int owned = 0; owned < 2; owned++)
        for (int route = 0; route < 4; route++)
        for (int fail = 0; fail < 2; fail++)
        {
            Type type = typeof(TEventLog).Assembly.GetType(name)!;
            bool guarded = new[] { "TMessageEventLog", "THelpMessageEventLog", "TTutorialMessageEventLog", "TSaveGameEventLog", "TPasswordEventLog", "TEnterCaveEventLog", "TPlayerConnectionEventLog", "TRazeStructureEventLog", "TRestartGameEventLog", "TFlagMessageEventLog" }.Contains(type.Name);
            if (fail == 1 && !guarded) continue;
            using var fixture = new OrderingRecording.Fixture(type);
            if (mapped == 1) Game.AoWHSMap = new TAoWHSMap { FDay = 7, FCurrentPlayer = unchecked((sbyte)200) };
            if (fail == 1) { Game.AoWEngine.FDictionary.Destroy(); Game.AoWEngine.FDictionary = null!; }
            var owner = owned == 1 ? new Owner() : null;
            int origin = new TObject().InstanceId;
            if (owner is not null) owner.Origin = origin;
            TEventLog? value = null;
            string state;
            try
            {
                value = (TEventLog)OrderingRecording.Create(type, owner, route, fixture.Engine);
                Assert.IsType(type, value);
                state = OrderingRecording.Snapshot(value, origin);
            }
            catch (System.Exception error) when (error is not Xunit.Sdk.XunitException) { state = error.GetType().FullName + ":" + error.Message; }
            Assert.Equal(fail == 0, value is not null);
            actual.Add(name + $".map{mapped}.owner{owned}.route{route}.fail{fail}", JsonSerializer.Serialize(new { State = state, owner?.Observations, owner?.Changes, Count = owner?.GetCount(), owner?.DestroyEntries, RefCount = (value ?? owner?.Last)?.FRefCount }));
            Game.AoWHSMap = new TAoWHSMap { FStatus = TSet<Aow.Units.HSEngine.THSMapStatusElement>.Of(Aow.Units.HSEngine.THSMapStatusElement.msDestroying) };
            if (owner is not null) value?.AddRef();
            value?.Destroy();
            owner?.Destroy();
        }
        OrderingRecording.Check("EventLogConstruction", actual);
    }
}
