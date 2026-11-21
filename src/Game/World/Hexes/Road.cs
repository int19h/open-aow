#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Road.pas and AoWRoad.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// { The Road unit of HSEPack.dpl: a road on a hexagon (TRoad, ClassID $20164) whose image is chosen
//   from the roads on the six neighbour hexagons (TRoadResource, ClassID $20165). }
// FTransitions (bit HN-1: a road on the neighbour HN. the pairs 2+6 and 3+5 become bits 7 and
// 6)
public class Road(CustomNode owner) : AbstractRoad(owner)
{

    // Road.pas:47
    public new const int ClassId = 131_428;

    protected internal byte Transitions { get; private set; }

    // Road.pas:52
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        GetResource<Resource>().Show(pixelX - pixelXSurface - 8 - 6, pixelY - pixelYSurface - 9, lockedSurface, road: this);
    }

    // Road.pas:60
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        HexSprite oldRoad = mapField.FindOwnedHexSprite(typeof(AbstractRoad));
        oldRoad?.Dispose();

        Owner = mapField;
        bool result = PlaceOnMap();
        UpdateTransitions();
        UpdateNeighbourTransitions();
        return result;
    }

    // Road.pas:101
    public void UpdateNeighbourTransitions()
    {
        int hexX = default;
        int hexY = default;
        MapLevel level = Owner.Owner;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            HexGeometry.HexNumberToHex(hexNumber, ref hexX, ref hexY);
            HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, hexX, hexY, out hexX, out hexY);
            if (hexX >= 0 && hexY >= 0 && hexX < level.Width && hexY < level.Height)
            {
                MapField mapField = level.GetField(hexX, hexY);
                AbstractRoad road = mapField.FindChild<AbstractRoad>(ClassId);
                road?.UpdateTransitions();
            }
        }
    }

    public new class Resource(CustomNode owner) : AbstractRoadResource(owner)
    {

        // Road.pas:136
        public new const int ClassId = 131_429;

        // Road.pas:126
        public override int ResourceUserClassId() => Road.ClassId;

        // Road.pas:141
        public void Show(int pixelX, int pixelY, in LockedSurface lockedSurface, Road road)
        {
            byte transitions = road.Transitions;
            byte baseValue = 1;
            if (transitions != 0)
            {
                if ((transitions & 0b0100_0000) == 0)
                {
                    if ((transitions & 0b0001_0000) != 0)
                    {
                        ImageLibrary[baseValue + 4].Show(pixelX, pixelY, lockedSurface);
                    }

                    if ((transitions & 0b0100) != 0)
                    {
                        ImageLibrary[baseValue + 2].Show(pixelX, pixelY, lockedSurface);
                    }
                }
                else
                {
                    ImageLibrary[baseValue + 6].Show(pixelX, pixelY, lockedSurface);
                }

                if ((transitions & 0b1000_0000) == 0)
                {
                    if ((transitions & 0b0010) != 0)
                    {
                        ImageLibrary[baseValue + 1].Show(pixelX, pixelY, lockedSurface);
                    }

                    if ((transitions & 0b0010_0000) != 0)
                    {
                        ImageLibrary[baseValue + 5].Show(pixelX, pixelY, lockedSurface);
                    }
                }
                else
                {
                    ImageLibrary[baseValue + 7].Show(pixelX, pixelY, lockedSurface);
                }

                if ((transitions & 1) != 0)
                {
                    ImageLibrary[baseValue].Show(pixelX, pixelY, lockedSurface);
                }

                if ((transitions & 0b1000) != 0)
                {
                    ImageLibrary[baseValue + 3].Show(pixelX, pixelY, lockedSurface);
                }
            }
            else
            {
                ImageLibrary[baseValue + 6].Show(pixelX, pixelY, lockedSurface);
            }
        }

        // Road.pas:171
        public override EditorClass Editor() => Engine.Editors.FindEditor(ClassId) ?? base.Editor();

        // Road.pas:131
        public override string ResourceEditName() => "Road";
    }

    // AoWRoad.pas:35 (Road.pas:74 counts only the neighbours' road sprites)
    public override void UpdateTransitions()
    {
        int hexX = default;
        int hexY = default;
        MapFieldMessage message = default;
        byte oldTransitions = Transitions;
        MapLevel level = Owner.Owner;
        Transitions = 0;
        message.MessageId = 4401;
        MapFieldMessageParameters param = message.Param;
        param[0] = Owner;
        message.Param = param;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            HexGeometry.HexNumberToHex(hexNumber, ref hexX, ref hexY);
            HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, hexX, hexY, out hexX, out hexY);
            if (hexX >= 0 && hexY >= 0 && level.Width > hexX && level.Height > hexY)
            {
                MapField mapField = level.GetField(hexX, hexY);
                bool hasRoad;
                if (mapField.FindOwnedHexSprite(typeof(AbstractRoad)) is null)
                {
                    // { Original message stores its response in Param[1], rather than Result. }
                    MapFieldMessageParameters param2 = message.Param;
                    param2[1] = 0;
                    message.Param = param2;
                    mapField.SendMapFieldMessage(ref message);
                    hasRoad = message.Param[1].Int != 0;
                }
                else
                {
                    hasRoad = true;
                }

                if (hasRoad)
                {
                    Transitions = (byte)(Transitions + (1 << (hexNumber - 1)));
                }
            }
        }

        if ((Transitions & 0b0010_0010) == 0b0010_0010)
        {
            Transitions = (byte)((Transitions & 0b1101_1101) + 128);
        }

        if ((Transitions & 0b0001_0100) == 0b0001_0100)
        {
            Transitions = (byte)((Transitions & 0b1110_1011) + 64);
        }

        if (oldTransitions != Transitions)
        {
            StrategicMap.Current.InvalidateStaticScene(Owner.HexX, Owner.HexY, Owner.HexLevel);
        }
    }
}
