// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class DivineStormSpell : StormSpell
{
    // Storms.pas:954
    public DivineStormSpell(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameTables.DivineStormText);
        SpellId = 6;
        BaseManaCost = 5;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Life;
    }

    // Storms.pas:964
    public override Storm CreateStorm() => new DivineStorm(null) { Holder = CreationHolder() };
}
