#nullable enable
using System.Collections;
using Aow.Rtl;
using Aow.Units;
using Aow.Units.ImageLib;

namespace Aow.Layout;

// A data module whose layout is generated. GameLayouts loads each root through the session.
public interface ILayoutModule
{
    void InitializeComponent(LayoutSession session);
}

// The layout code performs local fixups and calls Loaded explicitly. This list holds only
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
        Startup.InitializeSelected(["ImageLib"]);
        BeginRead(root);
    }

    // VCL sets csLoading and csReading before the reader creates a component. The port sets
    // them right after construction. No constructor body reads ComponentState, so the order is
    // not observable. Loaded clears csLoading.
    public void BeginRead(TComponent component) =>
        component.ComponentState = component.ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);

    public void EndRead(TComponent component) =>
        component.ComponentState = component.ComponentState.Without(TComponentState.csReading);

    // The module failure boundary. Application.CreateForm publishes the global before the
    // constructor runs and clears it when the constructor raises, and the failed module is
    // destroyed with every component it owns. Earlier modules stay as they are. OnCreate runs
    // inside InitializeComponent, so its failures reach this boundary as well. Delphi 3 differs
    // here: TDataModule.Create hands an OnCreate exception to Application.HandleException and
    // keeps the module.
    public void Load<T>(T module, Action<T?> publish) where T : TComponent, ILayoutModule
    {
        publish(module);
        try
        {
            module.InitializeComponent(this);
        }
        catch (System.Exception error)
        {
            publish(null);
            try { module.Destroy(); }
            catch (System.Exception cleanup) { throw new AggregateException(error, cleanup); }
            throw;
        }
    }

    public void Defer<T>(string module, Action<T> apply) where T : TComponent
    {
        if (modules.TryGetValue(module, out var ready)) apply((T)ready);
        else pending.Add(new(module, component => apply((T)component)));
    }

    public void Resolve(TComponent root)
    {
        if (!modules.TryAdd(root.Name, root)) throw new InvalidOperationException("Duplicate module " + root.Name);
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
        foreach (var module in modules.Values)
            for (int index = 0; index < module.ComponentCount; index++)
                if (module.Components[index] is TIvCustomTranslator translator) translator.Translate();
    }

    public static void ReadImageData(TCustomImageLibrary library, byte[] data)
    {
        var stream = new TMemoryStream();
        try { stream.WriteBuffer(data); stream.Position = 0; library.LoadFromStream(stream); }
        finally { stream.Free(); }
    }
}


// This adapter uses the list's getter and batches its changes, as TStrings.ReadData does.
// It also allows collection initializers without changing the frozen shim.
public sealed class LayoutStrings : IEnumerable<string>
{
    private readonly List<string> values = [];
    public void Add(string value) => values.Add(value);
    public IEnumerator<string> GetEnumerator() => values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public void CopyTo(TStrings target)
    {
        target.BeginUpdate();
        try { target.Clear(); foreach (var value in values) target.Add(value); }
        finally { target.EndUpdate(); }
    }
}
