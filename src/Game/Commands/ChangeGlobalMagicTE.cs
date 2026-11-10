// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// mode, player, enchantment ID, value
public partial class ChangeGlobalMagicTE : TokenEvent
{
    public ChangeGlobalMagicTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135446;

    public ChangeGlobalMagicMode FMode;
    public byte FPlayerIndex;
    public int FEnchantmentID;
    public int FValue;

    // Spells.PAS:2720
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FMode, 1), 1, 20);
        Stream.rwByte(ref FPlayerIndex, 21);
        Stream.rwInteger(ref FEnchantmentID, 22);
        Stream.rwInteger(ref FValue, 23);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Enum("Mode", ref FMode);
        persister.Byte("PlayerIndex", ref FPlayerIndex);
        persister.Int("EnchantmentID", ref FEnchantmentID);
        persister.Int("Value", ref FValue);
    }

    // Spells.PAS:2722
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        ExecuteResult Result;
        bool Success;
        base.Execute(TE);
        Success = false;
        switch (FMode)
        {
            case ChangeGlobalMagicMode.DispelEnchantment:
                Success = AoWHSMap.Current.FGlobalMagicControl.ExecuteDispelEnchantment((sbyte)FPlayerIndex, FEnchantmentID);
                break;
            case ChangeGlobalMagicMode.RemoveEnchantment:
                Success = AoWHSMap.Current.FGlobalMagicControl.ExecuteRemoveEnchantment((sbyte)FPlayerIndex, FEnchantmentID);
                break;
        }

        if (Success)
        {
            Result = ExecuteResult.Done;
        }
        else
        {
            // Original road-construction predicate
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    public enum ChangeGlobalMagicMode : byte
    {
        DispelEnchantment = 0, RemoveEnchantment = 1
    }

    // NetworkE.pas:230

}
