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
    internal static AoWHSSet GameSet { get; private set; } = null!;
    internal static string Root => s_services.GameRoot;

    private static readonly Dictionary<FieldInfo, object?> s_state = [];

    internal static void RestoreEngine()
    {
        foreach (var pair in s_state) pair.Key.SetValue(Engine, pair.Value);
        GameTables.GameEngine = Engine;
        AoWHSSet.Current = GameSet;
        AoWHSMap.Current = null!;
    }

    public EngineFixture()
    {
        Runtime.InstallHost(_host);
        s_services = _host;
        GameTables.ValidateGameEngine();
        Engine = GameTables.GameEngine;
        GameSet = AoWHSSet.Current;
        for (Type? type = Engine.GetType(); type is not null && type != typeof(object); type = type.BaseType)
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (!field.IsInitOnly) s_state.Add(field, field.GetValue(Engine));
            }
        _host.RestoreServices();
    }

    internal static void InstallServices() => Runtime.InstallHost(s_services);

    internal static IRtlHost Services => s_services;

    internal static AoWHSSet CreateGameSet(ImageLibraryManager manager)
    {
        var value = AoWHSSet.Create(null!);
        typeof(HSSet).GetField("_imageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(value, manager);
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

    // The owner a construction test passes for the type: the recorder itself, or an owner of the
    // type's owner type that forwards to it.
    internal static ECustomNode OwnerFor(Type type, Owner recorder) =>
        recorder is null ? null!
        : typeof(Draw.HexSprite).IsAssignableFrom(type) ? new FieldOwner(recorder)
        : typeof(MapField).IsAssignableFrom(type) ? new LevelOwner(recorder)
        : typeof(Session.Race).IsAssignableFrom(type) ? new RaceListOwner(recorder)
        : typeof(World.Structures.ProductionItem).IsAssignableFrom(type) ? new QueueOwner(recorder)
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
