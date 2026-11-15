// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class FireAuraEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1294
    public FireAuraEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.FireAuraText);
        AbilityId = 163;
    }

    // PassiveAb.pas:1301
    public override bool Remove(AbilityOwner owner)
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            MapCoordinate coordinate87 = troop.GetLocation();
            x = coordinate87.X;
            y = coordinate87.Y;
            l = coordinate87.MapLevel;
            if (x != -1)
            {
                MapField mapField = StrategicMap.Current.MapContainer.GetField(x, y, l);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return result;
    }
}
