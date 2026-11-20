#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.Commands;

// save filename
public class SaveGameTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135444;

    public string FileName { get; set; } = "";

    // MoveT.PAS:1848
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        FileName = persister.String("FileName", value: FileName);
    }

    // MoveT.PAS:1854
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        if (StrategicMap.Current.SeatedPlayer == tokenEvent.ActiveTacticalBattleIndex)
        {
            if (tokenEvent.ExecuteCount == 0)
            {
                string fileName = ExtractFileName(FileName);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_savingGameFormat), arguments: [fileName]);
                StrategicMap.Current.ShowMessage(format);
                return ExecuteResult.Retry;
            }
            else
            {
                string fileName = ExtractFileName(FileName);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GameSavedFormat), arguments: [fileName]);
                StrategicMap.Current.ShowMessage(format);
                try
                {
                    StrategicMap.Current.DisableTurnTimer();
                    if (GetEngine().Campaign is not null)
                    {
                        GetEngine().SaveCampaign(FileName);
                        GetEngine().Campaign.FileName = FileName;
                    }
                    else
                    {
                        GetEngine().SaveHsm(FileName, progress: null, profile: GameProfile.WorldMap);
                        StrategicMap.Current.FileName = FileName;
                    }

                    StrategicMap.Current.EnableTurnTimer();
                }
                catch
                {
                    // { Original leaves the turn timer disabled after a save failure. }
                    string formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_errorSavingFormat), arguments: [FileName]);
                    StrategicMap.Current.ShowMessage(formattedText);
                }

                return ExecuteResult.Done;
            }
        }
        else
        {
            return ExecuteResult.Done;
        }
    }

    private static readonly string s_savingGameFormat = "saving: %s";
    private static readonly string s_errorSavingFormat = "Error saving: %s";
}
