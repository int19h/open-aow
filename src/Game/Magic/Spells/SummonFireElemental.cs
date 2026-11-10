// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonFireElemental : SummonSpell
{
    // SummonSpells.pas:392
    public SummonFireElemental(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonFireElementalRStr);
        FSpellID = 29;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Fire;
        FUnitIndices.Add(226);
        FCastDuration = 40;
        FAIBasePriority = 500;
    }

    private static readonly string SummonFireElementalRStr = "Summon Fire Elemental";
}
