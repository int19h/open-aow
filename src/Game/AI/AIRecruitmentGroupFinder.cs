#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.AI;

// control, selector, range, step, target range, mode
public class AIRecruitmentGroupFinder : AIGroupTask
{
    // AIGroup.pas:1981
    public AIRecruitmentGroupFinder(CustomNode owner) : base(owner) => _selector = new AIMoveTargetSelector(null) { Holder = CreationHolder() };

    private AIGroupControl _control;
    private readonly AIMoveTargetSelector _selector;
    private int _range;
    private int _step;
    private int _targetRange;
    private Modes _mode;

    // AIGroup.pas:1987
    public override void Dispose()
    {
        _selector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1993
    public override bool Busy => _selector.Status == AIMoveTargetSelectorStatus.Searching;

    // AIGroup.pas:2051
    public override bool Activate(AIGroupControl control)
    {
        _ = base.Activate(control);
        _control = control;
        if (_mode.Contains(Modes.CalculateMovePathBfs))
        {
            _selector.Mode |= AIMoveTargetSelectorModes.CreateMovePaths;
        }

        _ = _selector.Search(control);
        return true;
    }

    // AIGroup.pas:2061
    public override bool Process()
    {
        bool result = _selector.Status == AIMoveTargetSelectorStatus.Searching && _selector.Process();
        if (!result)
        {
            Done();
        }

        return result;
    }

    // AIGroup.pas:2038
    public virtual bool Setup(AIGroupControl control)
    {
        Reset();
        _selector.SourceLocation = control.Location;
        _selector.MovePointTable = control.MovePointTable;
        AITargetInfo targetInfo = _selector.TargetInfo;
        targetInfo.Player = control.Group.Player;
        _selector.TargetInfo = targetInfo;
        AITargetInfo targetInfo2 = _selector.TargetInfo;
        targetInfo2.IndependentRelation = control.Group.IndependentRelation;
        _selector.TargetInfo = targetInfo2;
        AITargetInfo targetInfo3 = _selector.TargetInfo;
        targetInfo3.Mode = AITargetMode.Recruit;
        _selector.TargetInfo = targetInfo3;
        _selector.MovePointsMax = 75;
        _mode |= Modes.CalculateMovePathBfs;
        return true;
    }

    // AIGroup.pas:1998
    public void RecruitGroupFoundEvent(MoveTarget target, ref bool searchDone)
    {
        ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(target.Location.X, target.Location.Y, target.Location.MapLevel).GetArmyHexSprite();
        if (armyHexSprite is not null)
        {
            for (int i = armyHexSprite.Army.Units.Count - 1; i >= 0; i--)
            {
                Troop troop = armyHexSprite.Army.Units.GetUnit(i);
                if (troop.Group is not null && troop.Group.Leader == troop && troop.Group != _control.Group)
                {
                    AIRecruitmentGroup group = new(null) { Holder = CreationHolder() };
                    group.SetGroup(troop.Group);
                    group.Location = target.Location;
                    group.MovePoints = target.MovePoints;
                    group.TransporterId = target.TransporterId;
                    if (_mode.Contains(Modes.CalculateMovePathBfs))
                    {
                        group.PathStrength = AbilityRules.GetMovePathBattleFieldStrength(_control.Group.Player, _control.Group.IndependentRelation, target.MovePath, _targetRange, _range, _step, engine: GetEngine());
                    }

                    _control.RecruitmentGroupFoundEvent(group, searchDone);
                    _ = group.Release();
                }
            }
        }
    }

    // AIGroup.pas:2028
    public void Reset()
    {
        _mode = Modes.None;
        _selector.Reset();
        _selector.SetTargetFoundHandler(RecruitGroupFoundEvent);
        _range = 8;
        _step = 1;
        _targetRange = 5;
    }

    [Flags]
    public enum Modes : byte
    {
        None = 0,
        CalculateMovePathBfs = 1 << 0,
    }
}
