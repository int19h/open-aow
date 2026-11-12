// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// attack. defense. resistance. hits. damage. immunity. protection. hit points
public partial class HypotheticalUnit : Troop
{
    public sbyte Attack;
    public sbyte Defense;
    public sbyte Resistance;
    public sbyte Hits;
    public sbyte Damage;
    public Set<DamageType> ImmunityTypes;
    public Set<DamageType> ProtectionTypes;
    public sbyte HitPoints;

    // Unit.PAS:2534
    public HypotheticalUnit(ECustomNode owner) : base(owner)
    {
        MakeDefaultLevelUnit(1);
    }

    // Unit.PAS:2591
    public override sbyte GetAttack() => Attack;

    // Unit.PAS:2593
    public override sbyte GetDefense() => Defense;

    // Unit.PAS:2595
    public override sbyte GetDamage() => Damage;

    // Unit.PAS:2603
    public override sbyte GetResistance() => Resistance;

    // Unit.PAS:2597
    public override sbyte GetHits() => Hits;

    // Unit.PAS:2599
    public override sbyte GetHitPoints() => HitPoints;

    // Unit.PAS:2601
    public override void SetHitPoints(sbyte value)
    {
        HitPoints = value;
    }

    // Unit.PAS:2605
    public override Set<DamageType> GetImmunityTypes() => ImmunityTypes;

    // Unit.PAS:2607
    public override Set<DamageType> GetProtectionTypes() => ProtectionTypes;

    // Unit.PAS:2539
    public void MakeDefaultLevelUnit(sbyte level)
    {
        // { Original retains protection from an earlier, higher-level setup. }
        switch (level)
        {
            case 1:
                Attack = 4;
                Defense = 2;
                Resistance = 3;
                Damage = 3;
                Hits = 5;
                SetAbilitySet(21, 1);
                SetAbilitySet(0, 1);
                ImmunityTypes = Set<DamageType>.Of(DamageType.Wall);
                break;
            case 2:
                Attack = 5;
                Defense = 3;
                Resistance = 3;
                Damage = 3;
                Hits = 8;
                SetAbilitySet(21, 1);
                SetAbilitySet(0, 1);
                ImmunityTypes = Set<DamageType>.Of(DamageType.Wall);
                break;
            case 3:
                Attack = 6;
                Defense = 3;
                Resistance = 4;
                Damage = 7;
                Hits = 15;
                SetAbilitySet(21, 1);
                SetAbilitySet(0, 1);
                ImmunityTypes = Set<DamageType>.Of(DamageType.Wall);
                ProtectionTypes = Set<DamageType>.Of(DamageType.Physical);
                break;
            default:
                {
                    Attack = 6;
                    Defense = 3;
                    Resistance = 4;
                    Damage = 7;
                    Hits = 20;
                    ImmunityTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
                    SetAbilitySet(21, 1);
                    SetAbilitySet(0, 1);
                }

                break;
        }

        HitPoints = Hits;
    }

}
