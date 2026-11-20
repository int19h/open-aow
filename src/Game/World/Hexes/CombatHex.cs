#nullable disable
// Translated from CombatHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Hexes;

public class CombatHex(CustomNode owner) : AbstractHexagon(owner)
{
    public new const int ClassId = 2229504;

    private byte _imageVariant;
    private readonly byte[] _transitions = new byte[3];

    // CombatHex.pas:174
    public override void TerrainChanged(in HexSpriteTerrainInfo message)
    {
        if (Set<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)message.TerrainType))
        {
            if (message.TerrainType != 14)
            {
                sbyte hexLevel = HexLevel;
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                _ = base.Resource.Engine.HexSpriteSet.CombatHexes.Place(CombatMap.Current.MapContainer, hexX, hexY, L: hexLevel, message.TerrainType);
            }
            else
            {
                sbyte hexLevel = HexLevel;
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                _ = base.Resource.Engine.HexSpriteSet.CombatHexes.Place(CombatMap.Current.MapContainer, hexX, hexY, L: hexLevel, terrainType: 0);
            }
        }
    }

    // CombatHex.pas:138
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo info)
    {
        if (hexNumber == 0)
        {
            switch (info.HexNumber)
            {
                case 6:
                    UpdateTransition(1);
                    break;
                case 1:
                    UpdateTransition(2);
                    break;
                case 2:
                    UpdateTransition(3);
                    break;
                default:
                    break;
            }
        }
    }

    // CombatHex.pas:148
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (Owner.OverlayType != 4)
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY -= pixelYSurface;
            CustomImageLibrary images = base.Resource.ImageSequenceList.GetImageSequence(0).ImageLibrary;
            images[_imageVariant].Show(pixelX, pixelY, lockedSurface);
            images = GetResource<Resource>().TransitionLibrary;
            for (int side = 1; side <= 3; side++)
            {
                if (_transitions[side - 1] != 255)
                {
                    images[_transitions[side - 1]].Show(pixelX - 8, pixelY - 16, lockedSurface);
                }
            }
        }
    }

    // CombatHex.pas:168
    public override void Activate()
    {
        base.Activate();
        _imageVariant = (byte)Random(12);
    }

    // CombatHex.pas:73
    public override MapObjectResourceList GetResourceList(GameEngine engine) => GamePack.Current.CombatResources;

    // CombatHex.pas:134
    public override void UpdateTransitions()
    {
        for (int side = 1; side <= 3; side++)
        {
            UpdateTransition((byte)side);
        }
    }

    // CombatHex.pas:76
    public void UpdateTransition(byte side)
    {
        int x = default;
        int y = default;
        MapLevel mapLevel = Owner.Owner;
        HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, hexNumber: TransitionNeighbours[side - 1], ref x, ref y);
        if (x >= 0 && y >= 0 && x < mapLevel.Width && y < mapLevel.Height)
        {
            sbyte neighbourTerrain = mapLevel.GetField(x, y).TerrainType;
            sbyte terrainType = base.Resource.TerrainList.GetTerrain(0);
            int imageIndex = -1;
            if (neighbourTerrain != terrainType && neighbourTerrain != 7)
            {
                if (terrainType == 0)
                {
                    imageIndex = neighbourTerrain == 6 ? 10 : neighbourTerrain + 4;
                }
                else
                {
                    if (terrainType == 6)
                    {
                        imageIndex = neighbourTerrain == 0 ? 16 : neighbourTerrain + 10;
                    }
                    else
                    {
                        if (terrainType == 12)
                        {
                            imageIndex = 17;
                        }
                        else
                        {
                            if (terrainType == 10)
                            {
                                if (neighbourTerrain == 12)
                                {
                                    imageIndex = 18;
                                }
                                else
                                {
                                    if (neighbourTerrain == 13)
                                    {
                                        imageIndex = 19;
                                    }
                                    else
                                    {
                                        if (neighbourTerrain == 9)
                                        {
                                            imageIndex = 20;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (terrainType == 13)
                                {
                                    if (neighbourTerrain == 12)
                                    {
                                        imageIndex = 21;
                                    }
                                    else
                                    {
                                        if (neighbourTerrain == 10)
                                        {
                                            imageIndex = 22;
                                        }
                                        else
                                        {
                                            if (neighbourTerrain == 9)
                                            {
                                                imageIndex = 23;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    imageIndex = terrainType == 9 ? neighbourTerrain == 12 ? 24 : 25 : terrainType - 1;
                                }
                            }
                        }
                    }
                }
            }

            if (imageIndex != -1)
            {
                imageIndex = (Random(2) * 3) + (imageIndex * 6) + (side - 1);
                _transitions[side - 1] = GetResource<Resource>().TransitionLibrary.GetSafe(imageIndex) is not null ? (byte)imageIndex : (byte)255;
            }
            else
            {
                _transitions[side - 1] = 255;
            }
        }
    }

    private static ReadOnlySpan<byte> TransitionNeighbours => [6, 1, 2];

    public new class Resource(CustomNode owner) : AbstractHexagonResource(owner)
    {
        public new const int ClassId = 2229505;

        public CustomImageLibrary TransitionLibrary { get; private set; }

        // CombatHex.pas:194
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            bool condition = message.MessageId != 65556;
            if (!condition)
            {
                sbyte terrain = TerrainList.GetTerrain(0);
                condition = !Set<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)terrain);
            }

            if (condition)
            {
                base.MessageProc(ref message, sender);
            }

            if (message.MessageId == 65621)
            {
                message.Result = 5;
            }

            if (message.MessageId == 536870920)
            {
                TransitionLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), fileName: "TCombat\\Base_Hex\\CTransHx.ILB");
            }
        }

        // CombatHex.pas:207
        public override LibraryImage GetCellImage()
        {
            LibraryImage result = null;
            if (ImageSequenceList is not null && ImageSequenceList.Count > 0 && ImageSequenceList.GetImageSequence(0) is not null)
            {
                result = ImageSequenceList.GetImageSequence(0).ImageLibrary[10];
            }

            result ??= base.GetCellImage();

            return result;
        }

        // CombatHex.pas:204
        public override int ResourceUserClassId() => 2229504;

        // CombatHex.pas:191
        public override HexControl HexControl => Engine.HexSpriteSet.CombatHexes;

        // CombatHex.pas:188
        public override string ResourceEditName() => "Combat Hexagon";

    }
}
