
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// Kind 6: the source ends its alliance with the target.
public sealed partial record BreakAlliance : DiplomaticProposal
{
    public const byte Kind = 6;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

    public override ProposalView View => ProposalView.Notice;

    public override int Icon => 8;

    public override string Name(DiplomaticAction Action) => AbilityRules.TranslateRStr(GameText.P5BreakAllianceRStr);

    public override string LogText(DiplomaticAction Action) => AbilityRules.TranslateRStr(P5AllianceBrokenRStr);

    public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
        Text.Proposing(109);

    public override void Sent(DiplomaticAction Action)
    {
        Action.ApplyRelationModifiers();
        AoWHSMap.Current.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, DiplomaticRelation.Peace);
        if (Action.FSource == AoWHSMap.Current.FSeatedPlayer)
        {
            var p5AllianceBroken = AbilityRules.TranslateRStr(P5AllianceBrokenRStr);
            AoWHSMap.Current.ShowMessage(p5AllianceBroken);
        }

        Broadcast(Action, -1, Action.FTarget);
    }

    public override int RelationModifier(DiplomaticAction Action, sbyte Player, sbyte Race) =>
        Player == Action.FSource && Relation(Action.FSource, Action.FTarget) == DiplomaticRelation.Alliance
            ? RaceModifier(Action.FTarget, Race, -40)
            : 0;

    internal static readonly string P5AllianceBrokenRStr = "Alliance broken";
}
