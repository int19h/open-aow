// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class LightningStormSpell : StormSpell
{
    // Storms.pas:1014
    public LightningStormSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameTables.LightningStormRStr);
        FSpellID = 7;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Air;
    }

    // Storms.pas:1024
    public override Storm CreateStorm() => new LightningStorm(null);
}
