#nullable disable warnings
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public class Disjunction : Spell
{
    // GlobalSpells.pas:931
    public Disjunction(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_disjunctionText);
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
        for (int i = 0, last = GetEngine().StrategicMap.GlobalMagicControl.Enchantments.Count - 1; i <= last; i++)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (strategicMap.GlobalMagicControl.Enchantments.GetEnchantments(i).PlayerIndex != source.Player && strategicMap.GlobalMagicControl.Enchantments.GetEnchantments(i).VisibleForPlayer(source.Player) && GetEngine().StrategicMap.GlobalMagicControl.Enchantments.GetEnchantments(i).CanDispel(source.Player, ref reason))
            {
                count += 1;
            }
        }
        if (count == 0)
        {
            messageText = AbilityRules.TranslateResourceString(GetEngine(), S: s_noGlobalOpponentEnchantmentsActiveText);
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
        if (CanActivate(source, messageText: ref reason))
        {
            DisjunctionSpellCaster caster = new(null)
            {
                Holder = CreationHolder(),
                CasterSpellId = SpellId,
                CasterSourceId = source.UnitId
            };
            GetEngine().LocalInput.ChooseSpellTargets(caster);
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
