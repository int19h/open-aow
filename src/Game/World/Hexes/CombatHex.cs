// Translated from CombatHex.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class CombatHex : AbstractHexagon
{
    public CombatHex(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 2229504;

    private byte _imageVariant;
    private ByteArray3 _transitions;

    // CombatHex.pas:174
    public override void TerrainChanged(in HexSpriteTerrainInfo message)
    {
        if (Set<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)message.TerrainType))
        {
            if (message.TerrainType != 14)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                ((AoWHSSet)_resource.Engine.HexSpriteSet).CombatHexes.Place(CombatMap.Current.MapContainer, hexX, hexY, hexLevel, message.TerrainType);
            }
            else
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                ((AoWHSSet)_resource.Engine.HexSpriteSet).CombatHexes.Place(CombatMap.Current.MapContainer, hexX, hexY, hexLevel, 0);
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
            }
        }
    }

    // CombatHex.pas:148
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        CustomImageLibrary images;
        if (Owner.OverlayType != 4)
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
            pixelX = pixelX - pixelXSurface - 8;
            pixelY = pixelY - pixelYSurface;
            images = _resource.ImageSequenceList.GetImageSequence(0).ImageLibrary;
            images[_imageVariant].Show(pixelX, pixelY, lockedSurface);
            images = ((Resource)_resource).TransitionLibrary;
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
    public override MapObjectResourceList GetResourceList(GameEngine engine) => AoWHSSet.Current.CombatResources;

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
        MapLevel mapLevel;
        int imageIndex;
        sbyte neighbourTerrain;
        sbyte terrainType;
        int X = default;
        int Y = default;
        mapLevel = Owner.Owner;
        HexGeometry.CenterHexNumberToHex(Owner.HexX, Owner.HexY, s_transitionNeighbours[side - 1], ref X, ref Y);
        if (X >= 0 && Y >= 0 && X < mapLevel.Width && Y < mapLevel.Height)
        {
            neighbourTerrain = mapLevel.GetField(X, Y).TerrainType;
            terrainType = _resource.TerrainList.GetTerrain(0);
            imageIndex = -1;
            if (neighbourTerrain != terrainType && neighbourTerrain != 7)
            {
                if (terrainType == 0)
                {
                    if (neighbourTerrain == 6)
                    {
                        imageIndex = 10;
                    }
                    else
                    {
                        imageIndex = neighbourTerrain + 4;
                    }
                }
                else
                {
                    if (terrainType == 6)
                    {
                        if (neighbourTerrain == 0)
                        {
                            imageIndex = 16;
                        }
                        else
                        {
                            imageIndex = neighbourTerrain + 10;
                        }
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
                                    if (terrainType == 9)
                                    {
                                        if (neighbourTerrain == 12)
                                        {
                                            imageIndex = 24;
                                        }
                                        else
                                        {
                                            imageIndex = 25;
                                        }
                                    }
                                    else
                                    {
                                        imageIndex = terrainType - 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (imageIndex != -1)
            {
                imageIndex = Random(2) * 3 + imageIndex * 6 + (side - 1);
                if (((Resource)_resource).TransitionLibrary.GetSafe(imageIndex) != null)
                {
                    _transitions[side - 1] = (byte)imageIndex;
                }
                else
                {
                    _transitions[side - 1] = 255;
                }
            }
            else
            {
                _transitions[side - 1] = 255;
            }
        }
    }

    private static ByteArray3 s_transitionNeighbours = ByteArray3.From((ReadOnlySpan<byte>)new byte[3] { 6, 1, 2 });

    public new partial class Resource : AbstractHexagonResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassId = 2229505;

        public CustomImageLibrary TransitionLibrary { get; private set; }

        // CombatHex.pas:194
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            bool condition = message.MessageId != 65556;
            if (!condition)
            {
                var terrain = TerrainList.GetTerrain(0);
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
                TransitionLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TCombat\\Base_Hex\\CTransHx.ILB");
            }
        }

        // CombatHex.pas:207
        public override LibraryImage GetCellImage()
        {
            LibraryImage result = null;
            if (ImageSequenceList != null && ImageSequenceList.Count > 0 && ImageSequenceList.GetImageSequence(0) != null)
            {
                result = ImageSequenceList.GetImageSequence(0).ImageLibrary[10];
            }

            if (result == null)
            {
                result = base.GetCellImage();
            }

            return result;
        }

        // CombatHex.pas:204
        public override int ResourceUserClassId() => 2229504;

        // CombatHex.pas:191
        public override HexControl GetHexControl() => ((AoWHSSet)Engine.HexSpriteSet).CombatHexes;

        // CombatHex.pas:188
        public override string ResourceEditName() => "Combat Hexagon";

    }

}
