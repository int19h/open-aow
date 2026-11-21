#nullable disable warnings
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
public class DispelMagicCombatAction(CustomNode owner) : SingleTargetCombatAction(owner)
{

    public Int32List EnchantmentIds { get; } = new Int32List();

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
        persister.Int32List(name: "EnchantmentIds", list: EnchantmentIds);
    }

    // AoWAb.PAS:3951
    public override void Execute(Battle combat)
    {
        CombatObject? source = combat.CombatData.FindId(SourceId);
        CombatObject? target = combat.CombatData.FindId(TargetId);
        if (source is CombatUnit && target is CombatUnit targetUnit)
        {
            EnchantmentList enchantments = new(null) { Holder = CreationHolder() };
            targetUnit.Troop.ListEnchantments(enchantments);
            bool wasSummoned = targetUnit.Troop.GetAbilityEnabled(155);

            for (int i = EnchantmentIds.Count - 1; i >= 0; i--)
            {
                Enchantment? enchantment = enchantments.FindId(EnchantmentIds[i]);
                _ = (enchantment?.Dispel());
            }

            if (wasSummoned && !targetUnit.Troop.GetAbilityEnabled(155))
            {
                _ = source.DoDamage(target, damage: target.HitPoints);
            }

            enchantments?.Dispose();
        }
    }

    // AoWAb.PAS:3986
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            _ = GamePack.Current.AbilityControl[60].SfxLibrary.Play(0);
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
            _ = GamePack.Current.AbilityControl[60].ImageSequences.Show(0, X + 32, Y + 32, frame: viewer.Time, lockedSurface: surface);
        }
    }

    // AoWAb.PAS:3994
    public void Generate(CombatObject source, CombatObject? target, int dispelMana)
    {
        SourceId = source.Id;
        TargetId = target.Id;
        if (source is CombatUnit && target is CombatUnit generateTarget)
        {
            EnchantmentList enchantmentList = new(null) { Holder = CreationHolder() };
            generateTarget.Troop.ListEnchantments(enchantmentList);
            for (int i = enchantmentList.GetCount() - 1; i >= 0; i--)
            {
                int chance = enchantmentList.GetEnchantments(i).DispelChance(dispelMana);
                if (chance > StrategicMap.Current.Random(100))
                {
                    Enchantment enchantments = enchantmentList.GetEnchantments(i);
                    _ = EnchantmentIds.Add(enchantments.EnchantmentId);
                }
            }

            enchantmentList?.Dispose();
        }
    }
}
