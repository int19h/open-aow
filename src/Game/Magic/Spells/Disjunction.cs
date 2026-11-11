// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class Disjunction : Spell
{
    // GlobalSpells.pas:931
    public Disjunction(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(DisjunctionRStr);
        FSpellID = 51;
        FSpellType = SpellType.Global;
        FBaseManaCost = 0;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:942
    public override bool CanActivate(Troop Source, ref string MessageText)
    {
        bool Result;
        int Count;
        string Reason = "";
        Count = 0;
        for (int I = 0, last = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(I).FPlayerIndex != Source.FPlayer && AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(I).VisibleForPlayer(Source.FPlayer) && AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(I).CanDispel(Source.FPlayer, ref Reason))
            {
                Count += 1;
            }
        }

        if (Count == 0)
        {
            MessageText = AbilityRules.TranslateRStr(NoGlobalOpponentEnchantmentsActiveRStr);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalSpells.pas:962
    public override bool Activate(Troop Source)
    {
        bool Result;
        string Reason = "";
        DisjunctionSpellCaster Caster;
        if (CanActivate(Source, ref Reason))
        {
            Caster = new DisjunctionSpellCaster(null);
            Caster.FCasterSpellID = FSpellID;
            Caster.FCasterSourceID = Source.FUnitID;
            AoWHSMap.Current.FCosmeticSurgeryEventsStore.Raise(Caster);
            Caster.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string DisjunctionRStr = "Disjunction";
    private static readonly string NoGlobalOpponentEnchantmentsActiveRStr = "No global opponent enchantments active";
}
