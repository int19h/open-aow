// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

public class RestartGameEventLog(CustomNode owner, EngineObject holder) : MessageEventLog(owner, holder)
{

    // Diplomacy.pas:2450
    public new const int ClassId = 131_831;

    // Diplomacy.pas:2453
    public override string GetText() => AbilityRules.TranslateResourceString(GetEngine(), S: s_gameRestartedText);

    // Diplomacy.pas:2456
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done) => trigger != ExecuteTrigger.NewTurn && base.Execute(mode, trigger, done);

    private static readonly string s_gameRestartedText = "Game Restarted";

}
