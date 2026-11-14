// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// UnitGraphicsResourceIndex
public partial class TroopResource : EngineResource
{
    public TroopResource(ECustomNode owner) : base(owner)
    { }

    public int UnitGraphicsResourceIndex { get; private set; }

    // UnitRes.PAS:37
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        UnitGraphicsResourceIndex = persister.Int32("UnitGraphicsResourceIndex", value: UnitGraphicsResourceIndex);
    }

    // UnitRes.PAS:66
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65541)
        {
            EditDropMessage(ref message);
        }

        if (message.MessageId == 65621)
        {
            message.Result = 8;
        }
    }

    // UnitRes.PAS:29
    public override LibraryImage GetCellImage() => GetPreviewImage();

    // UnitRes.PAS:44
    public virtual bool DropUnit(int hexX, int hexY, int hexLevel) => false;

    // UnitRes.PAS:19
    public virtual LibraryImage GetPreviewImage()
    {
        UnitGraphicsResource resource;
        resource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(UnitGraphicsResourceIndex);
        if (resource != null)
        {
            return resource.GetCellImage();
        }

        return null;
    }

    // UnitRes.PAS:9
    public virtual LibraryImage GetFace()
    {
        UnitGraphicsResource resource;
        resource = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(UnitGraphicsResourceIndex);
        if (resource != null)
        {
            return resource.GetFaceImage();
        }

        return null;
    }

    // UnitRes.PAS:5
    public virtual int GetObtainValue() => 0;

    // UnitRes.PAS:33
    public void SetUnitGraphicsResourceIndex(int value)
    {
        UnitGraphicsResourceIndex = value;
    }

    // UnitRes.PAS:48
    public void EditDropMessage(ref EngineMessage message)
    {
        MapContainer map;
        int hexLevel;
        int hexX = default;
        int hexY = default;
        map = message.Param[0].As<MapContainer>();
        hexLevel = message.Data[1];
        HexGeometry.PixelToHex(message.Payload.Int, message.Data[0], out hexX, out hexY);
        if (hexX >= 0 && hexY >= 0 && hexX < map.Width && hexY < map.Height && DropUnit(hexX, hexY, hexLevel))
        {
            message.Result = 0;
        }
        else
        {
            message.Result = -1;
        }
    }

    // UnitRes.PAS:62
    public override string ResourceEditName() => "Unit";

}
