// Translated from Sound.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Audio;

// _fileName. One wave file of a sequence
public partial class Sfx(CustomNode owner) : EngineObject(owner)
{
    public new const int ClassId = 2162689;

    // Sound.pas:1027
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original ReadWrite leaves out the file name under text only (the translate stream).
        if (!PersistSupport.Translating(persister))
        {
            FileName = persister.String("FileName", value: FileName);
        }
    }

    // Sound.pas:1034
    public Sample Play() => AudioPlayback.PlaySample(FileName);

    // Sound.pas:1039
    public Sample Play(int volume, bool looped, int delay, SamplePanning panning, SampleCallBack callback) => AudioPlayback.PlaySample(FileName, volume, looped, delay, panning, callback);

    public string FileName { get; set; } = "";

}
