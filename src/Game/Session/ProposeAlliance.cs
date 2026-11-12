using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// Kind 2: the source proposes an alliance to the target.
public sealed partial record ProposeAlliance(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
{
    public const byte Kind = 2;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

    public override int Icon => 10;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(ProposeAllianceText);

    public override string LogText(DiplomaticAction action)
    {
        var subject = AbilityRules.TranslateResourceString(AllianceProposalText);
        var accepted = action.Source == AoWHSMap.Current.SeatedPlayer ? AcceptedLine(subject) : AbilityRules.TranslateResourceString(MadeAllianceText);
        return Outcome(action, subject, subject, accepted, subject);
    }

    public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
        text.Exchange(reaction, 106, 121, 124);

    public override void Sent(DiplomaticAction action) => Deliver(action);

    public override bool LapsesUnanswered(DiplomaticAction action) =>
        Relation(action.Source, action.Target) != DiplomaticRelation.Alliance;

    public override void Accepted(DiplomaticAction action)
    {
        AoWHSMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, (sbyte)(byte)action.Target, DiplomaticRelation.Alliance);
        Broadcast(action, -1, action.Source);
        RemoveTargetLog(action);
    }

    public override DiplomaticActionReaction RejectReaction(DiplomaticAction action) =>
        Relation(action.Source, action.Target) != DiplomaticRelation.Alliance ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

    public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
    {
        string errorText = "";
        if (AoWHSMap.Current.Players.GetPlayers(action.Target).AIControl.CanProposeAlliance((byte)action.Source))
        {
            RandSeed = (int)(((uint)(AoWHSMap.Current.InitialRandomSeed + AoWHSMap.Current.Day + action.Source + action.Target)) >> 2);
            int chance = (action.EscrowGold + action.EscrowMana) / 25 + 30;
            if (chance > 60)
            {
                chance = 60;
            }

            if (Random(100) < chance)
            {
                action.Accept(ref errorText);
            }
            else
            {
                action.Reject(ref errorText);
            }
        }
        else
        {
            action.Reject(ref errorText);
        }
    }

    public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
    {
        if (Relation(action.Source, action.Target) != DiplomaticRelation.Alliance)
        {
            int modifier = Relation(action.Source, action.Target) == DiplomaticRelation.War ? 30 : 20;
            if (player == action.Source)
            {
                return RaceModifier(action.Target, race, modifier);
            }

            if (player == action.Target)
            {
                return RaceModifier(action.Source, race, modifier);
            }
        }

        return 0;
    }

    internal static string ProposeAllianceText { get; } = "Propose alliance";
    internal static string AllianceProposalText { get; } = "Alliance proposal";
    internal static string MadeAllianceText { get; } = "Made alliance";
}
