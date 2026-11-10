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
        FPassword = "";
        FPrompt = AbilityRules.TranslateRStr(EnterPasswordRStr);
        FTitle = AbilityRules.TranslateRStr(PasswordVerificationRStr);
        FPressedButton = PressedEventLogButton.None;
    }

    public string FPassword = "";
    public string FTitle = "";
    public string FPrompt = "";
    public PressedEventLogButton FPressedButton;

    private static readonly string EnterPasswordRStr = "Enter password:";
    private static readonly string PasswordVerificationRStr = "Password Verification";

}
