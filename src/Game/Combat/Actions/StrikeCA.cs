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
public partial class StrikeCA : DamageCA
{
    public new const int ClassID = 131754;

    public StrikeCA(ECustomNode owner) : base(owner)
    { }

    public Set<StrikeModeElement> FStrikeMode;

    // AoWAb.PAS:1157
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FStrikeMode, 1, 20, Stream.rwData, 0);
    }

    // AoWAb.PAS:1163
    public override void Execute(Battle Combat)
    {
        CombatObject Source;
        CombatObject Target;
        UnitEnchantmentAbilityData Data;
        Source = Combat.FCombatData.FindID(FSourceID);
        Target = Combat.FCombatData.FindID(FTargetID);
        FDamageDone = (byte)Source.DoDamage(Target, FDamage);
        if (FEffects.Value != Set<DamageType>.Of())
        {
            Target.ExecuteDamageEffects(FEffects.Value);
        }

        if (FDamageDone != 0 && FStrikeMode.Contains(StrikeModeElement.LifeStealing) && Target is CombatUnit && ((CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine)
        {
            Source.SetHitPoints((byte)(Source.GetHitPoints() + 1));
        }

        if (FStrikeMode.Contains(StrikeModeElement.CausedFear) && Target is CombatUnit)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(108).Expand(((CombatUnit)Target).FUnit);
        }

        if (FStrikeMode.Contains(StrikeModeElement.Entangled) && Target is CombatUnit)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(94).Expand(((CombatUnit)Target).FUnit);
            Data = (UnitEnchantmentAbilityData)((CombatUnit)Target).FUnit.GetAbilityData(94);
            if (Data != null)
            {
                Data.FEnchantmentPlayerIndex = Source.FCombatPlayer;
            }
        }
    }

    // AoWAb.PAS:1187
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 1)
        {
            if (FDamage == 0)
            {
                Viewer.FCombat.FCombatData.FindID(FSourceID).PlaySFX(1);
            }
            else
            {
                Viewer.FCombat.FCombatData.FindID(FSourceID).PlaySFX(0);
            }
        }

        return base.Play(Viewer);
    }

    // AoWAb.PAS:1197
    public override void Generate(CombatObject Source, CombatObject Target, sbyte Attack, sbyte Damage, Set<DamageType> DamageTypes)
    {
        base.Generate(Source, Target, Attack, Damage, DamageTypes);
        if (FDamage != 0)
        {
            if (Source.GetAbilityEnabled(51) && !Target.GetAbilityEnabled(67) && HitChance.HitRole(3 - Target.GetResistance()))
            {
                FStrikeMode = FStrikeMode + Set<StrikeModeElement>.Of(StrikeModeElement.CausedFear);
            }

            if (Source.GetAbilityEnabled(119) && HitChance.HitRole(7 - Target.GetResistance()))
            {
                FStrikeMode = FStrikeMode + Set<StrikeModeElement>.Of(StrikeModeElement.Entangled);
            }
        }
    }

    // intf_P3_02.inc:38

}
