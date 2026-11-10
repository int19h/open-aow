// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class DeathStormSpell : StormSpell
{
    // Storms.pas:969
    public DeathStormSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameTables.DeathStormRStr);
        FSpellID = 5;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Death;
    }

    // Storms.pas:979
    public override Storm CreateStorm() => new DeathStorm(null);
}
