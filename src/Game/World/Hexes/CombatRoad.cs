// Translated from CombatRoad.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class CombatRoad : AbstractRoad
{
    public CombatRoad(ECustomNode owner) : base(owner)
    { }

    // CombatRoad.pas:51
    public new const int ClassId = 2229506;

    private ByteArray7 _images;

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
        int X = default;
        int Y = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref X, ref Y);
        X = X - pixelXSurface - 8;
        Y = Y - pixelYSurface;
        images = _resource.ImageSequenceList.GetImageSequence(0).ImageLibrary;
        images.Get(_images[0]).Show(X, Y, lockedSurface);
        for (int I = 1; I <= 6; I++)
        {
            if (_images[I] != 255)
            {
                images.Get(_images[I]).Show(X - 8, Y - 16, lockedSurface);
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
    public override MapObjectResourceList GetResourceList(GameEngine engine) => AoWHSSet.Current.CombatResources;

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
        for (int I = 1; I <= 6; I++)
        {
            UpdateTransition((byte)I);
        }
    }

    // CombatRoad.pas:77
    public void UpdateTransition(byte hexNumber)
    {
        MapLevel mapLevel;
        sbyte overlay;
        int X = default;
        int Y = default;
        mapLevel = Owner.Owner;
        HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, hexNumber, ref X, ref Y);
        if (X >= 0 && Y >= 0 && X < mapLevel.Width && Y < mapLevel.Height)
        {
            overlay = mapLevel.GetField(X, Y).OverlayType;
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
        public Resource(ECustomNode owner) : base(owner)
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
        public override RoadControl GetRoadControl() => ((AoWHSSet)Engine.HexSpriteSet).CombatRoads;

        // CombatRoad.pas:136
        public override string ResourceEditName() => "Combat Road";

    }

}
