#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Commands;

// Enchantment
public partial class GlobalEnchantmentSpellTokenEvent : SpellTokenEvent
{
    public GlobalEnchantmentSpellTokenEvent(CustomNode owner) : base(owner)
    { }

    // GlobalEnchantmentSpells.pas:389
    public new const int ClassId = 135954;

    public GlobalEnchantment Enchantment { get; set; }

    // GlobalEnchantmentSpells.pas:379
    public override void Dispose()
    {
        if (Enchantment != null)
        {
            Enchantment.Release();
            Enchantment = null;
        }

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

    // NetworkE.pas:230

}
