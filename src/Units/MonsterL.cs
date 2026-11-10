// Translated from MonsterL.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class Monsterlair : ItemExplorationSite
{
    public Monsterlair(ECustomNode owner) : base(owner)
    { }

    // MonsterL.pas:34
    public new const int ClassID = 131886;

    public new partial class Resource : ItemExplorationSiteResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MonsterL.pas:40
        public new const int ClassID = 131887;

        // MonsterL.pas:43
        public override int ResourceUserClassID() => 131886;

        // MonsterL.pas:37
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MonsterLairRStr);

        private static readonly string MonsterLairRStr = "Monster Lair";

    }

}
