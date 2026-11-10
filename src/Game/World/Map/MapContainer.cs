// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// FWidth, FHeight, FLevelCount, FMap, FEngine. The levels of a map (the children, TMapLevel)
public partial class MapContainer : ENode
{
    // HSEngine.pas:3208
    public new const int ClassID = 65795;

    internal int FWidth;
    internal int FHeight;
    internal int FLevelCount;
    internal HSMap FMap;
    internal GameEngine FEngine;

    // HSEngine.pas:3167
    public override void SetOwner(ECustomNode AOwner)
    {
        base.SetOwner(AOwner);
        FMap = (HSMap)AOwner;
    }

    // HSEngine.pas:3203
    public override GameEngine GetEngine() => FEngine;

    // HSEngine.pas:3173
    public override void ReadWrite(EStorageStream Stream)
    {
        byte W;
        byte H;
        byte L;
        base.ReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            FMap = (HSMap)Owner;
            FEngine = Stream.Engine;
            FMap.FMapContainer = this;
        }

        W = (byte)FWidth;
        Stream.rwByte(ref W, 3);
        FWidth = W;
        H = (byte)FHeight;
        Stream.rwByte(ref H, 4);
        FHeight = H;
        L = (byte)FLevelCount;
        Stream.rwByte(ref L, 5);
        FLevelCount = L;
    }

    // HSEngine.pas:3197
    public override void MainReadWrite(EStorageStream Stream)
    {
        if (!((HSMap)Owner).FStatus.Contains(HSMapStatusElement.PreviewMode))
        {
            base.MainReadWrite(Stream);
        }
        else
        {
            ReadWrite(Stream);
        }
    }

    // HSEngine.pas:3162
    public MapContainer(ECustomNode owner) : base(owner)
    {
    }

    // HSEngine.pas:3241
    public override void Clear()
    {
        base.Clear();
        FWidth = 0;
        FHeight = 0;
        FLevelCount = 0;
    }

    // HSEngine.pas:3213
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        if (Msg.MsgID == 536870920)
        {
            FEngine = Msg.Payload.As<GameEngine>();
        }

        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            UpdateTerrain();
            Activate();
        }
    }

    // HSEngine.pas:3249
    public virtual void RemoveLevel(int Index)
    {
        if (Index >= 0 && Index < FChildren.Count)
        {
            ((MapLevel)FChildren.List[Index]).DestroyHS();
            ((MapLevel)FChildren.List[Index])?.Dispose();
            FLevelCount -= 1;
        }
    }

    // HSEngine.pas:3259
    public virtual bool AddLevel(int AClassID)
    {
        bool Result;
        EClass LevelClass;
        MapLevel Level;
        LevelClass = FEngine.GetEClass(AClassID);
        if (LevelClass != null)
        {
            Level = (MapLevel)LevelClass.Create(this);
            Level.FLhx = FLevelCount;
            FEngine.ActivateEObject(Level);
            Level.CreateMapFieldList(FWidth, FHeight);
            FLevelCount += 1;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // HSEngine.pas:3277
    public virtual void New(NewHSMapSettings Settings)
    {
        EClass LevelClass;
        MapLevel Level;
        Clear();
        try
        {
            LevelClass = FEngine.GetEClass(Settings.MapLevelClassID);
            for (int I = 0, last = Settings.Levels - 1; I <= last; I++)
            {
                Level = (MapLevel)LevelClass.Create(this);
                Level.FLhx = I;
                FEngine.ActivateEObject(Level);
                Level.New(Settings);
            }

            FWidth = Settings.Width;
            FHeight = Settings.Height;
            FLevelCount = Settings.Levels;
        }
        catch
        {
            Clear();
            throw;
        }
    }

    // HSEngine.pas:3323
    public virtual void UpdateTerrain()
    {
        for (int I = 0, last = FLevelCount - 1; I <= last; I++)
        {
            ((MapLevel)FChildren.List[I]).UpdateTerrain();
        }
    }

    // HSEngine.pas:3224
    public MapLevel GetMapLevel(int Index) => (MapLevel)FChildren.List[Index];

    // HSEngine.pas:3331
    public void Loaded()
    {
        for (int I = 0, last = FLevelCount - 1; I <= last; I++)
        {
            ((MapLevel)FChildren.List[I]).Loaded();
        }
    }

    // HSEngine.pas:3339
    public void Activate()
    {
        for (int I = 0, last = FLevelCount - 1; I <= last; I++)
        {
            ((MapLevel)FChildren.List[I]).Activate();
        }
    }

    public int Width { get => FWidth; }
    public int Height { get => FHeight; }
    public int LevelCount { get => FLevelCount; }
    public HSMap Map { get => FMap; }

    public sealed class MapLevelsAccessor(MapContainer target)
    {
        public MapLevel this[int Index] { get => target.GetMapLevel(Index); }
    }

    public MapLevelsAccessor MapLevels => new MapLevelsAccessor(this);

}
