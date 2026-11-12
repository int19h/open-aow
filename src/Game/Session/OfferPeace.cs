using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Session;

// Kind 1: the source offers the target peace.
public sealed partial record OfferPeace(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
{
    public const byte Kind = 1;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

    public override int Icon => 8;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(s_offerPeaceText);

    public override string LogText(DiplomaticAction action)
    {
        var subject = AbilityRules.TranslateResourceString(s_peaceOfferText);
        var accepted = action.Source == AoWHSMap.Current.SeatedPlayer ? AcceptedLine(subject) : AbilityRules.TranslateResourceString(GameTables.s_peaceText);
        return Outcome(action, subject, subject, accepted, "");
    }

    public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation) =>
        text.Exchange(reaction, relation == DiplomaticRelation.Unknown ? 103 : 154, 127, 130);

    public override void Sent(DiplomaticAction action) => Deliver(action);

    public override bool LapsesUnanswered(DiplomaticAction action) =>
        Relation(action.Source, action.Target) != DiplomaticRelation.Peace;

    public override void Accepted(DiplomaticAction action)
    {
        AoWHSMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, (sbyte)(byte)action.Target, DiplomaticRelation.Peace);
        Broadcast(action, -1, action.Source);
        RemoveTargetLog(action);
    }

    public override DiplomaticActionReaction RejectReaction(DiplomaticAction action) =>
        Relation(action.Source, action.Target) != DiplomaticRelation.Peace ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

    public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
    {
        string errorText = "";
        sbyte source = action.Source;
        sbyte target = action.Target;
        var last = AoWHSMap.Current.Players.GetPlayers(target).DiplomaticActions.FindLastAction(Kind, DiplomaticActionReaction.None, target, source, -1);
        if (idealPeace.IndexOf((byte)source) != -1 || (last != null && AoWHSMap.Current.Day - last.Day <= 1))
        {
            if (AoWHSMap.Current.Players.GetPlayers(target).AIControl.CanOfferPeace((byte)source))
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
            if (AoWHSMap.Current.Players.GetPlayers(target).AIControl.CanOfferPeace((byte)source))
            {
                if (AoWHSMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)source, Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance)) + 1 < AoWHSMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)source, Set<DiplomaticRelation>.Of(DiplomaticRelation.War)))
                {
                    last = AoWHSMap.Current.Players.GetPlayers(target).DiplomaticActions.FindLastAction(Kind, DiplomaticActionReaction.Reject, source, target, -1);
                    if (last == null || AoWHSMap.Current.Day - last.Day > 8)
                    {
                        // { Original bug: escrow gold is counted twice. }
                        if (AoWHSMap.Current.Random(3) == 1 && AoWHSMap.Current.Random(250) + 100 < action.EscrowGold + action.EscrowGold)
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
    }

    public override void RefusedAI(DiplomaticAction action)
    {
        string errorText = "";
        var diplomaticRelation = Relation(action.Source, action.Target);
        if (!Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.War).Contains(diplomaticRelation))
        {
            var last = AoWHSMap.Current.Players.GetPlayers(action.Source).CreateDiplomaticAction(new DeclareWar(), action.Target, -1);
            if (last != null)
            {
                last.Send(ref errorText);
                last.Release();
            }
        }
    }

    public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
    {
        if (Relation(action.Source, action.Target) != DiplomaticRelation.Peace)
        {
            if (player == action.Source)
            {
                return RaceModifier(action.Target, race, 10);
            }

            if (player == action.Target)
            {
                return RaceModifier(action.Source, race, 10);
            }
        }

        return 0;
    }

    internal static readonly string s_offerPeaceText = "Offer peace";
    internal static readonly string s_peaceOfferText = "Peace offer";
}
