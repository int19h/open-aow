using Aow.Rtl;
using Aow.Units.AGCPlugins;
using Aow.Units.AoWE;

namespace Aow.Units.IndependentAG;

public partial class TRaidAGC
{
    public override void ProcessStates()
    {
        switch (FState)
        {
            case 0:
                var finder = new TAIGroupTargetFinder(null!);
                finder.Setup(this);
                finder.FSelector.FMovePointsMax = 100;
                finder.FSelector.FTargetInfo.Mode = TAITargetMode.tmRaid;
                ActivatePlugin(finder);
                finder.Release();
                FState++;
                break;
            case 1:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    FTargets.SortOnPriority();
                    SetupME();
                    var target = FTargets.GetTargets(0);
                    ExecuteME(target.FLocation.X, target.FLocation.Y, target.FLocation.L);
                    FTargets.Delete(0);
                    FState++;
                }
                else Done();
                break;
            case 2:
                var raze = new TAIGroupRazeControl(null!);
                raze.Setup(this);
                // 557D9AB4 loads the set byte 04 from 557D9ADC in the original.
                raze.FMode = TSet<TAIGroupRazeControlModeElement>.Of(TAIGroupRazeControlModeElement.rcmAlways);
                ActivatePlugin(raze);
                raze.Release();
                FState = 1;
                break;
        }
    }
}
