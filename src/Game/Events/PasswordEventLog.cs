// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

// password, title, prompt, button
public partial class PasswordEventLog : EventLog
{
    // EventLog.pas:877
    public PasswordEventLog(ECustomNode owner) : base(owner)
    {
        Password = "";
        Prompt = AbilityRules.TranslateResourceString(s_enterPasswordText);
        Title = AbilityRules.TranslateResourceString(s_passwordVerificationText);
        PressedButton = PressedEventLogButton.None;
    }

    public string Password = "";
    public string Title = "";
    public string Prompt = "";
    public PressedEventLogButton PressedButton;

    private static readonly string s_enterPasswordText = "Enter password:";
    private static readonly string s_passwordVerificationText = "Password Verification";

}
