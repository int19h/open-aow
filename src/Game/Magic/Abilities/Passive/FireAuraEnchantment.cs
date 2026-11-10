// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class FireAuraEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1294
    public FireAuraEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.FireAuraRStr);
        FAbilityID = 163;
    }

    // PassiveAb.pas:1301
    public override bool Remove(AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Result = base.Remove(owner);
        if (Result && owner is AbstractUnit unit && owner.Owner is Army army)
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
}
