// Translated from Hill.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Hexes;

namespace Aow.Game.World;

public partial class THillMO : TTerrainMO
{
    public THillMO(TECustomNode owner) : base(owner)
    { }

    // Hill.pas:39
    public new const int ClassID = 131432;

    // Terrain.pas:19

}

public partial class THillMOR : TTerrainMOR
{
    public THillMOR(TECustomNode owner) : base(owner)
    { }

    // Hill.pas:42
    public new const int ClassID = 131433;

    // Hill.pas:48
    public override TEObject CreateResourceUser()
    {
        THillMO MO;
        MO = new THillMO(null);
        MO.SetResource(this);
        MO.SetResourceLinked(true);
        return MO;
    }

    // Hill.pas:45
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.HillRStr);

}
