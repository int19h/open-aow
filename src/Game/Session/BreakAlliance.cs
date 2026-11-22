#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// Kind 6: the source ends its alliance with the target.
public sealed record BreakAlliance : DiplomaticProposal
{
    public const byte Kind = 6;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

    public override ProposalView View => ProposalView.Notice;

    public override int Icon => 8;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: GameText.BreakAllianceText);

    public override string LogText(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: AllianceBrokenText);

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
        => text.Proposing(109);

    public override void Sent(DiplomaticAction action)
    {
        action.ApplyRelationModifiers();
        StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, otherPlayer: (sbyte)(byte)action.Target, relation: DiplomaticRelation.Peace);
        if (action.Source == StrategicMap.Current.SeatedPlayer)
        {
            string allianceBroken = AbilityRules.TranslateResourceString(action.GetEngine(), S: AllianceBrokenText);
            StrategicMap.Current.ShowMessage(allianceBroken);
        }

        Broadcast(action, exclude: -1, viewer: action.Target);
    }

    public override int RelationModifier(DiplomaticAction action, sbyte player, int race)
        => player == action.Source && Relation(action.Source, second: action.Target) == DiplomaticRelation.Alliance
            ? RaceModifier(action.Target, race, modifier: -40)
            : 0;

    internal static string AllianceBrokenText { get; } = "Alliance broken";
}
