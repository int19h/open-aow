// Translated from Pyramid.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Pyramid(CustomNode owner) : ItemExplorationSite(owner)
{

    // Pyramid.pas:34
    public new const int ClassId = 131_982;

    public new class Resource(CustomNode owner) : ItemExplorationSiteResource(owner)
    {

        // Pyramid.pas:40
        public new const int ClassId = 131_983;

        // Pyramid.pas:43
        public override int ResourceUserClassId() => Pyramid.ClassId;

        // Pyramid.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_pyramidText);

        private static readonly string s_pyramidText = "Pyramid";

    }
}
