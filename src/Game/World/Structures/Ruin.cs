// Translated from Ruin.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class Ruin(CustomNode owner) : ItemExplorationSite(owner)
{

    // Ruin.pas:34
    public new const int ClassId = 131_994;

    public new class Resource(CustomNode owner) : ItemExplorationSiteResource(owner)
    {

        // Ruin.pas:40
        public new const int ClassId = 131_995;

        // Ruin.pas:43
        public override int ResourceUserClassId() => Ruin.ClassId;

        // Ruin.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_castleRuinText);

        private static readonly string s_castleRuinText = "Castle Ruin";

    }
}
