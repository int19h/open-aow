// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// (mode, player, values)
public partial class ChangePlayerMagicTokenEvent : TokenEvent
{
    public ChangePlayerMagicTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135445;

    public ChangePlayerMagicTokenEventMode Mode;
    public byte PlayerIndex;
    public int Value;
    private int _extraValue;

    // Spells.PAS:2448
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref Mode, 1), 1, 20);
        stream.Byte(ref PlayerIndex, 21);
        stream.Int32(ref Value, 22);
        stream.Int32(ref _extraValue, 23);
    }

    // Spells.PAS:2456
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        bool success;
        PlayerMagicControl magic;
        base.Execute(tokenEvent);
        success = false;
        magic = StrategicMap.Current.Players.GetPlayers((sbyte)PlayerIndex).MagicControl;
        switch (Mode)
        {
            case ChangePlayerMagicTokenEventMode.DispelEnchantment:
                success = magic.ExecuteDispelEnchantment(Value);
                break;
            case ChangePlayerMagicTokenEventMode.ChangePowerSettings:
                success = magic.ExecuteChangePowerSettings(Value);
                break;
            case ChangePlayerMagicTokenEventMode.ResearchSpell:
                success = magic.ExecuteResearchSpell(Value);
                break;
        }

        if (success)
        {
            result = ExecuteResult.Done;
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // NetworkE.pas:230

}

public enum ChangePlayerMagicTokenEventMode : byte
{
    DispelEnchantment = 0, ChangePowerSettings = 1, ResearchSpell = 2
}
