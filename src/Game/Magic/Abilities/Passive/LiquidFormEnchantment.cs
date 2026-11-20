// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public class LiquidFormEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1357
    public LiquidFormEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.LiquidFormText);
        AbilityId = 166;
    }

    // PassiveAb.pas:1364
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (result && owner is Troop troop && owner.Owner is Army army)
        {
            MapCoordinate coordinate88 = troop.GetLocation();
            sbyte x = coordinate88.X;
            sbyte y = coordinate88.Y;
            sbyte l = coordinate88.MapLevel;
            if (x != -1)
            {
                MapField mapField = StrategicMap.Current.MapContainer.GetField(x, y, l);
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

        int result = troop.UnitLevel + 300;
        if (troop is Leader)
        {
            result += 50;
        }

        return result;
    }
}
