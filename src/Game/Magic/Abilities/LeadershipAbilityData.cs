// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Magic.Abilities;

// (external level, source name)
public partial class LeadershipAbilityData : MultiLevelAbilityData
{
    public new const int ClassId = 131790;

    public LeadershipAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte ExternalLevel;
    public string ExternalSource = "";

    // AoWAb.PAS:957
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref ExternalLevel, 25);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.String(ref ExternalSource, 26);
        }
    }

    // Engine.pas:524

}
