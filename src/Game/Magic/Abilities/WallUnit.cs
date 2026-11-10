// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// FWallType. FHitPoints. FHits
public partial class WallUnit : AbstractUnit
{
    public WallUnit(ECustomNode owner) : base(owner)
    { }

    public WallType FWallType;
    public sbyte FHitPoints;
    public sbyte FHits;

    // Unit.PAS:2510
    public override sbyte GetDefense() => 0;

    // Unit.PAS:2518
    public override sbyte GetHits() => FHits;

    // Unit.PAS:2514
    public override sbyte GetHitPoints() => FHitPoints;

    // Unit.PAS:2503
    public override Set<DamageType> GetImmunityTypes() => AbilityRules.TWallUnit_GetImmunityTypes__AllDamageTypes - AbilityRules.TWallUnit_GetImmunityTypes__WallDamageTypes;

    // Unit.PAS:2522
    public override string GetName()
    {
        string Result = "";
        switch (FWallType)
        {
            case WallType.Wood:
                Result = AbilityRules.TranslateRStr(GameText.P2WoodenWallRStr);
                break;
            case WallType.Stone:
                Result = AbilityRules.TranslateRStr(GameText.P2StoneWallRStr);
                break;
            case WallType.None:
                Result = AbilityRules.TranslateRStr(GameText.P2UndefinedWallRStr);
                break;
        }

        return Result;
    }

    // Unit.PAS:2530
    public override UnitType GetUnitType() => UnitType.Machine;

    // Unit.PAS:2494
    public void SetWallType(WallType Value)
    {
        FWallType = Value;
        if (FWallType == WallType.Stone)
        {
            FHits = 10;
        }
        else
        {
            FHits = 5;
        }

        FHitPoints = FHits;
    }

}
