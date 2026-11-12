// Translated from Sound.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Audio;

// _fileName. One wave file of a sequence
public partial class Sfx : EngineObject
{
    public new const int ClassId = 2162689;

    public Sfx(ECustomNode owner) : base(owner)
    { }

    private string _fileName = "";

    // Sound.pas:1027
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.rwString(ref _fileName, 5);
        }
    }

    // Sound.pas:1034
    public Sample Play() => AudioPlayback.PlaySample(_fileName);

    // Sound.pas:1039
    public Sample Play(int volume, bool looped, int delay, SamplePanning panning, SampleCallBack callback) => AudioPlayback.PlaySample(_fileName, volume, looped, delay, panning, callback);

    public string FileName { get => _fileName; set => _fileName = value; }

}
