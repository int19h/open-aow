using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class MoveControl
{
    public void SetMapL(MapLevel level)
    {
        if (level == MapLevel) return;
        MapLevel = level;
        for (int parity = 0; parity < 2; parity++)
            for (int direction = 0; direction < 8; direction++)
                FieldOffsets[parity][direction] = MapLevel.HexNumberOffsets[parity][direction];
    }

    public bool Activate(MoveControlSettings settings)
    {
        RtlUnit.Assert(!Status.Contains(StatusElement.Activated), "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7ea8);
        RtlUnit.Assert(settings.SourceX != -1 && settings.SourceY != -1 && settings.SourceL != -1, "Assertion failure", @"D:\AoWDev\AoWE\inc\MoveT.PAS", 0x7ea9);
        if (settings.Status == MoveControlSettingsStatus.Failed) return false;
        Status += Set<StatusElement>.Of(StatusElement.Activated);
        Settings = settings;
        Settings.AddRef();
        if (Settings.Status == MoveControlSettingsStatus.Idle)
        {
            var container = Map.MapContainer;
            Settings.SetupMovePointData(container.Width, container.Height, container.LevelCount);
            MovePointsLimit = Settings.MovePointsMax;
        }
        else if (Settings.Status is MoveControlSettingsStatus.Processing or MoveControlSettingsStatus.Processed)
            MovePointsLimit = Settings.MovePointsLimit;
        PointOffsets[0] = Settings.EvenOffsets;
        PointOffsets[1] = Settings.OddOffsets;
        Player = Settings.Player;
        SourceX = Settings.SourceX;
        SourceY = Settings.SourceY;
        SourceL = Settings.SourceL;
        DestinationX = Settings.DestinationX;
        DestinationY = Settings.DestinationY;
        DestinationL = Settings.DestinationL;
        MovePointTable = Settings.MovePointTable;
        Mode = Settings.Mode;
        // Tactical movement retains its sprite in TCombatMoveControl.FUnitHS.
        // The base army cache is read only by world movement callbacks.
        // A tactical sprite on a world control is absent.
        ArmyHexSprite = this is CombatMoveControl ? null : Settings.ArmyHexSprite is ArmyHexSprite army ? army : null;
        Selection = Settings.Selection;
        TransporterArmy = Settings.TransporterArmy;
        ProcessedMapFields = Settings.ProcessedMapFields;
        SetMapL(Map.MapContainer.GetMapLevel(SourceL));
        if (Mode.Contains(MoveModeElement.ExploredTerrainOnly) && Player != -1)
        {
            OnMovePoint = ExploredOnlyMoveOnMovePointProc;
            ExploredMask = 1 << (Player - 1);
        }
        else OnMovePoint = DefaultMoveOnMovePointProc;
        return true;
    }
}
