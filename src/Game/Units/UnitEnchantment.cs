#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// (unit ID, ability ID)
public partial class UnitEnchantment : Enchantment
{
    public new const int ClassId = 135951;

    // AoWAb.PAS:785
    public UnitEnchantment(CustomNode owner) : base(null) =>
        // { Original ignores the supplied owner. }

        Source = -1;

    public int UnitId { get; set; }
    public int EnchantmentAbilityId { get; set; }

    // AoWAb.PAS:797
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentAbilityId = persister.Int32("EnchantmentAbilityId", value: EnchantmentAbilityId);
        UnitId = persister.Int32("UnitId", value: UnitId);
    }

    // AoWAb.PAS:804
    public override string GetName()
    {
        string result;
        Troop source;
        if (StrategicMap.Current != null)
        {
            source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
            result = source != null
                ? GamePack.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(source)
                : GamePack.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(null);
        }
        else
        {
            result = GamePack.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(null);
        }

        return result;
    }

    // AoWAb.PAS:831
    public override string GetBinding()
    {
        string result;
        Troop source;
        source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        result = source != null ? source.GetName() : "";

        return result;
    }

    // AoWAb.PAS:820
    public override int GetAIUpkeepPriority()
    {
        int result;
        Troop source;
        source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        result = source != null
            ? GamePack.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetAIUpkeepPriority(source)
            : 0;

        return result;
    }

    // AoWAb.PAS:859
    public override MapCoordinate GetLocation()
    {
        Troop source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        return source != null ? source.GetLocation() : MapCoordinate.None;
    }

    // AoWAb.PAS:874
    public override bool Dispel()
    {
        bool result;
        Troop source;
        source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        result = source != null && source.RemoveAbility(EnchantmentAbilityId);

        return result;
    }

    // AoWAb.PAS:842
    public override void Select()
    {
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        ArmyHexSprite armyHexSprite;
        MapCoordinate coordinate28 = GetLocation();
        hexX = coordinate28.X;
        hexY = coordinate28.Y;
        hexLevel = coordinate28.MapLevel;
        if (hexX != -1)
        {
            if (StrategicMap.Current.Visible(hexX, hexY, hexLevel, 0))
            {
                armyHexSprite = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel).GetArmyHexSprite();
                if (armyHexSprite != null)
                {
                    StrategicMap.Current.Select(armyHexSprite, true);
                }
            }
            else
            {
                string binding = GetBinding();
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.IsOutOfSightFormat), [binding]);
                StrategicMap.Current.ShowMessage(format);
            }
        }
    }

    // intf_P4_02.inc:770

}
