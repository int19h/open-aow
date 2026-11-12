#nullable enable
using Aow.Game.Session;

namespace Aow.Game.Saves;

/// <summary>TDiplomaticAction: the 9.6-7 diplomatic proposals, keyed by the one-byte Kind.</summary>
public sealed class DiplomaticProposalCases : KindCases
{
    public override IEnumerable<string> Members => ["Kind", "Gold", "Mana"];

    public override IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored)
    {
        var proposal = DiplomaticProposal.FromFields((byte)key, (int)stored.GetValueOrDefault("Gold"), (int)stored.GetValueOrDefault("Mana"));
        var (kind, gold, mana) = proposal.ToFields();
        return new Dictionary<string, long> { ["Kind"] = kind, ["Gold"] = gold, ["Mana"] = mana };
    }
}
