namespace Aow.Units.AoWE;

public partial class TSelectedArmy
{
    public override void UpdateMoveCursor()
    {
        var map = AoWE.AoWHSMap;
        var mouse = map.Mouse;
        if (mouse.Xhx == -1 || mouse.Yhx == -1 || mouse.Capture != this) return;
        var field = (TAoWMapField)map.MapContainer.GetField(mouse.Xhx, mouse.Yhx, map.ViewL);
        bool visible = !map.FFogEnabled || ((field.FFieldFlags >> 16) & 255) != 0;
        if (FCurrentMove != -1 && (!map.FExplorationEnabled || field.GetExplored(map.FSeatedPlayer)) && visible)
            mouse.CursorMode = field.GetMoveAction(GetArmyHS().FArmy.FPlayer) switch
            {
                TMoveAction.maAttack => 258,
                TMoveAction.maNegotiate => 259,
                _ => 256
            };
        else mouse.CursorMode = FMoveSettingsList.Count <= 1 ? 256 : 0;
        if (mouse.CursorMode != 256) return;
        var types = GetMoveTypes();
        if (types.Contains(TMoveType.mtWalking)) mouse.Cursor = 64;
        else if (types.Contains(TMoveType.mtSwimming)) mouse.Cursor = 66;
        else if (types.Contains(TMoveType.mtFlying)) mouse.Cursor = 68;
        int cost = FMoveControlSettings.FMovePointTable[field.TerrainType][field.OverlayType + 1];
        if (cost == 0 || map.FExplorationEnabled && !field.GetExplored(map.FSeatedPlayer)) mouse.Cursor |= 1;
    }
}
