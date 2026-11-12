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

    public TurnInfo TurnInfo;
    public int UnitsKilled;
    public int UnitsLost;

    // Player.PAS:188
    public override void Dispose()
    {
        TurnInfo?.Dispose();
        base.Dispose();
    }

    // Player.PAS:194
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(TurnInfo, 20);
        stream.rwInteger(ref UnitsKilled, 21);
        stream.rwInteger(ref UnitsLost, 22);
    }

    // intf_P1_03.inc:133

}
