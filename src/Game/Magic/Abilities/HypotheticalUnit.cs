// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// attack. defense. resistance. hits. damage. immunity. protection. hit points
public partial class HypotheticalUnit : Troop
{
    public sbyte FAttack;
    public sbyte FDefense;
    public sbyte FResistance;
    public sbyte FHits;
    public sbyte FDamage;
    public Set<DamageType> FImmunityTypes;
    public Set<DamageType> FProtectionTypes;
    public sbyte FHitPoints;

    // Unit.PAS:2534
    public HypotheticalUnit(ECustomNode owner) : base(owner)
    {
        MakeDefaultLevelUnit(1);
    }

    // Unit.PAS:2591
    public override sbyte GetAttack() => FAttack;

    // Unit.PAS:2593
    public override sbyte GetDefense() => FDefense;

    // Unit.PAS:2595
    public override sbyte GetDamage() => FDamage;

    // Unit.PAS:2603
    public override sbyte GetResistance() => FResistance;

    // Unit.PAS:2597
    public override sbyte GetHits() => FHits;

    // Unit.PAS:2599
    public override sbyte GetHitPoints() => FHitPoints;

    // Unit.PAS:2601
    public override void SetHitPoints(sbyte Value)
    {
        FHitPoints = Value;
    }

    // Unit.PAS:2605
    public override Set<DamageType> GetImmunityTypes() => FImmunityTypes;

    // Unit.PAS:2607
    public override Set<DamageType> GetProtectionTypes() => FProtectionTypes;

    // Unit.PAS:2539
    public void MakeDefaultLevelUnit(sbyte Level)
    {
        // { Original retains protection from an earlier, higher-level setup. }
        switch (Level)
        {
            case 1:
                FAttack = 4;
                FDefense = 2;
                FResistance = 3;
                FDamage = 3;
                FHits = 5;
                SetAbSet(21, 1);
                SetAbSet(0, 1);
                FImmunityTypes = Set<DamageType>.Of(DamageType.Wall);
                break;
            case 2:
                FAttack = 5;
                FDefense = 3;
                FResistance = 3;
                FDamage = 3;
                FHits = 8;
                SetAbSet(21, 1);
                SetAbSet(0, 1);
                FImmunityTypes = Set<DamageType>.Of(DamageType.Wall);
                break;
            case 3:
                FAttack = 6;
                FDefense = 3;
                FResistance = 4;
                FDamage = 7;
                FHits = 15;
                SetAbSet(21, 1);
                SetAbSet(0, 1);
                FImmunityTypes = Set<DamageType>.Of(DamageType.Wall);
                FProtectionTypes = Set<DamageType>.Of(DamageType.Physical);
                break;
            default:
                {
                    FAttack = 6;
                    FDefense = 3;
                    FResistance = 4;
                    FDamage = 7;
                    FHits = 20;
                    FImmunityTypes = Set<DamageType>.Of(DamageType.Physical, DamageType.Wall);
                    SetAbSet(21, 1);
                    SetAbSet(0, 1);
                }

                break;
        }

        FHitPoints = FHits;
    }

}
