// Translated from E2CampaignSupport.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWVideoDlg;

namespace Aow.Units.E2CampaignSupport
{
    public static partial class E2CampaignSupport
    {
        // E2CampaignSupport.pas:17
        // { AoWE.htAll }
        // Original campaign access routines. and
        public static TWMCampaignScenario GetCampaignScenario(TWMCampaign Campaign, int Index) => (TWMCampaignScenario)Campaign.FScenarios.GetScenario(Index);

        // E2CampaignSupport.pas:22
        public static void SelectCampaignPath(TWMCampaign Campaign, int Index)
        {
            if (Index >= 0 && Index < 3)
            {
                Campaign.FState = 2;
                Campaign.FPage = 0;
                var path = GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID).FPaths[Index];
                Campaign.SelectNextScenario(Campaign.FScenarios.IndexOfID(path));
                if (GetCampaignScenario(Campaign, Campaign.FNextScenarioID).FPaths[3] == -1)
                {
                    var path2 = GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID).FPaths[3];
                    GetCampaignScenario(Campaign, Campaign.FNextScenarioID).FPaths[3] = path2;
                }
            }
        }

        public static TSet<THeroType> AllHeroTypes = TSet<THeroType>.Of(THeroType.htHero, THeroType.htLeader, THeroType.htHeroLeader, THeroType.htMerchant);
    }
}
