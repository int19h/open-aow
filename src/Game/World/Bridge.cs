// Translated from Bridge.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// { The Bridge unit of HSEPack.dpl: a bridge over water (TBridge, ClassID $20174, an overlay at level
//   10) whose image is chosen from the roads and shores around it (TBridgeResource, ClassID $20175). }
// FTransitions (two bits per side HN 1.6: 0 none, 1 a road, 2 the shore the bridge reaches)
public partial class Bridge : AbstractRoad
{
    public Bridge(ECustomNode owner) : base(owner)
    {
    }

    // Bridge.pas:48
    public new const int ClassId = 131444;

    private ushort _transitions;

    // Bridge.pas:43

    // Bridge.pas:53
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
        ((Resource)_resource).Show(pixelX - pixelXSurface - 8 - 6, pixelY - pixelYSurface - 9, lockedSurface, this);
    }

    // Bridge.pas:63
    // { Original oddity: the overlay of the owner of the previous owner (a map level, not a map field)
    //   is updated. }
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField;
        HexSprite oldRoad;
        MapField oldOwner;
        mapField = mapContainer.GetField(hexX, hexY, hexLevel);
        oldRoad = mapField.FindHexSprite(typeof(AbstractRoad));
        if (oldRoad != null)
        {
            oldRoad.Dispose();
        }

        oldOwner = Owner;
        Owner = mapField;
        Status = Status + Set<MapObjectStatusElement>.Of(MapObjectStatusElement.PlacedOnMap);
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
        ByteArray6 sides = default;
        int transitions;
        int maxDistance;
        int maxSide;
        bool hasRoad;
        bool inShore;
        int hexX = default;
        int hexY = default;
        int shores;
        sbyte terrainType;
        MapLevel level;
        int start;
        int distance;
        MapField mapField;
        bool isShore;
        transitions = 0;
        level = Owner.Owner;
        shores = 0;
        inShore = false;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            HexGeometry.HexNumberToHex(hexNumber, ref hexX, ref hexY);
            HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, hexX, hexY, ref hexX, ref hexY);
            if (hexX >= 0 && hexY >= 0 && level.Width > hexX && level.Height > hexY)
            {
                mapField = level.GetField(hexX, hexY);
                terrainType = mapField.TerrainType;
                hasRoad = mapField.FindHexSprite(typeof(AbstractRoad)) != null;
                isShore = terrainType != 0 && _resource.TerrainList.GetTerrain(0) != terrainType && mapField.FindHexSprite(typeof(BorderHexagon)) == null;
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
        start = 1;
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
                maxDistance = 0;
                maxSide = 0;
                if (shore != sides[start - 1] || shores == 1)
                {
                    for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
                    {
                        if (shore == sides[hexNumber - 1])
                        {
                            distance = Math.Abs(hexNumber - start);
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
                        if (hexNumber != maxSide)
                        {
                            sides[hexNumber - 1] = 0;
                        }
                        else
                        {
                            sides[hexNumber - 1] = 2;
                        }
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

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            SetLevel(10);
        }

        // Bridge.pas:205
        public new const int ClassId = 131445;

        // Bridge.pas:189

        // Bridge.pas:195
        public override int ResourceUserClassId() => 131444;

        // Bridge.pas:210
        public void Show(int pixelX, int pixelY, in LockedSurface lockedSurface, Bridge bridge)
        {
            int transitions;
            int baseValue;
            transitions = bridge._transitions;
            baseValue = 1;
            ImageLibrary[baseValue + 18].Show(pixelX, pixelY, lockedSurface);
            if ((transitions & 3) != 0)
            {
                if ((transitions & 3) == 1)
                {
                    ImageLibrary[baseValue + 6].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 12].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 12) != 0)
            {
                if ((transitions & 12) == 4)
                {
                    ImageLibrary[baseValue + 7].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 13].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 48) != 0)
            {
                if ((transitions & 48) == 16)
                {
                    ImageLibrary[baseValue + 8].Show(pixelX, pixelY, lockedSurface);
                }
                else
                {
                    ImageLibrary[baseValue + 2].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 14].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 192) != 0)
            {
                if ((transitions & 192) == 64)
                {
                    ImageLibrary[baseValue + 9].Show(pixelX, pixelY, lockedSurface);
                }
                else
                {
                    ImageLibrary[baseValue + 3].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 15].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 768) != 0)
            {
                if ((transitions & 768) == 256)
                {
                    ImageLibrary[baseValue + 10].Show(pixelX, pixelY, lockedSurface);
                }
                else
                {
                    ImageLibrary[baseValue + 4].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 16].Show(pixelX, pixelY, lockedSurface);
            }

            if ((transitions & 3072) != 0)
            {
                if ((transitions & 3072) == 1024)
                {
                    ImageLibrary[baseValue + 11].Show(pixelX, pixelY, lockedSurface);
                }

                ImageLibrary[baseValue + 17].Show(pixelX, pixelY, lockedSurface);
            }
        }

        // Bridge.pas:253
        public override EditorClass Editor()
        {
            EditorClass result = Engine.Editors.FindEditor(131445);
            if (result == null)
            {
                result = base.Editor();
            }

            return result;
        }

        // Bridge.pas:259
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            if (message.MessageId != 65556)
            {
                base.MessageProc(ref message, sender);
            }
        }

        // Bridge.pas:28

        // Bridge.pas:200
        public override string ResourceEditName() => HexText.BridgeText;

    }

    // Bridge.pas:18

}
