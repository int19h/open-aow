// Translated from Crops.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// FCropLocations
public partial class PlayerCropStructureResource : PlayerStructureResource
{
    public PlayerCropStructureResource(ECustomNode owner) : base(owner)
    { }

    // Crops.pas:599
    public new const int ClassID = 131919;

    public WordList FCropLocations;

    // Crops.pas:592
    public override void Dispose()
    {
        FCropLocations?.Dispose();
        FCropLocations = null;
        base.Dispose();
    }

    // Crops.pas:605
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            if (FCropLocations != null)
            {
                FCropLocations?.Dispose();
            }

            FCropLocations = TerrainList.CreateSurroundingHNTable();
        }
    }

    // Crops.pas:602
    public override int ResourceUserClassID() => 131918;

}
