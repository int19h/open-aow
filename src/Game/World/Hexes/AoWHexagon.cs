// Translated from AoWHex.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

// inherited transition images [0.2], side flags
public partial class AoWHexagon : Hexagon
{
    public AoWHexagon(ECustomNode owner) : base(owner)
    { }

    // AoWHex.pas:119
    public override void ChangeTerrain(in HexSpriteTerrainInfo message)
    {
        MapObjectResource R;
        if (message.HexNumber == 0 && GetTerrain(0) != message.TerrainType)
        {
            var classId = _resource.EngineClass.ClassId;
            R = AoWHSSet.Current.HexControl.GetRandomResource(message.TerrainType, classId);
            if (R != null)
            {
                SetResource(R);
                UpdateTransitions();
                AoWHSMap.Current.InvalidateStaticScene(Owner.HexX, Owner.HexY, Owner.HexLevel);
            }
            else
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                _resource.Engine.HexSpriteSet.HexControl.Place(AoWHSMap.Current.MapContainer, hexX, hexY, hexLevel, message.TerrainType);
            }
        }
    }

    // AoWHex.pas:139
    public override void TerrainChanged(in HexSpriteTerrainInfo message)
    {
        if (Set<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)message.TerrainType))
        {
            if (message.TerrainType != 14)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                _resource.Engine.HexSpriteSet.HexControl.Place(AoWHSMap.Current.MapContainer, hexX, hexY, hexLevel, message.TerrainType);
            }
            else
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                _resource.Engine.HexSpriteSet.HexControl.Place(AoWHSMap.Current.MapContainer, hexX, hexY, hexLevel, 0);
            }
        }
    }

    // AoWHex.pas:109
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo info)
    {
        if (hexNumber == 0)
        {
            switch (info.HexNumber)
            {
                case 6:
                    UpdateTransition(0);
                    break;
                case 1:
                    UpdateTransition(1);
                    break;
                case 2:
                    UpdateTransition(2);
                    break;
            }
        }
    }

    // AoWHex.pas:150
    public override void UpdateTransition(byte side)
    {
        bool shifted;
        int nHexX = default;
        int nHexY = default;
        MapLevel mapLevel;
        int neighbourTerrain;
        int ownTerrain;
        CustomImageLibrary imageLibrary;
        int image;
        shifted = false;
        Transitions[side] = 255;
        TransitionSides = (byte)(TransitionSides & s_sideClear[side]);
        mapLevel = Owner.Owner;
        HexGeometry.HexNumberToHex(s_sideHexNumber[side], ref nHexX, ref nHexY);
        HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, nHexX, nHexY, ref nHexX, ref nHexY);
        if (nHexX >= 0 && nHexY >= 0 && mapLevel.Width > nHexX && mapLevel.Height > nHexY)
        {
            neighbourTerrain = mapLevel.GetField(nHexX, nHexY).TerrainType;
            ownTerrain = GetTerrain(0);
            if (ownTerrain != neighbourTerrain && ownTerrain >= 0 && neighbourTerrain >= 0 && neighbourTerrain != 8 && neighbourTerrain != 7 && neighbourTerrain != 15 && ownTerrain != 15)
            {
                if (ownTerrain == 9)
                {
                    if (neighbourTerrain == 10 || neighbourTerrain == 13)
                    {
                        var random = Random(3);
                        Transitions[side] = (byte)(random * 3 + (side + 182));
                    }
                    else
                    {
                        if (neighbourTerrain != 8 && neighbourTerrain != 7)
                        {
                            Transitions[side] = (byte)(side + 147);
                            shifted = true;
                        }
                    }
                }
                else
                {
                    if (neighbourTerrain == 9)
                    {
                        if (ownTerrain == 10 || ownTerrain == 13)
                        {
                            var random = Random(3);
                            Transitions[side] = (byte)(random * 3 + (side + 182));
                        }
                        else
                        {
                            if (ownTerrain != 8 && ownTerrain != 7)
                            {
                                Transitions[side] = (byte)(side + 144);
                            }
                        }
                    }
                    else
                    {
                        if (ownTerrain == 13)
                        {
                            if (neighbourTerrain == 10)
                            {
                                Transitions[side] = (byte)(side + 208);
                            }
                            else
                            {
                                Transitions[side] = (byte)(side + 217);
                                shifted = true;
                            }
                        }
                        else
                        {
                            if (neighbourTerrain == 13)
                            {
                                if (ownTerrain == 10)
                                {
                                    Transitions[side] = (byte)(side + 211);
                                    shifted = true;
                                }
                                else
                                {
                                    Transitions[side] = (byte)(side + 214);
                                }
                            }
                            else
                            {
                                if (ownTerrain == 10)
                                {
                                    Transitions[side] = (byte)(side + 163);
                                    shifted = true;
                                }
                                else
                                {
                                    if (neighbourTerrain == 10)
                                    {
                                        Transitions[side] = (byte)(side + 160);
                                    }
                                    else
                                    {
                                        if (neighbourTerrain != 0 && neighbourTerrain != 6)
                                        {
                                            if (ownTerrain == 6)
                                            {
                                                // { Original unreachable branch retained: the outer condition excludes terrain 0. }
                                                if (neighbourTerrain == 0)
                                                {
                                                    Transitions[side] = (byte)(side + 96);
                                                }
                                                else
                                                {
                                                    Transitions[side] = (byte)((byte)(neighbourTerrain << 4) + 3 + side);
                                                    shifted = true;
                                                }
                                            }
                                            else
                                            {
                                                if (ownTerrain == 0)
                                                {
                                                    Transitions[side] = (byte)(side + 3);
                                                    shifted = true;
                                                }
                                                else
                                                {
                                                    var random = Random(3);
                                                    Transitions[side] = (byte)(random * 3 + ((byte)(ownTerrain << 4) + side + 6));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Transitions[side] != 255)
        {
            imageLibrary = ((HexagonResource)_resource).TransitionLibrary;
            image = Transitions[side];
            if (image < imageLibrary.Count)
            {
                if (imageLibrary[image] != null)
                {
                    TransitionSides = (byte)(TransitionSides + (1 << side));
                    if (shifted)
                    {
                        TransitionSides = (byte)(TransitionSides + (1 << (side + 3)));
                    }
                }
            }
        }
    }

    private static ByteArray3 s_sideHexNumber = ByteArray3.From((ReadOnlySpan<byte>)new byte[3] { 6, 1, 2 });
    private static ByteArray3 s_sideClear = ByteArray3.From((ReadOnlySpan<byte>)new byte[3] { 246, 237, 219 });

    public new partial class Resource : HexagonResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // AoWHex.pas:254
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            if (message.MessageId == 65556)
            {
                var terrain = TerrainList.Terrain[0];
                if (Set<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)terrain))
                {
                    return;
                }
            }

            base.MessageProc(ref message, sender);
        }
    }
}
