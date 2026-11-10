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
    public new const int ClassID = 135951;

    // AoWAb.PAS:785
    public UnitEnchantment(ECustomNode owner) : base(null)
    {
        // { Original ignores the supplied owner. }

        FSource = -1;
    }

    public int FUnitID;
    public int FEnchantmentAbilityID;

    // AoWAb.PAS:797
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FEnchantmentAbilityID, 20);
        Stream.rwInteger(ref FUnitID, 21);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("EnchantmentAbilityID", ref FEnchantmentAbilityID);
        persister.Int("UnitID", ref FUnitID);
    }

    // AoWAb.PAS:804
    public override string GetName()
    {
        string Result;
        AbstractUnit Source;
        if (AoWHSMap.Current != null)
        {
            Source = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
            if (Source != null)
            {
                Result = AoWHSSet.Current.FAbilityControl.GetAbility(FEnchantmentAbilityID).GetName(Source);
            }
            else
            {
                Result = AoWHSSet.Current.FAbilityControl.GetAbility(FEnchantmentAbilityID).GetName(null);
            }
        }
        else
        {
            Result = AoWHSSet.Current.FAbilityControl.GetAbility(FEnchantmentAbilityID).GetName(null);
        }

        return Result;
    }

    // AoWAb.PAS:831
    public override string GetBinding()
    {
        string Result;
        AbstractUnit Source;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (Source != null)
        {
            Result = Source.GetName();
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    // AoWAb.PAS:820
    public override int GetAIUpkeepPriority()
    {
        int Result;
        AbstractUnit Source;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (Source != null)
        {
            Result = ((UnitEnchantmentAbility)AoWHSSet.Current.FAbilityControl.GetAbility(FEnchantmentAbilityID)).GetAIUpkeepPriority(Source);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:859
    public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        AbstractUnit Source;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (Source != null)
        {
            Source.GetLocation(ref X, ref Y, ref L);
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
        bool Result;
        AbstractUnit Source;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID);
        if (Source != null)
        {
            Result = Source.RemoveAbility(FEnchantmentAbilityID);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:842
    public override void Select()
    {
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        ArmyHS ArmyHS;
        GetLocation(ref Xhx, ref Yhx, ref Lhx);
        if (Xhx != -1)
        {
            if (AoWHSMap.Current.Visible(Xhx, Yhx, Lhx, 0))
            {
                ArmyHS = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(Xhx, Yhx, Lhx)).GetArmyHS();
                if (ArmyHS != null)
                {
                    AoWHSMap.Current.Select(ArmyHS, true);
                }
            }
            else
            {
                var binding = GetBinding();
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XIsOutOfSightRStr), [binding]);
                AoWHSMap.Current.ShowMessage(format);
            }
        }
    }

    // intf_P4_02.inc:770

}
