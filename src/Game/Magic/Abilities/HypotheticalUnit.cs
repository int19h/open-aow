// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

// attack. defense. resistance. hits. damage. immunity. protection. hit points
public class HypotheticalUnit : Troop
{
    private sbyte _attack;
    private sbyte _defense;
    private sbyte _resistance;
    private sbyte _hits;
    private sbyte _damage;
    private DamageTypes _immunityTypes;
    private DamageTypes _protectionTypes;
    private sbyte _hitPoints;

    // Unit.PAS:2534
    public HypotheticalUnit(CustomNode owner) : base(owner) => MakeDefaultLevelUnit(1);

    // Unit.PAS:2591
    public override sbyte GetAttack() => _attack;

    // Unit.PAS:2593
    public override sbyte GetDefense() => _defense;

    // Unit.PAS:2595
    public override sbyte GetDamage() => _damage;

    // Unit.PAS:2603
    public override sbyte GetResistance() => _resistance;

    // Unit.PAS:2597
    public override sbyte GetHits() => _hits;

    // Unit.PAS:2599
    public override sbyte GetHitPoints() => _hitPoints;

    // Unit.PAS:2601
    public override void SetHitPoints(sbyte value) => _hitPoints = value;

    // Unit.PAS:2605
    public override DamageTypes GetImmunityTypes() => _immunityTypes;

    // Unit.PAS:2607
    public override DamageTypes GetProtectionTypes() => _protectionTypes;

    // Unit.PAS:2539
    public void MakeDefaultLevelUnit(sbyte level)
    {
        // { Original retains protection from an earlier, higher-level setup. }
        switch (level)
        {
            case (sbyte)CompressStream.Level.Fastest:
                _attack = 4;
                _defense = 2;
                _resistance = 3;
                _damage = 3;
                _hits = 5;
                SetAbilitySet(21, value: 1);
                SetAbilitySet(0, value: 1);
                _immunityTypes = DamageTypes.Wall;
                break;
            case (sbyte)CompressStream.Level.Default:
                _attack = 5;
                _defense = 3;
                _resistance = 3;
                _damage = 3;
                _hits = 8;
                SetAbilitySet(21, value: 1);
                SetAbilitySet(0, value: 1);
                _immunityTypes = DamageTypes.Wall;
                break;
            case (sbyte)CompressStream.Level.Max:
                _attack = 6;
                _defense = 3;
                _resistance = 4;
                _damage = 7;
                _hits = 15;
                SetAbilitySet(21, value: 1);
                SetAbilitySet(0, value: 1);
                _immunityTypes = DamageTypes.Wall;
                _protectionTypes = DamageTypes.Physical;
                break;
            default:
                {
                    _attack = 6;
                    _defense = 3;
                    _resistance = 4;
                    _damage = 7;
                    _hits = 20;
                    _immunityTypes = DamageTypes.Physical | DamageTypes.Wall;
                    SetAbilitySet(21, value: 1);
                    SetAbilitySet(0, value: 1);
                }

                break;
        }

        _hitPoints = _hits;
    }
}
