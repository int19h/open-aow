// Translated from Hill.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Hexes;

namespace Aow.Game.World;

public partial class HillMO : TerrainMO
{
    public HillMO(ECustomNode owner) : base(owner)
    { }

    // Hill.pas:39
    public new const int ClassID = 131432;

    // Terrain.pas:19

}

public partial class HillMOR : TerrainMOR
{
    public HillMOR(ECustomNode owner) : base(owner)
    { }

    // Hill.pas:42
    public new const int ClassID = 131433;

    // Hill.pas:48
    public override EObject CreateResourceUser()
    {
        HillMO MO;
        MO = new HillMO(null);
        MO.SetResource(this);
        MO.SetResourceLinked(true);
        return MO;
    }

    // Hill.pas:45
    public override string ResourceEditName() => Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.HillRStr);

}
