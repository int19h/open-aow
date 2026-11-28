#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Hexagon.pas and AoWHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Hexes;

// { The Hexagon unit of HSEPack.dpl: the plain terrain hexagon (THexagon, ClassID $20152) drawn from
//   the first image of its resource's image library, with the transitions to the neighbour terrain
//   on three sides from HexTrans.ILB (THexagonResource, ClassID $20153). }
// FTransitions (the transition image of the sides 0.2), FTransitionSides (bits 0.2: the sides
// with a transition, bits 3.5: the shifted variants)
public class Hex(CustomNode owner) : AbstractHexagon(owner)
{
    public new const int ClassId = 131_410;

    protected internal byte[] Transitions { get; } = new byte[3];

    protected internal int TransitionSides { get; private set; }

    // Hexagon.pas:52
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        GetResource<HexagonResource>().Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface, hex: this);
    }

    // Hexagon.pas:74
    public override void UpdateTransitions()
    {
        for (int sideOrdinal = 0; sideOrdinal <= 2; sideOrdinal++)
        {
            int side = sideOrdinal.TruncateToByte();
            UpdateTransition(side);
        }
    }

    // AoWHex.pas:119
    public override void ChangeTerrain(in HexSpriteTerrainInfo message)
    {
        if (message.HexNumber == 0 && (GetTerrain(0) is not { IsCovered: true } terrainCell || terrainCell.Value != message.Terrain))
        {
            int classId = base.Resource.EngineClass.ClassId;
            MapObjectResource r = GetEngine().GamePack.HexControl.GetRandomResource(terrainType: message.TerrainNumber, classId);
            if (r is not null)
            {
                SetResource(r);
                UpdateTransitions();
                GetEngine().StrategicMap.InvalidateStaticScene(Owner.HexX, Owner.HexY, Owner.HexLevel);
            }
            else
            {
                int hexLevel = HexLevel;
                int hexY = HexY;
                int hexX = HexX;
                _ = base.Resource.Engine.GamePack.HexControl.Place(GetEngine().StrategicMap.MapContainer, hexX, hexY, L: hexLevel, terrainType: message.TerrainNumber);
            }
        }
    }

    // AoWHex.pas:139
    public override void TerrainChanged(in HexSpriteTerrainInfo message)
    {
        if (message.Terrain is TerrainType.Water or TerrainType.Ice or TerrainType.Coast)
        {
            if (message.Terrain != TerrainType.Coast)
            {
                int hexLevel = HexLevel;
                int hexY = HexY;
                int hexX = HexX;
                _ = base.Resource.Engine.GamePack.HexControl.Place(GetEngine().StrategicMap.MapContainer, hexX, hexY, L: hexLevel, terrainType: message.TerrainNumber);
            }
            else
            {
                int hexLevel = HexLevel;
                int hexY = HexY;
                int hexX = HexX;
                _ = base.Resource.Engine.GamePack.HexControl.Place(GetEngine().StrategicMap.MapContainer, hexX, hexY, L: hexLevel, terrainType: 0);
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
                default:
                    break;
            }
        }
    }

    // AoWHex.pas:150 (Hexagon.pas:70 is empty)
    public virtual void UpdateTransition(int side)
    {
        int nHexX = default;
        int nHexY = default;
        bool shifted = false;
        Transitions[side] = 255;
        TransitionSides &= SideClear[side];
        MapLevel mapLevel = Owner.Owner;
        HexGeometry.HexNumberToHex(SideHexNumber[side], ref nHexX, ref nHexY);
        HexGeometry.TranslateHex(0, 0, Owner.HexX, Owner.HexY, nHexX, nHexY, out nHexX, out nHexY);
        if (nHexX >= 0 && nHexY >= 0 && mapLevel.Width > nHexX && mapLevel.Height > nHexY)
        {
            // A neighbour without a terrain yet reads -1, as in the original, so the test below skips it
            // while the map is set up (AoWHex.pas:150).
            int neighbourTerrain = mapLevel.GetField(nHexX, nHexY).TryGetTerrain(out TerrainType neighbour) ? neighbour.ToOriginal() : -1;
            int ownTerrain = GetTerrain(0).ToOriginal();
            if (ownTerrain != neighbourTerrain && ownTerrain >= 0 && neighbourTerrain is >= 0 and not 8 and not 7 and not 15 && ownTerrain != 15)
            {
                if (ownTerrain == 9)
                {
                    if (neighbourTerrain is 10 or 13)
                    {
                        int random = Random(3);
                        Transitions[side] = (byte)((random * 3) + side + 182);
                    }
                    else
                    {
                        if (neighbourTerrain is not 8 and not 7)
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
                        if (ownTerrain is 10 or 13)
                        {
                            int random = Random(3);
                            Transitions[side] = (byte)((random * 3) + side + 182);
                        }
                        else
                        {
                            if (ownTerrain is not 8 and not 7)
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
                                        if (neighbourTerrain is not 0 and not 6)
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
                                                    int random = Random(3);
                                                    Transitions[side] = (byte)((random * 3) + (byte)(ownTerrain << 4) + side + 6);
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
            CustomImageLibrary imageLibrary = GetResource<HexagonResource>().TransitionLibrary;
            int image = Transitions[side];
            if (image < imageLibrary.Count)
            {
                if (imageLibrary[image] is not null)
                {
                    TransitionSides = (TransitionSides + (1 << side)).TruncateToByte();
                    if (shifted)
                    {
                        TransitionSides = (TransitionSides + (1 << (side + 3))).TruncateToByte();
                    }
                }
            }
        }
    }

    private static ReadOnlySpan<byte> SideHexNumber => [6, 1, 2];
    private static ReadOnlySpan<byte> SideClear => [246, 237, 219];

    public new class Resource(CustomNode owner) : HexagonResource(owner)
    {

        // AoWHex.pas:254
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            if (message.MessageId == 65556)
            {
                int terrain = TerrainList.Terrain[0].ToOriginal();
                if ((byte)terrain is 0 or 6)
                {
                    return;
                }
            }

            base.MessageProc(ref message, sender);
        }
    }
}
