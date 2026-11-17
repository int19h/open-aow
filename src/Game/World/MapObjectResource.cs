#nullable disable
// Translated from HSEngine.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World;

// the resource of map objects. FEngine (set by the engine activation message),
// _showPriorityOffset (id 4), _imageLibrary (id 8), _imageSequenceList (nil without complex
// images. ids 9, 10), _terrainList (id 7), _level (id 5), _name (id 6)
public partial class MapObjectResource : EngineResource
{
    public new const int ClassId = 65876;

    public MapObjectResource(CustomNode owner) : base(owner)
    {
        _terrainList = new TerrainList();
        _imageLibrary = new ImageLibrary(null);
        SetLevel(26);
    }

    private GameEngine _engine;
    private int _showPriorityOffset;
    private readonly ImageLibrary _imageLibrary;
    private ImageSequenceList _imageSequenceList;
    private readonly TerrainList _terrainList;
    private byte _level;
    private string _name = "";

    // MAPOBJ.PAS:391
    public override void Dispose()
    {
        _terrainList?.Dispose();
        _imageLibrary?.Dispose();
        if (_imageSequenceList != null)
        {
            _imageSequenceList?.Dispose();
        }

        base.Dispose();
    }

    // MAPOBJ.PAS:405
    public override int ClassVersion() => 1;

    // MAPOBJ.PAS:400. Before the activation message the resource reaches the engine as any node does.
    public override GameEngine GetEngine() => _engine ?? base.GetEngine();

    // MAPOBJ.PAS:470
    public override void PersistFields(IPersister persister)
    {
        bool complex = default;
        base.PersistFields(persister);
        _showPriorityOffset = persister.Int32("ShowPriorityOffset", value: _showPriorityOffset);
        _level = persister.Byte("Level", value: _level);
        _name = persister.String("Name", value: _name);
        PersistSupport.TerrainList(persister, name: "TerrainList", list: _terrainList);
        _imageLibrary.ReduceCount();
        PersistSupport.ImageLibrary(persister, name: "ImageLibrary", library: _imageLibrary);
        if (persister.Loading)
        {
            complex = persister.Boolean("Complex", value: complex);
            if (complex)
            {
                if (_imageSequenceList == null)
                {
                    _imageSequenceList = new ImageSequenceList(null, _imageLibrary);
                }
            }
            else
            {
                _imageSequenceList?.Dispose();
                _imageSequenceList = null;
            }
        }
        else
        {
            complex = _imageSequenceList != null;
            complex = persister.Boolean("Complex", value: complex);
        }

        if (complex)
        {
            PersistSupport.ImageSequences(persister, name: "ImageSequenceList", list: _imageSequenceList);
        }
    }

    // MAPOBJ.PAS:383

