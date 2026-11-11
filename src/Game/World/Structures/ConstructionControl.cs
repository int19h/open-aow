// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class ConstructionControl : EngineObject
{

    public ConstructionControl(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:5494
    public virtual void GetInfo(Troop AUnit, ref string Text, ref int Gold, ref int Days)
    {
        Text = "";
        Gold = 0;
        Days = 0;
    }

    // AoWAb.PAS:5500
    public virtual bool CanConstruct(Troop AUnit, ref string Reason)
    {
        bool Result;
        int Gold = default;
        int Days = default;
        string Text = "";
        GetInfo(AUnit, ref Text, ref Gold, ref Days);
        if (AoWHSMap.Current.FPlayers.GetPlayers(AUnit.FPlayer).FGems < Gold)
        {
            Reason = AbilityRules.TranslateRStr(P4NotEnoughGoldRStr);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWAb.PAS:5513
    public virtual bool Construct(Troop AUnit) => false;

    // AoWAb.PAS:5517
    public virtual int ID() => 0;

    private static readonly string P4NotEnoughGoldRStr = "Not enough gold.";

}
