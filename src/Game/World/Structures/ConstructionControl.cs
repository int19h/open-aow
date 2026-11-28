#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public class ConstructionControl(CustomNode owner) : EngineObject(owner)
{

    // AoWAb.PAS:5494
    public virtual void GetInfo(Troop troop, out string text, out int gold, out int days)
    {
        text = "";
        gold = 0;
        days = 0;
    }

    // AoWAb.PAS:5500
    public virtual bool CanConstruct(Troop troop, ref string reason)
    {
        GetInfo(troop, text: out _, out int gold, days: out _);
        if (GetEngine().StrategicMap.Players.GetPlayers(troop.Player).Gems < gold)
        {
            reason = AbilityRules.TranslateResourceString(GetEngine(), S: s_notEnoughGoldText);
            return false;
        }
        else
        {
            return true;
        }
    }

    // AoWAb.PAS:5513
    public virtual bool Construct(Troop troop) => false;

    // AoWAb.PAS:5517
    public virtual int Id() => 0;

    private static readonly string s_notEnoughGoldText = "Not enough gold.";

}
