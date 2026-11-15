// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class WaterWalkingEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1315
    public WaterWalkingEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.WaterWalkingText);
        AbilityId = 164;
    }

    // PassiveAb.pas:1322
    public override bool Remove(AbilityOwner owner)
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            MapCoordinate coordinate89 = troop.GetLocation();
            x = coordinate89.X;
            y = coordinate89.Y;
            l = coordinate89.MapLevel;
            if (x != -1)
            {
                MapField mapField = StrategicMap.Current.MapContainer.GetField(x, y, l);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return result;
    }
}
