#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Bridge.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.World;

// { The Bridge unit of HSEPack.dpl: a bridge over water (TBridge, ClassID $20174, an overlay at level
//   10) whose image is chosen from the roads and shores around it (TBridgeResource, ClassID $20175). }
// FTransitions (two bits per side HN 1.6: 0 none, 1 a road, 2 the shore the bridge reaches)
public class Bridge(CustomNode owner) : AbstractRoad(owner)
{

    // Bridge.pas:48
    public new const int ClassId = 131_444;

    private ushort _transitions;

    // Bridge.pas:53
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        GetResource<Resource>().Show(pixelX - pixelXSurface - 8 - 6, pixelY - pixelYSurface - 9, lockedSurface, bridge: this);
    }

    // Bridge.pas:63
    // { Original oddity: the overlay of the owner of the previous owner (a map level, not a map field)
    //   is updated. }
    public override bool PlaceHex(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        MapField mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        HexSprite oldRoad = mapField.FindHexSprite(typeof(AbstractRoad));
        oldRoad?.Dispose();

        _ = Owner;
        Owner = mapField;
        Status |= MapObjectStatuses.PlacedOnMap;
        // { The original refreshes the overlay of the previous field's owner, a map level read as a map field, which is undefined, so skipped.
        //   The owner of a field is always a map level, so the port has no call here. }
        mapField.UpdateOverlayType();
        UpdateTransitions();
        mapContainer.Map.InvalidateStaticScene(hexX, hexY, hexLevel);
        return true;
    }

    // Bridge.pas:82
    public override void UpdateTransitions()
    {
        Span<byte> sides = stackalloc byte[6];
        int hexX = default;
        int hexY = default;
        int transitions = 0;
        MapLevel level = Owner.Owner;
        int shores = 0;
        bool inShore = false;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            HexGeometry.HexNumberToHex(hexNumber, ref hexX, ref hexY);
            HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, hexX, hexY, out hexX, out hexY);
            if (hexX >= 0 && hexY >= 0 && level.Width > hexX && level.Height > hexY)
            {
                MapField mapField = level.GetField(hexX, hexY);
                int terrainType = mapField.TerrainType;
                bool hasRoad = mapField.FindHexSprite(typeof(AbstractRoad)) is not null;
                bool isShore = terrainType != 0 && base.Resource.TerrainList.GetTerrain(0) != terrainType && mapField.FindHexSprite(typeof(BorderHex)) is null;
                if (hasRoad)
                {
                    sides[hexNumber - 1] = 1;
                    inShore = false;
                }
                else
                {
                    if (isShore)
                    {
                        if (!inShore)
                        {
                            shores += 1;
                        }

                        sides[hexNumber - 1] = (byte)(shores + 2);
                        inShore = true;
                    }
                    else
                    {
                        sides[hexNumber - 1] = 0;
                        inShore = false;
                    }
                }
            }
            else
            {
                sides[hexNumber - 1] = 0;
            }
        }

        // { the shores on side 1 and side 6 are one shore }
        if (sides[1 - 1] > 2 && sides[6 - 1] > 2 && sides[1 - 1] != sides[6 - 1])
        {
            for (int hexNumber = 3; hexNumber <= 6; hexNumber++)
            {
                if (sides[hexNumber - 1] == sides[6 - 1])
                {
                    sides[hexNumber - 1] = sides[1 - 1];
                }
            }

            shores -= 1;
        }

        // { the side the bridge starts from: a road, else the first side without water }
        int start = 1;
        while (start <= 6 && sides[start - 1] != 1)
        {
            start += 1;
        }

        if (start == 1)
        {
            if (sides[2 - 1] == 1)
            {
                start = 2;
            }
            else
            {
                if (sides[3 - 1] == 1)
                {
                    start = 3;
                }
            }
        }

        if (start == 7)
        {
            start = 1;
            while (start <= 6 && sides[start - 1] == 0)
            {
                start += 1;
            }

            if (start == 1)
            {
                if (sides[2 - 1] != 0)
                {
                    start = 2;
                }
                else
                {
                    if (sides[3 - 1] == 0)
                    {
                        start = 3;
                    }
                }
            }
        }

        if (start <= 6)
        {
            // { of every shore keep the side farthest from the start }
            for (int shore = 3; shore <= shores + 2; shore++)
            {
                int maxSide = 0;
                if (shore != sides[start - 1] || shores == 1)
                {
                    int maxDistance = 0;
                    for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
                    {
                        if (shore == sides[hexNumber - 1])
                        {
                            int distance = Math.Abs(hexNumber - start);
                            if (distance > 3)
                            {
                                distance = Math.Abs(distance - 6);
                            }

                            if (distance > maxDistance)
                            {
                                maxDistance = distance;
                                maxSide = hexNumber;
                            }
                        }
                    }
                }
                else
                {
                    maxSide = start;
                }

                for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
                {
                    if (hexNumber != start && shore == sides[hexNumber - 1])
                    {
                        sides[hexNumber - 1] = hexNumber != maxSide ? (byte)0 : (byte)2;
                    }
                }
            }

            for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
            {
                transitions += sides[hexNumber - 1] << ((hexNumber - 1) * 2);
            }
        }

        _transitions = (ushort)transitions;
    }

    public new class Resource : AbstractRoadResource
    {
        public Resource(CustomNode owner) : base(owner) => SetLevel(10);

        // Bridge.pas:205
        public new const int ClassId = 131_445;

        // Bridge.pas:195
        public override int ResourceUserClassId() => Bridge.ClassId;

        // Bridge.pas:210
        public void Show(int pixelX, int pixelY, in LockedSurface lockedSurface, Bridge bridge)
        {
            int transitions = bridge._transitions;
            int baseValue = 1;
            ImageLibrary[baseValue + 18].Show(pixelX, pixelY, lockedSurface);
            if ((transitions & 0b0011) != 0)
            {
                if ((transitions & 0b0011) == 1)
                {
                    ImageLibrary[baseValue + 6].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 12].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 0b1100) != 0)
            {
                if ((transitions & 0b1100) == 0b0100)
                {
                    ImageLibrary[baseValue + 7].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 13].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 0b0011_0000) != 0)
            {
                if ((transitions & 0b0011_0000) == 0b0001_0000)
                {
                    ImageLibrary[baseValue + 8].Show(pixelX, pixelY, lockedSurface);
                }
                else
                {
                    ImageLibrary[baseValue + 2].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 14].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 0b1100_0000) != 0)
            {
                if ((transitions & 0b1100_0000) == 0b0100_0000)
                {
                    ImageLibrary[baseValue + 9].Show(pixelX, pixelY, lockedSurface);
                }
                else
                {
                    ImageLibrary[baseValue + 3].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 15].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 0b0011_0000_0000) != 0)
            {
                if ((transitions & 0b0011_0000_0000) == 0b0001_0000_0000)
                {
                    ImageLibrary[baseValue + 10].Show(pixelX, pixelY, lockedSurface);
                }
                else
                {
                    ImageLibrary[baseValue + 4].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 16].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 0b1100_0000_0000) != 0)
            {
                if ((transitions & 0b1100_0000_0000) == 0b0100_0000_0000)
                {
                    ImageLibrary[baseValue + 11].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 17].Show(pixelX, pixelY, lockedSurface);
            }
        }

        // Bridge.pas:253
        public override EditorClass Editor() => Engine.Editors.FindEditor(ClassId) ?? base.Editor();

        // Bridge.pas:259
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            if (message.MessageId != 65556)
            {
                base.MessageProc(ref message, sender);
            }
        }

        // Bridge.pas:200
        public override string ResourceEditName() => HexText.BridgeText;

    }
}
