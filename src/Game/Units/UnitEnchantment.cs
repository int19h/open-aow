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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref EnchantmentAbilityId, 20);
        stream.rwInteger(ref UnitId, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("EnchantmentAbilityID", ref EnchantmentAbilityId);
        persister.Int32("UnitID", ref UnitId);
    }

    // AoWAb.PAS:804
    public override string GetName()
    {
        string result;
        Troop source;
        if (AoWHSMap.Current != null)
        {
            source = AoWHSMap.Current.UnitControl.FindUnit(UnitId);
            if (source != null)
            {
                result = AoWHSSet.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(source);
            }
            else
            {
                result = AoWHSSet.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(null);
            }
        }
        else
        {
            result = AoWHSSet.Current.AbilityControl.GetAbility(EnchantmentAbilityId).GetName(null);
        }

        return result;
    }

    // AoWAb.PAS:831
    public override string GetBinding()
    {
        string result;
        Troop source;
        source = AoWHSMap.Current.UnitControl.FindUnit(UnitId);
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
        source = AoWHSMap.Current.UnitControl.FindUnit(UnitId);
        if (source != null)
        {
            result = ((UnitEnchantmentAbility)AoWHSSet.Current.AbilityControl.GetAbility(EnchantmentAbilityId)).GetAIUpkeepPriority(source);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:859
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        Troop source;
        source = AoWHSMap.Current.UnitControl.FindUnit(UnitId);
        if (source != null)
        {
            source.GetLocation(ref X, ref Y, ref L);
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
        source = AoWHSMap.Current.UnitControl.FindUnit(UnitId);
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
        GetLocation(ref hexX, ref hexY, ref hexLevel);
        if (hexX != -1)
        {
            if (AoWHSMap.Current.Visible(hexX, hexY, hexLevel, 0))
            {
                armyHexSprite = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(hexX, hexY, hexLevel)).GetArmyHexSprite();
                if (armyHexSprite != null)
                {
                    AoWHSMap.Current.Select(armyHexSprite, true);
                }
            }
            else
            {
                var binding = GetBinding();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_isOutOfSightFormat), [binding]);
                AoWHSMap.Current.ShowMessage(format);
            }
        }
    }

    // intf_P4_02.inc:770

}
