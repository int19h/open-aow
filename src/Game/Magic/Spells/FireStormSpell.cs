// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class FireStormSpell : StormSpell
{
    // Storms.pas:939
    public FireStormSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameTables.FireStormRStr);
        FSpellID = 3;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Fire;
    }

    // Storms.pas:949
    public override Storm CreateStorm() => new FireStorm(null);
}
