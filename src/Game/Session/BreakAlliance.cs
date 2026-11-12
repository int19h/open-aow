
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

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(GameText.BreakAllianceText);

    public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(AllianceBrokenText);

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
        text.Proposing(109);

    public override void Sent(DiplomaticAction action)
    {
        action.ApplyRelationModifiers();
        StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, (sbyte)(byte)action.Target, DiplomaticRelation.Peace);
        if (action.Source == StrategicMap.Current.SeatedPlayer)
        {
            var allianceBroken = AbilityRules.TranslateResourceString(AllianceBrokenText);
            StrategicMap.Current.ShowMessage(allianceBroken);
        }

        Broadcast(action, -1, action.Target);
    }

    public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race) =>
        player == action.Source && Relation(action.Source, action.Target) == DiplomaticRelation.Alliance
            ? RaceModifier(action.Target, race, -40)
            : 0;

    internal static string AllianceBrokenText { get; } = "Alliance broken";
}
