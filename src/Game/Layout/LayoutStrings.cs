#nullable enable
using System.Collections;

using Aow.Rtl;

namespace Aow.Game.Layout;

// This adapter uses the list's getter and batches its changes, as TStrings.ReadData does.
// It also allows collection initializers without changing the frozen shim.
public sealed class LayoutStrings : IEnumerable<string>
{
    private readonly List<string> _values = [];

    public void Add(string value) => _values.Add(value);
    public IEnumerator<string> GetEnumerator() => _values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyTo(StringCollection target)
    {
        target.BeginUpdate();
        try { target.Clear(); foreach (var value in _values) target.Add(value); }
        finally { target.EndUpdate(); }
    }
}
