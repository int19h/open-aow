#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// control, selector, mode, ranges, step, damage types
public class AIGroupTargetFinder : AIGroupTask
{
    // AIGroup.pas:1889
    public AIGroupTargetFinder(CustomNode owner) : base(owner) => Selector = new AIMoveTargetSelector(null) { Holder = CreationHolder() };

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
    public override bool Busy => Selector.Status == AIMoveTargetSelectorStatus.Searching;

    // AIGroup.pas:1948
    public override bool Activate(AIGroupControl control)
    {
        _ = base.Activate(control);
        _control = control;
        if (Mode.Contains(AIGroupTargetFinderModeElement.CalculateMovePathBfs))
        {
            Selector.Mode = Selector.Mode + Set<AIMoveTargetSelectorModeElement>.Of(AIMoveTargetSelectorModeElement.CreateMovePaths);
        }

        _ = Selector.Search(control);
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
        Selector.SetTargetFoundHandler(TargetFoundEvent);
        Selector.MovePointsMax = 200;
        Selector.MovePointsMin = 150;
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
        AITargetInfo targetInfo = Selector.TargetInfo;
        targetInfo.Player = control.Group.Player;
        Selector.TargetInfo = targetInfo;
        AITargetInfo targetInfo2 = Selector.TargetInfo;
        targetInfo2.IndependentRelation = control.Group.IndependentRelation;
        Selector.TargetInfo = targetInfo2;
        AITargetInfo targetInfo3 = Selector.TargetInfo;
        targetInfo3.Mode = AITargetMode.Attack;
        Selector.TargetInfo = targetInfo3;
        _ = Selector.Units.AddUnits(control.Group.Units);
        Mode += Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
        return true;
    }

    // AIGroup.pas:1901
    public void TargetFoundEvent(MoveTarget target, ref bool searchDone)
    {
        AIGroupControlTarget newTarget = new(null)
        {
            Holder = CreationHolder(),
            Info = Selector.TargetInfo,
            Data = target.Data,
            Location = target.Location,
            MovePoints = target.MovePoints
        };
        if (Mode.Contains(AIGroupTargetFinderModeElement.CalculateMovePathBfs))
        {
            newTarget.PathStrength = AbilityRules.GetMovePathBattleFieldStrength(_control.Group.Player, _control.Group.IndependentRelation, target.MovePath, _targetRange, Range, _step, engine: GetEngine());
        }

        if (target.TransporterId != -1)
        {
            newTarget.TransporterId = target.TransporterId;
        }

        _control.TargetFoundEvent(this, target: newTarget, ref searchDone);
        _ = newTarget.Release();
    }
}
