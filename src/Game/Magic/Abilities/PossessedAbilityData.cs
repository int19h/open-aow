// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (source experience)
public partial class PossessedAbilityData : AbilityData
{
    public new const int ClassId = 131797;

    public PossessedAbilityData(ECustomNode owner) : base(owner)
    { }

    public int SourceExperience;

    // AoWAb.PAS:2556
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref SourceExperience, 30);
    }

    // AoWAb.PAS:2551
    public override int GetAbilityId() => 107;

    // Engine.pas:524

}
