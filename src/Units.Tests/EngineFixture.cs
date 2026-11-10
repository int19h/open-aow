using System.Reflection;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;
using Xunit.Sdk;

namespace Aow.Units.Tests;

[CollectionDefinition("Engine", DisableParallelization = true)]
public sealed class EngineCollection : ICollectionFixture<EngineFixture> { }

// Real startup over the isolated host used by EngineStartupTests.
public sealed partial class EngineFixture : IDisposable
{
    private readonly RecordingHost host = new("fixture");
    private static RecordingHost services = null!;

    internal static AoWEngine Engine { get; private set; } = null!;
    internal static AoWHSSet GameSet { get; private set; } = null!;
    internal static string Root => services.GameRoot;

    private static readonly Dictionary<FieldInfo, object?> state = [];

    internal static void RestoreEngine()
    {
        foreach (var pair in state) pair.Key.SetValue(Engine, pair.Value);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine = Engine;
        Aow.Game.Engine.AoWHSSet.Current = GameSet;
        Aow.Game.World.Map.AoWHSMap.Current = null!;
    }

    public EngineFixture()
    {
        Runtime.InstallHost(host);
        services = host;
        Aow.Game.Magic.Abilities.GameTables.ValidateAoWEngine();
        Engine = Aow.Game.Magic.Abilities.GameTables.AoWEngine;
        GameSet = Aow.Game.Engine.AoWHSSet.Current;
        for (Type? type = Engine.GetType(); type is not null && type != typeof(object); type = type.BaseType)
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (field.Name == "captureOrdinal") continue;
                if (!field.IsInitOnly) state.Add(field, field.GetValue(Engine));
            }
        host.RestoreServices();
    }

    internal static void InstallServices() => Runtime.InstallHost(services);

    internal static IRtlHost Services => services;

    internal static AoWHSSet CreateGameSet(ImageLibraryManager manager)
    {
        var value = AoWHSSet.Create(null!);
        typeof(HSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(value, manager);
        return value;
    }

    // Observers override insertion around this one membership and removal implementation.
    internal class Owner() : ECustomNode(null!)
    {
        internal readonly List<EObject> Members = [];
        internal int Removals;

        public override int AddChild(EObject child)
        {
            Members.Add(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EObject child)
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
            services = null!;
            state.Clear();
            host.Dispose();
        }
    }
}

// Each case restores the fields it injects into the shared startup engine.
[AttributeUsage(AttributeTargets.Class)]
public sealed class EngineStateAttribute : BeforeAfterTestAttribute
{
    private IFileSystem files = null!;
    private ISettingsStore? settings;
    private IDialogService? dialogs;
    private IShellService? shell;
    private IClock clock = null!;
    private string exe = "";
    private Action<string> message = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        files = Runtime.Files; settings = Runtime.Settings; dialogs = Runtime.Dialogs; shell = Runtime.Shell;
        clock = Runtime.Clock; exe = Runtime.Application.ExeName;
        message = Runtime.MessageHandler;
        EngineFixture.InstallServices();
        EngineFixture.RestoreEngine();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        EngineFixture.RestoreEngine();
        Runtime.Files = files; Runtime.Settings = settings; Runtime.Dialogs = dialogs; Runtime.Shell = shell;
        Runtime.Clock = clock; Runtime.Application.ExeName = exe;
        Runtime.MessageHandler = message;
    }
}
