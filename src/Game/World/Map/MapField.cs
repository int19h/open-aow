// Translated from HSEngine.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Map;

// FHSList, FCapacity, FCount, FStaticCount, FStatus, FXhx, FYhx, FLhx, FReserved, FTerrainType,
// FOverlayType. A hexagon field of a map level. The original pooled all fields of a level in
// one block and freed it with the level. The port allocates each field through the base
// allocator. The sprites are sorted by their show priority. The first FStaticCount (priority
// shr 26 <= 15) are the static ones
public partial class MapField : ECustomNode
{
    // HSEngine.pas:1933
    public new const int ClassID = 65793;

    public MapField(ECustomNode owner) : base(owner)
    {
        FTerrainType = -1;
        FOverlayType = -1;
        FStatus = Set<MapFieldStatusElement>.Of();
        Resize(3);
    }

    internal HexagonSprite[] FHSList = [];
    internal byte FCapacity;
    internal byte FCount;
    internal byte FStaticCount;
    internal Set<MapFieldStatusElement> FStatus;
    internal sbyte FXhx;
    internal sbyte FYhx;
    internal sbyte FLhx;
    internal sbyte FTerrainType;
    internal sbyte FOverlayType;

    // HSEngine.pas:1907
    public override void Dispose()
    {
        Array.Resize(ref FHSList, 0);
        FCount = 0;
    }

    // HSEngine.pas:1928
    public override Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of(EngineServices.csStored, EngineServices.csShared);

    // HSEngine.pas:1923
    public override void SetOwner(ECustomNode AOwner)
    {
        FOwner = AOwner;
    }

