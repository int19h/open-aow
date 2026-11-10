// Translated from Crypt.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Crypt : ItemExplorationSite
{
    public Crypt(ECustomNode owner) : base(owner)
    { }

    // Crypt.pas:34
    public new const int ClassID = 131850;

    public new partial class Resource : ItemExplorationSiteResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Crypt.pas:40
        public new const int ClassID = 131851;

        // Crypt.pas:43
        public override int ResourceUserClassID() => 131850;

        // Crypt.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateRStr(CryptRStr);

        private static readonly string CryptRStr = "Crypt";

    }

}
