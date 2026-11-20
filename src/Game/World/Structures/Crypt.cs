// Translated from Crypt.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Crypt(CustomNode owner) : ItemExplorationSite(owner)
{

    // Crypt.pas:34
    public new const int ClassId = 131850;

    public new class Resource(CustomNode owner) : ItemExplorationSiteResource(owner)
    {

        // Crypt.pas:40
        public new const int ClassId = 131851;

        // Crypt.pas:43
        public override int ResourceUserClassId() => 131850;

        // Crypt.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_cryptText);

        private static readonly string s_cryptText = "Crypt";

    }
}
