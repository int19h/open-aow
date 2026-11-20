// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// WallType. HitPoints. Hits
public class WallCombatant(CustomNode owner) : Troop(owner)
{
    private WallType _wallType;

    public sbyte HitPoints { get; set; }
    public sbyte Hits { get; set; }

    // Unit.PAS:2510
    public override sbyte GetDefense() => 0;

    // Unit.PAS:2518
    public override sbyte GetHits() => Hits;

    // Unit.PAS:2514
    public override sbyte GetHitPoints() => HitPoints;

    // Unit.PAS:2503
    public override Set<DamageType> GetImmunityTypes() => AbilityRules.AllDamageTypesForWallImmunity - AbilityRules.WallDamageTypes;

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
        Hits = _wallType == WallType.Stone ? (sbyte)10 : (sbyte)5;

        HitPoints = Hits;
    }
}
