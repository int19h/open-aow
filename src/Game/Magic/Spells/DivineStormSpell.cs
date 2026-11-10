// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class DivineStormSpell : StormSpell
{
    // Storms.pas:954
    public DivineStormSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameTables.DivineStormRStr);
        FSpellID = 6;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Life;
    }

    // Storms.pas:964
    public override Storm CreateStorm() => new DivineStorm(null);
}
