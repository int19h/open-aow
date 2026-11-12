// Translated from EventLog.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Events;

// message, inherited player
public partial class GameOverEventLog : EventLog
{
    // EventLog.pas:771
    public GameOverEventLog(ECustomNode owner) : base(owner)
    {
        Message = new StringList();
    }

    // EventLog.pas:777
    public new const int ClassId = 131812;

    public StringList Message { get; }
    public sbyte TargetPlayer { get; set; }

    // EventLog.pas:774
    public override void Dispose()
    {
        Message?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:780
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Lines(Message, 20);
    }

    // EventLog.pas:783
    public override string GetText()
    {
        string result = "";
        switch (StrategicMap.Current.Players.GetPlayers(_player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                var name = StrategicMap.Current.Players.GetPlayers(_player).GetName();
                result = RtlUnit.Format(AbilityRules.TranslateResourceString(s_gameOverDefeatEventLogFormat), [name]);
                break;
            case GameOverStatus.Won:
                var name2 = StrategicMap.Current.Players.GetPlayers(_player).GetName();
                result = RtlUnit.Format(AbilityRules.TranslateResourceString(s_gameOverVictoryEventLogFormat), [name2]);
                break;
        }

        return result;
    }

    // EventLog.pas:794
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        switch (trigger)
        {
            case ExecuteTrigger.User:
                if (StrategicMap.Current.SeatedPlayer != _player)
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
    public void Handled()
    {
        StrategicMap.Current.GameOverEventHandled(_player);
    }

    private static readonly string s_gameOverDefeatEventLogFormat = "%s is defeated!";
    private static readonly string s_gameOverVictoryEventLogFormat = "%s is victorious!";

}
