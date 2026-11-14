// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (external level, source name)
public partial class LeadershipAbilityData : MultiLevelAbilityData
{
    public new const int ClassId = 131790;

    public LeadershipAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte ExternalLevel { get; set; }
    public string ExternalSource { get; set; } = "";

    // AoWAb.PAS:957
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ExternalLevel = persister.Byte("ExternalLevel", value: ExternalLevel);
        ExternalSource = persister.String("ExternalSource", value: ExternalSource);
    }

    // Engine.pas:524

}
