// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class PestilenceStormSpell : StormSpell
{
    // Storms.pas:999
    public PestilenceStormSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(PestilenceRStr);
        FSpellID = 9;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Death;
    }

    // Storms.pas:1009
    public override Storm CreateStorm() => new PestilenceStorm(null);

    private static readonly string PestilenceRStr = "Pestilence";
}
