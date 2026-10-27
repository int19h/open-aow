using System.Collections;
using Aow.Rtl;
using Aow.Units.ImageLib;

namespace Aow.Layout;

public interface ILayoutComponent
{
    void EndRead();
}

// The generated code performs local fixups and calls Loaded explicitly. This list holds only
// references to modules that have not finished construction yet.
public sealed class LayoutSession
{
    private sealed record Fixup(string Module, Action<TComponent> Apply);
    private readonly Dictionary<string, TComponent> modules = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Fixup> pending = [];
    public static LayoutSession Default { get; } = new();
    public bool FireCreateEvents { get; init; } = true;
    public IReadOnlyDictionary<string, TComponent> Modules => modules;
    public int PendingFixups => pending.Count;

    public void Begin(TComponent root)
    {
        if (modules.Values.Contains(root)) throw new InvalidOperationException("Layout already initialized");
        // FileName setters read image headers during loading. The registry must exist before
        // the first resource component is built, including callers that load one module.
        Aow.Units.Startup.InitializeSelected(["ImageLib"]);
    }

    public void Defer<T>(string module, Action<T> apply) where T : TComponent
    {
        if (modules.TryGetValue(module, out var ready)) apply((T)ready);
        else pending.Add(new(module, component => apply((T)component)));
    }

    public void Resolve(TComponent root)
    {
        if (!modules.TryAdd(root.Name.ToString(), root)) throw new InvalidOperationException("Duplicate module " + root.Name.ToString());
        for (int i = 0; i < pending.Count;)
        {
            var fixup = pending[i];
            if (modules.TryGetValue(fixup.Module, out var module)) { fixup.Apply(module); pending.RemoveAt(i); }
            else i++;
        }
    }

    public void Complete()
    {
        if (pending.Count != 0) throw new InvalidOperationException($"{pending.Count} unresolved references, first module {pending[0].Module}");
    }

    public static void EndRead(TComponent component) => ((ILayoutComponent)component).EndRead();

    public static void ReadImageData(TCustomImageLibrary library, byte[] data)
    {
        var stream = new TMemoryStream();
        try { stream.WriteBuffer(data); stream.Position = 0; library.LoadFromStream(stream); }
        finally { stream.Free(); }
    }
}

public sealed record TranslationTarget(AnsiString ClassName, AnsiString PropertyName, int Flags);

// This adapter uses the list's getter and batches its changes, as TStrings.ReadData does.
// It also allows collection initializers without changing the frozen shim.
public sealed class LayoutStrings : IEnumerable<AnsiString>
{
    private readonly List<AnsiString> values = [];
    public void Add(AnsiString value) => values.Add(value);
    public IEnumerator<AnsiString> GetEnumerator() => values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public void CopyTo(TStrings target)
    {
        target.BeginUpdate();
        try { target.Clear(); foreach (var value in values) target.Add(value); }
        finally { target.EndUpdate(); }
    }
}
