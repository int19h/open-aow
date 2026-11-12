// Translated from Engine.pas.
using Aow.Game;
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// _onRegisterClasses (called from Loaded), _onUnregisterClasses (called from Destroy), _engine,
// the engine the component owns and frees.
public partial class EngineComponent : Component
{
    private NotifyEvent<object> _onRegisterClasses;
    private NotifyEvent<object> _onUnregisterClasses;
    private GameEngine _engine;

    // Engine.pas:8642
    public override void Loaded()
    {
        base.Loaded();
        if (_onRegisterClasses != null)
        {
            _onRegisterClasses(this);
        }
    }

    protected internal NotifyEvent<object> OnRegisterClasses { get => _onRegisterClasses; set => _onRegisterClasses = value; }
    protected internal NotifyEvent<object> OnUnregisterClasses { get => _onUnregisterClasses; set => _onUnregisterClasses = value; }

    // Engine.pas:8635
    public override void Dispose()
    {
        if (_onUnregisterClasses != null)
        {
            _onUnregisterClasses(this);
        }

        _engine?.Dispose();
        base.Dispose();
    }

    public GameEngine Engine { get => _engine; set => _engine = value; }

    public EngineComponent(Component owner) : base(owner)
    { }
}
