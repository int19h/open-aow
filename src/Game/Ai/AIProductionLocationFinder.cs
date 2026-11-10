// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Ai.Groups;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.Ai;

// control, selector, mode, range, step, target range
public partial class AIProductionLocationFinder : AIGroupControlPlugin
{
    // AIGroup.pas:2303
    public AIProductionLocationFinder(ECustomNode owner) : base(owner)
    {
        FSelector = new AIMoveTargetSelector(null);
    }

    public AIGroupControl FControl;
    public AIMoveTargetSelector FSelector;
    public Set<ModeElement> FMode;
    public int FRange;
    public int FStep;
    public int FTargetRange;

    // AIGroup.pas:2309
    public override void Dispose()
    {
        FSelector?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:2315
    public override bool GetBusy() => FSelector.FStatus == AIMoveTargetSelector.Status.Searching;

    // AIGroup.pas:2365
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

    // AIGroup.pas:2375
    public override bool Process()
    {
        bool Result;
        if (FSelector.FStatus == AIMoveTargetSelector.Status.Searching)
        {
            Result = FSelector.Process();
            if (!Result)
            {
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

    // AIGroup.pas:2352
    public virtual bool Setup(AIGroupControl Control)
    {
        Reset();
        FSelector.FSourceLocation = Control.FLocation;
        FSelector.FMovePointTable = Control.FMovePointTable;
        FSelector.FTargetInfo.Player = Control.FGroup.FPlayer;
        FSelector.FTargetInfo.IndependentRelation = Control.FGroup.FIndependentRelation;
        FSelector.FTargetInfo.Mode = AITargetMode.Produce;
        FSelector.FMovePointsMax = 75;
        FMode = FMode + Set<ModeElement>.Of(ModeElement.CalculateMovepathBFS);
        return true;
    }

    // AIGroup.pas:2320
    public void ProductionLocationFoundEvent(MoveTarget Target, ref bool SearchDone)
    {
        AIGroupProductionLocation Location;
        ProductionControl Control;
        Control = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(Target.FLocation.X, Target.FLocation.Y, Target.FLocation.L)).GetProductionControl();
        if (FControl.FProductionLocations.IndexOfPC(Control) == -1)
        {
            Location = new AIGroupProductionLocation(null);
            Location.FProductionControl = Control;
            Location.FLocation = Target.FLocation;
            Location.FMovePoints = Target.FMovePoints;
            Location.FTransporterID = Target.FTransporterID;
            if (FMode.Contains(ModeElement.CalculateMovepathBFS))
            {
                Location.FPathStrength = AbilityRules.GetMovepathBattleFieldStrength(FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, Target.FMovepath, FTargetRange, FRange, FStep);
            }

            FControl.ProductionLocationFoundEvent(Location, ref SearchDone);
            Location.Release();
        }
    }

    // AIGroup.pas:2342
    public void Reset()
    {
        FSelector.Clear();
        FSelector.FOnTargetFound = ProductionLocationFoundEvent;
        FMode = Set<ModeElement>.Of();
        FRange = 8;
        FStep = 1;
        FTargetRange = 5;
    }

    public enum ModeElement : byte
    {
        CalculateMovepathBFS = 0
    }

    // intf_P5_07.inc:459

}
