// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (external level, source name)
public class LeadershipAbilityData(CustomNode owner) : MultiLevelAbilityData(owner)
{
    public new const int ClassId = 131_790;

    public int ExternalLevel { get; set; }
    public string ExternalSource { get; set; } = "";

    // AoWAb.PAS:957
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ExternalLevel = persister.Byte("ExternalLevel", value: ExternalLevel.TruncateToByte());
        // The original leaves the external source out under the text-only mode of the translate stream.
        if (!persister.Translating())
        {
            ExternalSource = persister.String("ExternalSource", value: ExternalSource);
        }
    }
}
