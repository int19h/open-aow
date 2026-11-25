#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Aow.Engine.Nodes;

using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Xunit.Sdk;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class EventLogConstructionTests
{
    private sealed class EventListOwner : EventLogList
    {
        internal EventLog? Last { get; private set; }
        internal int Changes { get; private set; }
        internal int DestroyEntries { get; private set; }
        internal List<string> Observations { get; } = [];

        // Removals in progress. A removal can run inside another one.
        private int _removing;

        internal EventListOwner() : base(null!)
        {
            Changed += (_, _) =>
            {
                Changes++;
                if (_removing != 0)
                {
                    return;
                }

                EventLog child = GetEvents(GetCount() - 1);
                bool nil = true;
                for (Type? type = child.GetType(); type != typeof(EventLog); type = type!.BaseType)
                {
                    foreach (FieldInfo field in type!.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                    {
                        if (!field.FieldType.IsValueType && field.FieldType != typeof(string) && field.GetValue(child) is not null)
                        {
                            nil = false;
                        }
                    }
                }

                Observations.Add(JsonSerializer.Serialize(new { Type = child.GetType().FullName, child.Day, Player = PlayerIdExtensions.ToOriginal(child.Player), child.Id, child.RefCount, AddRefReturn = child.RefCount, DescendantReferencesNil = nil, State = OrderingRecording.Snapshot(child) }));
            };
        }

        public override int AddChild(EngineObject child)
        {
            Last = (EventLog)child;
            return base.AddChild(child);
        }

        public override int RemoveChild(EngineObject child)
        {
            DestroyEntries = Math.Max(DestroyEntries, new StackTrace().GetFrames().Count(x => x.GetMethod() is { Name: "Dispose", DeclaringType: { } t } && t == typeof(EngineObject)));
            // Record removal notifications without indexing an empty list.
            _removing++;
            try
            {
                return base.RemoveChild(child);
            }
            finally
            {
                _removing--;
            }
        }
    }

    private static readonly string[] s_names =
    [
        "Aow.Game.Events.ArmyEventLog",
        "Aow.Game.Events.ArmyInfoEventLog",
        "Aow.Game.Events.CityProductionEventLog",
        "Aow.Game.Events.CityRebellionEventLog",
        "Aow.Game.Events.CombatEventLog",
        "Aow.Game.Events.CombatTypeRequestEventLog",
        "Aow.Game.Events.DiplomaticActionEventLog",
        "Aow.Game.Events.EndTurnEventLog",
        "Aow.Game.Events.EnterArenaEventLog",
        "Aow.Game.Events.EnterCaveEventLog",
        "Aow.Game.Events.EnterWizardsTowerEventLog",
        "Aow.Game.Events.EventLog",
        "Aow.Game.Events.FlagMessageEventLog",
        "Aow.Game.Events.GameOverEventLog",
        "Aow.Game.Events.GlobalEnchantmentEventLog",
        "Aow.Game.Events.HelpMessageEventLog",
        "Aow.Game.Events.HeroEmergeEventLog",
        "Aow.Game.Events.HeroJoinEventLog",
        "Aow.Game.Events.HeroUpgradeEventLog",
        "Aow.Game.Events.ItemExplorationSiteEventLog",
        "Aow.Game.Events.JoinEventLog",
        "Aow.Game.Events.LocationTextEventLog",
        "Aow.Game.Events.MessageEventLog",
        "Aow.Game.Events.NegotiateRequestEventLog",
        "Aow.Game.Events.NewDayEventLog",
        "Aow.Game.Events.NewTurnEventLog",
        "Aow.Game.Events.PasswordEventLog",
        "Aow.Game.Events.PlayerConnectionEventLog",
        "Aow.Game.Events.PlayerMagicEventLog",
        "Aow.Game.Events.PrisonersJoinEventLog",
        "Aow.Game.Events.ProductionEventLog",
        "Aow.Game.Events.ProductionPlaceEventLog",
        "Aow.Game.Events.RazeStructureEventLog",
        "Aow.Game.Events.RestartGameEventLog",
        "Aow.Game.Events.SmtpEventLog",
        "Aow.Game.Events.SaveGameEventLog",
        "Aow.Game.Events.SendEmailGameEventLog",
        "Aow.Game.Events.ShowBookEventLog",
        "Aow.Game.Events.SpellCastEventLog",
        "Aow.Game.Events.TextEventLog",
        "Aow.Game.Events.TutorialMessageEventLog",
        "Aow.Game.Events.UserEventLog",
    ];

    [Fact]
    public void StateInsertionAndOffsetsMatchBaseCommit()
    {
        SortedDictionary<string, string> actual = [with(StringComparer.Ordinal)];
        foreach (string name in s_names)
        {
            for (int mapped = 0; mapped < 2; mapped++)
            {
                for (int owned = 0; owned < 2; owned++)
                {
                    for (int route = 0; route < 4; route++)
                    {
                        Type type = typeof(EventLog).Assembly.GetType(name)!;
                        using OrderingRecording.Fixture fixture = new(type);
                        if (mapped == 1)
                        {
                            EngineFixture.SetMap(EngineFixture.Configure(new StrategicMap(null!, holder: EngineFixture.Engine), value =>
                            {
                                value.Day = 7;
                                value.CurrentPlayer = PlayerId.FromOriginal(unchecked((sbyte)200));
                            }));
                        }

                        EventListOwner? owner = owned == 1 ? new EventListOwner() : null;
                        EventLog? value = null;
                        string state;
                        try
                        {
                            value = (EventLog)OrderingRecording.Create(type, owner, route, fixture.Engine);
                            Assert.IsType(type, @object: value);
                            state = OrderingRecording.Snapshot(value);
                        }
                        catch (Exception error) when (error is not XunitException)
                        {
                            state = $"{error.GetType().FullName}:{error.Message}";
                        }
                        Assert.NotNull(value);
                        actual.Add($"{name}.map{mapped}.owner{owned}.route{route}.fail0", JsonSerializer.Serialize(new { State = state, owner?.Observations, owner?.Changes, Count = owner?.GetCount(), owner?.DestroyEntries, (value ?? owner?.Last)?.RefCount }));
                        EngineFixture.Engine.Dictionary = fixture.Dictionary;
                        StrategicMap destroying = EngineFixture.Configure(new StrategicMap(null!, holder: EngineFixture.Engine), value => { value.Status = HexSpriteMapStatuses.Destroying; });
                        EngineFixture.SetMap(destroying);
                        // Route 3 builds the log on the fixture's own engine. That engine holds the
                        // same destroying map the global holds, so disposal reads that map.
                        fixture.Engine.StrategicMap = destroying;
                        if (owner is not null)
                        {
                            _ = (value?.AddRef());
                        }

                        // A log built with no holder still disposes against the fixture map.
                        value?.Holder = EngineFixture.Engine;
                        value?.Dispose();
                        owner?.Dispose();
                    }
                }
            }
        }

        OrderingRecording.Check("EventLogConstruction", actual);
    }
}
