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
public class UnitEnchantment : Enchantment
{
    public new const int ClassId = 135_951;

    // AoWAb.PAS:785
    public UnitEnchantment(CustomNode owner) : base(null)
        // { Original ignores the supplied owner. }

        => Source = -1;

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
        if (StrategicMap.Current is not null)
        {
            Troop source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
            return source is not null
                ? GamePack.Current.AbilityControl[EnchantmentAbilityId].GetName(source)
                : GamePack.Current.AbilityControl[EnchantmentAbilityId].GetName(null);
        }
        else
        {
            return GamePack.Current.AbilityControl[EnchantmentAbilityId].GetName(null);
        }
    }

    // AoWAb.PAS:831
    public override string GetBinding()
    {
        Troop source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        return source is not null ? source.GetName() : "";
    }

    // AoWAb.PAS:820
    public override int AIUpkeepPriority
    {
        get
        {
            Troop source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
            return source is not null
                ? GamePack.Current.AbilityControl.GetAbility<UnitEnchantmentAbility>(EnchantmentAbilityId).GetAIUpkeepPriority(source)
                : 0;
        }
    }

    // AoWAb.PAS:859
    public override MapCoordinate GetLocation()
    {
        Troop source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        return source is not null ? source.GetLocation() : MapCoordinate.None;
    }

    // AoWAb.PAS:874
    public override bool Dispel()
    {
        Troop source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        return source is not null && source.RemoveAbility(EnchantmentAbilityId);
    }

    // AoWAb.PAS:842
    public override void Select()
    {
        MapCoordinate coordinate28 = GetLocation();
        sbyte hexX = coordinate28.X;
        sbyte hexY = coordinate28.Y;
        sbyte hexLevel = coordinate28.MapLevel;
        if (hexX != -1)
        {
            if (StrategicMap.Current.Visible(hexX, hexY, L: hexLevel, radius: 0))
            {
                ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel).GetArmyHexSprite();
                if (armyHexSprite is not null)
                {
                    StrategicMap.Current.Select(armyHexSprite, center: true);
                }
            }
            else
            {
                string binding = GetBinding();
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsOutOfSightFormat), arguments: [binding]);
                StrategicMap.Current.ShowMessage(format);
            }
        }
    }
}
