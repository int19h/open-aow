// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class WindWalkingEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1336
    public WindWalkingEnchantment(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.s_windWalkingText);
        AbilityId = 165;
    }

    // PassiveAb.pas:1343
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
}
