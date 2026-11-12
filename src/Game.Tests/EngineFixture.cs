using System.Reflection;
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Real startup over the isolated host used by EngineStartupTests.
public sealed partial class EngineFixture : IDisposable
{
    private readonly RecordingHost _host = new("fixture");
    private static RecordingHost s_services = null!;

    internal static GameEngine Engine { get; private set; } = null!;
    internal static GamePack GameSet { get; private set; } = null!;
    internal static string Root => s_services.GameRoot;

    private static readonly Dictionary<FieldInfo, object?> s_state = [];

    internal static void RestoreEngine()
    {
        foreach (var pair in s_state) pair.Key.SetValue(Engine, pair.Value);
        GameTables.GameEngine = Engine;
        GamePack.Current = GameSet;
        StrategicMap.Current = null!;
    }

    public EngineFixture()
    {
        Runtime.InstallHost(_host);
        s_services = _host;
        GameTables.ValidateGameEngine();
        Engine = GameTables.GameEngine;
        GameSet = GamePack.Current;
        for (Type? type = Engine.GetType(); type is not null && type != typeof(object); type = type.BaseType)
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (!field.IsInitOnly) s_state.Add(field, field.GetValue(Engine));
            }
        _host.RestoreServices();
    }

    internal static void InstallServices() => Runtime.InstallHost(s_services);

    internal static IRtlHost Services => s_services;

    internal static GamePack CreateGameSet(ImageLibraryManager manager)
    {
        var value = GamePack.Create(null!);
        typeof(GamePack).GetField("_imageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(value, manager);
        return value;
    }

    // Observers override insertion around this one membership and removal implementation.
    internal class Owner() : ECustomNode(null!)
    {
        internal List<EngineObject> Members { get; } = [];
        internal int Removals { get; private set; }

        public override int AddChild(EngineObject child)
        {
            Members.Add(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
        {
            Removals++;
            int index = Members.IndexOf(child);
            Members.Remove(child);
            return index;
        }
    }

    // A field that records membership as Owner does, for a test that needs the field itself.
    internal class RecordingField() : MapField(null!)
    {
        internal List<EngineObject> Members { get; } = [];

        public override int AddChild(EngineObject child)
        {
            Members.Add(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
        {
            int index = Members.IndexOf(child);
            Members.Remove(child);
            return index;
        }
    }

    // A class with a typed owner takes only that owner (issue 158): a sprite a map field, a field
    // a map level, a race the race list and a production item the queue. These stand in for a
    // recording owner there and pass each membership change on to it.
    internal interface IForwarder
    {
        Owner Recorder { get; }
    }

    internal sealed class FieldOwner(Owner recorder) : MapField(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class LevelOwner(Owner recorder) : MapLevel(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class RaceListOwner(Owner recorder) : Session.RaceList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class QueueOwner(Owner recorder) : World.Structures.ProductionQueue(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class ContainerOwner(Owner recorder) : MapContainer(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    // Issue 158 part 3: a troop takes a unit list, an item an item list, and the children of a
    // reference list, an event log and a player take their list.
    internal sealed class UnitsOwner(Owner recorder) : Units.UnitList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class ItemsOwner(Owner recorder) : Units.ItemList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class RecruitmentGroupsOwner(Owner recorder) : AI.AIRecruitmentGroupList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class ControlTargetsOwner(Owner recorder) : AI.Groups.AIGroupControlTargetList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class ProductionLocationsOwner(Owner recorder) : AI.Groups.AIGroupProductionLocationList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class UnitProductionsOwner(Owner recorder) : AI.Groups.AIGroupUnitProductionList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class CombatActionsOwner(Owner recorder) : Combat.CombatActionList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class TokenEventsOwner(Owner recorder) : Net.TokenEventReferenceList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class DiplomaticActionsOwner(Owner recorder) : Session.DiplomaticActionList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class MoveTargetsOwner(Owner recorder) : World.Movement.MoveTargetList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class EventLogsOwner(Owner recorder) : Events.EventLogList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    internal sealed class PlayersOwner(Owner recorder) : Session.PlayerList(null!), IForwarder
    {
        public Owner Recorder => recorder;

        public override int AddChild(EngineObject child) => recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => recorder.RemoveChild(child);
    }

    // A map makes its selection and visibility lists in its constructor. They stay its own
    // children, and forwarding starts once the map is built.
    internal sealed class MapOwner : GameMap, IForwarder
    {
        private readonly Owner _recorder;

        public MapOwner(Owner recorder) : base(null!)
        {
            _recorder = recorder;
        }

        public Owner Recorder => _recorder;

        public override int AddChild(EngineObject child) => _recorder is null ? base.AddChild(child) : _recorder.AddChild(child);

        public override int RemoveChild(EngineObject child) => _recorder is null ? base.RemoveChild(child) : _recorder.RemoveChild(child);
    }

    // The owner a construction test passes for the type: the recorder itself, or an owner of the
    // type's owner type that forwards to it.
    internal static ECustomNode OwnerFor(Type type, Owner recorder) =>
        recorder is null ? null!
        : typeof(Draw.HexSprite).IsAssignableFrom(type) ? new FieldOwner(recorder)
        : typeof(MapField).IsAssignableFrom(type) ? new LevelOwner(recorder)
        : typeof(MapLevel).IsAssignableFrom(type) ? new ContainerOwner(recorder)
        : typeof(MapContainer).IsAssignableFrom(type) || typeof(VisibilityList).IsAssignableFrom(type) || typeof(HexSpriteSelection).IsAssignableFrom(type) ? new MapOwner(recorder)
        : typeof(Session.Race).IsAssignableFrom(type) ? new RaceListOwner(recorder)
        : typeof(World.Structures.ProductionItem).IsAssignableFrom(type) ? new QueueOwner(recorder)
        : typeof(Magic.Abilities.Troop).IsAssignableFrom(type) ? new UnitsOwner(recorder)
        : typeof(Magic.Abilities.Item).IsAssignableFrom(type) ? new ItemsOwner(recorder)
        : typeof(AI.AIRecruitmentGroup).IsAssignableFrom(type) ? new RecruitmentGroupsOwner(recorder)
        : typeof(AI.Groups.AIGroupControlTarget).IsAssignableFrom(type) ? new ControlTargetsOwner(recorder)
        : typeof(AI.Groups.AIGroupProductionLocation).IsAssignableFrom(type) ? new ProductionLocationsOwner(recorder)
        : typeof(AI.Groups.AIGroupUnitProduction).IsAssignableFrom(type) ? new UnitProductionsOwner(recorder)
        : typeof(Combat.Actions.CombatAction).IsAssignableFrom(type) ? new CombatActionsOwner(recorder)
        : typeof(Commands.TokenEvent).IsAssignableFrom(type) ? new TokenEventsOwner(recorder)
        : typeof(Session.DiplomaticAction).IsAssignableFrom(type) ? new DiplomaticActionsOwner(recorder)
        : typeof(World.Movement.MoveTarget).IsAssignableFrom(type) ? new MoveTargetsOwner(recorder)
        : typeof(Events.EventLog).IsAssignableFrom(type) ? new EventLogsOwner(recorder)
        : typeof(Session.Player).IsAssignableFrom(type) ? new PlayersOwner(recorder)
        : recorder;

    // Whether the owner is the recorder or an owner that forwards to it.
    internal static bool Forwards(ECustomNode owner, Owner recorder) =>
        ReferenceEquals(owner, recorder) || owner is IForwarder { Recorder: var forwarded } && forwarded == recorder;

    internal static T Configure<T>(T value, Action<T> configure)
    {
        configure(value);
        return value;
    }

    public void Dispose()
    {
        RestoreEngine();
        try { Engine.Dispose(); }
        finally
        {
            Engine = null!;
            GameSet = null!;
            s_services = null!;
            s_state.Clear();
            _host.Dispose();
        }
    }
}
