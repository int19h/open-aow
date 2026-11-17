// Translated from Tower.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class TowerConstructionControl : StructureConstructionControl
{
    public TowerConstructionControl(CustomNode owner) : base(owner)
    { }

    // Tower.pas:139
    public override void GetInfo(Troop troop, out string text, out int gold, out int days)
    {
        string tower = AbilityRules.TranslateResourceString(GetEngine(), GameText.TowerText);
        text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.BuildFormat), [tower]);
        gold = 50;
        days = 2;
    }

    // Tower.pas:146
    public override bool CanConstruct(Troop troop, ref string reason)
    {
        bool result;
        MapCoordinate coordinate40 = troop.GetLocation();
        _ = coordinate40.X;
        _ = coordinate40.Y;
        sbyte hexLevel = coordinate40.MapLevel;
        if (hexLevel == 0)
        {
            result = base.CanConstruct(troop, ref reason);
        }
        else
        {
            result = false;
            string tower = AbilityRules.TranslateResourceString(GetEngine(), GameText.TowerText);
            reason = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_cannotBuildInUndergroundFormat), [tower]);
        }

        return result;
    }

    // Tower.pas:133
    public override StructureResource StructureResource(Troop troop) => GamePack.Current.MapObjectResourceList.FindChild<StructureResource>(132159);

    // Tower.pas:130
    public override int Id() => 3;

    private static readonly string s_cannotBuildInUndergroundFormat = "Cannot build %s underground";

}
