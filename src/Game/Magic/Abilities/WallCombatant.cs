// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

// WallType. HitPoints. Hits
public class WallCombatant(CustomNode owner) : Troop(owner)
{
    private WallType _wallType;

    public int HitPoints { get; set; }
    public int Hits { get; set; }

    // Unit.PAS:2510
    public override int GetDefense() => 0;

    // Unit.PAS:2518
    public override int GetHits() => Hits;

    // Unit.PAS:2514
    public override int GetHitPoints() => HitPoints;

    // Unit.PAS:2503
    public override DamageTypes GetImmunityTypes() => AbilityRules.AllDamageTypesForWallImmunity & ~AbilityRules.WallDamageTypes;

    // Unit.PAS:2522
    public override string GetName()
    {
        return _wallType switch
        {
            WallType.Wood => AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WoodenWallText),
            WallType.Stone => AbilityRules.TranslateResourceString(GetEngine(), S: GameText.StoneWallText),
            WallType.None => AbilityRules.TranslateResourceString(GetEngine(), S: GameText.UndefinedWallText),
            _ => "",
        };
    }

    // Unit.PAS:2530
    public override UnitType GetUnitType() => UnitType.Machine;

    // Unit.PAS:2494
    public void SetWallType(WallType value)
    {
        _wallType = value;
        Hits = _wallType == WallType.Stone ? 10 : 5;

        HitPoints = Hits;
    }
}
