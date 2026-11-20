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
public partial class SaveGameTokenEvent(CustomNode owner) : TokenEvent(owner)
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
        ExecuteResult result = default;
        _ = base.Execute(tokenEvent);
        if (StrategicMap.Current.SeatedPlayer == tokenEvent.ActiveTacticalBattleIndex)
        {
            if (tokenEvent.ExecuteCount == 0)
            {
                string fileName = ExtractFileName(FileName);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_savingGameFormat), [fileName]);
                StrategicMap.Current.ShowMessage(format);
                result = ExecuteResult.Retry;
            }
            else
            {
                string fileName = ExtractFileName(FileName);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.GameSavedFormat), [fileName]);
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
                        GetEngine().SaveHsm(FileName, null, GameProfile.WorldMap);
                        StrategicMap.Current.FileName = FileName;
                    }

                    StrategicMap.Current.EnableTurnTimer();
                }
                catch
                {
                    // { Original leaves the turn timer disabled after a save failure. }
                    string formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_errorSavingFormat), [FileName]);
                    StrategicMap.Current.ShowMessage(formattedText);
                }

                result = ExecuteResult.Done;
            }
        }
        else
        {
            result = ExecuteResult.Done;
        }

        return result;
    }

    private static readonly string s_savingGameFormat = "saving: %s";
    private static readonly string s_errorSavingFormat = "Error saving: %s";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
