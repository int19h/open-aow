// Translated from House.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Terrain;

public partial class House : CombatStructure
{
    public House(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 2229522;

    private int _race;
    private int _flagId;

    // House.pas:45
    public override void Activate()
    {
        PlayerStructure structure;
        base.Activate();
        if (!CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            structure = (PlayerStructure)StrategicMap.Current.MapContainer.GetMapLevel(CombatPlacement.CombatLLocation).GetField(CombatPlacement.CombatXLocation, CombatPlacement.CombatYLocation).FindNoneTransparentHexSprite(typeof(PlayerStructure));
            if (structure != null)
            {
                _race = structure is City ? ((City)structure).Race : 0;

                _flagId = structure.Player == -1 ? -1 : StrategicMap.Current.Players.GetPlayers(structure.Player).PlayerRace;

                if (_flagId == -1)
                {
                    _flagId = 32;
                }
            }
        }
    }

    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int imageIndex;
        int terrainOffset;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        if (HitPoints == 0)
        {
            sequence = GamePack.Current.ImageSequences.GetImageSequence(50);
            sbyte hexY = GetHexY();
            sbyte hexX = GetHexX();
            switch (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType)
            {
                case 3:
                    terrainOffset = 1;
                    break;
                case 5:
                case 7:
                case 12:
                    terrainOffset = 2;
                    break;
                default:
                    terrainOffset = 0;
                    break;
            }

            if (sequence != null)
            {
                do
                {
                    imageIndex = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, 3) + (terrainOffset * 3));
                    if (imageIndex != -1)
                    {
                        sequence.ImageLibrary[imageIndex].Show(pixelXSurface + sequence.OffsetX + 16, pixelYSurface + sequence.OffsetY + 16, lockedSurface);
                    }

                    sequence = sequence.Next;
                }
                while (!(sequence == null));
            }
        }
        else
        {
            sbyte hexY = GetHexY();
            sbyte hexX = GetHexX();
            switch (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType)
            {
                case 3:
                    terrainOffset = 20;
                    break;
                case 5:
                case 7:
                case 12:
                    terrainOffset = 40;
                    break;
                default:
                    terrainOffset = 0;
                    break;
            }

            sequence = ((MapObject)this).Resource.ImageSequenceList.GetImageSequence(terrainOffset + _race);
            if (sequence != null)
            {
                do
                {
                    switch (sequence.Layer)
                    {
                        case 7:
                            imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (imageIndex != -1)
                            {
                                if (!CombatMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
                                {
                                    sbyte hexX2 = GetHexX();
                                    sbyte hexY2 = GetHexY();
                                    GamePack.Current.FlagControl.ShowStructureFlag(pixelXSurface + 21 + sequence.OffsetX, pixelYSurface + sequence.OffsetY + 16, hexX2 + StrategicMap.Current.Frame + (hexY2 * 100), (byte)_flagId, lockedSurface, false);
                                }
                            }

                            break;
                        default:
                            {
                                imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                                if (imageIndex != -1)
                                {
                                    sequence.ImageLibrary[imageIndex].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                                }
                            }

                            break;
                    }

                    sequence = sequence.Next;
                }
                while (!(sequence == null));
            }
        }
    }

    public override void ShowShadow(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int imageIndex;
        int pixelX = default;
        int pixelY = default;
        if (HitPoints > 0)
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out pixelX, out pixelY);
            pixelXSurface = pixelX - pixelXSurface;
            pixelYSurface = pixelY - pixelYSurface;
            sequence = ((MapObject)this).Resource.ImageSequenceList.GetImageSequence(_race + 60);
            if (sequence != null)
            {
                do
                {
                    imageIndex = sequence.GetFrameIndex(CombatMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (imageIndex != -1)
                    {
                        sequence.ImageLibrary[imageIndex].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                    }

                    sequence = sequence.Next;
                }
                while (!(sequence == null));
            }
        }
    }

    public new partial class Resource : CombatStructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        { }

        // House.pas:145
        public new const int ClassId = 2229523;

        // House.pas:151
        public override int ResourceUserClassId() => 2229522;

        // House.pas:148
        public override string ResourceEditName() => "Combat House";

    }
}
