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
        Selector = new AIMoveTargetSelector(null);
    }

    public AIGroupControl Control;
    public AIMoveTargetSelector Selector;
    public int Range;
    public int Step;
    public int TargetRange;
    public Set<ModeElement> Mode;

    // AIGroup.pas:1987
    public override void Dispose()
    {
        Selector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:1993
    public override bool GetBusy() => Selector.Status == AIMoveTargetSelectorStatus.Searching;

    // AIGroup.pas:2051
    public override bool Activate(AIGroupControl control)
    {
        base.Activate(control);
        Control = control;
        if (Mode.Contains(ModeElement.CalculateMovePathBfs))
        {
            Selector.Mode = Selector.Mode + Set<AIMoveTargetSelectorModeElement>.Of(AIMoveTargetSelectorModeElement.CreateMovePaths);
        }

        Selector.Search(control);
        return true;
    }

    // AIGroup.pas:2061
    public override bool Process()
    {
        bool result;
        if (Selector.Status == AIMoveTargetSelectorStatus.Searching)
        {
            result = Selector.Process();
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
        Selector.SourceLocation = control.Location;
        Selector.MovePointTable = control.MovePointTable;
        Selector.TargetInfo.Player = control.Group.Player;
        Selector.TargetInfo.IndependentRelation = control.Group.IndependentRelation;
        Selector.TargetInfo.Mode = AITargetMode.Recruit;
        Selector.MovePointsMax = 75;
        Mode = Mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
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
                if (troop.Group != null && troop.Group.Leader == troop && troop.Group != Control.Group)
                {
                    group = new AIRecruitmentGroup(null);
                    group.SetGroup(troop.Group);
                    group.Location = target.Location;
                    group.MovePoints = target.MovePoints;
                    group.TransporterId = target.TransporterId;
                    if (Mode.Contains(ModeElement.CalculateMovePathBfs))
                    {
                        group.PathStrength = AbilityRules.GetMovePathBattleFieldStrength(Control.Group.Player, Control.Group.IndependentRelation, target.MovePath, TargetRange, Range, Step);
                    }

                    Control.RecruitmentGroupFoundEvent(group, ref searchDone);
                    group.Release();
                }
            }
        }
    }

    // AIGroup.pas:2028
    public void Reset()
    {
        Mode = Set<ModeElement>.Of();
        Selector.Reset();
        Selector.OnTargetFound = RecruitGroupFoundEvent;
        Range = 8;
        Step = 1;
        TargetRange = 5;
    }

    public enum ModeElement : byte
    {
        CalculateMovePathBfs = 0
    }

    // intf_P5_07.inc:356

}
