#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// (strike mode)
public class StrikeCombatAction(CustomNode owner) : DamageCombatAction(owner)
{
    public new const int ClassId = 131_754;

    public StrikeModes StrikeMode { get; set; }

    // AoWAb.PAS:1157
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        StrikeMode = persister.Set("StrikeMode", value: StrikeMode);
    }

    // AoWAb.PAS:1163
    public override void Execute(Battle combat)
    {
        CombatObject source = combat.CombatData.FindId(SourceId);
        CombatObject target = combat.CombatData.FindId(TargetId);
        DamageDone = source.DoDamage(target, Damage).TruncateToByte();
        if (Effects != DamageTypes.None)
        {
            _ = target.ExecuteDamageEffects(Effects);
        }

        if (DamageDone != 0 && StrikeMode.Contains(StrikeModes.LifeStealing) && target is CombatUnit targetUnit && targetUnit.Troop.GetUnitType() != UnitType.Machine)
        {
            source.SetHitPoints((source.HitPoints + 1).TruncateToByte());
        }

        if (StrikeMode.Contains(StrikeModes.CausedFear) && target is CombatUnit fearTarget)
        {
            _ = GamePack.Current.AbilityControl[108].Expand(fearTarget.Troop);
        }

        if (StrikeMode.Contains(StrikeModes.Entangled) && target is CombatUnit entangledTarget)
        {
            _ = GamePack.Current.AbilityControl[94].Expand(entangledTarget.Troop);
            UnitEnchantmentAbilityData data = entangledTarget.Troop.GetAbilityData<UnitEnchantmentAbilityData>(94);
            data?.EnchantmentPlayerIndex = PlayerId.FromOriginal(source.CombatPlayer);
        }
    }

    // AoWAb.PAS:1187
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 1)
        {
            if (Damage == 0)
            {
                viewer.Combat.CombatData.FindId(SourceId).PlaySfx(1);
            }
            else
            {
                viewer.Combat.CombatData.FindId(SourceId).PlaySfx(0);
            }
        }

        return base.Play(viewer);
    }

    // AoWAb.PAS:1197
    public override void Generate(CombatObject source, CombatObject target, int attack, int damage, DamageTypes damageTypes)
    {
        base.Generate(source, target, attack, damage, damageTypes);
        if (Damage != 0)
        {
            if (source.GetAbilityEnabled(51) && !target.GetAbilityEnabled(67) && HitChance.HitRole(3 - target.GetResistance()))
            {
                StrikeMode |= StrikeModes.CausedFear;
            }

            if (source.GetAbilityEnabled(119) && HitChance.HitRole(7 - target.GetResistance()))
            {
                StrikeMode |= StrikeModes.Entangled;
            }
        }
    }
}
