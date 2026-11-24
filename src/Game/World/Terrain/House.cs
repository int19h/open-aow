#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from House.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Terrain;

public class House(CustomNode owner) : CombatStructure(owner)
{
    public new const int ClassId = 2_229_522;

    private int _race;
    private int _flagId;

    // House.pas:45
    public override void Activate()
    {
        base.Activate();
        if (!CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            PlayerStructure structure = StrategicMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLocation).GetField(CombatPlacement.CombatXLocation, CombatPlacement.CombatYLocation).FindNoneTransparentHexSprite<PlayerStructure>();
            if (structure is not null)
            {
                _race = structure is City city ? city.Race : 0;

                _flagId = structure.Player is null ? -1 : StrategicMap.Current.Players.GetPlayers(structure.Player).PlayerRace;

                if (_flagId == -1)
                {
                    _flagId = 32;
                }
            }
        }
    }

    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        ImageSequence sequence;
        int imageIndex;
        int terrainOffset;
        if (HitPoints == 0)
        {
            sequence = GamePack.Current.ImageSequences.GetImageSequence(50);
            int hexY = HexY;
            int hexX = HexX;
            terrainOffset = CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType switch
            {
                3 => 1,
                5 or 7 or 12 => 2,
                _ => 0,
            };
            if (sequence is not null)
            {
                do
                {
                    imageIndex = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, divisor: 3) + (terrainOffset * 3));
                    if (imageIndex != -1)
                    {
                        sequence.ImageLibrary[imageIndex].Show(pixelXSurface + sequence.OffsetX + 16, pixelYSurface + sequence.OffsetY + 16, lockedSurface);
                    }

                    sequence = sequence.Next;
                }
                while (sequence is not null);
            }
        }
        else
        {
            int hexY = HexY;
            int hexX = HexX;
            terrainOffset = CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType switch
            {
                3 => 20,
                5 or 7 or 12 => 40,
                _ => 0,
            };
            sequence = ((MapObject)this).Resource.ImageSequenceList.GetImageSequence(terrainOffset + _race);
            if (sequence is not null)
            {
                do
                {
                    switch (sequence.Layer)
                    {
                        case 7:
                            imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                            if (imageIndex != -1)
                            {
                                if (!CombatMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
                                {
                                    int hexX2 = HexX;
                                    int hexY2 = HexY;
                                    GamePack.Current.FlagControl.ShowStructureFlag(pixelXSurface + 21 + sequence.OffsetX, pixelYSurface + sequence.OffsetY + 16, frameOffset: hexX2 + StrategicMap.Current.Frame + (hexY2 * 100), flagId: (byte)_flagId, lockedSurface, showBase: false);
                                }
                            }

                            break;
                        default:
                            {
                                imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                                if (imageIndex != -1)
                                {
                                    sequence.ImageLibrary[imageIndex].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                                }
                            }

                            break;
                    }

                    sequence = sequence.Next;
                }
                while (sequence is not null);
            }
        }
    }

    public override void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (HitPoints > 0)
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
            pixelXSurface = pixelX - pixelXSurface;
            pixelYSurface = pixelY - pixelYSurface;
            ImageSequence sequence = ((MapObject)this).Resource.ImageSequenceList.GetImageSequence(_race + 60);
            if (sequence is not null)
            {
                do
                {
                    int imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                    if (imageIndex != -1)
                    {
                        sequence.ImageLibrary[imageIndex].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                    }

                    sequence = sequence.Next;
                }
                while (sequence is not null);
            }
        }
    }

    public new class Resource(CustomNode owner) : CombatStructureResource(owner)
    {

        // House.pas:145
        public new const int ClassId = 2_229_523;

        // House.pas:151
        public override int ResourceUserClassId() => House.ClassId;

        // House.pas:148
        public override string ResourceEditName() => "Combat House";

    }
}
