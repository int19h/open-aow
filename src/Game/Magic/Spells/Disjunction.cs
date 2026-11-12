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
        SpellName = AbilityRules.TranslateResourceString(s_disjunctionText);
        SpellId = 51;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:942
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool result;
        int count;
        string reason = "";
        count = 0;
        for (int I = 0, last = StrategicMap.Current.GlobalMagicControl.Enchantments.Count - 1; I <= last; I++)
        {
            if (StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(I).PlayerIndex != source.Player && StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(I).VisibleForPlayer(source.Player) && StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(I).CanDispel(source.Player, ref reason))
            {
                count += 1;
            }
        }

        if (count == 0)
        {
            messageText = AbilityRules.TranslateResourceString(s_noGlobalOpponentEnchantmentsActiveText);
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // GlobalSpells.pas:962
    public override bool Activate(Troop source)
    {
        bool result;
        string reason = "";
        DisjunctionSpellCaster caster;
        if (CanActivate(source, ref reason))
        {
            caster = new DisjunctionSpellCaster(null);
            caster.CasterSpellId = SpellId;
            caster.CasterSourceId = source.UnitId;
            StrategicMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
            caster.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_disjunctionText = "Disjunction";
    private static readonly string s_noGlobalOpponentEnchantmentsActiveText = "No global opponent enchantments active";
}
