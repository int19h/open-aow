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
        _files = Runtime.Files; _settings = Runtime.Settings; _dialogs = Runtime.Dialogs; _shell = Runtime.Shell;
        _clock = Runtime.Clock; _executable = Runtime.Application.ExeName;
        _message = Runtime.MessageHandler;
        EngineFixture.InstallServices();
        EngineFixture.RestoreEngine();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        EngineFixture.RestoreEngine();
        Runtime.Files = _files; Runtime.Settings = _settings; Runtime.Dialogs = _dialogs; Runtime.Shell = _shell;
        Runtime.Clock = _clock; Runtime.Application.ExeName = _executable;
        Runtime.MessageHandler = _message;
    }
}
