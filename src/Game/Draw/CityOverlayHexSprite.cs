#nullable disable
// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Draw;

// City, ModeStartFrame, Mode, ShellAnimation, ShellEndFrame
public partial class CityOverlayHexSprite(CustomNode owner) : SingleHexSprite(owner)
{
    public City City { get; init; }
    public OverlayMode Mode { get; private set; }

    private ShellAnimationPhase _shellAnimation;
    private int _shellEndFrame;

    // City.pas:718
    public override int GetShowPriority() => City.GetShowPriority() + 134217728;

    // City.pas:733
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(City.StoredHexX, City.StoredHexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        if (City.EnchantmentsVisible)
        {
            int intensity;
            int index;
            ImageSequence sequence;
            if (City.Enchantments.Contains(CityEnchantmentId.Warmonger))
            {
                intensity = Math.Abs(((int)(((uint)StrategicMap.Current.Frame) >> 1) % 20) - 10) + 50;
                sequence = City.Resource.ImageSequenceList.ImageSequences[17];
                while (sequence is not null)
                {
                    index = sequence.GetFrameIndex(0, 0);
                    if (sequence == City.Resource.ImageSequenceList.ImageSequences[17])
                    {
                        sequence.ImageLibrary[index].ShowBlended(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
                    }
                    else
                    {
                        sequence.ImageLibrary[index].ShowBlended(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[intensity]);
                    }

                    sequence = sequence.Next;
                }
            }

            if (City.Enchantments.Contains(CityEnchantmentId.GoldRush))
            {
                intensity = Math.Abs(((int)(((uint)StrategicMap.Current.Frame) >> 1) % 20) - 10) + 70;
                sequence = City.Resource.ImageSequenceList.ImageSequences[18];
                while (sequence is not null)
                {
                    if (sequence == City.Resource.ImageSequenceList.ImageSequences[18])
                    {
                        index = sequence.GetFrameIndex(0, 0);
                        if (sequence.ImageLibrary.ImageExist(index))
                        {
                            sequence.ImageLibrary[index].ShowBlended(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
                        }
                    }
                    else
                    {
                        index = sequence.GetFrameIndex(0, StrategicMap.Current.Frame);
                        if (sequence.ImageLibrary.ImageExist(index))
                        {
                            sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                        }
                    }

                    sequence = sequence.Next;
                }
            }

            if (City.Enchantments.Contains(CityEnchantmentId.AntiMagicShell))
            {
                sequence = City.Resource.ImageSequenceList.ImageSequences[19];
                if (sequence is not null)
                {
                    intensity = 50;
                    index = sequence.GetFrameIndex(0, 0);
                    if (_shellEndFrame != -1 && StrategicMap.Current.Frame < _shellEndFrame)
                    {
                        int frame;
                        switch (_shellAnimation)
                        {
                            case ShellAnimationPhase.Setup:
                                frame = _shellEndFrame - StrategicMap.Current.Frame;
                                if (frame <= 50)
                                {
                                    intensity = 50 - frame;
                                }
                                else
                                {
                                    _shellEndFrame = -1;
                                }

                                break;
                            case ShellAnimationPhase.Fade:
                                frame = _shellEndFrame - StrategicMap.Current.Frame;
                                if (frame <= 20)
                                {
                                    intensity += (10 - Math.Abs(frame - 10)) * 3;
                                }
                                else
                                {
                                    _shellEndFrame = -1;
                                }

                                break;
                            default:
                                break;
                        }
                    }

                    sequence.ImageLibrary[index].ShowBlended(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[intensity]);
                }
            }

            if (Mode == OverlayMode.Looting)
            {
                City.Resource.ShowComplexImage(pixelXSurface, pixelYSurface, lockedSurface, 22, (uint)(City.Owner.HexX + (City.Owner.HexY * 16)));
            }
        }
    }

    // City.pas:721
    public void PlaySetupShellAnimation()
    {
        _shellAnimation = ShellAnimationPhase.Setup;
        _shellEndFrame = StrategicMap.Current.Frame + 50;
    }

    // City.pas:727
    public void PlayFadeShellAnimation()
    {
        _shellAnimation = ShellAnimationPhase.Fade;
        _shellEndFrame = StrategicMap.Current.Frame + 20;
    }

    // City.pas:815
    public void SetMode(OverlayMode mode)
    {
        if (mode != Mode)
        {
            Mode = mode;
        }
    }

    public enum ShellAnimationPhase : byte
    {
        Setup = 0, Fade = 1
    }

    public enum OverlayMode : byte
    {
        Idle = 0, ShowRazeAnimation = 1, Looting = 2
    }

    // Engine.pas:524

}
