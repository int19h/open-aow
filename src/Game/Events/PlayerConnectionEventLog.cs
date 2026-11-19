#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Events;

// { Interface declaration run owned by P1. }
// reason, disconnected player
public partial class PlayerConnectionEventLog(CustomNode owner, EngineObject holder) : MessageEventLog(owner, holder)
{
    private byte _reason;
    private sbyte _disconnectedPlayer;

    // PlayerC.PAS:1714
    public override bool Yes()
    {
        SaveGameEventLog log = default;
        log = new SaveGameEventLog(null, holder: CreationHolder())
        {
            DefaultName = "",
            Directory = $"{GetEngine().StartupDirectory}Save\\"
        };
        StrategicMap.Current.TriggerTypedExecuteEventLog(log, false, ExitSaveGameCallback);
        _ = log.Release();
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
                string outOfSync = AbilityRules.TranslateResourceString(GetEngine(), s_outOfSyncText);
                Message.Text = outOfSync;
                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_terminateGameSaveText);
                _ = Message.Add(translatedText);
                break;
            case 1:
                Buttons = MessageEventLogButtons.YesNo;
                string name = StrategicMap.Current.Players.GetPlayers(_disconnectedPlayer).GetName();
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_connectionTimeOutFormat), [name]);
                Message.Text = format;
                _ = Message.Add("");
                string terminateGameSave = AbilityRules.TranslateResourceString(GetEngine(), s_terminateGameSaveText);
                _ = Message.Add(terminateGameSave);
                break;
            case 2:
                Buttons = MessageEventLogButtons.YesNo;
                if (_disconnectedPlayer != -1)
                {
                    string name2 = StrategicMap.Current.Players.GetPlayers(_disconnectedPlayer).GetName();
                    string formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_connectionLostFormat), [name2]);
                    Message.Text = formattedText;
                }
                else
                {
                    string connectionLost = AbilityRules.TranslateResourceString(GetEngine(), s_connectionLostText);
                    Message.Text = connectionLost;
                }

                _ = Message.Add("");
                string terminateGameSaveText = AbilityRules.TranslateResourceString(GetEngine(), s_terminateGameSaveText);
                _ = Message.Add(terminateGameSaveText);
                break;
            default:
                break;
        }
    }

    // PlayerC.PAS:1703
    public void ExitSaveGameCallback(SaveGameEventLog log)
    {
        if (!IsEmpty(log.FileName))
        {
            if (GetEngine().Campaign != null)
            {
                GetEngine().SaveCampaign(log.FileName);
            }
            else
            {
                GetEngine().SaveHsm(log.FileName, null, GameProfile.WorldMap);
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
