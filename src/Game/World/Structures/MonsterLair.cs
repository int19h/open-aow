// Translated from MonsterL.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class MonsterLair : ItemExplorationSite
{
    public MonsterLair(ECustomNode owner) : base(owner)
    { }

    // MonsterL.pas:34
    public new const int ClassId = 131886;

    public new partial class Resource : ItemExplorationSiteResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MonsterL.pas:40
        public new const int ClassId = 131887;

        // MonsterL.pas:43
        public override int ResourceUserClassId() => 131886;

        // MonsterL.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_monsterLairText);

        private static readonly string s_monsterLairText = "Monster Lair";

    }

}
