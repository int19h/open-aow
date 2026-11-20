// Translated from Crypt.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Crypt(CustomNode owner) : ItemExplorationSite(owner)
{

    // Crypt.pas:34
    public new const int ClassId = 131_850;

    public new class Resource(CustomNode owner) : ItemExplorationSiteResource(owner)
    {

        // Crypt.pas:40
        public new const int ClassId = 131_851;

        // Crypt.pas:43
        public override int ResourceUserClassId() => Crypt.ClassId;

        // Crypt.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_cryptText);

        private static readonly string s_cryptText = "Crypt";

    }
}
