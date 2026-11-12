// Translated from Ruin.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Ruin : ItemExplorationSite
{
    public Ruin(ECustomNode owner) : base(owner)
    { }

    // Ruin.pas:34
    public new const int ClassId = 131994;

    public new partial class Resource : ItemExplorationSiteResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Ruin.pas:40
        public new const int ClassId = 131995;

        // Ruin.pas:43
        public override int ResourceUserClassId() => 131994;

        // Ruin.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_castleRuinText);

        private static readonly string s_castleRuinText = "Castle Ruin";

    }

}
