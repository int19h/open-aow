// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Ai.Groups;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Ai;

// control, selector, range, step, target range, mode
public partial class AIRecruitmentGroupFinder : AIGroupControlPlugin
{
    // AIGroup.pas:1981
    public AIRecruitmentGroupFinder(ECustomNode owner) : base(owner)
    {
        FSelector = new AIMoveTargetSelector(null);
    }

    public AIGroupControl FControl;
    public AIMoveTargetSelector FSelector;
    public int FRange;
    public int FStep;
    public int FTargetRange;
    public Set<ModeElement> FMode;

    // AIGroup.pas:1987
    public override void Dispose()
    {
        FSelector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1993
    public override bool GetBusy() => FSelector.FStatus == AIMoveTargetSelector.Status.Searching;

    // AIGroup.pas:2051
    public override bool Activate(AIGroupControl Control)
    {
        base.Activate(Control);
        FControl = Control;
        if (FMode.Contains(ModeElement.CalculateMovepathBFS))
        {
            FSelector.FMode = FSelector.FMode + Set<AIMoveTargetSelectorModeElement>.Of(AIMoveTargetSelectorModeElement.CreateMovepaths);
        }

        FSelector.Search(Control);
        return true;
    }

    // AIGroup.pas:2061
    public override bool Process()
    {
        bool Result;
        if (FSelector.FStatus == AIMoveTargetSelector.Status.Searching)
        {
            Result = FSelector.Process();
        }
        else
        {
            Result = false;
        }

        if (!Result)
        {
            Done();
        }

        return Result;
    }

    // AIGroup.pas:2038
    public virtual bool Setup(AIGroupControl Control)
    {
        Reset();
        FSelector.FSourceLocation = Control.FLocation;
        FSelector.FMovePointTable = Control.FMovePointTable;
        FSelector.FTargetInfo.Player = Control.FGroup.FPlayer;
        FSelector.FTargetInfo.IndependentRelation = Control.FGroup.FIndependentRelation;
        FSelector.FTargetInfo.Mode = AITargetMode.Recruit;
        FSelector.FMovePointsMax = 75;
        FMode = FMode + Set<ModeElement>.Of(ModeElement.CalculateMovepathBFS);
        return true;
    }

    // AIGroup.pas:1998
    public void RecruitGroupFoundEvent(MoveTarget Target, ref bool SearchDone)
    {
        ArmyHS ArmyHS;
        AbstractUnit AUnit;
        AIRecruitmentGroup Group;
        ArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(Target.FLocation.X, Target.FLocation.Y, Target.FLocation.L)).GetArmyHS();
        if (ArmyHS != null)
        {
            for (int I = ArmyHS.FArmy.Count - 1; I >= 0; I--)
            {
                AUnit = ArmyHS.FArmy.GetUnit(I);
                if (AUnit.FGroup != null && AUnit.FGroup.FLeader == AUnit && AUnit.FGroup != FControl.FGroup)
                {
                    Group = new AIRecruitmentGroup(null);
                    Group.SetGroup(AUnit.FGroup);
                    Group.FLocation = Target.FLocation;
                    Group.FMovePoints = Target.FMovePoints;
                    Group.FTransporterID = Target.FTransporterID;
                    if (FMode.Contains(ModeElement.CalculateMovepathBFS))
                    {
                        Group.FPathStrength = AbilityRules.GetMovepathBattleFieldStrength(FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, Target.FMovepath, FTargetRange, FRange, FStep);
                    }

                    FControl.RecruitmentGroupFoundEvent(Group, ref SearchDone);
                    Group.Release();
                }
            }
        }
    }

    // AIGroup.pas:2028
    public void Reset()
    {
        FMode = Set<ModeElement>.Of();
        FSelector.Reset();
        FSelector.FOnTargetFound = RecruitGroupFoundEvent;
        FRange = 8;
        FStep = 1;
        FTargetRange = 5;
    }

    public enum ModeElement : byte
    {
        CalculateMovepathBFS = 0
    }

    // intf_P5_07.inc:356

}
