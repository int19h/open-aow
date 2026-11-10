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

    public override string Name(DiplomaticAction Action) => AbilityRules.TranslateRStr(P5ProposeAllianceRStr);

    public override string LogText(DiplomaticAction Action)
    {
        var subject = AbilityRules.TranslateRStr(P5AllianceProposalRStr);
        var accepted = Action.FSource == AoWHSMap.Current.FSeatedPlayer ? AcceptedLine(subject) : AbilityRules.TranslateRStr(P5MadeAllianceRStr);
        return Outcome(Action, subject, subject, accepted, subject);
    }

    public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

    public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
        Text.Exchange(Reaction, 106, 121, 124);

    public override void Sent(DiplomaticAction Action) => Deliver(Action);

    public override bool LapsesUnanswered(DiplomaticAction Action) =>
        Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.Alliance;

    public override void Accepted(DiplomaticAction Action)
    {
        AoWHSMap.Current.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, DiplomaticRelation.Alliance);
        Broadcast(Action, -1, Action.FSource);
        RemoveTargetLog(Action);
    }

    public override DiplomaticActionReaction RejectReaction(DiplomaticAction Action) =>
        Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.Alliance ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

    public override void AnswerAI(DiplomaticAction Action, sbyte Player, DiplomaticRelation Relation, ByteList IdealPeace, ByteList IdealWar)
    {
        string ErrorText = "";
        if (AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).FAIControl.CanProposeAlliance((byte)Action.FSource))
        {
            RandSeed = (int)(((uint)(AoWHSMap.Current.FInitialRandomSeed + AoWHSMap.Current.FDay + Action.FSource + Action.FTarget)) >> 2);
            int Chance = (Action.FEscrowGold + Action.FEscrowMana) / 25 + 30;
            if (Chance > 60)
            {
                Chance = 60;
            }

            if (Random(100) < Chance)
            {
                Action.Accept(ref ErrorText);
            }
            else
            {
                Action.Reject(ref ErrorText);
            }
        }
        else
        {
            Action.Reject(ref ErrorText);
        }
    }

    public override int RelationModifier(DiplomaticAction Action, sbyte Player, sbyte Race)
    {
        if (Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.Alliance)
        {
            int Modifier = Relation(Action.FSource, Action.FTarget) == DiplomaticRelation.War ? 30 : 20;
            if (Player == Action.FSource)
            {
                return RaceModifier(Action.FTarget, Race, Modifier);
            }

            if (Player == Action.FTarget)
            {
                return RaceModifier(Action.FSource, Race, Modifier);
            }
        }

        return 0;
    }

    internal static readonly string P5ProposeAllianceRStr = "Propose alliance";
    internal static readonly string P5AllianceProposalRStr = "Alliance proposal";
    internal static readonly string P5MadeAllianceRStr = "Made alliance";
}
