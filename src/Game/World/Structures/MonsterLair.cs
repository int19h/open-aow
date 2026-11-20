// Translated from MonsterL.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class MonsterLair(CustomNode owner) : ItemExplorationSite(owner)
{

    // MonsterL.pas:34
    public new const int ClassId = 131_886;

    public new class Resource(CustomNode owner) : ItemExplorationSiteResource(owner)
    {

        // MonsterL.pas:40
        public new const int ClassId = 131_887;

        // MonsterL.pas:43
        public override int ResourceUserClassId() => MonsterLair.ClassId;

        // MonsterL.pas:37
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_monsterLairText);

        private static readonly string s_monsterLairText = "Monster Lair";

    }
}
