// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Commands;

// Enchantment
public partial class GlobalEnchantmentSpellTokenEvent : SpellTokenEvent
{
    public GlobalEnchantmentSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalEnchantmentSpells.pas:389
    public new const int ClassId = 135954;

    public GlobalEnchantment Enchantment;

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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwCreateEObject(ref Enchantment, null, 25);
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
