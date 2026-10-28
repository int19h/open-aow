using Aow.Rtl;

namespace Aow.Units.AoWE;

public partial class TMoveControl
{
    public void SetMapL(HSEngine.TMapLevel level)
    {
        if (level == FMapLevel) return;
        FMapLevel = level;
        for (int parity = 0; parity < 2; parity++)
            for (int direction = 0; direction < 8; direction++)
                FFieldOffsets[parity][direction] = FMapLevel.FHNOffsets[parity][direction];
    }

    public bool Activate(TMoveControlSettings settings)
    {
        Rtl.Rtl.Assert(!FStatus.Contains(TMoveControlStatusElement.mcsActivated), "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7ea8);
        Rtl.Rtl.Assert(settings.FSourceX != -1 && settings.FSourceY != -1 && settings.FSourceL != -1, "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7ea9);
        if (settings.FStatus == TMoveControlSettingsStatus.mcssFailed) return false;
        FStatus += TSet<TMoveControlStatusElement>.Of(TMoveControlStatusElement.mcsActivated);
        FSettings = settings;
        FSettings.AddRef();
        if (FSettings.FStatus == TMoveControlSettingsStatus.mcssIdle)
        {
            var container = FMap.MapContainer;
            FSettings.SetupMovePointData(container.Width, container.Height, container.LevelCount);
            FMovePointsLimit = FSettings.FMovePointsMax;
        }
        else if (FSettings.FStatus is TMoveControlSettingsStatus.mcssProcessing or TMoveControlSettingsStatus.mcssProcessed)
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
        FArmyHS = this is Aow.Units.AoWTC.TCombatMoveControl ? null : (TArmyHS)FSettings.FArmyHS;
        FSelection = FSettings.FSelection;
        FTransporterArmy = FSettings.FTransporterArmy;
        FProcessedMapFields = FSettings.FProcessedMapFields;
        SetMapL(FMap.MapContainer.GetMapLevel(FSourceL));
        if (FMode.Contains(TMoveModeElement.mmExploredTerrainOnly) && FPlayer != -1)
        {
            FOnMovePoint = ExploredOnlyMoveOnMovePointProc;
            FExploredMask = 1 << (FPlayer - 1);
        }
        else FOnMovePoint = DefaultMoveOnMovePointProc;
        return true;
    }
}
