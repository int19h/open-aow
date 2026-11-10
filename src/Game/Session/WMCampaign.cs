// Translated from AoWVideoDlg.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// faction, state, page
public partial class WMCampaign : AoWCampaign
{
    // AoWVideoDlg.pas:334
    public new const int ClassID = 2248704;

    public int FFaction;
    public int FState;
    public int FPage;

    // AoWVideoDlg.pas:337
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FFaction, 30);
    }

    // AoWVideoDlg.pas:324
    public WMCampaign(ECustomNode owner) : base(owner)
    {
    }

    // AoWVideoDlg.pas:348
    public override void Clear()
    {
        base.Clear();
    }

    // AoWVideoDlg.pas:343
    public override AoWCampaignScenario CreateScenario() => new WMCampaignScenario(null);

    // AoWVideoDlg.pas:353
    public override string GetAutoSaveFileName(CampaignAutoSaveType Auto)
    {
        string Result;
        if (Auto == CampaignAutoSaveType.ScenarioFinished)
        {
            Result = AoWHSMap.Current.FMapName;
            if (FFaction == 0)
            {
                Result = Result + "_" + AbilityRules.TranslateRStr(WonRStr);
            }
            else
            {
                // { Original uses Won for both factions. }
                Result = Result + "-" + AbilityRules.TranslateRStr(WonRStr);
            }

            // The campaign autosave takes the extension the host writes (own-formats.md 4.2).
            Result = Result + FileBoundary.WriteExtension(DocumentKind.CampaignSave);
        }
        else
        {
            Result = base.GetAutoSaveFileName(Auto);
        }

        return Result;
    }

    private static readonly string WonRStr = "Won";

    internal static WMCampaignScenario GetCampaignScenario(WMCampaign Campaign, int Index) => (WMCampaignScenario)Campaign.FScenarios.GetScenario(Index);

}
