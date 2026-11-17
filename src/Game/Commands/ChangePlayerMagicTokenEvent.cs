// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (mode, player, values)
public partial class ChangePlayerMagicTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135445;

    public ChangePlayerMagicTokenEventMode Mode { get; set; }
    public byte PlayerIndex { get; set; }
    public int Value { get; set; }

    private int _extraValue;

    // Spells.PAS:2448
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Mode = persister.Enum("Mode", value: Mode);
        PlayerIndex = persister.Byte("PlayerIndex", value: PlayerIndex);
        Value = persister.Int32("Value", value: Value);
        _extraValue = persister.Int32("ExtraValue", value: _extraValue);
    }

    // Spells.PAS:2456
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        bool success;
        PlayerMagicControl magic;
        _ = base.Execute(tokenEvent);
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

        result = success ? ExecuteResult.Done : ExecuteResult.DoneHalt;

        return result;
    }

    // NetworkE.pas:230

}

public enum ChangePlayerMagicTokenEventMode : byte
{
    DispelEnchantment = 0, ChangePowerSettings = 1, ResearchSpell = 2
}
