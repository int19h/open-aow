// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonBoar : SummonSpell
{
    // SummonSpells.pas:354
    public SummonBoar(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonBoarRStr);
        FSpellID = 31;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Earth;
        FUnitIndices.Add(235);
        FCastDuration = 5;
    }

    private static readonly string SummonBoarRStr = "Summon Boar";
}
