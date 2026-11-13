// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

// password, title, prompt, button
public partial class PasswordEventLog : EventLog
{
    // EventLog.pas:877
    public PasswordEventLog(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Password = "";
        Prompt = AbilityRules.TranslateResourceString(GetEngine(), s_enterPasswordText);
        Title = AbilityRules.TranslateResourceString(GetEngine(), s_passwordVerificationText);
        PressedButton = PressedEventLogButton.None;
    }

    public string Password { get; set; } = "";
    public string Title { get; set; } = "";
    public string Prompt { get; set; } = "";
    public PressedEventLogButton PressedButton { get; set; }

    private static readonly string s_enterPasswordText = "Enter password:";
    private static readonly string s_passwordVerificationText = "Password Verification";

}