    // MAPOBJ.PAS:620
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            _engine = message.Payload.As<GameEngine>();
            if (_imageSequenceList != null)
            {
                _imageSequenceList.ImageLibraryManager = _engine.HexSpriteSet.ImageLibraryManager;
                _imageSequenceList.LinkToImageLibrary();
            }
        }
        else
        {
            if (message.MessageId == 65541)
            {
                EditDropMessage(ref message);
            }
            else
            {
                if (message.MessageId == 65621)
                {
                    message.Result = 8;
                }
            }
        }
    }

    // MAPOBJ.PAS:450
    public override LibraryImage GetCellImage()
    {
        int i;
        i = 0;
        LibraryImage result = null;
        while (i != _imageLibrary.Count && result == null)
        {
            result = _imageLibrary.Get(i);
            i += 1;
        }

        if (result == null && _imageSequenceList != null)
        {
            result = _imageSequenceList.GetPreviewImage();
        }

        return result;
    }

    // MAPOBJ.PAS:465
    public override void SetCellImage(LibraryImage value) => _imageLibrary.SetImage(0, value);

    // MAPOBJ.PAS:525
    // The user class is a map object. Another class throws, and a class id of zero stays null.
    public override MapObject CreateResourceUser()
    {
        int id = ResourceUserClassId();
        if (id == 0)
        {
            return null;
        }

        MapObject mapObject = _engine.CreateEngineObject<MapObject>(id, owner: null);
        if (mapObject != null)
        {
            mapObject.SetResource(this);
            mapObject.SetResourceLinked(true);
        }

        return mapObject;
    }

    // The resource's user class selects the map object. Another class throws, and null stays null.
    public T CreateResourceUser<T>() where T : MapObject => (T)CreateResourceUser();

    // MAPOBJ.PAS:420
    public override void Draw(Canvas canvas, Rect rectangle, BlitMode mode, int scaleX, int scaleY, int bkColor, int color)
    {
        int i;
        LibraryImage image;
        ImageSequence sequence;
        Rect drawRectangle = default;
        i = 0;
        image = null;
        while (i != _imageLibrary.Count && image == null)
        {
            image = _imageLibrary.Get(i);
            i += 1;
        }

        if (image == null && _imageSequenceList != null)
        {
            i = 0;
            sequence = null;
            while (i != _imageSequenceList.Count && sequence == null)
            {
                sequence = _imageSequenceList.GetImageSequence(i);
                if (sequence == null)
                {
                    i += 1;
                }
            }

            BlitImageSequence(canvas, sequence, 0, rectangle, mode, scaleX, scaleY, bkColor, color, false, ref drawRectangle);
        }
        else
        {
            LibraryDrawing.BlitLibraryImage(canvas, image, rectangle, mode, scaleX, scaleY, bkColor, color, false, ref drawRectangle);
        }
    }

    // MAPOBJ.PAS:550
    public virtual void SetLevel(byte value)
    {
        _level = value;
        CalculateShowPriorityOffset(16, 32, _level);
    }

    // MAPOBJ.PAS:556
    public virtual MapObject PlaceResourceUser(GameMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
        => PlaceCreatedUser(hexSpriteMap, hexX, hexY, hexLevel);

    // Places the object this resource creates. A subclass that adjusts the placed object calls this
    // and then the adjustment, so the typed form does not re-enter that override.
    protected MapObject PlaceCreatedUser(GameMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapObject result = CreateResourceUser();
        HexGeometry.HexToPixel(hexX, hexY, out int pixelX, out int pixelY);
        pixelX += 16;
        pixelY += 16;
        if (!hexSpriteMap.Place(result, pixelX, pixelY, hexLevel))
        {
            result?.Dispose();
            result = null;
        }

        return result;
    }

    // The created user has the caller's class. Another class throws, and null stays null.
    protected T PlaceCreatedUser<T>(GameMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
        where T : MapObject
        => (T)PlaceCreatedUser(hexSpriteMap, hexX, hexY, hexLevel);

    // The resource's user class selects the map object. Another class throws, and null stays null.
    public T PlaceResourceUser<T>(GameMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
        where T : MapObject
        => (T)PlaceResourceUser(hexSpriteMap, hexX, hexY, hexLevel);

    // MAPOBJ.PAS:545
    public virtual void CalculateShowPriorityOffset(int dX, int dY, int level) => _showPriorityOffset = dX + (level << 26) + (dY << 13);

    // The control of this resource kind, null for a resource with none. Hexagon resources answer
    // the hexagon control, road resources the road control.
    public virtual MapObjectResourceTerrainControl GetTerrainControl() => null;

    // MAPOBJ.PAS:507
    public void ShowComplexImage(int X, int Y, in LockedSurface lockedSurface, int index, uint offset)
    {
        ImageSequence sequence;
        int frame;
        sequence = index < _imageSequenceList.Count ? _imageSequenceList.List[index] : null;

        while (sequence != null)
        {
            frame = sequence.GetFrameIndex(_engine.HexSpriteMap.Frame, (int)offset);
            if (frame != -1)
            {
                sequence.ImageLibrary.Get(frame).Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
            }

            sequence = sequence.Next;
        }
    }

    // MAPOBJ.PAS:571
    public void ClearTerrain(int X, int Y, int hexLevel)
    {
        int hexX = default;
        int hexY = default;
        for (int i = _terrainList.Count - 1; i >= 0; i--)
        {
            if (_terrainList.GetTerrain(i) >= 0)
            {
                HexGeometry.CenterHexNumberToHex(X, Y, i, ref hexX, ref hexY);
                _engine.HexSpriteMap.ClearTerrain(hexX, hexY, hexLevel, 0, -1);
            }
        }
    }

    // MAPOBJ.PAS:586
    // { The resource was dropped on the map (message $10005): fill the hexagons in Param[1] rings around
    //   the drop point with its users, clearing the terrain first when Param[2] = 1 }
    public void EditDropMessage(ref EngineMessage message)
    {
        MapLevel mapLevel;
        int hexLevel;
        int radius;
        bool clear;
        bool placed;
        placed = true;
        mapLevel = message.Param[0].As<MapLevel>();
        hexLevel = message.Data[1];
        radius = message.Param[1].Int;
        HexGeometry.PixelToHex(message.Payload.Int, message.Data[0], out int hexX, out int hexY);
        clear = message.Param[2].Int == 1;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                if (clear)
                {
                    ClearTerrain(walk.X, walk.Y, hexLevel);
                }

                placed = placed && PlaceResourceUser(_engine.HexSpriteMap, (sbyte)walk.X, (sbyte)walk.Y, (sbyte)hexLevel) != null;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        message.Result = placed ? 0 : -1;
    }

    public GameEngine Engine => _engine;
    public int ShowPriorityOffset { get => _showPriorityOffset; set => _showPriorityOffset = value; }
    public ImageLibrary ImageLibrary => _imageLibrary;
    public ImageSequenceList ImageSequenceList { get => _imageSequenceList; set => _imageSequenceList = value; }
    public TerrainList TerrainList => _terrainList;
    public byte Level { get => _level; set => SetLevel(value); }
    public string Name { get => _name; set => _name = value; }

    // MAPOBJ.PAS:415
    public override string ResourceEditName() => "";

    internal static void BlitImageSequence(Canvas canvas, ImageSequence sequence, int frame, Rect rectangle, BlitMode mode, int scaleX, int scaleY, int bkColor, int color, bool selected, ref Rect drawRectangle) => throw new NotImplementedException();

}
