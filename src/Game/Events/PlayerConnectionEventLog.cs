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
    public PlayerConnectionEventLog(ECustomNode owner, EngineObject holder) : base(owner, holder)
    { }

    private byte _reason;
    private sbyte _disconnectedPlayer;

    // PlayerC.PAS:1714
    public override bool Yes()
    {
        SaveGameEventLog log = default;
        log = new SaveGameEventLog(null, holder: CreationHolder());
        log.DefaultName = "";
        log.Directory = GetEngine().StartupDirectory + "Save\\";
        StrategicMap.Current.TriggerExecuteEventLog(log, false, ExitSaveGameCallback);
        log.Release();
        return true;
    }

    // PlayerC.PAS:1726
    public override bool No()
    {
        StrategicMap.Current.ExecuteExitGame();
        return true;
    }

    // PlayerC.PAS:1671
    public void Setup(sbyte player, byte reason)
    {
        _reason = reason;
        _disconnectedPlayer = player;
        switch (_reason)
        {
            case 3:
                Buttons = MessageEventLogButtons.YesNo;
                var outOfSync = AbilityRules.TranslateResourceString(GetEngine(), s_outOfSyncText);
                Message.Text = outOfSync;
                var translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_terminateGameSaveText);
                Message.Add(translatedText);
                break;
            case 1:
                Buttons = MessageEventLogButtons.YesNo;
                var name = StrategicMap.Current.Players.GetPlayers(_disconnectedPlayer).GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_connectionTimeOutFormat), [name]);
                Message.Text = format;
                Message.Add("");
                var terminateGameSave = AbilityRules.TranslateResourceString(GetEngine(), s_terminateGameSaveText);
                Message.Add(terminateGameSave);
                break;
            case 2:
                Buttons = MessageEventLogButtons.YesNo;
                if (_disconnectedPlayer != -1)
                {
                    var name2 = StrategicMap.Current.Players.GetPlayers(_disconnectedPlayer).GetName();
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_connectionLostFormat), [name2]);
                    Message.Text = formattedText;
                }
                else
                {
                    var connectionLost = AbilityRules.TranslateResourceString(GetEngine(), s_connectionLostText);
                    Message.Text = connectionLost;
                }

                Message.Add("");
                var terminateGameSaveText = AbilityRules.TranslateResourceString(GetEngine(), s_terminateGameSaveText);
                Message.Add(terminateGameSaveText);
                break;
        }
    }

    // PlayerC.PAS:1703
    public void ExitSaveGameCallback(EventLog log)
    {
        if (!IsEmpty(((SaveGameEventLog)log).FileName))
        {
            if (GetEngine().Campaign != null)
            {
                GetEngine().SaveCampaign(((SaveGameEventLog)log).FileName);
            }
            else
            {
                GetEngine().SaveHsm(((SaveGameEventLog)log).FileName, null, GameProfile.WorldMap);
            }
        }
        else
        {
            // { Original exits twice when the dialog is canceled. }
            StrategicMap.Current.ExecuteExitGame();
        }

        StrategicMap.Current.ExecuteExitGame();
    }

    private static readonly string s_connectionLostText = "Connection Lost!";
    private static readonly string s_connectionLostFormat = "Connection to %s lost!";
    private static readonly string s_connectionTimeOutFormat = "Connection to %s timed out!";
    private static readonly string s_outOfSyncText = "Out of sync!";
    private static readonly string s_terminateGameSaveText = "The game will be terminated, do you want to save?";

}
