// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat.Actions;

// (strike mode)
public partial class StrikeCombatAction : DamageCombatAction
{
    public new const int ClassId = 131754;

    public StrikeCombatAction(ECustomNode owner) : base(owner)
    { }

    public Set<StrikeModeElement> StrikeMode;

    // AoWAb.PAS:1157
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        RwSet(ref StrikeMode, 1, 20, stream.rwData, 0);
    }

    // AoWAb.PAS:1163
    public override void Execute(Battle combat)
    {
        CombatObject source;
        CombatObject target;
        UnitEnchantmentAbilityData data;
        source = combat.CombatData.FindId(SourceId);
        target = combat.CombatData.FindId(TargetId);
        DamageDone = (byte)source.DoDamage(target, Damage);
        if (Effects.Value != Set<DamageType>.Of())
        {
            target.ExecuteDamageEffects(Effects.Value);
        }

        if (DamageDone != 0 && StrikeMode.Contains(StrikeModeElement.LifeStealing) && target is CombatUnit && ((CombatUnit)target).Troop.GetUnitType() != UnitType.Machine)
        {
            source.SetHitPoints((byte)(source.GetHitPoints() + 1));
        }

        if (StrikeMode.Contains(StrikeModeElement.CausedFear) && target is CombatUnit)
        {
            GamePack.Current.AbilityControl.GetAbility(108).Expand(((CombatUnit)target).Troop);
        }

        if (StrikeMode.Contains(StrikeModeElement.Entangled) && target is CombatUnit)
        {
            GamePack.Current.AbilityControl.GetAbility(94).Expand(((CombatUnit)target).Troop);
            data = (UnitEnchantmentAbilityData)((CombatUnit)target).Troop.GetAbilityData(94);
            if (data != null)
            {
                data.EnchantmentPlayerIndex = source.CombatPlayer;
            }
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
    public override void Generate(CombatObject source, CombatObject target, sbyte attack, sbyte damage, Set<DamageType> damageTypes)
    {
        base.Generate(source, target, attack, damage, damageTypes);
        if (Damage != 0)
        {
            if (source.GetAbilityEnabled(51) && !target.GetAbilityEnabled(67) && HitChance.HitRole(3 - target.GetResistance()))
            {
                StrikeMode = StrikeMode + Set<StrikeModeElement>.Of(StrikeModeElement.CausedFear);
            }

            if (source.GetAbilityEnabled(119) && HitChance.HitRole(7 - target.GetResistance()))
            {
                StrikeMode = StrikeMode + Set<StrikeModeElement>.Of(StrikeModeElement.Entangled);
            }
        }
    }

    // intf_P3_02.inc:38

}
