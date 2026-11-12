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
// _showPriorityOffset (id 4), _imageLibrary (id 8), _imageSequenceList (nil without complex
// images. ids 9, 10), _terrainList (id 7), _level (id 5), _name (id 6)
public partial class MapObjectResource : EngineResource
{
    public new const int ClassId = 65876;

    public MapObjectResource(ECustomNode owner) : base(owner)
    {
        _terrainList = new TerrainList();
        _imageLibrary = new ImageLibrary(null);
        SetLevel(26);
    }

    private GameEngine _engine;
    protected internal int _showPriorityOffset;
    private ImageLibrary _imageLibrary;
    private ImageSequenceList _imageSequenceList;
    private TerrainList _terrainList;
    protected internal byte _level;
    protected internal string _name = "";

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

    // MAPOBJ.PAS:400
    public override GameEngine GetEngine() => _engine;

    // MAPOBJ.PAS:470
    public override void ReadWrite(EStorageStream stream)
    {
        bool complex = default;
        base.ReadWrite(stream);
        stream.rwInteger(ref _showPriorityOffset, 4);
        stream.rwByte(ref _level, 5);
        stream.rwString(ref _name, 6);
        stream.rwStreamProc(7, _terrainList.LoadFromStream, _terrainList.SaveToStream);
        _imageLibrary.ReduceCount();
        stream.rwStreamProc(8, _imageLibrary.LoadFromStream, _imageLibrary.SaveToStream);
        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            if (stream.StorageMode().Contains(StorageModeElement.Read))
            {
                stream.rwBool(ref complex, 9);
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
                stream.rwBool(ref complex, 9);
            }

            if (complex)
            {
                stream.rwStreamProc(10, _imageSequenceList.LoadFromStream, _imageSequenceList.SaveToStream);
            }
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
        int I;
        I = 0;
        LibraryImage result = null;
        while (I != _imageLibrary.Count && result == null)
        {
            result = _imageLibrary.Get(I);
            I += 1;
        }

        if (result == null && _imageSequenceList != null)
        {
            result = _imageSequenceList.GetPreviewImage();
        }

        return result;
    }

    // MAPOBJ.PAS:465
    public override void SetCellImage(LibraryImage value)
    {
        _imageLibrary.SetImage(0, value);
    }

    // MAPOBJ.PAS:525
    public override EngineObject CreateResourceUser()
    {
        int id;
        MapObject mapObject;
        id = ResourceUserClassId();
        if (id != 0)
        {
            mapObject = (MapObject)_engine.CreateEngineObject(id, null);
            if (mapObject != null)
            {
                mapObject.SetResource(this);
                mapObject.SetResourceLinked(true);
            }
        }
        else
        {
            mapObject = null;
        }

        return mapObject;
    }

    // MAPOBJ.PAS:420
    public override void Draw(Canvas canvas, Rect rectangle, Draw.Images.BlitMode mode, int scaleX, int scaleY, int bkColor, int color)
    {
        int I;
        LibraryImage image;
        ImageSequence sequence;
        Rect drawRectangle = default;
        I = 0;
        image = null;
        while (I != _imageLibrary.Count && image == null)
        {
            image = _imageLibrary.Get(I);
            I += 1;
        }

        if (image == null && _imageSequenceList != null)
        {
            I = 0;
            sequence = null;
            while (I != _imageSequenceList.Count && sequence == null)
            {
                sequence = _imageSequenceList.GetImageSequence(I);
                if (sequence == null)
                {
                    I += 1;
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
    public virtual MapObject PlaceResourceUser(HSMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        int pixelX = default;
        int pixelY = default;
        MapObject result = (MapObject)CreateResourceUser();
        HexGeometry.HexToPixel(hexX, hexY, ref pixelX, ref pixelY);
        pixelX += 16;
        pixelY += 16;
        if (!hexSpriteMap.Place(result, pixelX, pixelY, hexLevel))
        {
            result?.Dispose();
            result = null;
        }

        return result;
    }

    // MAPOBJ.PAS:545
    public virtual void CalculateShowPriorityOffset(int dX, int dY, int level)
    {
        _showPriorityOffset = dX + ((level << 26) + (dY << 13));
    }

    // MAPOBJ.PAS:507
    public void ShowComplexImage(int X, int Y, in LockedSurface lockedSurface, int index, uint offset)
    {
        ImageSequence sequence;
        int frame;
        if (index < _imageSequenceList.Count)
        {
            sequence = _imageSequenceList.List[index];
        }
        else
        {
            sequence = null;
        }

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
        for (int I = _terrainList._count - 1; I >= 0; I--)
        {
            if (_terrainList.GetTerrain(I) >= 0)
            {
                HexGeometry.CenterHexNumberToHex(X, Y, I, ref hexX, ref hexY);
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
        int hexX = default;
        int hexY = default;
        int hexLevel;
        int radius;
        bool clear;
        bool placed;
        HexNumberWalkData walk = default;
        placed = true;
        mapLevel = message.Param[0].As<MapLevel>();
        hexLevel = message.Data[1];
        radius = message.Param[1].Int;
        HexGeometry.PixelToHex(message.Payload.Int, message.Data[0], ref hexX, ref hexY);
        clear = message.Param[2].Int == 1;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, hexX, hexY, 0);
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

        if (placed)
        {
            message.Result = 0;
        }
        else
        {
            message.Result = -1;
        }
    }

    public GameEngine Engine { get => _engine; }
    public int ShowPriorityOffset { get => _showPriorityOffset; set => _showPriorityOffset = value; }
    public ImageLibrary ImageLibrary { get => _imageLibrary; }
    public ImageSequenceList ImageSequenceList { get => _imageSequenceList; set => _imageSequenceList = value; }
    public TerrainList TerrainList { get => _terrainList; }
    public byte Level { get => _level; set => SetLevel(value); }
    public string Name { get => _name; set => _name = value; }

    // MAPOBJ.PAS:415
    public override string ResourceEditName() => "";

    internal static void BlitImageSequence(Canvas canvas, ImageSequence sequence, int frame, Rect rectangle, Draw.Images.BlitMode mode, int scaleX, int scaleY, int bkColor, int color, bool selected, ref Rect drawRectangle)
    {
        throw new NotImplementedException();
    }

}
