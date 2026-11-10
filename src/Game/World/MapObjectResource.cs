// Translated from HSEngine.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World;

// the resource of map objects. FEngine (set by the engine activation message),
// FShowPriorityOffset (id 4), FImageLibrary (id 8), FImageSequenceList (nil without complex
// images. ids 9, 10), FTerrainList (id 7), FLevel (id 5), FName (id 6)
public partial class MapObjectResource : EResource
{
    public new const int ClassID = 65876;

    public MapObjectResource(ECustomNode owner) : base(owner)
    {
        FTerrainList = new TerrainList();
        FImageLibrary = new ImageLibrary(null);
        SetLevel(26);
    }

    protected internal GameEngine FEngine;
    protected internal int FShowPriorityOffset;
    protected internal ImageLibrary FImageLibrary;
    protected internal ImageSequenceList FImageSequenceList;
    protected internal TerrainList FTerrainList;
    protected internal byte FLevel;
    protected internal string FName = "";

    // MAPOBJ.PAS:391
    public override void Dispose()
    {
        FTerrainList?.Dispose();
        FImageLibrary?.Dispose();
        if (FImageSequenceList != null)
        {
            FImageSequenceList?.Dispose();
        }

        base.Dispose();
    }

    // MAPOBJ.PAS:405
    public override int ClassVersion() => 1;

    // MAPOBJ.PAS:400
    public override GameEngine GetEngine() => FEngine;

