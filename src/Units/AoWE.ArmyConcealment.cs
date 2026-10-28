namespace Aow.Units.AoWE;

public partial class TArmy
{
    public virtual bool GetUnitConcealedForPlayer(int index, sbyte player)
    {
        if (player == FPlayer || player == -1 || FX == -1) return false;
        var field = (TAoWMapField)AoWE.AoWHSMap.MapContainer.GetField(FX, FY, FL);
        return GetUnit(index).ConcealedOnMapF(field) && GetConcealedForPlayer(player);
    }
}
