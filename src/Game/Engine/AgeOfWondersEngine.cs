// Translated from AoWE.pas.
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Engine;

// FGameEngine
public partial class AgeOfWondersEngine : HexagonSpriteEngine
{

    public AgeOfWondersEngine(Component owner) : base(owner)
    { }

    public GameEngine FGameEngine;

    // Army.PAS:6104
    public override void Dispose()
    {
        FGameEngine = null;
        Engine = null;
        base.Dispose();
    }

    // Army.PAS:6092
    public override void SetHSEngine(GameEngine Value)
    {
        base.SetHSEngine(Value);
        FGameEngine = Value;
    }

    // Army.PAS:6098
    public override void CreateEngine()
    {
        GameTables.ValidateAoWEngine();
        if (GameTables.AoWEngine != null)
        {
            SetHSEngine(GameTables.AoWEngine);
        }
    }
}
