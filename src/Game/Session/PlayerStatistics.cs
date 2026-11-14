// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Session;

// turn info, units killed, units lost
public partial class PlayerStatistics : EngineObject
{
    // Player.PAS:182
    public PlayerStatistics(ECustomNode owner) : base(owner)
    {
        TurnInfo = new TurnInfo(null) { Holder = CreationHolder() };
    }

    public TurnInfo TurnInfo { get; }

    private int _unitsKilled;
    private int _unitsLost;

    // Player.PAS:188
    public override void Dispose()
    {
        TurnInfo?.Dispose();
        base.Dispose();
    }

    // Player.PAS:194
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("TurnInfo", objectValue: TurnInfo);
        _unitsKilled = persister.Int32("UnitsKilled", value: _unitsKilled);
        _unitsLost = persister.Int32("UnitsLost", value: _unitsLost);
    }

    // intf_P1_03.inc:133

}
