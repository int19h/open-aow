// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.AI;

// control, selector, mode, range, step, target range
public partial class AIProductionLocationFinder : AIGroupTask
{
    // AIGroup.pas:2303
    public AIProductionLocationFinder(ECustomNode owner) : base(owner)
    {
        Selector = new AIMoveTargetSelector(null);
    }

    public AIGroupControl Control;
    public AIMoveTargetSelector Selector;
    public Set<ModeElement> Mode;
    public int Range;
    public int Step;
    public int TargetRange;

    // AIGroup.pas:2309
    public override void Dispose()
    {
        Selector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2315
    public override bool GetBusy() => Selector.Status == AIMoveTargetSelectorStatus.Searching;

    // AIGroup.pas:2365
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

    // AIGroup.pas:2375
    public override bool Process()
    {
        bool result;
        if (Selector.Status == AIMoveTargetSelectorStatus.Searching)
        {
            result = Selector.Process();
            if (!result)
            {
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

    // AIGroup.pas:2352
    public virtual bool Setup(AIGroupControl control)
    {
        Reset();
        Selector.SourceLocation = control.Location;
        Selector.MovePointTable = control.MovePointTable;
        Selector.TargetInfo.Player = control.Group.Player;
        Selector.TargetInfo.IndependentRelation = control.Group.IndependentRelation;
        Selector.TargetInfo.Mode = AITargetMode.Produce;
        Selector.MovePointsMax = 75;
        Mode = Mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
        return true;
    }

    // AIGroup.pas:2320
    public void ProductionLocationFoundEvent(MoveTarget target, ref bool searchDone)
    {
        AIGroupProductionLocation location;
        ProductionControl control;
        control = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(target.Location.X, target.Location.Y, target.Location.MapLevel)).GetProductionControl();
        if (Control.ProductionLocations.IndexOfPC(control) == -1)
        {
            location = new AIGroupProductionLocation(null);
            location.ProductionControl = control;
            location.Location = target.Location;
            location.MovePoints = target.MovePoints;
            location.TransporterId = target.TransporterId;
            if (Mode.Contains(ModeElement.CalculateMovePathBfs))
            {
                location.PathStrength = AbilityRules.GetMovePathBattleFieldStrength(Control.Group.Player, Control.Group.IndependentRelation, target.MovePath, TargetRange, Range, Step);
            }

            Control.ProductionLocationFoundEvent(location, ref searchDone);
            location.Release();
        }
    }

    // AIGroup.pas:2342
    public void Reset()
    {
        Selector.Clear();
        Selector.OnTargetFound = ProductionLocationFoundEvent;
        Mode = Set<ModeElement>.Of();
        Range = 8;
        Step = 1;
        TargetRange = 5;
    }

    public enum ModeElement : byte
    {
        CalculateMovePathBfs = 0
    }

    // intf_P5_07.inc:459

}
