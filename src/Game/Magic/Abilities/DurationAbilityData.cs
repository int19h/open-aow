// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (duration, player index, ability ID, unresolved Integer)
public class DurationAbilityData(CustomNode owner) : AbilityData(owner)
{
    public new const int ClassId = 139_266;

    public int Duration { get; set; }
    public PlayerId PlayerIndex { get; set; }
    public ushort AbilityId { get; set; }
    public int Source { get; set; }

    // AoWAb.PAS:16
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Duration = persister.Byte("Duration", value: Duration.TruncateToByte());
        AbilityId = (ushort)persister.UInt16("AbilityId", value: AbilityId);
        PlayerIndex = persister.Player("PlayerIndex", value: PlayerIndex, width: IdWidth.Byte, fallback: 0);
        Source = persister.Int32("Source", value: Source);
    }

    // AoWAb.PAS:11
    public override int GetAbilityId() => AbilityId;
}
