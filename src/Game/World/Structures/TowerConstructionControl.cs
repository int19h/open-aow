// Translated from Tower.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class TowerConstructionControl : StructureConstructionControl
{
    public TowerConstructionControl(ECustomNode owner) : base(owner)
    { }

    // Tower.pas:139
    public override void GetInfo(Troop AUnit, ref string Text, ref int Gold, ref int Days)
    {
        var tower = AbilityRules.TranslateRStr(GameText.TowerRStr);
        Text = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.BuildXRStr), [tower]);
        Gold = 50;
        Days = 2;
    }

    // Tower.pas:146
    public override bool CanConstruct(Troop AUnit, ref string Reason)
    {
        bool Result;
        sbyte Xhx = default;
        sbyte Yhx = default;
        sbyte Lhx = default;
        AUnit.GetLocation(ref Xhx, ref Yhx, ref Lhx);
        if (Lhx == 0)
        {
            Result = base.CanConstruct(AUnit, ref Reason);
        }
        else
        {
            Result = false;
            var tower = AbilityRules.TranslateRStr(GameText.TowerRStr);
            Reason = RtlUnit.Format(AbilityRules.TranslateRStr(CannotBuildXInUndergroundRStr), [tower]);
        }

        return Result;
    }

    // Tower.pas:133
    public override StructureResource StructureResource(Troop AUnit) => (StructureResource)AoWHSSet.Current.MapObjectResourceList.FindChild(132159);

    // Tower.pas:130
    public override int ID() => 3;

    private static readonly string CannotBuildXInUndergroundRStr = "Cannot build %s underground";

}
