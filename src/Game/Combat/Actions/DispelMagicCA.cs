// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.Magic;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (dispelled enchantment IDs)
public partial class DispelMagicCA : SingleTargetCA
{

    // AoWAb.PAS:3936
    public DispelMagicCA(ECustomNode owner) : base(owner)
    {
        FEnchantmentIDs = new IntegerList();
    }

    public IntegerList FEnchantmentIDs;

    // AoWAb.PAS:3941
    public override void Dispose()
    {
        FEnchantmentIDs?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:3946
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(50, FEnchantmentIDs.LoadFromStream, FEnchantmentIDs.SaveToStream);
    }

    // AoWAb.PAS:3951
    public override void Execute(Battle Combat)
    {
        EnchantmentList Enchantments;
        CombatObject Source;
        CombatObject Target;
        Enchantment Enchantment;
        bool WasSummoned;
        Source = Combat.FCombatData.FindID(FSourceID);
        Target = Combat.FCombatData.FindID(FTargetID);
        if (Source is CombatUnit && Target is CombatUnit)
        {
            Enchantments = new EnchantmentList(null);
            var combatUnit = (CombatUnit)Target;
            combatUnit.FUnit.ListEnchantments(Enchantments);
            WasSummoned = combatUnit.FUnit.GetAbilityEnabled(155);

            for (int I = FEnchantmentIDs.Count - 1; I >= 0; I--)
            {
                Enchantment = Enchantments.FindID(FEnchantmentIDs[I]);
                if (Enchantment != null)
                {
                    Enchantment.Dispel();
                }
            }

            if (WasSummoned && !((CombatUnit)Target).FUnit.GetAbilityEnabled(155))
            {
                Source.DoDamage(Target, Target.GetHitPoints());
            }

            Enchantments?.Dispose();
        }
    }

    // AoWAb.PAS:3986
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(60).FSFXLibrary.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // AoWAb.PAS:3979
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FTargetID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(60).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:3994
    public void Generate(CombatObject Source, CombatObject Target, int DispelMana)
    {
        EnchantmentList Enchantments;
        int Chance;
        FSourceID = Source.FID;
        FTargetID = Target.FID;
        if (Source is CombatUnit && Target is CombatUnit)
        {
            Enchantments = new EnchantmentList(null);
            ((CombatUnit)Target).FUnit.ListEnchantments(Enchantments);
            for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
            {
                Chance = Enchantments.GetEnchantments(I).DispelChance(DispelMana);
                if (Chance > AoWHSMap.Current.Random(100))
                {
                    var enchantments = Enchantments.GetEnchantments(I);
                    FEnchantmentIDs.Add(enchantments.FEnchantmentID);
                }
            }

            Enchantments?.Dispose();
        }
    }

    // intf_P4_02.inc:660

}
