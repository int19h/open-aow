using System.Reflection;

using Xunit.Sdk;

namespace Aow.Game.Tests;

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
