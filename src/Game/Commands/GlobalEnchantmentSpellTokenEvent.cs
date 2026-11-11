// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Commands;

// FEnchantment
public partial class GlobalEnchantmentSpellTokenEvent : SpellTokenEvent
{
    public GlobalEnchantmentSpellTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalEnchantmentSpells.pas:389
    public new const int ClassID = 135954;

    public GlobalEnchantment FEnchantment;

    // GlobalEnchantmentSpells.pas:379
    public override void Dispose()
    {
        if (FEnchantment != null)
        {
            FEnchantment.Release();
            FEnchantment = null;
        }

        base.Dispose();
    }

    // GlobalEnchantmentSpells.pas:394
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FEnchantment, null, 25);
    }

    // GlobalEnchantmentSpells.pas:400
    public override bool Validate(Hero source)
    {
        string Reason = "";
        bool Result = base.Validate(source);
        return Result && FEnchantment.CanCast(source.FPlayer, ref Reason);
    }

    // GlobalEnchantmentSpells.pas:408
    public override bool Process(Troop source, int value) => base.Process(source, value);

    // NetworkE.pas:230

}
