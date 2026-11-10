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
    public Structure GetStructure(AbstractUnit AUnit)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        AUnit.GetLocation(ref X, ref Y, ref L);
        Structure Result = (Structure)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(Structure));
        if (Result != null)
        {
            if (!Result.GetRazed() || Result.GetBuilding())
            {
                Result = null;
            }
        }

        return Result;
    }

    // AoWAb.PAS:5413
    public override void GetInfo(AbstractUnit AUnit, ref string Text, ref int Gold, ref int Days)
    {
        Structure Structure;
        Structure = GetStructure(AUnit);
        if (Structure != null)
        {
            var editName = Structure.EditName();
            Text = AbilityRules.Format(AbilityRules.TranslateRStr(P4RebuildXRStr), [editName]);
            Structure.GetRebuildInfo(ref Gold, ref Days);
        }
        else
        {
            var p4Structure = AbilityRules.TranslateRStr(P4StructureRStr);
            Text = AbilityRules.Format(AbilityRules.TranslateRStr(P4RebuildXRStr), [p4Structure]);
            Gold = 0;
            Days = 0;
        }
    }

    // AoWAb.PAS:5430
    public override bool CanConstruct(AbstractUnit AUnit, ref string Reason)
    {
        bool Result;
        if (GetStructure(AUnit) != null)
        {
            Result = base.CanConstruct(AUnit, ref Reason);
        }
        else
        {
            Reason = AbilityRules.TranslateRStr(P4NoRazedStructuresRStr);
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:5439
    public override bool Construct(AbstractUnit AUnit)
    {
        bool Result;
        string Reason = "";
        if (CanConstruct(AUnit, ref Reason))
        {
            Result = GetStructure(AUnit).Rebuild(AUnit);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Engine.pas:524

    // AoWAb.PAS:5400
    public override int ID() => 1;

    private static readonly string P4RebuildXRStr = "Rebuild %s";
    private static readonly string P4StructureRStr = "Structure";
    private static readonly string P4NoRazedStructuresRStr = "No razed structure found";

}
