// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Session;

// turn info, units killed, units lost
public partial class PlayerStatistics : EngineObject
{
    // Player.PAS:182
    public PlayerStatistics(ECustomNode owner) : base(owner)
    {
        TurnInfo = new TurnInfo(null);
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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Object(TurnInfo, 20);
        stream.Int32(ref _unitsKilled, 21);
        stream.Int32(ref _unitsLost, 22);
    }

    // intf_P1_03.inc:133

}
