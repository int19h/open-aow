// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (external level, source name)
public partial class LeadershipAbilityData : MultiLevelAbilityData
{
    public new const int ClassID = 131790;

    public LeadershipAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte FExternalLevel;
    public string FExternalSource = "";

    // AoWAb.PAS:957
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref FExternalLevel, 25);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FExternalSource, 26);
        }
    }

    // Engine.pas:524

}
