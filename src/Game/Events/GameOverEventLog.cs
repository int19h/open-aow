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
    public new const int ClassId = 131812;

    public DelphiStringList Message { get; } = new DelphiStringList();
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
        PersistSupport.Lines(persister, name: "Message", lines: Message);
    }

    // EventLog.pas:783
    public override string GetText()
    {
        string result = "";
        switch (StrategicMap.Current.Players.GetPlayers(Player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                string name = StrategicMap.Current.Players.GetPlayers(Player).GetName();
                result = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), s_gameOverDefeatEventLogFormat), [name]);
                break;
            case GameOverStatus.Won:
                string name2 = StrategicMap.Current.Players.GetPlayers(Player).GetName();
                result = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), s_gameOverVictoryEventLogFormat), [name2]);
                break;
            case GameOverStatus.None:
            default:
                break;
        }

        return result;
    }

    // EventLog.pas:794
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result;
        switch (trigger)
        {
            case ExecuteTrigger.User:
                if (StrategicMap.Current.SeatedPlayer != Player)
                {
                    StrategicMap.Current.TriggerExecuteEventLog(this, mode, done);
                    result = true;
                }
                else
                {
                    result = false;
                }

                break;
            case ExecuteTrigger.Event:
                StrategicMap.Current.TriggerExecuteEventLog(this, mode, done);
                result = true;
                break;
            case ExecuteTrigger.NewTurn:
                StrategicMap.Current.TriggerExecuteEventLog(this, mode, done);
                result = true;
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    // EventLog.pas:791
    public void Handled() => StrategicMap.Current.GameOverEventHandled(Player);

    private static readonly string s_gameOverDefeatEventLogFormat = "%s is defeated!";
    private static readonly string s_gameOverVictoryEventLogFormat = "%s is victorious!";

}
