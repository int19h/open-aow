// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (enabled)
public partial class HealingAbilityData : AbilityData
{
    public new const int ClassID = 131758;

    public HealingAbilityData(ECustomNode owner) : base(owner)
    { }

    public bool FEnabled;

    // AoWAb.PAS:3611
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FEnabled, 10);
    }

    // AoWAb.PAS:3607
    public override int GetAbilityID() => 47;

    // Engine.pas:524

}
