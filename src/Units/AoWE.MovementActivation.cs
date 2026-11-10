using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class MoveControl
{
    public void SetMapL(MapLevel level)
    {
        if (level == FMapLevel) return;
        FMapLevel = level;
        for (int parity = 0; parity < 2; parity++)
            for (int direction = 0; direction < 8; direction++)
                FFieldOffsets[parity][direction] = FMapLevel.FHNOffsets[parity][direction];
    }

    public bool Activate(MoveControlSettings settings)
    {
        RtlUnit.Assert(!FStatus.Contains(StatusElement.Activated), "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7ea8);
        RtlUnit.Assert(settings.FSourceX != -1 && settings.FSourceY != -1 && settings.FSourceL != -1, "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7ea9);
        if (settings.FStatus == Aow.Game.World.Movement.MoveControlSettings.Status.Failed) return false;
        FStatus += Set<StatusElement>.Of(StatusElement.Activated);
        FSettings = settings;
        FSettings.AddRef();
        if (FSettings.FStatus == Aow.Game.World.Movement.MoveControlSettings.Status.Idle)
        {
            var container = FMap.MapContainer;
            FSettings.SetupMovePointData(container.Width, container.Height, container.LevelCount);
            FMovePointsLimit = FSettings.FMovePointsMax;
        }
        else if (FSettings.FStatus is Aow.Game.World.Movement.MoveControlSettings.Status.Processing or Aow.Game.World.Movement.MoveControlSettings.Status.Processed)
            FMovePointsLimit = FSettings.FMovePointsLimit;
        FPointOffsets[0] = FSettings.FEvenOffsets;
        FPointOffsets[1] = FSettings.FOddOffsets;
        FPlayer = FSettings.FPlayer;
        FSourceX = FSettings.FSourceX;
        FSourceY = FSettings.FSourceY;
        FSourceL = FSettings.FSourceL;
        FDestinationX = FSettings.FDestinationX;
        FDestinationY = FSettings.FDestinationY;
        FDestinationL = FSettings.FDestinationL;
        FMovePointTable = FSettings.FMovePointTable;
        FMode = FSettings.FMode;
        // Tactical movement retains its sprite in TCombatMoveControl.FUnitHS.
        // The base army cache is read only by world movement callbacks.
        FArmyHS = this is CombatMoveControl ? null : (ArmyHS)FSettings.FArmyHS;
        FSelection = FSettings.FSelection;
        FTransporterArmy = FSettings.FTransporterArmy;
        FProcessedMapFields = FSettings.FProcessedMapFields;
        SetMapL(FMap.MapContainer.GetMapLevel(FSourceL));
        if (FMode.Contains(MoveModeElement.ExploredTerrainOnly) && FPlayer != -1)
        {
            FOnMovePoint = ExploredOnlyMoveOnMovePointProc;
            FExploredMask = 1 << (FPlayer - 1);
        }
        else FOnMovePoint = DefaultMoveOnMovePointProc;
        return true;
    }
}
