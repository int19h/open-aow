// Translated from Pyramid.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Pyramid : ItemExplorationSite
{
    public Pyramid(ECustomNode owner) : base(owner)
    { }

    // Pyramid.pas:34
    public new const int ClassID = 131982;

    public new partial class Resource : ItemExplorationSiteResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Pyramid.pas:40
        public new const int ClassID = 131983;

        // Pyramid.pas:43
        public override int ResourceUserClassID() => 131982;

        // Pyramid.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateRStr(PyramidRStr);

        private static readonly string PyramidRStr = "Pyramid";

    }

}
