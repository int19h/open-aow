// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// { Interface declaration run owned by P4. }
public partial class RebuildConstructionControl : ConstructionControl
{
    public RebuildConstructionControl(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:5404
    public Structure GetStructure(Troop troop)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        troop.GetLocation(ref X, ref Y, ref L);
        Structure result = (Structure)StrategicMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHexSprite(typeof(Structure));
        if (result != null)
        {
            if (!result.GetRazed() || result.GetBuilding())
            {
                result = null;
            }
        }

        return result;
    }

    // AoWAb.PAS:5413
    public override void GetInfo(Troop troop, ref string text, ref int gold, ref int days)
    {
        Structure structure;
        structure = GetStructure(troop);
        if (structure != null)
        {
            var editName = structure.EditName();
            text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_rebuildFormat), [editName]);
            structure.GetRebuildInfo(ref gold, ref days);
        }
        else
        {
            var structureLocal = AbilityRules.TranslateResourceString(s_structureText);
            text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_rebuildFormat), [structureLocal]);
            gold = 0;
            days = 0;
        }
    }

    // AoWAb.PAS:5430
    public override bool CanConstruct(Troop troop, ref string reason)
    {
        bool result;
        if (GetStructure(troop) != null)
        {
            result = base.CanConstruct(troop, ref reason);
        }
        else
        {
            reason = AbilityRules.TranslateResourceString(s_noRazedStructuresText);
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:5439
    public override bool Construct(Troop troop)
    {
        bool result;
        string reason = "";
        if (CanConstruct(troop, ref reason))
        {
            result = GetStructure(troop).Rebuild(troop);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Engine.pas:524

    // AoWAb.PAS:5400
    public override int Id() => 1;

    private static readonly string s_rebuildFormat = "Rebuild %s";
    private static readonly string s_structureText = "Structure";
    private static readonly string s_noRazedStructuresText = "No razed structure found";

}
