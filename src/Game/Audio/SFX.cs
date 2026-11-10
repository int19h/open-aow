// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Audio;

// FFileName. One wave file of a sequence
public partial class SFX : EObject
{
    public new const int ClassID = 2162689;

    public SFX(ECustomNode owner) : base(owner)
    { }

    internal string FFileName = "";

    // Sound.pas:1027
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            Stream.rwString(ref FFileName, 5);
        }
    }

    // Sound.pas:1034
    public Sample Play() => AudioPlayback.PlaySample(FFileName);

    // Sound.pas:1039
    public Sample PlayEx(int Volume, bool Looped, int Delay, SamplePanning Panning, SampleCallBack CallBack) => AudioPlayback.PlaySampleEx(FFileName, Volume, Looped, Delay, Panning, CallBack);

    public string FileName { get => FFileName; set => FFileName = value; }

}
