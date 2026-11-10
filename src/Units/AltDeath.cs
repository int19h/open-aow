// Translated from AltDeath.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;
using static Aow.Game.Magic.Abilities.AoWEUnit;

namespace Aow.Game.World.Structures;

public partial class TAltarOfDeath : TAltar
{
    public TAltarOfDeath(TECustomNode owner) : base(owner)
    { }

    // AltDeath.pas:36
    public new const int ClassID = 131637;

    // AltDeath.pas:39
    public override TStorm CreateStorm() => new TDeathStorm(null);

}

public partial class TAltarOfDeathResource : TAltarResource
{
    public TAltarOfDeathResource(TECustomNode owner) : base(owner)
    {
    }

    // AltDeath.pas:52
    public new const int ClassID = 131638;

    // AltDeath.pas:44

    // AltDeath.pas:55
    public override int ResourceUserClassID() => 131637;

    // AltDeath.pas:26

    // AltDeath.pas:49
    public override string ResourceEditName() => TranslateRStr(AltarOfDeathRStr);

}

public static partial class AltDeathUnit
{
    // AltDeath.pas:58
    public static void RegisterAltarOfDeath(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TAltarOfDeath>(), EClass.Of<TAltarOfDeathResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
