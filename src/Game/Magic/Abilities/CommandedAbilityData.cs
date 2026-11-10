// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (ability ID, commander ID, command ability ID, original combat player)
public partial class CommandedAbilityData : AbilityData
{
    public new const int ClassID = 131757;

    public CommandedAbilityData(ECustomNode owner) : base(owner)
    { }

    public int FAbilityID;
    public byte FSourceID;
    public int FCommandAbilityID;
    public byte FOriginalCombatPlayer;

    // AoWAb.PAS:5005
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAbilityID, 20);
        Stream.rwInteger(ref FCommandAbilityID, 21);
        Stream.rwByte(ref FSourceID, 22);
        Stream.rwByte(ref FOriginalCombatPlayer, 23);
    }

    // AoWAb.PAS:5001
    public override int GetAbilityID() => FAbilityID;

}
