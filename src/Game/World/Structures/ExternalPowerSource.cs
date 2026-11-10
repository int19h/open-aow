// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

namespace Aow.Game.World.Structures;

// (external power control)
public partial class ExternalPowerSource : PowerSource
{
    public ExternalPowerSource(ECustomNode owner) : base(owner)
    { }

    public PlayerMagicControl FExternalPowerControl;

    // Spells.PAS:1835
    public override string Name() => AbilityRules.TranslateRStr(P4ExternalPowerRStr);

    // Engine.pas:524
    public override int Power() => FExternalPowerControl.FExternalPower;

    private static readonly string P4ExternalPowerRStr = "External";
}
