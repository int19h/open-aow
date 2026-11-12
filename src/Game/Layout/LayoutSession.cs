#nullable enable
using Aow.Game.Draw.Images;

using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Layout;

// The layout code performs local fixups and calls Loaded explicitly. This list holds only
// references to modules that have not finished construction yet.
public sealed class LayoutSession
{
    private sealed record Fixup(string Module, Action<Component> Apply);

    private readonly Dictionary<string, Component> _modules = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Fixup> _pending = [];

    public static LayoutSession Default { get; } = new();
    public bool FireCreateEvents { get; init; } = true;
    public IReadOnlyDictionary<string, Component> Modules => _modules;
    public int PendingFixups => _pending.Count;

    public void Begin(Component root)
    {
        if (_modules.Values.Contains(root)) throw new InvalidOperationException("Layout already initialized");
        // FileName setters read image headers during loading. The registry must exist before
        // the first resource component is built, including callers that load one module.
        Startup.InitializeSelected(["ImageLib"]);
        BeginRead(root);
    }

    // VCL sets csLoading and csReading before the reader creates a component. The port sets
    // them right after construction. No constructor body reads ComponentState, so the order is
    // not observable. Loaded clears csLoading.
    public void BeginRead(Component component) =>
        component.ComponentState = component.ComponentState.With(ComponentState.Loading).With(ComponentState.Reading);

    public void EndRead(Component component) =>
        component.ComponentState = component.ComponentState.Without(ComponentState.Reading);

    // The module failure boundary. Application.CreateForm publishes the global before the
    // constructor runs and clears it when the constructor raises, and the failed module is
    // destroyed with every component it owns. Earlier modules stay as they are. OnCreate runs
    // inside InitializeComponent, so its failures reach this boundary as well. Delphi 3 differs
    // here: TDataModule.Create hands an OnCreate exception to Application.HandleException and
    // keeps the module.
    public void Load<T>(T module, Action<T?> publish) where T : Component, ILayoutModule
    {
        publish(module);
        try
        {
            module.InitializeComponent(this);
        }
        catch (Exception error)
        {
            publish(null);
            try { module.Dispose(); }
            catch (Exception cleanup) { throw new AggregateException(error, cleanup); }
            throw;
        }
    }

    public void Defer<T>(string module, Action<T> apply) where T : Component
    {
        if (_modules.TryGetValue(module, out var ready)) apply((T)ready);
        else _pending.Add(new(module, component => apply((T)component)));
    }

    public void Resolve(Component root)
    {
        if (!_modules.TryAdd(root.Name, root)) throw new InvalidOperationException("Duplicate module " + root.Name);
        for (int i = 0; i < _pending.Count;)
        {
            var fixup = _pending[i];
            if (_modules.TryGetValue(fixup.Module, out var module)) { fixup.Apply(module); _pending.RemoveAt(i); }
            else i++;
        }
    }

    public void Complete()
    {
        if (_pending.Count != 0) throw new InvalidOperationException($"{_pending.Count} unresolved references, first module {_pending[0].Module}");
        foreach (var module in _modules.Values)
            for (int index = 0; index < module.ComponentCount; index++)
                if (module.Components[index] is IvCustomTranslator translator) translator.Translate();
    }

    public static void ReadImageData(CustomImageLibrary library, byte[] data)
    {
        using var stream = new MemoryStream();
        stream.WriteBuffer(data); stream.Position = 0; library.LoadFromStream(stream);
    }
}
