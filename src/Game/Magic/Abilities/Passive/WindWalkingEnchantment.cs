// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class WindWalkingEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1336
    public WindWalkingEnchantment(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.WindWalkingText);
        AbilityId = 165;
    }

    // PassiveAb.pas:1343
    public override bool Remove(AbilityOwner owner)
    {
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            troop.GetLocation(out x, out y, out l);
            if (x != -1)
            {
                var mapField = StrategicMap.Current.MapContainer.GetField(x, y, l);
                army.ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
            }
        }

        return result;
    }
}
