// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

public partial class RestartGameEventLog : MessageEventLog
{
    public RestartGameEventLog(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:2450
    public new const int ClassID = 131831;

    // Diplomacy.pas:2453
    public override string GetText() => AbilityRules.TranslateRStr(GameRestartedRStr);

    // Diplomacy.pas:2456
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        if (Trigger != ExecuteTrigger.NewTurn)
        {
            Result = base.Execute(Mode, Trigger, Done);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string GameRestartedRStr = "Game Restarted";

}
