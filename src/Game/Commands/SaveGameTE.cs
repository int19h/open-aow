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
public partial class SaveGameTE : TokenEvent
{
    public SaveGameTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135444;

    public string FFileName = "";

    // MoveT.PAS:1848
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FFileName, 15);
    }

    // MoveT.PAS:1854
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        ExecuteResult Result = default;
        base.Execute(TE);
        if (AoWHSMap.Current.FSeatedPlayer == TE.ActiveTCIndex)
        {
            if (TE.ExecuteCount == 0)
            {
                var fileName = ExtractFileName(FFileName);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(P5SavingGameRStr), [fileName]);
                AoWHSMap.Current.ShowMessage(format);
                Result = ExecuteResult.Retry;
            }
            else
            {
                var fileName = ExtractFileName(FFileName);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.GameSavedRStr), [fileName]);
                AoWHSMap.Current.ShowMessage(format);
                try
                {
                    AoWHSMap.Current.DisableTurnTimer();
                    if (GameTables.AoWEngine.FCampaign != null)
                    {
                        GameTables.AoWEngine.SaveCampaign(FFileName);
                        GameTables.AoWEngine.FCampaign.FFileName = FFileName;
                    }
                    else
                    {
                        GameTables.AoWEngine.SaveHSM(FFileName, null, GameProfile.WorldMap);
                        AoWHSMap.Current.FileName = FFileName;
                    }

                    AoWHSMap.Current.EnableTurnTimer();
                }
                catch
                {
                    // { Original leaves the turn timer disabled after a save failure. }
                    var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(P5ErrorSavingRStr), [FFileName]);
                    AoWHSMap.Current.ShowMessage(aoWEFormat);
                }

                Result = ExecuteResult.Done;
            }
        }
        else
        {
            Result = ExecuteResult.Done;
        }

        return Result;
    }

    private static readonly string P5SavingGameRStr = "saving: %s";
    private static readonly string P5ErrorSavingRStr = "Error saving: %s";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
