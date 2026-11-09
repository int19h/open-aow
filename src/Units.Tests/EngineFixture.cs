using System.Reflection;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Xunit.Sdk;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

[CollectionDefinition("Engine", DisableParallelization = true)]
public sealed class EngineCollection : ICollectionFixture<EngineFixture> { }

// Real startup over the isolated host used by EngineStartupTests.
public sealed partial class EngineFixture : IDisposable
{
    private readonly RecordingHost host = new("fixture");
    private static RecordingHost services = null!;

    internal static TAoWEngine Engine { get; private set; } = null!;
    internal static TAoWHSSet GameSet { get; private set; } = null!;
    internal static string Root => services.GameRoot;

    private static readonly Dictionary<FieldInfo, object?> state = [];

    internal static void RestoreEngine()
    {
        foreach (var pair in state) pair.Key.SetValue(Engine, pair.Value);
        Game.AoWEngine = Engine;
        Game.AoWHSSet = GameSet;
        Game.AoWHSMap = null!;
    }

    public EngineFixture()
    {
        Runtime.InstallHost(host);
        services = host;
        Game.ValidateAoWEngine();
        Engine = Game.AoWEngine;
        GameSet = Game.AoWHSSet;
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

    internal static TAoWHSSet CreateGameSet(TImageLibraryManager manager)
    {
        var value = TAoWHSSet.Create(null!);
        typeof(THSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(value, manager);
        return value;
    }

    // Observers override insertion around this one membership and removal implementation.
    internal class Owner() : TECustomNode(null!)
    {
        internal readonly List<TEObject> Members = [];
        internal int Removals;

        public override int AddChild(TEObject child)
        {
            Members.Add(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(TEObject child)
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
