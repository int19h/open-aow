// Translated from AoWE.pas.
using Aow.Engine;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P4. }
public partial class AbilityPlugin : Plugin
{
    // AoWAb.PAS:5569
    public override string Name() => s_abilitiesText;

    // AoWAb.PAS:5573
    public override bool Initialize(GameEngine engine)
    {
        bool result;
        if (engine is GameEngine)
        {
            result = base.Initialize(engine);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AoWAb.PAS:5578
    public override bool Finalize(GameEngine engine)
    {
        AddRef();
        bool result = base.Finalize(engine);
        Release();
        return result;
    }

    private static readonly string s_abilitiesText = "Abilities";

    // Engine.pas:550

    // Engine.pas:550

}
