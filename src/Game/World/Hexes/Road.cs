// Translated from Road.pas and AoWRoad.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// { The Road unit of HSEPack.dpl: a road on a hexagon (TRoad, ClassID $20164) whose image is chosen
//   from the roads on the six neighbour hexagons (TRoadResource, ClassID $20165). }
// FTransitions (bit HN-1: a road on the neighbour HN. the pairs 2+6 and 3+5 become bits 7 and
// 6)
public partial class Road : AbstractRoad
{
    public Road(ECustomNode owner) : base(owner)
    {
    }

    // Road.pas:47
    public new const int ClassId = 131428;

    protected internal byte Transitions { get; protected set; }

    // Road.pas:42

    // Road.pas:52
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
        ((Resource)_resource).Show(pixelX - pixelXSurface - 8 - 6, pixelY - pixelYSurface - 9, lockedSurface, this);
    }

    // Road.pas:60
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField;
        HexSprite oldRoad;
        mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        oldRoad = mapField.FindOwnedHexSprite(typeof(AbstractRoad));
        if (oldRoad != null)
        {
            oldRoad.Dispose();
        }

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
        MapLevel level;
        MapField mapField;
        AbstractRoad road;
        level = Owner.Owner;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            HexGeometry.HexNumberToHex(hexNumber, ref hexX, ref hexY);
            HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, hexX, hexY, out hexX, out hexY);
            if (hexX >= 0 && hexY >= 0 && hexX < level.Width && hexY < level.Height)
            {
                mapField = level.GetField(hexX, hexY);
                road = (AbstractRoad)mapField.FindChild(131428);
                if (road != null)
                {
                    road.UpdateTransitions();
                }
            }
        }
    }

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Road.pas:136
        public new const int ClassId = 131429;

        // Road.pas:126
        public override int ResourceUserClassId() => 131428;

        // Road.pas:141
        public void Show(int pixelX, int pixelY, in LockedSurface lockedSurface, Road road)
        {
            byte transitions;
            byte baseValue;
            transitions = road.Transitions;
            baseValue = 1;
            if (transitions != 0)
            {
                if ((transitions & 64) == 0)
                {
                    if ((transitions & 16) != 0)
                    {
                        ImageLibrary[baseValue + 4].Show(pixelX, pixelY, lockedSurface);
                    }

                    if ((transitions & 4) != 0)
                    {
                        ImageLibrary[baseValue + 2].Show(pixelX, pixelY, lockedSurface);
                    }
                }
                else
                {
                    ImageLibrary[baseValue + 6].Show(pixelX, pixelY, lockedSurface);
                }

                if ((transitions & 128) == 0)
                {
                    if ((transitions & 2) != 0)
                    {
                        ImageLibrary[baseValue + 1].Show(pixelX, pixelY, lockedSurface);
                    }

                    if ((transitions & 32) != 0)
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

                if ((transitions & 8) != 0)
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
        public override EditorClass Editor()
        {
            EditorClass result = Engine.Editors.FindEditor(131429);
            if (result == null)
            {
                result = base.Editor();
            }

            return result;
        }

        // Road.pas:131
        public override string ResourceEditName() => "Road";
    }

    // Road.pas:18

    // AoWRoad.pas:35 (Road.pas:74 counts only the neighbours' road sprites)
    public override void UpdateTransitions()
    {
        byte oldTransitions;
        int hexX = default;
        int hexY = default;
        MapFieldMessage message = default;
        MapLevel level;
        MapField mapField;
        bool hasRoad;
        oldTransitions = Transitions;
        level = Owner.Owner;
        Transitions = 0;
        message.MessageId = 4401;
        message.Param[0] = Owner;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            HexGeometry.HexNumberToHex(hexNumber, ref hexX, ref hexY);
            HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, hexX, hexY, out hexX, out hexY);
            if (hexX >= 0 && hexY >= 0 && level.Width > hexX && level.Height > hexY)
            {
                mapField = level.GetField(hexX, hexY);
                if (mapField.FindOwnedHexSprite(typeof(AbstractRoad)) == null)
                {
                    // { Original message stores its response in Param[1], rather than Result. }
                    message.Param[1] = 0;
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

        if ((Transitions & 34) == 34)
        {
            Transitions = (byte)((Transitions & 221) + 128);
        }

        if ((Transitions & 20) == 20)
        {
            Transitions = (byte)((Transitions & 235) + 64);
        }

        if (oldTransitions != Transitions)
        {
            StrategicMap.Current.InvalidateStaticScene(Owner.HexX, Owner.HexY, Owner.HexLevel);
        }
    }
}
