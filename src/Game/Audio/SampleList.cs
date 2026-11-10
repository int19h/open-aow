// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// every TSample (ProcessSound updates them)
public partial class SampleList : DelphiList
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
        for (int I = Count - 1; I >= 0; I--)
        {
            ((List[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        base.Clear();
    }

}
