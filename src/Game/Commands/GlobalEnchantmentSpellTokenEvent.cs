#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Commands;

// Enchantment
public class GlobalEnchantmentSpellTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // GlobalEnchantmentSpells.pas:389
    public new const int ClassId = 135_954;

    public GlobalEnchantment Enchantment { get; set; }

    // GlobalEnchantmentSpells.pas:379
    public override void Dispose()
    {
        _ = (Enchantment?.Release());
        Enchantment = null;

        base.Dispose();
    }

    // GlobalEnchantmentSpells.pas:394
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Enchantment = persister.Created("Enchantment", objectValue: Enchantment);
    }

    // GlobalEnchantmentSpells.pas:400
    public override bool Validate(Hero source)
    {
        string reason = "";
        bool result = base.Validate(source);
        return result && Enchantment.CanCast(source.Player, ref reason);
    }

    // GlobalEnchantmentSpells.pas:408
    public override bool Process(Troop source, int value) => base.Process(source, value);
}
