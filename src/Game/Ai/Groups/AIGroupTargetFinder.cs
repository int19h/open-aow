// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

// control, selector, mode, ranges, step, damage types
public partial class AIGroupTargetFinder : AIGroupControlPlugin
{
    // AIGroup.pas:1889
    public AIGroupTargetFinder(ECustomNode owner) : base(owner)
    {
        FSelector = new AIMoveTargetSelector(null);
    }

    public AIGroupControl FControl;
    public AIMoveTargetSelector FSelector;
    public Set<AIGroupTargetFinderModeElement> FMode;
    public int FRange;
    public int FTargetRange;
    public int FStep;
    public Set<DamageType> FDamageTypes;

    // AIGroup.pas:1895
    public override void Dispose()
    {
        FSelector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1958
    public override bool GetBusy() => FSelector.FStatus == AIMoveTargetSelector.Status.Searching;

    // AIGroup.pas:1948
    public override bool Activate(AIGroupControl Control)
    {
        base.Activate(Control);
        FControl = Control;
        if (FMode.Contains(AIGroupTargetFinderModeElement.CalculateMovepathBFS))
        {
            FSelector.FMode = FSelector.FMode + Set<AIMoveTargetSelectorModeElement>.Of(AIMoveTargetSelectorModeElement.CreateMovepaths);
        }

        FSelector.Search(Control);
        return true;
    }

    // AIGroup.pas:1963
    public override bool Process()
    {
        bool Result;
        if (FSelector.FStatus == AIMoveTargetSelector.Status.Searching)
        {
            Result = FSelector.Process();
            if (!Result)
            {
                FControl.FTargets.SortOnPriority();
                Done();
            }
        }
        else
        {
            Result = false;
            Done();
        }

        return Result;
    }

    // AIGroup.pas:1919
    public virtual void Reset()
    {
        FSelector.Reset();
        this.FSelector.FOnTargetFound = TargetFoundEvent;
        this.FSelector.FMovePointsMax = 200;
        this.FSelector.FMovePointsMin = 150;

        FDamageTypes = AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall);
        FRange = 9;
        FTargetRange = 9;
        FStep = 4;
        FMode = Set<AIGroupTargetFinderModeElement>.Of();
    }

    // AIGroup.pas:1935
    public virtual bool Setup(AIGroupControl Control)
    {
        Reset();
        FSelector.FSourceLocation = Control.FLocation;
        FSelector.FMovePointTable = Control.FMovePointTable;
        FSelector.FTargetInfo.Player = Control.FGroup.FPlayer;
        FSelector.FTargetInfo.IndependentRelation = Control.FGroup.FIndependentRelation;
        FSelector.FTargetInfo.Mode = AITargetMode.Attack;
        FSelector.FUnits.AddUnits(Control.FGroup);
        FMode = FMode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovepathBFS);
        return true;
    }

    // AIGroup.pas:1901
    public void TargetFoundEvent(MoveTarget Target, ref bool SearchDone)
    {
        AIGroupControlTarget NewTarget;
        NewTarget = new AIGroupControlTarget(null);
        NewTarget.FInfo = FSelector.FTargetInfo;
        NewTarget.FData = Target.FData;
        NewTarget.FLocation = Target.FLocation;
        NewTarget.FMovePoints = Target.FMovePoints;
        if (FMode.Contains(AIGroupTargetFinderModeElement.CalculateMovepathBFS))
        {
            NewTarget.FPathStrength = AbilityRules.GetMovepathBattleFieldStrength(FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, Target.FMovepath, FTargetRange, FRange, FStep);
        }

        if (Target.FTransporterID != -1)
        {
            NewTarget.FTransporterID = Target.FTransporterID;
        }

        FControl.TargetFoundEvent(this, NewTarget, ref SearchDone);
        NewTarget.Release();
    }

    // intf_P5_07.inc:301

}
