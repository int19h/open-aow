// Translated from Sound.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Audio;

// _fileName. One wave file of a sequence
public partial class Sfx : EngineObject
{
    public new const int ClassId = 2162689;

    public Sfx(ECustomNode owner) : base(owner)
    { }

    private string _fileName = "";

    // Sound.pas:1027
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original ReadWrite leaves out the file name under text only (the translate stream).
        if (!PersistSupport.Translating(persister))
        {
            _fileName = persister.String("FileName", value: _fileName);
        }
    }

    // Sound.pas:1034
    public Sample Play() => AudioPlayback.PlaySample(_fileName);

    // Sound.pas:1039
    public Sample Play(int volume, bool looped, int delay, SamplePanning panning, SampleCallBack callback) => AudioPlayback.PlaySample(_fileName, volume, looped, delay, panning, callback);

    public string FileName { get => _fileName; set => _fileName = value; }

}
