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

    private AIGroupControl _control;

    public AIMoveTargetSelector Selector { get; }

    private Set<ModeElement> _mode;
    private int _range;
    private int _step;
    private int _targetRange;

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
        _control = control;
        if (_mode.Contains(ModeElement.CalculateMovePathBfs))
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
        _mode = _mode + Set<ModeElement>.Of(ModeElement.CalculateMovePathBfs);
        return true;
    }

    // AIGroup.pas:2320
    public void ProductionLocationFoundEvent(MoveTarget target, ref bool searchDone)
    {
        AIGroupProductionLocation location;
        ProductionControl control;
        control = StrategicMap.Current.MapContainer.GetField(target.Location.X, target.Location.Y, target.Location.MapLevel).GetProductionControl();
        if (_control.ProductionLocations.IndexOfPC(control) == -1)
        {
            location = new AIGroupProductionLocation(null);
            location.ProductionControl = control;
            location.Location = target.Location;
            location.MovePoints = target.MovePoints;
            location.TransporterId = target.TransporterId;
            if (_mode.Contains(ModeElement.CalculateMovePathBfs))
            {
                location.PathStrength = AbilityRules.GetMovePathBattleFieldStrength(_control.Group.Player, _control.Group.IndependentRelation, target.MovePath, _targetRange, _range, _step);
            }

            _control.ProductionLocationFoundEvent(location, ref searchDone);
            location.Release();
        }
    }

    // AIGroup.pas:2342
    public void Reset()
    {
        Selector.Clear();
        Selector.SetTargetFoundHandler(ProductionLocationFoundEvent);
        _mode = Set<ModeElement>.Of();
        _range = 8;
        _step = 1;
        _targetRange = 5;
    }

    public enum ModeElement : byte
    {
        CalculateMovePathBfs = 0
    }

    // intf_P5_07.inc:459

}
