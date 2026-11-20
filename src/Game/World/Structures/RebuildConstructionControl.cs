#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// { Interface declaration run owned by P4. }
public partial class RebuildConstructionControl(CustomNode owner) : ConstructionControl(owner)
{

    // AoWAb.PAS:5404
    public Structure GetStructure(Troop troop)
    {
        MapCoordinate coordinate36 = troop.GetLocation();
        sbyte x = coordinate36.X;
        sbyte y = coordinate36.Y;
        sbyte l = coordinate36.MapLevel;
        Structure result = StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite<Structure>();
        if (result is not null)
        {
            if (!result.GetRazed() || result.GetBuilding())
            {
                result = null;
            }
        }

        return result;
    }

    // AoWAb.PAS:5413
    public override void GetInfo(Troop troop, out string text, out int gold, out int days)
    {
        Structure structure = GetStructure(troop);
        if (structure is not null)
        {
            string editName = structure.EditName();
            text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_rebuildFormat), [editName]);
            structure.GetRebuildInfo(out gold, out days);
        }
        else
        {
            string structureLocal = AbilityRules.TranslateResourceString(GetEngine(), s_structureText);
            text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_rebuildFormat), [structureLocal]);
            gold = 0;
            days = 0;
        }
    }

    // AoWAb.PAS:5430
    public override bool CanConstruct(Troop troop, ref string reason)
    {
        bool result;
        if (GetStructure(troop) is not null)
        {
            result = base.CanConstruct(troop, ref reason);
        }
        else
        {
            reason = AbilityRules.TranslateResourceString(GetEngine(), s_noRazedStructuresText);
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:5439
    public override bool Construct(Troop troop)
    {
        string reason = "";
        bool result = CanConstruct(troop, ref reason) && GetStructure(troop).Rebuild(troop);

        return result;
    }

    // Engine.pas:524

    // AoWAb.PAS:5400
    public override int Id() => 1;

    private static readonly string s_rebuildFormat = "Rebuild %s";
    private static readonly string s_structureText = "Structure";
    private static readonly string s_noRazedStructuresText = "No razed structure found";

}
