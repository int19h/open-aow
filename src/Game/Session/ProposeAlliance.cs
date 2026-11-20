using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Session;

// Kind 2: the source proposes an alliance to the target.
public sealed record ProposeAlliance(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
{
    public const byte Kind = 2;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

    public override int Icon => 10;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: ProposeAllianceText);

    public override string LogText(DiplomaticAction action)
    {
        string subject = AbilityRules.TranslateResourceString(action.GetEngine(), S: AllianceProposalText);
        return Outcome(accepted: action.Source == StrategicMap.Current.SeatedPlayer ? AcceptedLine(subject, engine: action.GetEngine()) : AbilityRules.TranslateResourceString(action.GetEngine(), S: MadeAllianceText), action: action, proposed: subject, subject: subject, otherwise: subject);
    }

    public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
        => text.Exchange(reaction, proposed: 106, accepted: 121, rejected: 124);

    public override void Sent(DiplomaticAction action) => Deliver(action);

    public override bool LapsesUnanswered(DiplomaticAction action)
        => Relation(action.Source, second: action.Target) != DiplomaticRelation.Alliance;

    public override void Accepted(DiplomaticAction action)
    {
        StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, otherPlayer: (sbyte)(byte)action.Target, relation: DiplomaticRelation.Alliance);
        Broadcast(action, exclude: -1, viewer: action.Source);
        RemoveTargetLog(action);
    }

    public override DiplomaticActionReaction RejectReaction(DiplomaticAction action)
        => Relation(action.Source, second: action.Target) != DiplomaticRelation.Alliance ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

    public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
    {
        string errorText = "";
        if (StrategicMap.Current.Players.GetPlayers(action.Target).AIControl.CanProposeAlliance((byte)action.Source))
        {
            RandSeed = (StrategicMap.Current.InitialRandomSeed + StrategicMap.Current.Day + action.Source + action.Target) >>> 2;
            int chance = ((action.EscrowGold + action.EscrowMana) / 25) + 30;
            if (chance > 60)
            {
                chance = 60;
            }

            _ = Random(100) < chance ? action.Accept(errorText) : action.Reject(errorText);
        }
        else
        {
            _ = action.Reject(errorText);
        }
    }

    public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
    {
        if (Relation(action.Source, second: action.Target) != DiplomaticRelation.Alliance)
        {
            int modifier = Relation(action.Source, second: action.Target) == DiplomaticRelation.War ? 30 : 20;
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
