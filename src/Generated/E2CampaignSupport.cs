// Generated from E2CampaignSupport.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;

namespace Aow.Units.E2CampaignSupport
{
    public static partial class E2CampaignSupport
    {
        // E2CampaignSupport.pas:17
        // { AoWE.htAll }
        // { Original campaign access routines, 0040BBB8, 0040BDE4 and 0040BF1C. }
        public static Aow.Units.AoWVideoDlg.TWMCampaignScenario GetCampaignScenario(Aow.Units.AoWVideoDlg.TWMCampaign Campaign, int Index)
        {
            Aow.Units.AoWVideoDlg.TWMCampaignScenario Result = default;
            var call__1 = Campaign.FScenarios.GetScenario(Index);
            Result = (Aow.Units.AoWVideoDlg.TWMCampaignScenario)call__1;
            return Result;
        }

        // E2CampaignSupport.pas:22
        public static void SelectCampaignPath(Aow.Units.AoWVideoDlg.TWMCampaign Campaign, int Index)
        {
            if ((Index >= 0) && (Index < 3))
            {
                Campaign.FState = 2;
                Campaign.FPage = 0;
                var call__2 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID);
                var call__3 = call__2.FPaths[Index];
                var call__4 = Campaign.FScenarios.IndexOfID(call__3);
                Campaign.SelectNextScenario(call__4);
                var call__5 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(Campaign, Campaign.FNextScenarioID);
                var call__6 = call__5.FPaths[3];
                if (call__6 == -1)
                {
                    var call__7 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID);
                    var call__8 = call__7.FPaths[3];
                    var call__9 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(Campaign, Campaign.FNextScenarioID);
                    call__9.FPaths[3] = call__8;
                }
            }
        }

        public static TSet<Aow.Units.AoWE.THeroType> AllHeroTypes = TSet<Aow.Units.AoWE.THeroType>.Of(Aow.Units.AoWE.THeroType.htHero, Aow.Units.AoWE.THeroType.htLeader, Aow.Units.AoWE.THeroType.htHeroLeader, Aow.Units.AoWE.THeroType.htMerchant);
    }
}
