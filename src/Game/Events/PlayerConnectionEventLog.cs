// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Events;

// { Interface declaration run owned by P1. }
// reason, disconnected player
public partial class PlayerConnectionEventLog : MessageEventLog
{
    public PlayerConnectionEventLog(ECustomNode owner) : base(owner)
    { }

    public byte FReason;
    public sbyte FDisconnectedPlayer;

    // PlayerC.PAS:1714
    public override bool Yes()
    {
        SaveGameEventLog Log = default;
        Log = new SaveGameEventLog(null);
        Log.FDefaultName = "";
        Log.FDirectory = GameTables.AoWEngine.StartupDirectory + "Save\\";
        AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ExitSaveGameCallBack);
        Log.Release();
        return true;
    }

    // PlayerC.PAS:1726
    public override bool No()
    {
        AoWHSMap.Current.ExecuteExitGame();
        return true;
    }

    // PlayerC.PAS:1671
    public void Setup(sbyte Player, byte Reason)
    {
        FReason = Reason;
        FDisconnectedPlayer = Player;
        switch (FReason)
        {
            case 3:
                FButtons = Buttons.YesNo;
                var outOfSync = AbilityRules.TranslateRStr(OutOfSyncRStr);
                FMessage.Text = outOfSync;
                var aoWETranslateRStr = AbilityRules.TranslateRStr(TerminateGameSaveRStr);
                FMessage.Add(aoWETranslateRStr);
                break;
            case 1:
                FButtons = Buttons.YesNo;
                var name = AoWHSMap.Current.FPlayers.GetPlayers(FDisconnectedPlayer).GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(ConnectionXTimeOutRStr), [name]);
                FMessage.Text = format;
                FMessage.Add("");
                var terminateGameSave = AbilityRules.TranslateRStr(TerminateGameSaveRStr);
                FMessage.Add(terminateGameSave);
                break;
            case 2:
                FButtons = Buttons.YesNo;
                if (FDisconnectedPlayer != -1)
                {
                    var name2 = AoWHSMap.Current.FPlayers.GetPlayers(FDisconnectedPlayer).GetName();
                    var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(ConnectionXLostRStr), [name2]);
                    FMessage.Text = aoWEFormat;
                }
                else
                {
                    var connectionLost = AbilityRules.TranslateRStr(ConnectionLostRStr);
                    FMessage.Text = connectionLost;
                }

                FMessage.Add("");
                var terminateGameSaveText = AbilityRules.TranslateRStr(TerminateGameSaveRStr);
                FMessage.Add(terminateGameSaveText);
                break;
        }
    }

    // PlayerC.PAS:1703
    public void ExitSaveGameCallBack(EventLog Log)
    {
        if (!IsEmpty(((SaveGameEventLog)Log).FFileName))
        {
            if (GameTables.AoWEngine.FCampaign != null)
            {
                GameTables.AoWEngine.SaveCampaign(((SaveGameEventLog)Log).FFileName);
            }
            else
            {
                GameTables.AoWEngine.SaveHSM(((SaveGameEventLog)Log).FFileName, null, GameProfile.WorldMap);
            }
        }
        else
        {
            // { Original exits twice when the dialog is canceled. }
            AoWHSMap.Current.ExecuteExitGame();
        }

        AoWHSMap.Current.ExecuteExitGame();
    }

    private static readonly string ConnectionLostRStr = "Connection Lost!";
    private static readonly string ConnectionXLostRStr = "Connection to %s lost!";
    private static readonly string ConnectionXTimeOutRStr = "Connection to %s timed out!";
    private static readonly string OutOfSyncRStr = "Out of sync!";
    private static readonly string TerminateGameSaveRStr = "The game will be terminated, do you want to save?";

}
