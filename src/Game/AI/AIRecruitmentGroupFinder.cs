// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI;

// control, selector, range, step, target range, mode
public partial class AIRecruitmentGroupFinder : AIGroupTask
{
    // AIGroup.pas:1981
    public AIRecruitmentGroupFinder(ECustomNode owner) : base(owner)
    {
        _selector = new AIMoveTargetSelector(null);
    }

    private AIGroupControl _control;
    private AIMoveTargetSelector _selector;
    private int _range;
    private int _step;
    private int _targetRange;
    private Set<ModeElement> _mode;

    // AIGroup.pas:1987
    public override void Dispose()
    {
        _selector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1993
    public override bool GetBusy() => _selector.Status == AIMoveTargetSelectorStatus.Searching;

    // AIGroup.pas:2051
    public override bool Activate(AIGroupControl control)
    {
        base.Activate(control);
        _control = control;
        if (_mode.Contains(ModeElement.CalculateMovePathBfs))
        {
            _selector.Mode = _selector.Mode + Set<AIMoveTargetSelectorModeElement>.Of(AIMoveTargetSelectorModeElement.CreateMovePaths);
        }

        _selector.Search(control);
        return true;
    }

    // AIGroup.pas:2061
    public override bool Process()
    {
        bool result;
        if (_selector.Status == AIMoveTargetSelectorStatus.Searching)
        {
            result = _selector.Process();
        }
        else
        {
            result = false;
        }

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
        _selector.TargetInfo.Player = control.Group.Player;
        _selector.TargetInfo.IndependentRelation = control.Group.IndependentRelation;
        _selector.TargetInfo.Mode = AITargetMode.Recruit;
        _selector.MovePointsMax = 75;
        _mode = _mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
        return true;
    }

    // AIGroup.pas:1998
    public void RecruitGroupFoundEvent(MoveTarget target, ref bool searchDone)
    {
        ArmyHexSprite armyHexSprite;
        Troop troop;
        AIRecruitmentGroup group;
        armyHexSprite = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(target.Location.X, target.Location.Y, target.Location.MapLevel)).GetArmyHexSprite();
        if (armyHexSprite != null)
        {
            for (int I = armyHexSprite.Army.Count - 1; I >= 0; I--)
            {
                troop = armyHexSprite.Army.GetUnit(I);
                if (troop.Group != null && troop.Group.Leader == troop && troop.Group != _control.Group)
                {
                    group = new AIRecruitmentGroup(null);
                    group.SetGroup(troop.Group);
                    group.Location = target.Location;
                    group.MovePoints = target.MovePoints;
                    group.TransporterId = target.TransporterId;
                    if (_mode.Contains(ModeElement.CalculateMovePathBfs))
                    {
                        group.PathStrength = AbilityRules.GetMovePathBattleFieldStrength(_control.Group.Player, _control.Group.IndependentRelation, target.MovePath, _targetRange, _range, _step);
                    }

                    _control.RecruitmentGroupFoundEvent(group, ref searchDone);
                    group.Release();
                }
            }
        }
    }

    // AIGroup.pas:2028
    public void Reset()
    {
        _mode = Set<ModeElement>.Of();
        _selector.Reset();
        _selector.OnTargetFound = RecruitGroupFoundEvent;
        _range = 8;
        _step = 1;
        _targetRange = 5;
    }

    public enum ModeElement : byte
    {
        CalculateMovePathBfs = 0
    }

    // intf_P5_07.inc:356

}
