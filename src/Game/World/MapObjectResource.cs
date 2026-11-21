#nullable disable warnings
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
public class MapObjectResource : EngineResource
{
    public new const int ClassId = 65_876;

    public MapObjectResource(CustomNode? owner) : base(owner)
    {
        TerrainList = new TerrainList();
        ImageLibrary = new ImageLibrary(null);
        SetLevel(26);
    }

    private byte _level;

    // MAPOBJ.PAS:391
    public override void Dispose()
    {
        TerrainList?.Dispose();
        ImageLibrary?.Dispose();
        ImageSequenceList?.Dispose();

        base.Dispose();
    }

    // MAPOBJ.PAS:405
    public override int ClassVersion() => 1;

    // MAPOBJ.PAS:400. Before the activation message the resource reaches the engine as any node does.
    public override GameEngine? GetEngine() => Engine ?? base.GetEngine();

    // MAPOBJ.PAS:470
    public override void PersistFields(IPersister persister)
    {
        bool complex = default;
        base.PersistFields(persister);
        ShowPriorityOffset = persister.Int32("ShowPriorityOffset", value: ShowPriorityOffset);
        _level = persister.Byte("Level", value: _level);
        Name = persister.String("Name", value: Name);
        persister.TerrainList(name: "TerrainList", list: TerrainList);
        _ = ImageLibrary.ReduceCount();
        persister.ImageLibrary(name: "ImageLibrary", library: ImageLibrary);
        if (persister.Loading)
        {
            complex = persister.Boolean("Complex", value: complex);
            if (complex)
            {
                ImageSequenceList ??= new ImageSequenceList(null, ImageLibrary);
            }
            else
            {
                ImageSequenceList?.Dispose();
                ImageSequenceList = null;
            }
        }
        else
        {
            complex = ImageSequenceList is not null;
            complex = persister.Boolean("Complex", value: complex);
        }

        if (complex)
        {
            persister.ImageSequences(name: "ImageSequenceList", list: ImageSequenceList);
        }
    }

