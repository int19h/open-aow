// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// control, selector, mode, ranges, step, damage types
public partial class AIGroupTargetFinder : AIGroupTask
{
    // AIGroup.pas:1889
    public AIGroupTargetFinder(ECustomNode owner) : base(owner)
    {
        Selector = new AIMoveTargetSelector(null);
    }

    private AIGroupControl _control;

    public AIMoveTargetSelector Selector { get; }
    public Set<AIGroupTargetFinderModeElement> Mode { get; set; }
    public int Range { get; set; }

    private int _targetRange;
    private int _step;

    // AIGroup.pas:1895
    public override void Dispose()
    {
        Selector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1958
    public override bool GetBusy() => Selector.Status == AIMoveTargetSelectorStatus.Searching;

    // AIGroup.pas:1948
    public override bool Activate(AIGroupControl control)
    {
        base.Activate(control);
        _control = control;
        if (Mode.Contains(AIGroupTargetFinderModeElement.CalculateMovePathBfs))
        {
            Selector.Mode = Selector.Mode + Set<AIMoveTargetSelectorModeElement>.Of(AIMoveTargetSelectorModeElement.CreateMovePaths);
        }

        Selector.Search(control);
        return true;
    }

    // AIGroup.pas:1963
    public override bool Process()
    {
        bool result;
        if (Selector.Status == AIMoveTargetSelectorStatus.Searching)
        {
            result = Selector.Process();
            if (!result)
            {
                _control.Targets.SortOnPriority();
                Done();
            }
        }
        else
        {
            result = false;
            Done();
        }

        return result;
    }

    // AIGroup.pas:1919
    public virtual void Reset()
    {
        Selector.Reset();
        this.Selector.OnTargetFound = TargetFoundEvent;
        this.Selector.MovePointsMax = 200;
        this.Selector.MovePointsMin = 150;
        Range = 9;
        _targetRange = 9;
        _step = 4;
        Mode = Set<AIGroupTargetFinderModeElement>.Of();
    }

    // AIGroup.pas:1935
    public virtual bool Setup(AIGroupControl control)
    {
        Reset();
        Selector.SourceLocation = control.Location;
        Selector.MovePointTable = control.MovePointTable;
        Selector.TargetInfo.Player = control.Group.Player;
        Selector.TargetInfo.IndependentRelation = control.Group.IndependentRelation;
        Selector.TargetInfo.Mode = AITargetMode.Attack;
        Selector.Units.AddUnits(control.Group);
        Mode = Mode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
        return true;
    }

    // AIGroup.pas:1901
    public void TargetFoundEvent(MoveTarget target, ref bool searchDone)
    {
        AIGroupControlTarget newTarget;
        newTarget = new AIGroupControlTarget(null);
        newTarget.Info = Selector.TargetInfo;
        newTarget.Data = target.Data;
        newTarget.Location = target.Location;
        newTarget.MovePoints = target.MovePoints;
        if (Mode.Contains(AIGroupTargetFinderModeElement.CalculateMovePathBfs))
        {
            newTarget.PathStrength = AbilityRules.GetMovePathBattleFieldStrength(_control.Group.Player, _control.Group.IndependentRelation, target.MovePath, _targetRange, Range, _step);
        }

        if (target.TransporterId != -1)
        {
            newTarget.TransporterId = target.TransporterId;
        }

        _control.TargetFoundEvent(this, newTarget, ref searchDone);
        newTarget.Release();
    }

    // intf_P5_07.inc:301

}
