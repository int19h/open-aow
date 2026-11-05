using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.HSEngine;

namespace Aow.Units.GlobalTargetSpells;

public partial class TFreezeWater
{
    public override bool ValidTargetMapF(TMapField field)
    {
        if (!base.ValidTargetMapF(field)) return false;
        sbyte terrain = field.FTerrainType;
        return (terrain == 0 || terrain == 10) && field.FindNoneTransparentHS(typeof(TStructure)) == null && ((TAoWMapField)field).GetArmyHS() == null;
    }
    public override bool ValidTargetSelectionMapF(TMapField field)
    {
        if (!base.ValidTargetSelectionMapF(field)) return false;
        sbyte terrain = field.FTerrainType;
        if (terrain != 0 && terrain != 10 || field.FindNoneTransparentHS(typeof(TStructure)) != null) return false;
        var army = ((TAoWMapField)field).GetArmyHS();
        return army == null || !army.VisibleForSeatedPlayer();
    }
}
public partial class TWatcherEnchantment
{
    // The unused fourth byte in the original register result has no defined value.
    public override TXYL GetLocation() => new() { X = FWatcher.GetXhx(), Y = FWatcher.GetYhx(), L = FWatcher.GetLhx() };
    public override bool Deactivate(byte player)
    {
        if ((FEnchantmentStatus.Image & 1) == 0) return false;
        bool result = base.Deactivate(player);
        if ((FWatcher.FWatcherState & 1) != 0) FWatcher.Free();
        return result;
    }
}
public partial class TTornadoTE { public override bool Validate(THero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid()); }
public partial class TVortexTE { public override bool Validate(THero source) => base.Validate(source) && (FArmyID == null || FArmyID.Valid()); }
