// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// (mode, player, values)
public partial class ChangePlayerMagicTE : TokenEvent
{
    public ChangePlayerMagicTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135445;

    public ChangePlayerMagicMode FMode;
    public byte FPlayerIndex;
    public int FValue;
    public int FExtraValue;

    // Spells.PAS:2448
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FMode, 1), 1, 20);
        Stream.rwByte(ref FPlayerIndex, 21);
        Stream.rwInteger(ref FValue, 22);
        Stream.rwInteger(ref FExtraValue, 23);
    }

    // Spells.PAS:2456
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        ExecuteResult Result;
        bool Success;
        PlayerMagicControl Magic;
        base.Execute(TE);
        Success = false;
        Magic = AoWHSMap.Current.FPlayers.GetPlayers((sbyte)FPlayerIndex).FMagicControl;
        switch (FMode)
        {
            case ChangePlayerMagicMode.DispelEnchantment:
                Success = Magic.ExecuteDispelEnchantment(FValue);
                break;
            case ChangePlayerMagicMode.ChangePowerSettings:
                Success = Magic.ExecuteChangePowerSettings(FValue);
                break;
            case ChangePlayerMagicMode.ResearchSpell:
                Success = Magic.ExecuteResearchSpell(FValue);
                break;
        }

        if (Success)
        {
            Result = ExecuteResult.Done;
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    public enum ChangePlayerMagicMode : byte
    {
        DispelEnchantment = 0, ChangePowerSettings = 1, ResearchSpell = 2
    }

    // NetworkE.pas:230

}
