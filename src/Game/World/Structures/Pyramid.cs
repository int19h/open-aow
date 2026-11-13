// Translated from Pyramid.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Pyramid : ItemExplorationSite
{
    public Pyramid(ECustomNode owner) : base(owner)
    { }

    // Pyramid.pas:34
    public new const int ClassId = 131982;

    public new partial class Resource : ItemExplorationSiteResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Pyramid.pas:40
        public new const int ClassId = 131983;

        // Pyramid.pas:43
        public override int ResourceUserClassId() => 131982;

        // Pyramid.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_pyramidText);

        private static readonly string s_pyramidText = "Pyramid";

    }

}