    // MAPOBJ.PAS:620
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            Engine = message.Payload.As<GameEngine>();
            if (ImageSequenceList is not null)
            {
                ImageSequenceList.ImageLibraryManager = Engine.HexSpriteSet.ImageLibraryManager;
                ImageSequenceList.LinkToImageLibrary();
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
    public override LibraryImage? GetCellImage()
    {
        int i = 0;
        LibraryImage? result = null;
        while (i != ImageLibrary.Count && result is null)
        {
            result = ImageLibrary.Get(i);
            i += 1;
        }

        return result is null && ImageSequenceList is not null ? ImageSequenceList.GetPreviewImage() : result;
    }

    // MAPOBJ.PAS:465
    public override void SetCellImage(LibraryImage value) => ImageLibrary.SetImage(0, image: value);

    // MAPOBJ.PAS:525
    // The user class is a map object. Another class throws, and a class id of zero stays null.
    public override MapObject? CreateResourceUser()
    {
        int id = ResourceUserClassId();
        if (id == 0)
        {
            return null;
        }

        MapObject mapObject = Engine.CreateEngineObject<MapObject>(id, owner: null);
        if (mapObject is not null)
        {
            mapObject.SetResource(this);
            mapObject.ResourceLinked = true;
        }

        return mapObject;
    }

    // MAPOBJ.PAS:420
    public override void Draw(Canvas canvas, Rect rectangle, BlitMode mode, int scaleX, int scaleY, int bkColor, int color)
    {
        Rect drawRectangle = default;
        int i = 0;
        LibraryImage? image = null;
        while (i != ImageLibrary.Count && image is null)
        {
            image = ImageLibrary.Get(i);
            i += 1;
        }

        if (image is null && ImageSequenceList is not null)
        {
            i = 0;
            ImageSequence? sequence = null;
            while (i != ImageSequenceList.Count && sequence is null)
            {
                sequence = ImageSequenceList.GetImageSequence(i);
                if (sequence is null)
                {
                    i += 1;
                }
            }

            BlitImageSequence(canvas, sequence, frame: 0, rectangle, mode, scaleX, scaleY, bkColor, color, selected: false, in drawRectangle);
        }
        else
        {
            LibraryDrawing.BlitLibraryImage(canvas, image, rectangle, mode, scaleX, scaleY, bkColor, color, selected: false, in drawRectangle);
        }
    }

    // MAPOBJ.PAS:550
    public virtual void SetLevel(byte value)
    {
        _level = value;
        CalculateShowPriorityOffset(16, 32, _level);
    }

    // MAPOBJ.PAS:556
    public virtual MapObject? PlaceResourceUser(GameMap? hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
        => PlaceCreatedUser(hexSpriteMap, hexX, hexY, hexLevel);

    // Places the object this resource creates. A subclass that adjusts the placed object calls this
    // and then the adjustment, so the typed form does not re-enter that override.
    protected MapObject? PlaceCreatedUser(GameMap? hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapObject? result = CreateResourceUser();
        HexGeometry.HexToPixel(hexX, hexY, out int pixelX, out int pixelY);
        pixelX += 16;
        pixelY += 16;
        if (!hexSpriteMap.Place(result, pixelX, pixelY, L: hexLevel))
        {
            result?.Dispose();
            return null;
        }

        return result;
    }

    // The created user has the caller's class. Another class throws, and null stays null.
    protected T PlaceCreatedUser<T>(GameMap? hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
        where T : MapObject
        => (T)PlaceCreatedUser(hexSpriteMap, hexX, hexY, hexLevel);

    // The resource's user class selects the map object. Another class throws, and null stays null.
    public T PlaceResourceUser<T>(GameMap? hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
        where T : MapObject
        => (T)PlaceResourceUser(hexSpriteMap, hexX, hexY, hexLevel);

    // MAPOBJ.PAS:545
    public virtual void CalculateShowPriorityOffset(int dX, int dY, int level) => ShowPriorityOffset = dX + (level << 26) + (dY << 13);

    // The control of this resource kind, null for a resource with none. Hexagon resources answer
    // the hexagon control, road resources the road control.
    public virtual MapObjectResourceTerrainControl? TerrainControl => null;

    // MAPOBJ.PAS:507
    public void ShowComplexImage(int X, int Y, in LockedSurface lockedSurface, int index, uint offset)
    {
        ImageSequence? sequence = index < ImageSequenceList.Count ? ImageSequenceList.List[index] : null;

        while (sequence is not null)
        {
            int frame = sequence.GetFrameIndex(Engine.HexSpriteMap.Frame, offset: (int)offset);
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
        for (int i = TerrainList.Count - 1; i >= 0; i--)
        {
            if (TerrainList.GetTerrain(i) >= 0)
            {
                HexGeometry.CenterHexNumberToHex(X, Y, hexNumber: i, ref hexX, ref hexY);
                Engine.HexSpriteMap.ClearTerrain(hexX, hexY, L: hexLevel, radius: 0, terrainType: -1);
            }
        }
    }

    // MAPOBJ.PAS:586
    // { The resource was dropped on the map (message $10005): fill the hexagons in Param[1] rings around
    //   the drop point with its users, clearing the terrain first when Param[2] = 1 }
    public void EditDropMessage(ref EngineMessage message)
    {
        bool placed = true;
        MapLevel? mapLevel = message.Param[0].As<MapLevel>();
        int hexLevel = message.Data[1];
        int radius = message.Param[1].Int;
        HexGeometry.PixelToHex(message.Payload.Int, vertical: message.Data[0], out int hexX, out int hexY);
        bool clear = message.Param[2].Int == 1;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
        while (walk.Radius <= radius)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                if (clear)
                {
                    ClearTerrain(walk.X, walk.Y, hexLevel);
                }

                placed = placed && PlaceResourceUser(Engine.HexSpriteMap, (sbyte)walk.X, (sbyte)walk.Y, (sbyte)hexLevel) is not null;
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        message.Result = placed ? 0 : -1;
    }

    public GameEngine? Engine { get; private set; }
    public int ShowPriorityOffset { get; private set; }
    public ImageLibrary ImageLibrary { get; }
    public ImageSequenceList? ImageSequenceList { get; set; }
    public TerrainList TerrainList { get; }
    public byte Level { get => _level; init => SetLevel(value); }
    public string Name { get; private set; } = "";

    // MAPOBJ.PAS:415
    public override string ResourceEditName() => "";

    internal static void BlitImageSequence(Canvas canvas, ImageSequence? sequence, int frame, Rect rectangle, BlitMode mode, int scaleX, int scaleY, int bkColor, int color, bool selected, ref readonly Rect drawRectangle) => throw new NotImplementedException();

}
