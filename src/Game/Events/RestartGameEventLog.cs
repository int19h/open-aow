// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

public partial class RestartGameEventLog : MessageEventLog
{
    public RestartGameEventLog(ECustomNode owner, EngineObject holder) : base(owner, holder)
    { }

    // Diplomacy.pas:2450
    public new const int ClassId = 131831;

    // Diplomacy.pas:2453
    public override string GetText() => AbilityRules.TranslateResourceString(GetEngine(), s_gameRestartedText);

    // Diplomacy.pas:2456
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        if (trigger != ExecuteTrigger.NewTurn)
        {
            result = base.Execute(mode, trigger, done);
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_gameRestartedText = "Game Restarted";

}
