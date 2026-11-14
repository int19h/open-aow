// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// FWidth, FHeight, FLevelCount, FMap, FEngine. The levels of a map (the children, TMapLevel)
public partial class MapContainer : ENode, IChildList<MapLevel>, IOwned<GameMap>, IOwnedSink<GameMap>
{
    // The level in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a level, so the cast never fails.
    public MapLevel this[int index] => (MapLevel)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is MapLevel;

    int IChildSink<MapLevel>.AddChild(MapLevel child) => AddChild(child);

    // The map the container belongs to. AcceptsOwner admits only a map, so the cast never fails.
    public new GameMap Owner { get => (GameMap)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is GameMap;

    void IOwnedSink<GameMap>.SetOwner(GameMap owner) => SetOwner(owner);

    // HSEngine.pas:3208
    public new const int ClassId = 65795;

    private int _width;
    private int _height;
    private int _levelCount;
    private GameMap _map;
    private GameEngine _engine;

    // HSEngine.pas:3167
    public override void SetOwner(ECustomNode owner)
    {
        base.SetOwner(owner);
        _map = Owner;
    }

    // HSEngine.pas:3203. Before the activation message or a load the map reaches the engine as any node does.
    public override GameEngine GetEngine() => _engine ?? base.GetEngine();

    // HSEngine.pas:3173
    public override void ReadWrite(StorageStream stream)
    {
        byte W;
        byte H;
        byte L;
        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            _map = Owner;
            _engine = stream.Engine;
            _map.MapContainer = this;
        }

        W = (byte)_width;
        stream.Byte(ref W, 3);
        _width = W;
        H = (byte)_height;
        stream.Byte(ref H, 4);
        _height = H;
        L = (byte)_levelCount;
        stream.Byte(ref L, 5);
        _levelCount = L;
    }

    public override void PersistFields(IPersister persister)
    {
        byte W;
        byte H;
        byte L;
        base.PersistFields(persister);
        if (persister.Loading)
        {
            _map = Owner;
            _engine = PersistSupport.Engine(persister);
            _map.MapContainer = this;
        }

        W = (byte)_width;
        W = persister.Byte("Width", value: W);
        _width = W;
        H = (byte)_height;
        H = persister.Byte("Height", value: H);
        _height = H;
        L = (byte)_levelCount;
        L = persister.Byte("LevelCount", value: L);
        _levelCount = L;
    }

    // HSEngine.pas:3197
    public override void MainReadWrite(StorageStream stream)
    {
        if (!Owner.Status.Contains(HexSpriteMapStatusElement.PreviewMode))
        {
            base.MainReadWrite(stream);
        }
        else
        {
            ReadWrite(stream);
        }
    }

    // A preview reads the container's own members and leaves its stored levels unread, as
    // MainReadWrite does, so the load skips them.
    public override void MainPersist(IPersister persister)
    {
        if (!Owner.Status.Contains(HexSpriteMapStatusElement.PreviewMode))
        {
            base.MainPersist(persister);
        }
        else
        {
            PersistFields(persister);
            persister.Skip("Children");
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
        if (index >= 0 && index < Count)
        {
            this[index].DestroyHexSprite();
            this[index]?.Dispose();
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
            level = (MapLevel)levelClass.CreateInstance(this, holder: CreationHolder());
            level.HexLevel = _levelCount;
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
                level = (MapLevel)levelClass.CreateInstance(this, holder: CreationHolder());
                level.HexLevel = I;
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
            this[I].UpdateTerrain();
        }
    }

    // HSEngine.pas:3224
    public MapLevel GetMapLevel(int index) => this[index];

    // HSEngine.pas:3331
    public void Loaded()
    {
        for (int I = 0, last = _levelCount - 1; I <= last; I++)
        {
            this[I].Loaded();
        }
    }

    // HSEngine.pas:3339
    public void Activate()
    {
        for (int I = 0, last = _levelCount - 1; I <= last; I++)
        {
            this[I].Activate();
        }
    }

    public int Width { get => _width; internal set => _width = value; }
    public int Height { get => _height; internal set => _height = value; }
    public int LevelCount { get => _levelCount; internal set => _levelCount = value; }
    public GameMap Map { get => _map; internal set => _map = value; }

    public sealed class MapLevelsAccessor(MapContainer target)
    {
        public MapLevel this[int index] { get => target.GetMapLevel(index); }
    }

    public MapLevelsAccessor MapLevels => new MapLevelsAccessor(this);

}
