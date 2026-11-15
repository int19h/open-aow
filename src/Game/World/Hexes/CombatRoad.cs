#nullable disable
// Translated from CombatRoad.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class CombatRoad : AbstractRoad
{
    public CombatRoad(CustomNode owner) : base(owner)
    { }

    // CombatRoad.pas:51
    public new const int ClassId = 2229506;

    private readonly byte[] _images = new byte[7];

    // main image, neighbour images
    // CombatRoad.pas:108
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo message)
    {
        if (hexNumber == 0)
        {
            UpdateTransition((byte)message.HexNumber);
        }
    }

    // CombatRoad.pas:113
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        CustomImageLibrary images;
        int x = default;
        int y = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out x, out y);
        x = x - pixelXSurface - 8;
        y = y - pixelYSurface;
        images = base.Resource.ImageSequenceList.GetImageSequence(0).ImageLibrary;
        images.Get(_images[0]).Show(x, y, lockedSurface);
        for (int i = 1; i <= 6; i++)
        {
            if (_images[i] != 255)
            {
                images.Get(_images[i]).Show(x - 8, y - 16, lockedSurface);
            }
        }
    }

    // CombatRoad.pas:127
    public override void Activate()
    {
        base.Activate();
        var random = Random(12);
        _images[0] = (byte)(random + 10);
    }

    // CombatRoad.pas:54
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    // CombatRoad.pas:57
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        if (hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < mapContainer.Width && hexY < mapContainer.Height && hexLevel < mapContainer.LevelCount)
        {
            result = !(mapContainer.GetField(hexX, hexY, hexLevel).OverlayType == 4 || mapContainer.GetField(hexX, hexY, hexLevel).TerrainType == 15);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // CombatRoad.pas:66
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        HexSprite oldRoad;
        MapField mapField;
        mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        oldRoad = mapField.FindHexSprite(typeof(AbstractRoad));
        if (oldRoad != null)
        {
            oldRoad.Dispose();
        }

        return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
    }

    // CombatRoad.pas:101
    public override void UpdateTransitions()
    {
        for (int i = 1; i <= 6; i++)
        {
            UpdateTransition((byte)i);
        }
    }

    // CombatRoad.pas:77
    public void UpdateTransition(byte hexNumber)
    {
        MapLevel mapLevel;
        sbyte overlay;
        int x = default;
        int y = default;
        mapLevel = Owner.Owner;
        HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, hexNumber, ref x, ref y);
        if (x >= 0 && y >= 0 && x < mapLevel.Width && y < mapLevel.Height)
        {
            overlay = mapLevel.GetField(x, y).OverlayType;
            if (overlay != 4)
            {
                switch (hexNumber)
                {
                    case 1:
                        var random = Random(2);
                        _images[hexNumber] = (byte)(random * 3 + 1);
                        break;
                    case 2:
                        var random2 = Random(2);
                        _images[hexNumber] = (byte)(random2 * 3 + 2);
                        break;
                    case 3:
                        _images[hexNumber] = 6;
                        break;
                    case 4:
                        _images[hexNumber] = 7;
                        break;
                    case 5:
                        _images[hexNumber] = 8;
                        break;
                    case 6:
                        var random3 = Random(2);
                        _images[hexNumber] = (byte)(random3 * 3);
                        break;
                }
            }
            else
            {
                _images[hexNumber] = 255;
            }
        }
    }

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(CustomNode owner) : base(owner)
        { }

        // CombatRoad.pas:133
        public new const int ClassId = 2229507;

        // CombatRoad.pas:144
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            base.MessageProc(ref message, sender);
            if (message.MessageId == 65621)
            {
                message.Result = 5;
            }
        }

        // CombatRoad.pas:153
        public override LibraryImage GetCellImage()
        {
            LibraryImage result = null;
            if (ImageSequenceList != null && ImageSequenceList.Count > 0 && ImageSequenceList.GetImageSequence(0) != null)
            {
                result = ImageSequenceList.GetImageSequence(0).ImageLibrary.Get(10);
            }

            if (result == null)
            {
                result = base.GetCellImage();
            }

            return result;
        }

        // CombatRoad.pas:150
        public override int ResourceUserClassId() => 2229506;

        // CombatRoad.pas:139
        public override RoadControl GetRoadControl() => Engine.HexSpriteSet.CombatRoads;

        // CombatRoad.pas:136
        public override string ResourceEditName() => "Combat Road";

    }

}
