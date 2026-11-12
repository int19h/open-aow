// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// WallType. HitPoints. Hits
public partial class WallCombatant : Troop
{
    public WallCombatant(ECustomNode owner) : base(owner)
    { }

    public WallType WallType;
    public sbyte HitPoints;
    public sbyte Hits;

    // Unit.PAS:2510
    public override sbyte GetDefense() => 0;

    // Unit.PAS:2518
    public override sbyte GetHits() => Hits;

    // Unit.PAS:2514
    public override sbyte GetHitPoints() => HitPoints;

    // Unit.PAS:2503
    public override Set<DamageType> GetImmunityTypes() => AbilityRules.s_allDamageTypesForWallImmunity - AbilityRules.s_wallDamageTypes;

    // Unit.PAS:2522
    public override string GetName()
    {
        string result = "";
        switch (WallType)
        {
            case WallType.Wood:
                result = AbilityRules.TranslateResourceString(GameText.s_woodenWallText);
                break;
            case WallType.Stone:
                result = AbilityRules.TranslateResourceString(GameText.s_stoneWallText);
                break;
            case WallType.None:
                result = AbilityRules.TranslateResourceString(GameText.s_undefinedWallText);
                break;
        }

        return result;
    }

    // Unit.PAS:2530
    public override UnitType GetUnitType() => UnitType.Machine;

    // Unit.PAS:2494
    public void SetWallType(WallType value)
    {
        WallType = value;
        if (WallType == WallType.Stone)
        {
            Hits = 10;
        }
        else
        {
            Hits = 5;
        }

        HitPoints = Hits;
    }

}