    // HSEngine.pas:2029
    public override void ReadWrite(EStorageStream Stream)
    {
        int J;
        int I;
        EObject HS = default;
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(EngineServices.smText)) == Set<StorageModeElement>.Of())
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                if (Stream.StorageMode().Contains(StorageModeElement.Compare))
                {
                    J = 0;
                    I = Stream.PropertyTable.First;
                    int last = Stream.PropertyTable.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        if (Stream.PropertyTable.Items[I].ID >= 50)
                        {
                            HS = FHSList[J];
                            Stream.rwCreateEObject(ref HS, this, Stream.PropertyTable.Items[I].ID);
                            J += 1;
                        }
                    }
                }
                else
                {
                    I = Stream.PropertyTable.First;
                    int last = Stream.PropertyTable.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        J = Stream.PropertyTable.Items[I].ID;
                        if (J >= 50)
                        {
                            Stream.rwCreateEObject(ref HS, this, J);
                        }
                    }
                }
            }
            else
            {
                I = 50;
                J = 0;
                int last = FCount - 1;
                for (; J <= last; J += 1)
                {
                    HS = FHSList[J];
                    if (HS != null && HS.Owner == this && !HS.GetControlStyle().Contains(EngineServices.csShared) && HS.GetControlStyle().Contains(EngineServices.csStored))
                    {
                        Stream.rwCreateEObject(ref HS, this, I);
                        I += 1;
                    }
                }
            }
        }
        else
        {
            ReadWriteChildren(Stream);
        }
    }

    // HSEngine.pas:2075
    public override void MainReadWrite(EStorageStream Stream)
    {
        ReadWrite(Stream);
    }

    // HSEngine.pas:1898

    // HSEngine.pas:1943
    public override EObject GetChild(int Index)
    {
        if (Index >= FCount)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        return FHSList[Index];
    }

    // HSEngine.pas:1949
    public override void SetChild(int Index, EObject Child)
    {
        if (Index >= FCount)
        {
            throw new IndexOutOfRangeException("Index out of bounds");
        }

        FHSList[Index] = (HexagonSprite)Child;
    }

    // HSEngine.pas:1972
    public override int GetCount() => FCount;

    // HSEngine.pas:2226
    public override int RemoveChild(EObject Child) => RemoveHS((HexagonSprite)Child);

    // HSEngine.pas:2221
    public override int AddChild(EObject Child) => AddHS((HexagonSprite)Child, 0);

    // HSEngine.pas:2154
    public override EObject FindChild(int AClassID)
    {
        EObject Result;
        int I;
        I = 0;
        while (I < FCount && FHSList[I].EClass.ClassID != AClassID)
        {
            I += 1;
        }

        if (I < FCount)
        {
            Result = FHSList[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // HSEngine.pas:2211
    public override EObject FindOwnedChild(int AClassID)
    {
        EObject Result;
        int I;
        I = 0;
        while (I < FCount && !(FHSList[I].EClass.ClassID == AClassID && FHSList[I].Owner == this))
        {
            I += 1;
        }

        if (I < FCount)
        {
            Result = FHSList[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // HSEngine.pas:1938
    public int GetXYL() => FXhx + (FYhx << 8) + (FLhx << 16);

    // HSEngine.pas:1955
    public int HSListDataSize() => (FCapacity << 2) + ((FCapacity + 3) & 252);

    // HSEngine.pas:2163
    public HexagonSprite FindHS(Type AClass)
    {
        HexagonSprite Result;
        int I;
        I = 0;
        while (I < FCount && !AClass.IsInstanceOfType(FHSList[I]))
        {
            I += 1;
        }

        if (I < FCount)
        {
            Result = FHSList[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // HSEngine.pas:2172
    public HexagonSprite FindOwnedHS(Type AClass)
    {
        int I;
        HexagonSprite HS;
        I = 0;
        HexagonSprite Result = null;
        while (I < FCount && Result == null)
        {
            HS = FHSList[I];
            if (AClass.IsInstanceOfType(HS) && HS.Owner == this)
            {
                Result = HS;
            }

            I += 1;
        }

        return Result;
    }

    // HSEngine.pas:2300
    public void CopyDynamicVisibleHS(VisibilityList List)
    {
        byte last = FStaticCount;
        for (int I = FCount - 1; I >= last; I--)
        {
            FHSList[I].MakeVisible(List);
        }
    }

    // HSEngine.pas:2308
    public void CopyStaticVisibleHS(VisibilityList List)
    {
        for (int I = FStaticCount - 1; I >= 0; I--)
        {
            FHSList[I].MakeVisible(List);
        }
    }

    // HSEngine.pas:2316
    public void ResetDynamicVisibleHS()
    {
        byte last = FStaticCount;
        for (int I = FCount - 1; I >= last; I--)
        {
            FHSList[I].SetVisible(false);
        }
    }

    // HSEngine.pas:2324
    public void ResetStaticVisibleHS()
    {
        for (int I = FStaticCount - 1; I >= 0; I--)
        {
            FHSList[I].SetVisible(false);
        }
    }

    // HSEngine.pas:2335
    // { The sprites with a level from MinLevel to MaxLevel that a map field owns are destroyed; the list
    //   is sorted, so the walk stops at the first sprite below MinLevel. The HP position is computed and
    //   never used. }
    public void DestroyLevelHS(int MinLevel, int MaxLevel)
    {
        int Xhp = default;
        int Yhp = default;
        bool Done;
        int I;
        int Level;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        I = FCount - 1;
        // { Preserve the original stack slot. }
        Done = false;
        while (!Done && I >= 0)
        {
            Level = FHSList[I].GetLevel();
            if (Level <= MaxLevel)
            {
                if (Level >= MinLevel)
                {
                    if (FHSList[I].Owner is MapField)
                    {
                        FHSList[I].Dispose();
                    }
                }
                else
                {
                    Done = true;
                }
            }

            I -= 1;
            if (I >= FCount)
            {
                I = FCount - 1;
            }
        }
    }

    // HSEngine.pas:2360
    public void DestroyHS()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (I <= FCount - 1)
            {
                if (FHSList[I].Owner == this)
                {
                    FHSList[I].Dispose();
                }
            }
        }
    }

    // HSEngine.pas:2369
    public void Activate()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (FHSList[I].Owner == this)
            {
                FHSList[I].Activate();
            }
        }
    }

    // HSEngine.pas:2581
    public sbyte GetDecorationType()
    {
        MapFieldMsg Msg = default;
        Msg.MsgID = 17;
        Msg.Result = -1;
        SendMapFieldMsg(ref Msg);
        return (sbyte)Msg.Result.Int;
    }

    // HSEngine.pas:2591
    public void UpdateNeighbourTerrain()
    {
        HSTerrainInfo Info = default;
        HNWalkData Walk = default;
        Info.MapField = this;
        Info.TerrainType = FTerrainType;
        Info.OverlayType = FOverlayType;
        HNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 0);
        for (int I = 1; I <= 6; I++)
        {
            HNWalkData.WalkToNextHN(ref Walk);
            if (Walk.X >= 0 && Walk.X < ((MapLevel)Owner).FWidth && Walk.Y >= 0 && ((MapLevel)Owner).FHeight > Walk.Y)
            {
                Info.HN = (sbyte)OppositeHTTable[I - 1];
                ((MapLevel)Owner).GetField(Walk.X, Walk.Y).NeighbourTerrainChanged(ref Info);
            }
        }
    }

    public HexagonSprite[] HSList { get => FHSList; }
    public byte Capacity { get => FCapacity; }
    public byte StaticCount { get => FStaticCount; }
    public Set<MapFieldStatusElement> Status { get => FStatus; set => FStatus = value; }
    public sbyte Xhx { get => FXhx; set => FXhx = value; }
    public sbyte Yhx { get => FYhx; set => FYhx = value; }
    public sbyte Lhx { get => FLhx; set => FLhx = value; }
    public int XYL { get => GetXYL(); }
    public sbyte TerrainType { get => FTerrainType; set => FTerrainType = value; }
    public sbyte OverlayType { get => FOverlayType; set => FOverlayType = value; }

    public sealed class HNAccessor(MapField target)
    {
        public byte this[int Index] { get => target.GetHN(Index); set => target.SetHN(Index, value); }
    }

    public HNAccessor HN => new HNAccessor(this);

    public override void SendMsg(ref EMsg message, EObject sender)
    {
        if ((message.MsgID & 0x10000000) != 0)
        {
            for (int i = FCount - 1; i >= 0; i--)
            {
                var sprite = FHSList[i];
                if (sprite.GetLastMsg() == message.Seq) continue;
                if ((message.MsgID & 0x20000000) != 0 && sprite.Owner != this) continue;
                sprite.SetLastMsg(message.Seq);
                sprite.MsgProc(ref message, this);
            }
        }
        else if ((message.MsgID & 0x20000000) == 0)
        {
            for (int i = Count - 1; i >= 0; i--) FHSList[i].MsgProc(ref message, this);
        }
        else
        {
            int count = FCount;
            for (int i = 0; i < count; i++) SendMsgList[i] = FHSList[i];
            for (int i = count - 1; i >= 0; i--)
                if (SendMsgList[i].Owner == this) SendMsgList[i].MsgProc(ref message, this);
        }
    }

    private static FixedArray_byte_6 OppositeHTTable = FixedArray_byte_6.From((ReadOnlySpan<byte>)new byte[6] { 4, 5, 6, 1, 2, 3 });
    private static FixedArray_Aow_Units_HSEngine_THexagonSprite_256 SendMsgList;
    private static FixedArray_Aow_Units_HSEngine_THexagonSprite_256 LoadedList;
}
