#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public class WindWalkingEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1336
    public WindWalkingEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WindWalkingText);
        AbilityId = 165;
    }

    // PassiveAb.pas:1343
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            MapCoordinate coordinate90 = troop.GetLocation();
            int x = coordinate90.X;
            int y = coordinate90.Y;
            int l = coordinate90.MapLevel;
            if (x != -1)
            {
                MapField mapField = StrategicMap.Current.MapContainer.GetField(x, y, l);
                army.ValidateTerrain(terrainType: mapField.TerrainNumber, overlayType: mapField.OverlayType.ToOriginal());
            }
        }

        return result;
    }
}
