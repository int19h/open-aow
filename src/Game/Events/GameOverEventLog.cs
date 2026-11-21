#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from EventLog.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Events;

// message, inherited player
public partial class GameOverEventLog(CustomNode owner) : EventLog(owner)
{

    // EventLog.pas:777
    public new const int ClassId = 131_812;

    public DelphiStringList Message { get; } = [];
    public sbyte TargetPlayer { get; set; }

    // EventLog.pas:774
    public override void Dispose()
    {
        Message?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:780
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Lines(name: "Message", lines: Message);
    }

    // EventLog.pas:783
    public override string GetText()
    {
        switch (StrategicMap.Current.Players.GetPlayers(Player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                string name = StrategicMap.Current.Players.GetPlayers(Player).GetName();
                return DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameOverDefeatEventLogFormat), args: [name]);
            case GameOverStatus.Won:
                string name2 = StrategicMap.Current.Players.GetPlayers(Player).GetName();
                return DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameOverVictoryEventLogFormat), args: [name2]);
            case GameOverStatus.None:
            default:
                break;
        }

        return "";
    }

    // EventLog.pas:794
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        switch (trigger)
        {
            case ExecuteTrigger.User:
                if (StrategicMap.Current.SeatedPlayer != Player)
                {
                    StrategicMap.Current.TriggerExecuteEventLog(this, immediate: mode, done);
                    return true;
                }
                else
                {
                    return false;
                }
            case ExecuteTrigger.Event:
                StrategicMap.Current.TriggerExecuteEventLog(this, immediate: mode, done);
                return true;
            case ExecuteTrigger.NewTurn:
                StrategicMap.Current.TriggerExecuteEventLog(this, immediate: mode, done);
                return true;
            default:
                return false;
        }
    }

    // EventLog.pas:791
    public void Handled() => StrategicMap.Current.GameOverEventHandled(Player);

    private static readonly string s_gameOverDefeatEventLogFormat = "%s is defeated!";
    private static readonly string s_gameOverVictoryEventLogFormat = "%s is victorious!";

}
