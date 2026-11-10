// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

// FID. A sound effect: its children are TSFX, one of them plays (at random)
public partial class SFXSequence : ENode
{
    public new const int ClassID = 2162690;

    public SFXSequence(ECustomNode owner) : base(owner)
    { }

    internal int FID;

    // Sound.pas:1057
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FID, 5);
    }

    // Sound.pas:1052
    public SFX GetSFX(int Index) => (SFX)this.FChildren.List[Index];

    // Sound.pas:1063
    public Sample Play()
    {
        Sample Result;
        if (this.FChildren.Count != 0)
        {
            var random = Random(this.FChildren.Count);
            Result = ((SFX)this.FChildren.List[random]).Play();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1071
    public Sample PlayEx(int Volume, bool Looped, int Delay, SamplePanning Panning, SampleCallBack CallBack)
    {
        Sample Result = default;
        if (this.FChildren.Count != 0)
        {
            var random = Random(this.FChildren.Count);
            Result = ((SFX)this.FChildren.List[random]).PlayEx(Volume, Looped, Delay, Panning, CallBack);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public int ID { get => FID; set => FID = value; }

    public sealed class SFXAccessor(SFXSequence target)
    {
        public SFX this[int Index] { get => target.GetSFX(Index); }
    }

    public SFXAccessor SFX => new SFXAccessor(this);

}
