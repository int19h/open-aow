// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// every Sample (ProcessSound updates them)
public sealed class SampleList : DelphiList<Sample>
{
    // Sound.pas:710
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Sound.pas:716
    public new void Clear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            ((List[i] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        base.Clear();
    }
}
