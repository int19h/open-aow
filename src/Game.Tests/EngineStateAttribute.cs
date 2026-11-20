using System.Reflection;
using Xunit.Sdk;

namespace Aow.Game.Tests;

// Each case restores the fields it injects into the shared startup engine.
[AttributeUsage(AttributeTargets.Class)]
public sealed class EngineStateAttribute : BeforeAfterTestAttribute
{
    private IFileSystem _files = null!;
    private ISettingsStore? _settings;
    private IDialogService? _dialogs;
    private IShellService? _shell;
    private IClock _clock = null!;
    private string _executable = "";
    private Action<string> _message = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        _files = RtlHost.Files;
        _settings = RtlHost.Settings;
        _dialogs = RtlHost.Dialogs;
        _shell = RtlHost.Shell;
        _clock = RtlHost.Clock;
        _executable = LegacyApplication.Application.ExeName;
        _message = LegacyApplication.MessageHandler;
        EngineFixture.InstallServices();
        EngineFixture.RestoreEngine();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        EngineFixture.RestoreEngine();
        RtlHost.Files = _files;
        RtlHost.Settings = _settings;
        RtlHost.Dialogs = _dialogs;
        RtlHost.Shell = _shell;
        RtlHost.Clock = _clock;
        LegacyApplication.Application.ExeName = _executable;
        LegacyApplication.MessageHandler = _message;
    }
}
