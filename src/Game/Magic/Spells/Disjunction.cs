#nullable disable
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class Disjunction : Spell
{
    // GlobalSpells.pas:931
    public Disjunction(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_disjunctionText);
        SpellId = 51;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:942
    public override bool CanActivate(Troop source, ref string messageText)
    {
        string reason = "";
        int count = 0;
        for (int i = 0, last = StrategicMap.Current.GlobalMagicControl.Enchantments.Count - 1; i <= last; i++)
        {
            if (StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(i).PlayerIndex != source.Player && StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(i).VisibleForPlayer(source.Player) && StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(i).CanDispel(source.Player, ref reason))
            {
                count += 1;
            }
        }
        if (count == 0)
        {
            messageText = AbilityRules.TranslateResourceString(GetEngine(), s_noGlobalOpponentEnchantmentsActiveText);
            return false;
        }
        else
        {
            return true;
        }
    }

    // GlobalSpells.pas:962
    public override bool Activate(Troop source)
    {
        string reason = "";
        if (CanActivate(source, ref reason))
        {
            DisjunctionSpellCaster caster = new(null)
            {
                Holder = CreationHolder(),
                CasterSpellId = SpellId,
                CasterSourceId = source.UnitId
            };
            StrategicMap.Current.CosmeticSurgeryEventsStore.Raise(caster);
            _ = caster.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_disjunctionText = "Disjunction";
    private static readonly string s_noGlobalOpponentEnchantmentsActiveText = "No global opponent enchantments active";
}
