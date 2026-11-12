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
        internal readonly List<EngineObject> Members = [];
        internal int Removals;

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
