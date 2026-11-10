// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Session;

// turn info, units killed, units lost
public partial class PlayerStatistics : EObject
{
    // Player.PAS:182
    public PlayerStatistics(ECustomNode owner) : base(owner)
    {
        FTurnInfo = new TurnInfo(null);
    }

    public TurnInfo FTurnInfo;
    public int FUnitsKilled;
    public int FUnitsLost;

    // Player.PAS:188
    public override void Dispose()
    {
        FTurnInfo?.Dispose();
        base.Dispose();
    }

    // Player.PAS:194
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FTurnInfo, 20);
        Stream.rwInteger(ref FUnitsKilled, 21);
        Stream.rwInteger(ref FUnitsLost, 22);
    }

    // intf_P1_03.inc:133

}
