// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (enabled)
public partial class HealingAbilityData : AbilityData
{
    public new const int ClassId = 131758;

    public HealingAbilityData(ECustomNode owner) : base(owner)
    { }

    public bool Enabled;

    // AoWAb.PAS:3611
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwBool(ref Enabled, 10);
    }

    // AoWAb.PAS:3607
    public override int GetAbilityId() => 47;

    // Engine.pas:524

}
