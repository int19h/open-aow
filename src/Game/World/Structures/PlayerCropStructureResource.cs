// Translated from Crops.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

// CropLocations
public partial class PlayerCropStructureResource : PlayerStructureResource
{
    public PlayerCropStructureResource(ECustomNode owner) : base(owner)
    { }

    // Crops.pas:599
    public new const int ClassId = 131919;

    public UInt16List CropLocations { get; private set; }

    // Crops.pas:592
    public override void Dispose()
    {
        CropLocations?.Dispose();
        CropLocations = null;
        base.Dispose();
    }

    // Crops.pas:605
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            if (CropLocations != null)
            {
                CropLocations?.Dispose();
            }

            CropLocations = TerrainList.CreateSurroundingHexNumberTable();
        }
    }

    // Crops.pas:602
    public override int ResourceUserClassId() => 131918;

}