    // MAPOBJ.PAS:470
    public override void ReadWrite(EStorageStream Stream)
    {
        bool Complex = default;
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FShowPriorityOffset, 4);
        Stream.rwByte(ref FLevel, 5);
        Stream.rwString(ref FName, 6);
        Stream.rwStreamProc(7, FTerrainList.LoadFromStream, FTerrainList.SaveToStream);
        FImageLibrary.ReduceCount();
        Stream.rwStreamProc(8, FImageLibrary.LoadFromStream, FImageLibrary.SaveToStream);
        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                Stream.rwBool(ref Complex, 9);
                if (Complex)
                {
                    if (FImageSequenceList == null)
                    {
                        FImageSequenceList = new ImageSequenceList(null, FImageLibrary);
                    }
                }
                else
                {
                    FImageSequenceList?.Dispose();
                    FImageSequenceList = null;
                }
            }
            else
            {
                Complex = FImageSequenceList != null;
                Stream.rwBool(ref Complex, 9);
            }

            if (Complex)
            {
                Stream.rwStreamProc(10, FImageSequenceList.LoadFromStream, FImageSequenceList.SaveToStream);
            }
        }
    }

    // MAPOBJ.PAS:383

    // MAPOBJ.PAS:620
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            FEngine = Msg.Payload.As<GameEngine>();
            if (FImageSequenceList != null)
            {
                FImageSequenceList.FILManager = FEngine.FHSSet.FImageLibraryManager;
                FImageSequenceList.LinkToIL();
            }
        }
        else
        {
            if (Msg.MsgID == 65541)
            {
                EditDropMsg(ref Msg);
            }
            else
            {
                if (Msg.MsgID == 65621)
                {
                    Msg.Result = 8;
                }
            }
        }
    }

    // MAPOBJ.PAS:450
    public override LibraryImage GetCellImage()
    {
        int I;
        I = 0;
        LibraryImage Result = null;
        while (I != FImageLibrary.Count && Result == null)
        {
            Result = FImageLibrary.Get(I);
            I += 1;
        }

        if (Result == null && FImageSequenceList != null)
        {
            Result = FImageSequenceList.GetPreviewImage();
        }

        return Result;
    }

    // MAPOBJ.PAS:465
    public override void SetCellImage(LibraryImage Value)
    {
        FImageLibrary.SetImage(0, Value);
    }

    // MAPOBJ.PAS:525
    public override EObject CreateResourceUser()
    {
        int ID;
        MapObject MO;
        ID = ResourceUserClassID();
        if (ID != 0)
        {
            MO = (MapObject)FEngine.CreateEObject(ID, null);
            if (MO != null)
            {
                MO.SetResource(this);
                MO.SetResourceLinked(true);
            }
        }
        else
        {
            MO = null;
        }

        return MO;
    }

    // MAPOBJ.PAS:420
    public override void Draw(Canvas Canvas, Rect Rect, Draw.Images.BltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
    {
        int I;
        LibraryImage Image;
        ImageSequence Sequence;
        Rect DrawRect = default;
        I = 0;
        Image = null;
        while (I != FImageLibrary.Count && Image == null)
        {
            Image = FImageLibrary.Get(I);
            I += 1;
        }

        if (Image == null && FImageSequenceList != null)
        {
            I = 0;
            Sequence = null;
            while (I != FImageSequenceList.Count && Sequence == null)
            {
                Sequence = FImageSequenceList.GetImageSequence(I);
                if (Sequence == null)
                {
                    I += 1;
                }
            }

            BltImageSequenceEx(Canvas, Sequence, 0, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref DrawRect);
        }
        else
        {
            LibraryDrawing.BltLibraryImage(Canvas, Image, Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref DrawRect);
        }
    }

    // MAPOBJ.PAS:550
    public virtual void SetLevel(byte Value)
    {
        FLevel = Value;
        CalcShowPriorityOffset(16, 32, FLevel);
    }

    // MAPOBJ.PAS:556
    public virtual MapObject PlaceResourceUser(HSMap HSMap, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        int Xhp = default;
        int Yhp = default;
        MapObject Result = (MapObject)CreateResourceUser();
        HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
        Xhp += 16;
        Yhp += 16;
        if (!HSMap.Place(Result, Xhp, Yhp, Lhx))
        {
            Result?.Dispose();
            Result = null;
        }

        return Result;
    }

    // MAPOBJ.PAS:545
    public virtual void CalcShowPriorityOffset(int dX, int dY, int Level)
    {
        FShowPriorityOffset = dX + ((Level << 26) + (dY << 13));
    }

    // MAPOBJ.PAS:507
    public void ShowComplexImage(int X, int Y, in SurfaceDesc SurfaceDesc, int Index, uint Offset)
    {
        ImageSequence Sequence;
        int Frame;
        if (Index < FImageSequenceList.Count)
        {
            Sequence = (ImageSequence)FImageSequenceList.List[Index];
        }
        else
        {
            Sequence = null;
        }

        while (Sequence != null)
        {
            Frame = Sequence.GetFrameIndex(FEngine.FHSMap.FFrame, (int)Offset);
            if (Frame != -1)
            {
                Sequence.ImageLibrary.Get(Frame).Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    // MAPOBJ.PAS:571
    public void ClearTerrain(int X, int Y, int Lhx)
    {
        int Xhx = default;
        int Yhx = default;
        for (int I = FTerrainList.FCount - 1; I >= 0; I--)
        {
            if (FTerrainList.GetTerrain(I) >= 0)
            {
                HexGeometry.CenterHNtoHX(X, Y, I, ref Xhx, ref Yhx);
                FEngine.FHSMap.ClearTerrain(Xhx, Yhx, Lhx, 0, -1);
            }
        }
    }

    // MAPOBJ.PAS:586
    // { The resource was dropped on the map (message $10005): fill the hexagons in Param[1] rings around
    //   the drop point with its users, clearing the terrain first when Param[2] = 1 }
    public void EditDropMsg(ref EMsg Msg)
    {
        MapLevel MapLevel;
        int Xhx = default;
        int Yhx = default;
        int Lhx;
        int Rad;
        bool Clear;
        bool Placed;
        HNWalkData Walk = default;
        Placed = true;
        MapLevel = Msg.Param[0].As<MapLevel>();
        Lhx = Msg.Data[1];
        Rad = Msg.Param[1].Int;
        HexGeometry.HPtoHX(Msg.Payload.Int, Msg.Data[0], ref Xhx, ref Yhx);
        Clear = Msg.Param[2].Int == 1;
        HNWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (Walk.Radius <= Rad)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.FWidth && Walk.Y < MapLevel.FHeight)
            {
                if (Clear)
                {
                    ClearTerrain(Walk.X, Walk.Y, Lhx);
                }

                Placed = Placed && PlaceResourceUser(FEngine.FHSMap, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)Lhx) != null;
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }

        if (Placed)
        {
            Msg.Result = 0;
        }
        else
        {
            Msg.Result = -1;
        }
    }

    public GameEngine Engine { get => FEngine; }
    public int ShowPriorityOffset { get => FShowPriorityOffset; set => FShowPriorityOffset = value; }
    public ImageLibrary ImageLibrary { get => FImageLibrary; }
    public ImageSequenceList ImageSequenceList { get => FImageSequenceList; set => FImageSequenceList = value; }
    public TerrainList TerrainList { get => FTerrainList; }
    public byte Level { get => FLevel; set => SetLevel(value); }
    public string Name { get => FName; set => FName = value; }

    // MAPOBJ.PAS:415
    public override string ResourceEditName() => "";

    internal static void BltImageSequenceEx(Canvas Canvas, ImageSequence Sequence, int Frame, Rect Rect, Draw.Images.BltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color, bool Selected, ref Rect DrawRect)
    {
        throw new NotImplementedException();
    }

}
