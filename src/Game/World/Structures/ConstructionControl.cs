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
    public virtual void GetInfo(Troop troop, ref string text, ref int gold, ref int days)
    {
        text = "";
        gold = 0;
        days = 0;
    }

    // AoWAb.PAS:5500
    public virtual bool CanConstruct(Troop troop, ref string reason)
    {
        bool result;
        int gold = default;
        int days = default;
        string text = "";
        GetInfo(troop, ref text, ref gold, ref days);
        if (StrategicMap.Current.Players.GetPlayers(troop.Player).Gems < gold)
        {
            reason = AbilityRules.TranslateResourceString(s_notEnoughGoldText);
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWAb.PAS:5513
    public virtual bool Construct(Troop troop) => false;

    // AoWAb.PAS:5517
    public virtual int Id() => 0;

    private static readonly string s_notEnoughGoldText = "Not enough gold.";

}
