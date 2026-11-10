// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (source experience)
public partial class PossessedAbilityData : AbilityData
{
    public new const int ClassID = 131797;

    public PossessedAbilityData(ECustomNode owner) : base(owner)
    { }

    public int FSourceExperience;

    // AoWAb.PAS:2556
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FSourceExperience, 30);
    }

    // AoWAb.PAS:2551
    public override int GetAbilityID() => 107;

    // Engine.pas:524

}
