// Translated from HSEngine.pas.
using Aow.Game.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// FHSEngine. The component that creates the engine (CreateEngine) and keeps it as
// TEComponent.Engine. OnRegisterClasses and OnUnRegisterClasses are TEComponent's events,
// called from Loaded and Destroy
public partial class HexagonSpriteEngine : EComponent
{
    internal GameEngine FHSEngine;

    // HSEngine.pas:5381
    public HexagonSpriteEngine(Component owner) : base(owner)
    {
        CreateEngine();
        if (FHSEngine != null)
        {
            FHSEngine.FComponent = this;
        }
    }

    // HSEngine.pas:5388
    public virtual void SetHSEngine(GameEngine Value)
    {
        FHSEngine = Value;
        Engine = FHSEngine;
    }

    // HSEngine.pas:5394
    public virtual void CreateEngine()
    {
        SetHSEngine(GameEngine.Create(null));
    }

    // HSEngine.pas:5399
    public void SetHSSetSearchPath(string Value)
    {
        if (FHSEngine != null)
        {
            FHSEngine.FHSSetSearchPath = Value;
        }
    }

    // HSEngine.pas:5404
    public string GetHSSetSearchPath()
    {
        string Result;
        if (FHSEngine != null)
        {
            Result = FHSEngine.FHSSetSearchPath;
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    public GameEngine HSEngine { get => FHSEngine; set => SetHSEngine(value); }
    public string HSSetSearchPath { get => GetHSSetSearchPath(); set => SetHSSetSearchPath(value); }
}
