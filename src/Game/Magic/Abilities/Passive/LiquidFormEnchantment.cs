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
        AbilityName = AbilityRules.TranslateResourceString(GameText.LiquidFormText);
        AbilityId = 166;
    }

    // PassiveAb.pas:1364
    public override bool Remove(AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            troop.GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                var mapField = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return result;
    }

    // PassiveAb.pas:1378
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result = troop.GetUnitLevel() + 300;
        if (troop is Leader)
        {
            result += 50;
        }

        return result;
    }
}
