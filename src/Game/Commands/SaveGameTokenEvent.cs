// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// save filename
public partial class SaveGameTokenEvent : TokenEvent
{
    public SaveGameTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135444;

    public string FileName = "";

    // MoveT.PAS:1848
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.String(ref FileName, 15);
    }

    // MoveT.PAS:1854
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = default;
        base.Execute(tokenEvent);
        if (StrategicMap.Current.SeatedPlayer == tokenEvent.ActiveTacticalBattleIndex)
        {
            if (tokenEvent.ExecuteCount == 0)
            {
                var fileName = ExtractFileName(FileName);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_savingGameFormat), [fileName]);
                StrategicMap.Current.ShowMessage(format);
                result = ExecuteResult.Retry;
            }
            else
            {
                var fileName = ExtractFileName(FileName);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.GameSavedFormat), [fileName]);
                StrategicMap.Current.ShowMessage(format);
                try
                {
                    StrategicMap.Current.DisableTurnTimer();
                    if (GetEngine().Campaign != null)
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
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_errorSavingFormat), [FileName]);
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
