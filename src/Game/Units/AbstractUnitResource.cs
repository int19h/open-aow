// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// FUnitGFXResourceIndex
public partial class AbstractUnitResource : EResource
{
    public AbstractUnitResource(ECustomNode owner) : base(owner)
    { }

    public int FUnitGFXResourceIndex;

    // UnitRes.PAS:1
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // UnitRes.PAS:37
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FUnitGFXResourceIndex, 8);
    }

    // UnitRes.PAS:66
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65541)
        {
            EditDropMsg(ref Msg);
        }

        if (Msg.MsgID == 65621)
        {
            Msg.Result = 8;
        }
    }

    // UnitRes.PAS:29
    public override LibraryImage GetCellImage() => GetPreviewImage();

    // UnitRes.PAS:44
    public virtual bool DropUnit(int Xhx, int Yhx, int Lhx) => false;

    // UnitRes.PAS:19
    public virtual LibraryImage GetPreviewImage()
    {
        UnitGFXResource Resource;
        Resource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(FUnitGFXResourceIndex);
        if (Resource != null)
        {
            return Resource.GetCellImage();
        }

        return null;
    }

    // UnitRes.PAS:9
    public virtual LibraryImage GetFace()
    {
        UnitGFXResource Resource;
        Resource = AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(FUnitGFXResourceIndex);
        if (Resource != null)
        {
            return Resource.GetFaceImage();
        }

        return null;
    }

    // UnitRes.PAS:5
    public virtual int GetObtainValue() => 0;

    // UnitRes.PAS:33
    public void SetUnitGFXResourceIndex(int Value)
    {
        FUnitGFXResourceIndex = Value;
    }

    // UnitRes.PAS:48
    public void EditDropMsg(ref EMsg Msg)
    {
        MapContainer Map;
        int Lhx;
        int Xhx = default;
        int Yhx = default;
        Map = Msg.Param[0].As<MapContainer>();
        Lhx = Msg.Data[1];
        HexGeometry.HPtoHX(Msg.Payload.Int, Msg.Data[0], ref Xhx, ref Yhx);
        if (Xhx >= 0 && Yhx >= 0 && Xhx < Map.Width && Yhx < Map.Height && DropUnit(Xhx, Yhx, Lhx))
        {
            Msg.Result = 0;
        }
        else
        {
            Msg.Result = -1;
        }
    }

    // UnitRes.PAS:62
    public override string ResourceEditName() => "Unit";

}
