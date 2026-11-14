// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.Magic;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (dispelled enchantment IDs)
public partial class DispelMagicCombatAction : SingleTargetCombatAction
{

    // AoWAb.PAS:3936
    public DispelMagicCombatAction(ECustomNode owner) : base(owner)
    {
        EnchantmentIds = new Int32List();
    }

    public Int32List EnchantmentIds { get; }

    // AoWAb.PAS:3941
    public override void Dispose()
    {
        EnchantmentIds?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:3946
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Int32List(persister, name: "EnchantmentIds", list: EnchantmentIds);
    }

    // AoWAb.PAS:3951
    public override void Execute(Battle combat)
    {
        EnchantmentList enchantments;
        CombatObject source;
        CombatObject target;
        Enchantment enchantment;
        bool wasSummoned;
        source = combat.CombatData.FindId(SourceId);
        target = combat.CombatData.FindId(TargetId);
        if (source is CombatUnit && target is CombatUnit)
        {
            enchantments = new EnchantmentList(null) { Holder = CreationHolder() };
            var combatUnit = (CombatUnit)target;
            combatUnit.Troop.ListEnchantments(enchantments);
            wasSummoned = combatUnit.Troop.GetAbilityEnabled(155);

            for (int I = EnchantmentIds.Count - 1; I >= 0; I--)
            {
                enchantment = enchantments.FindId(EnchantmentIds[I]);
                if (enchantment != null)
                {
                    enchantment.Dispel();
                }
            }

            if (wasSummoned && !((CombatUnit)target).Troop.GetAbilityEnabled(155))
            {
                source.DoDamage(target, target.GetHitPoints());
            }

            enchantments?.Dispose();
        }
    }

    // AoWAb.PAS:3986
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            GamePack.Current.AbilityControl.GetAbility(60).SfxLibrary.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // AoWAb.PAS:3979
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId)
        {
            GamePack.Current.AbilityControl.GetAbility(60).ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:3994
    public void Generate(CombatObject source, CombatObject target, int dispelMana)
    {
        EnchantmentList Enchantments;
        int chance;
        SourceId = source.Id;
        TargetId = target.Id;
        if (source is CombatUnit && target is CombatUnit)
        {
            Enchantments = new EnchantmentList(null) { Holder = CreationHolder() };
            ((CombatUnit)target).Troop.ListEnchantments(Enchantments);
            for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
            {
                chance = Enchantments.GetEnchantments(I).DispelChance(dispelMana);
                if (chance > StrategicMap.Current.Random(100))
                {
                    var enchantments = Enchantments.GetEnchantments(I);
                    EnchantmentIds.Add(enchantments.EnchantmentId);
                }
            }

            Enchantments?.Dispose();
        }
    }

    // intf_P4_02.inc:660

}
