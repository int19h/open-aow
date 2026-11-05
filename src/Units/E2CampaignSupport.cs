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
        public static TWMCampaignScenario GetCampaignScenario(TWMCampaign Campaign, int Index)
        {
            TWMCampaignScenario Result = default;
            var call__1 = Campaign.FScenarios.GetScenario(Index);
            Result = (TWMCampaignScenario)call__1;
            return Result;
        }

        // E2CampaignSupport.pas:22
        public static void SelectCampaignPath(TWMCampaign Campaign, int Index)
        {
            if (Index >= 0 && Index < 3)
            {
                Campaign.FState = 2;
                Campaign.FPage = 0;
                var call__2 = GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID);
                var call__3 = call__2.FPaths[Index];
                var call__4 = Campaign.FScenarios.IndexOfID(call__3);
                Campaign.SelectNextScenario(call__4);
                var call__5 = GetCampaignScenario(Campaign, Campaign.FNextScenarioID);
                var call__6 = call__5.FPaths[3];
                if (call__6 == -1)
                {
                    var call__7 = GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID);
                    var call__8 = call__7.FPaths[3];
                    var call__9 = GetCampaignScenario(Campaign, Campaign.FNextScenarioID);
                    call__9.FPaths[3] = call__8;
                }
            }
        }

        public static TSet<THeroType> AllHeroTypes = TSet<THeroType>.Of(THeroType.htHero, THeroType.htLeader, THeroType.htHeroLeader, THeroType.htMerchant);
    }
}
