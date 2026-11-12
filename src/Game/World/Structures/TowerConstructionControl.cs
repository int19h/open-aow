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
    public override void GetInfo(Troop troop, out string text, out int gold, out int days)
    {
        var tower = AbilityRules.TranslateResourceString(GetEngine(), GameText.TowerText);
        text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.BuildFormat), [tower]);
        gold = 50;
        days = 2;
    }

    // Tower.pas:146
    public override bool CanConstruct(Troop troop, ref string reason)
    {
        bool result;
        sbyte hexX = default;
        sbyte hexY = default;
        sbyte hexLevel = default;
        troop.GetLocation(out hexX, out hexY, out hexLevel);
        if (hexLevel == 0)
        {
            result = base.CanConstruct(troop, ref reason);
        }
        else
        {
            result = false;
            var tower = AbilityRules.TranslateResourceString(GetEngine(), GameText.TowerText);
            reason = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_cannotBuildInUndergroundFormat), [tower]);
        }

        return result;
    }

    // Tower.pas:133
    public override StructureResource StructureResource(Troop troop) => (StructureResource)GamePack.Current.MapObjectResourceList.FindChild(132159);

    // Tower.pas:130
    public override int Id() => 3;

    private static readonly string s_cannotBuildInUndergroundFormat = "Cannot build %s underground";

}
