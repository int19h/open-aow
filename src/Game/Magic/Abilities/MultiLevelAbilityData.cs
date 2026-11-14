// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

namespace Aow.Game.Magic.Abilities;

// (level, ability ID)
public partial class MultiLevelAbilityData : AbilityData
{
    public new const int ClassId = 139265;

    public MultiLevelAbilityData(ECustomNode owner) : base(owner)
    { }

    public byte Level { get; set; }
    public ushort LevelAbilityId { get; set; }

    // AoWAb.PAS:231
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Level = persister.Byte("Level", value: Level);
        LevelAbilityId = persister.UInt16("LevelAbilityId", value: LevelAbilityId);
    }

    // AoWAb.PAS:243
    public override int GetAbilityId() => LevelAbilityId;

    // Engine.pas:524

}
