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
        FMessage = new StringList();
    }

    // EventLog.pas:777
    public new const int ClassID = 131812;

    public StringList FMessage;
    public sbyte FTargetPlayer;

    // EventLog.pas:774
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // EventLog.pas:780
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 20);
    }

    // EventLog.pas:783
    public override string GetText()
    {
        string Result = "";
        switch (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FGameOverStatus)
        {
            case GameOverStatus.Lost:
                var name = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetName();
                Result = RtlUnit.Format(AbilityRules.TranslateRStr(GameOverDefeatEventLogRStr), [name]);
                break;
            case GameOverStatus.Won:
                var name2 = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetName();
                Result = RtlUnit.Format(AbilityRules.TranslateRStr(GameOverVictoryEventLogRStr), [name2]);
                break;
        }

        return Result;
    }

    // EventLog.pas:794
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        switch (Trigger)
        {
            case ExecuteTrigger.User:
                if (AoWHSMap.Current.FSeatedPlayer != FPlayer)
                {
                    AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
                    Result = true;
                }
                else
                {
                    Result = false;
                }

                break;
            case ExecuteTrigger.Event:
                AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
                Result = true;
                break;
            case ExecuteTrigger.NewTurn:
                AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
                Result = true;
                break;
            default:
                Result = false;
                break;
        }

        return Result;
    }

    // EventLog.pas:791
    public void Handled()
    {
        AoWHSMap.Current.GameOverEventHandled(FPlayer);
    }

    private static readonly string GameOverDefeatEventLogRStr = "%s is defeated!";
    private static readonly string GameOverVictoryEventLogRStr = "%s is victorious!";

}
