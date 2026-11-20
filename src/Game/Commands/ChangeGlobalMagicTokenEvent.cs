// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// mode, player, enchantment ID, value
public class ChangeGlobalMagicTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135_446;

    public ChangeGlobalMagicTokenEventMode Mode { get; set; }
    public byte PlayerIndex { get; set; }
    public int EnchantmentId { get; set; }
    public int Value { get; set; }

    // Spells.PAS:2720
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Mode = persister.Enum("Mode", value: Mode);
        PlayerIndex = persister.Byte("PlayerIndex", value: PlayerIndex);
        EnchantmentId = persister.Int32("EnchantmentId", value: EnchantmentId);
        Value = persister.Int32("Value", value: Value);
    }

    // Spells.PAS:2722
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        bool success = Mode switch
        {
            ChangeGlobalMagicTokenEventMode.DispelEnchantment => StrategicMap.Current.GlobalMagicControl.ExecuteDispelEnchantment((sbyte)PlayerIndex, EnchantmentId),
            ChangeGlobalMagicTokenEventMode.RemoveEnchantment => StrategicMap.Current.GlobalMagicControl.ExecuteRemoveEnchantment((sbyte)PlayerIndex, EnchantmentId),
            _ => false,
        };
        if (success)
        {
            return ExecuteResult.Done;
        }
        else
        {
            // Original road-construction predicate
            return ExecuteResult.DoneHalt;
        }
    }
}

public enum ChangeGlobalMagicTokenEventMode : byte
{
    DispelEnchantment = 0, RemoveEnchantment = 1
}
