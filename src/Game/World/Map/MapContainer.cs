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
    public new const int ClassId = 65795;

    internal int _width;
    internal int _height;
    internal int _levelCount;
    internal HSMap _map;
    internal GameEngine _engine;

    // HSEngine.pas:3167
    public override void SetOwner(ECustomNode owner)
    {
        base.SetOwner(owner);
        _map = (HSMap)owner;
    }

    // HSEngine.pas:3203
    public override GameEngine GetEngine() => _engine;

    // HSEngine.pas:3173
    public override void ReadWrite(EStorageStream stream)
    {
        byte W;
        byte H;
        byte L;
        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            _map = (HSMap)Owner;
            _engine = stream.Engine;
            _map._mapContainer = this;
        }

        W = (byte)_width;
        stream.rwByte(ref W, 3);
        _width = W;
        H = (byte)_height;
        stream.rwByte(ref H, 4);
        _height = H;
        L = (byte)_levelCount;
        stream.rwByte(ref L, 5);
        _levelCount = L;
    }

    // HSEngine.pas:3197
    public override void MainReadWrite(EStorageStream stream)
    {
        if (!((HSMap)Owner)._status.Contains(HexSpriteMapStatusElement.PreviewMode))
        {
            base.MainReadWrite(stream);
        }
        else
        {
            ReadWrite(stream);
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
        _width = 0;
        _height = 0;
        _levelCount = 0;
    }

    // HSEngine.pas:3213
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 536870920)
        {
            _engine = message.Payload.As<GameEngine>();
        }

        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            UpdateTerrain();
            Activate();
        }
    }

    // HSEngine.pas:3249
    public virtual void RemoveLevel(int index)
    {
        if (index >= 0 && index < _children.Count)
        {
            ((MapLevel)_children.List[index]).DestroyHexSprite();
            ((MapLevel)_children.List[index])?.Dispose();
            _levelCount -= 1;
        }
    }

    // HSEngine.pas:3259
    public virtual bool AddLevel(int classId)
    {
        bool result;
        EngineClass levelClass;
        MapLevel level;
        levelClass = _engine.GetEngineClass(classId);
        if (levelClass != null)
        {
            level = (MapLevel)levelClass.CreateInstance(this);
            level._hexLevel = _levelCount;
            _engine.ActivateEngineObject(level);
            level.CreateMapFieldList(_width, _height);
            _levelCount += 1;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // HSEngine.pas:3277
    public virtual void New(NewHexSpriteMapSettings settings)
    {
        EngineClass levelClass;
        MapLevel level;
        Clear();
        try
        {
            levelClass = _engine.GetEngineClass(settings.MapLevelClassId);
            for (int I = 0, last = settings.Levels - 1; I <= last; I++)
            {
                level = (MapLevel)levelClass.CreateInstance(this);
                level._hexLevel = I;
                _engine.ActivateEngineObject(level);
                level.New(settings);
            }

            _width = settings.Width;
            _height = settings.Height;
            _levelCount = settings.Levels;
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
        for (int I = 0, last = _levelCount - 1; I <= last; I++)
        {
            ((MapLevel)_children.List[I]).UpdateTerrain();
        }
    }

    // HSEngine.pas:3224
    public MapLevel GetMapLevel(int index) => (MapLevel)_children.List[index];

    // HSEngine.pas:3331
    public void Loaded()
    {
        for (int I = 0, last = _levelCount - 1; I <= last; I++)
        {
            ((MapLevel)_children.List[I]).Loaded();
        }
    }

    // HSEngine.pas:3339
    public void Activate()
    {
        for (int I = 0, last = _levelCount - 1; I <= last; I++)
        {
            ((MapLevel)_children.List[I]).Activate();
        }
    }

    public int Width { get => _width; }
    public int Height { get => _height; }
    public int LevelCount { get => _levelCount; }
    public HSMap Map { get => _map; }

    public sealed class MapLevelsAccessor(MapContainer target)
    {
        public MapLevel this[int index] { get => target.GetMapLevel(index); }
    }

    public MapLevelsAccessor MapLevels => new MapLevelsAccessor(this);

}
