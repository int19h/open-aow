// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public UnitEnchantment(ECustomNode owner) : base(null)
    {
        // { Original ignores the supplied owner. }

        Source = -1;
    }

    public int UnitId;
    public int EnchantmentAbilityId;

    // AoWAb.PAS:797
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref EnchantmentAbilityId, 20);
        stream.Int32(ref UnitId, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentAbilityId = persister.Int32("EnchantmentAbilityID", EnchantmentAbilityId);
        UnitId = persister.Int32("UnitID", UnitId);
    }

    // AoWAb.PAS:804
    public override string GetName()
    {
        string result;
        Troop source;
        if (StrategicMap.Current != null)
        {
            source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
            if (source != null)
            {
                result = GamePack.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(source);
            }
            else
            {
                result = GamePack.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(null);
            }
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
        if (source != null)
        {
            result = source.GetName();
        }
        else
        {
            result = "";
        }

        return result;
    }

    // AoWAb.PAS:820
    public override int GetAIUpkeepPriority()
    {
        int result;
        Troop source;
        source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (source != null)
        {
            result = ((UnitEnchantmentAbility)GamePack.Current.AbilityControl.GetAbility(EnchantmentAbilityId)).GetAIUpkeepPriority(source);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:859
    public override void GetLocation(out sbyte X, out sbyte Y, out sbyte L)
    {
        Troop source;
        source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (source != null)
        {
            source.GetLocation(out X, out Y, out L);
        }
        else
        {
            X = -1;
            Y = -1;
            L = -1;
        }
    }

    // AoWAb.PAS:874
    public override bool Dispel()
    {
        bool result;
        Troop source;
        source = StrategicMap.Current.UnitControl.FindUnit(UnitId);
        if (source != null)
        {
            result = source.RemoveAbility(EnchantmentAbilityId);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:842
    public override void Select()
    {
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        ArmyHexSprite armyHexSprite;
        GetLocation(out hexX, out hexY, out hexLevel);
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
                var binding = GetBinding();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.IsOutOfSightFormat), [binding]);
                StrategicMap.Current.ShowMessage(format);
            }
        }
    }

    // intf_P4_02.inc:770

}
