// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class IceStormSpell : StormSpell
{
    // Storms.pas:984
    public IceStormSpell(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameTables.IceStormRStr);
        FSpellID = 4;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Air;
    }

    // Storms.pas:994
    public override Storm CreateStorm() => new IceStorm(null);
}
