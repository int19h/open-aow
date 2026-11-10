// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Game.Engine.Nodes;

// FOnRegisterClasses (called from Loaded), FOnUnRegisterClasses (called from Destroy), FEngine,
// the engine the component owns and frees. Descendants such as HSEngine's THexagonSpriteEngine
// publish the two events
public partial class EComponent : Component
{
    internal NotifyEvent<object> FOnRegisterClasses;
    internal NotifyEvent<object> FOnUnRegisterClasses;
    internal GameEngine FEngine;

    // Engine.pas:8642
    public override void Loaded()
    {
        base.Loaded();
        if (FOnRegisterClasses != null)
        {
            FOnRegisterClasses(this);
        }
    }

    protected internal NotifyEvent<object> OnRegisterClasses { get => FOnRegisterClasses; set => FOnRegisterClasses = value; }
    protected internal NotifyEvent<object> OnUnRegisterClasses { get => FOnUnRegisterClasses; set => FOnUnRegisterClasses = value; }

    // Engine.pas:8635
    public override void Dispose()
    {
        if (FOnUnRegisterClasses != null)
        {
            FOnUnRegisterClasses(this);
        }

        FEngine?.Dispose();
        base.Dispose();
    }

    public GameEngine Engine { get => FEngine; set => FEngine = value; }

    public EComponent(Component owner) : base(owner)
    { }
}
