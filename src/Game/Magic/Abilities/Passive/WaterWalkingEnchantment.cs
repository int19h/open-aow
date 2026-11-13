// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class WaterWalkingEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1315
    public WaterWalkingEnchantment(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.WaterWalkingText);
        AbilityId = 164;
    }

    // PassiveAb.pas:1322
    public override bool Remove(AbilityOwner owner)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            troop.GetLocation(out X, out Y, out L);
            if (X != -1)
            {
                var mapField = StrategicMap.Current.MapContainer.GetField(X, Y, L);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return result;
    }
}
