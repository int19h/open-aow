// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SummonAirElemental : SummonSpell
{
    // SummonSpells.pas:366
    public SummonAirElemental(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(SummonAirElementalRStr);
        FSpellID = 27;
        FBaseManaCost = 5;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Air;
        FUnitIndices.Add(224);
        FCastDuration = 40;
        FAIBasePriority = 750;
    }

    private static readonly string SummonAirElementalRStr = "Summon Air Elemental";
}
