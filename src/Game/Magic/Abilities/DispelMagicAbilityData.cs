// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (enabled, level)
public partial class DispelMagicAbilityData : AbilityData
{
    public new const int ClassID = 131789;

    public DispelMagicAbilityData(ECustomNode owner) : base(owner)
    { }

    public bool FEnabled;
    public byte FLevel;

    // AoWAb.PAS:4034
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwBool(ref FEnabled, 10);
        Stream.rwByte(ref FLevel, 11);
    }

    // AoWAb.PAS:4030
    public override int GetAbilityID() => 60;

    // Engine.pas:524

}
