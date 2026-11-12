// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// mode, player, enchantment ID, value
public partial class ChangeGlobalMagicTokenEvent : TokenEvent
{
    public ChangeGlobalMagicTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135446;

    public ChangeGlobalMagicTokenEventMode Mode;
    public byte PlayerIndex;
    public int EnchantmentId;
    public int Value;

    // Spells.PAS:2720
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwData(Bytes.Of(ref Mode, 1), 1, 20);
        stream.rwByte(ref PlayerIndex, 21);
        stream.rwInteger(ref EnchantmentId, 22);
        stream.rwInteger(ref Value, 23);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Enum("Mode", ref Mode);
        persister.Byte("PlayerIndex", ref PlayerIndex);
        persister.Int32("EnchantmentID", ref EnchantmentId);
        persister.Int32("Value", ref Value);
    }

    // Spells.PAS:2722
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        bool success;
        base.Execute(tokenEvent);
        success = false;
        switch (Mode)
        {
            case ChangeGlobalMagicTokenEventMode.DispelEnchantment:
                success = AoWHSMap.Current.GlobalMagicControl.ExecuteDispelEnchantment((sbyte)PlayerIndex, EnchantmentId);
                break;
            case ChangeGlobalMagicTokenEventMode.RemoveEnchantment:
                success = AoWHSMap.Current.GlobalMagicControl.ExecuteRemoveEnchantment((sbyte)PlayerIndex, EnchantmentId);
                break;
        }

        if (success)
        {
            result = ExecuteResult.Done;
        }
        else
        {
            // Original road-construction predicate
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // NetworkE.pas:230

}

public enum ChangeGlobalMagicTokenEventMode : byte
{
    DispelEnchantment = 0, RemoveEnchantment = 1
}
