// Translated from AoWE.pas.
using Aow.Engine;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P4. }
public partial class AbilityPlugIn : PlugIn
{
    // AoWAb.PAS:5569
    public override string Name() => P4AbilitiesRStr;

    // AoWAb.PAS:5573
    public override bool Initialize(GameEngine AEngine)
    {
        bool Result;
        if (AEngine is GameEngine)
        {
            Result = base.Initialize(AEngine);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:5578
    public override bool Finalize(GameEngine AEngine)
    {
        AddRef();
        bool Result = base.Finalize(AEngine);
        Release();
        return Result;
    }

    private static readonly string P4AbilitiesRStr = "Abilities";

    // Engine.pas:550

    // Engine.pas:550

}
