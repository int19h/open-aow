#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public class FireAuraEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1294
    public FireAuraEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FireAuraText);
        AbilityId = 163;
    }

    // PassiveAb.pas:1301
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            MapCoordinate coordinate87 = troop.GetLocation();
            int x = coordinate87.X;
            int y = coordinate87.Y;
            int l = coordinate87.MapLevel;
            if (x != -1)
            {
                MapField mapField = StrategicMap.Current.MapContainer.GetField(x, y, l);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return result;
    }
}
