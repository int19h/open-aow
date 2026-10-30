using System.Reflection;
using Xunit.Sdk;
using Aow.Units.AoWE;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

[CollectionDefinition("Engine", DisableParallelization = true)]
public sealed class EngineCollection : ICollectionFixture<EngineFixture> { }

// Real startup over the isolated host used by EngineStartupTests.
public sealed class EngineFixture : IDisposable
{
    private readonly EngineStartupTests.RecordingHost host = new("fixture");
    internal static TAoWEngine Engine { get; private set; } = null!;

    private static readonly Dictionary<FieldInfo, object?> state = [];
    private static TAoWHSSet set = null!;

    internal static void RestoreEngine()
    {
        foreach (var pair in state) pair.Key.SetValue(Engine, pair.Value);
        Game.AoWEngine = Engine;
        Game.AoWHSSet = set;
        Game.AoWHSMap = null!;
    }

    public EngineFixture()
    {
        Rtl.InstallHost(host);
        Game.ValidateAoWEngine();
        Engine = Game.AoWEngine;
        set = Game.AoWHSSet;
        for (Type? type = Engine.GetType(); type is not null && type != typeof(TObject); type = type.BaseType)
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                if (!field.IsInitOnly) state.Add(field, field.GetValue(Engine));
        host.RestoreServices();
    }

    internal static T Configure<T>(T value, Action<T> configure)
    {
        configure(value);
        return value;
    }

    public void Dispose()
    {
        RestoreEngine();
        try { Engine.Destroy(); }
        finally
        {
            Engine = null!;
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
    private IClock clock = null!;
    private AnsiString exe;
    public override void Before(MethodInfo methodUnderTest)
    {
        files = Rtl.Files; settings = Rtl.Settings; dialogs = Rtl.Dialogs;
        clock = Rtl.Clock; exe = Rtl.Application.ExeName;
        Rtl.Files = new SystemIoFileSystem();
        Rtl.Settings = new MemorySettingsStore();
        EngineFixture.RestoreEngine();
    }
    public override void After(MethodInfo methodUnderTest)
    {
        EngineFixture.RestoreEngine();
        Rtl.Files = files; Rtl.Settings = settings; Rtl.Dialogs = dialogs;
        Rtl.Clock = clock; Rtl.Application.ExeName = exe;
    }
}
