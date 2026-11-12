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

    public PlayerMagicControl ExternalPowerControl { get; set; }

    // Spells.PAS:1835
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), s_externalPowerText);

    // Engine.pas:524
    public override int Power() => ExternalPowerControl.ExternalPower;

    private static readonly string s_externalPowerText = "External";
}
