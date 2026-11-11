// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class LiquidFormEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1357
    public LiquidFormEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.LiquidFormRStr);
        FAbilityID = 166;
    }

    // PassiveAb.pas:1364
    public override bool Remove(AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is Troop unit && owner.Owner is Army army)
        {
            unit.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return Result;
    }

    // PassiveAb.pas:1378
    public override int GetAIUpkeepPriority(AbilityOwner Owner)
    {
        if (Owner is not Troop unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 300;
        if (unit is Leader)
        {
            Result += 50;
        }

        return Result;
    }
}
