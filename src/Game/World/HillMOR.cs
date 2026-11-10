// Translated from Hill.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Hexes;

namespace Aow.Game.World;

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
    public override string ResourceEditName() => AbilityRules.TranslateRStr(GameText.HillRStr);

}
