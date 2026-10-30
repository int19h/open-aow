using Aow.Rtl;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWCampaignTransferDlg;

public partial class TCampaignTransferDlg
{
    // AoW.exe pushes "Scenario\Campaign\"
    public void Execute()
    {
        var campaign = AoWEngine.FCampaign;
        ResetSelection();
        campaign.FTransferSettings.SetupSelection(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Campaign\\" +
            campaign.FScenarios.GetScenario(campaign.FNextScenarioID).FFileName);
        if (campaign.FTransferSettings.FTransferPointsMax > 0)
        {
            GoldSlider.Max = campaign.FTransferSettings.FTransferGoldMax;
            ManaSlider.Max = campaign.FTransferSettings.FTransferManaMax;
            UpdateControls();
            TransBookWin.Visible = true;
        }
        else BookStartClick(this);
    }
}
