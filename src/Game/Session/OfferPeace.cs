using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// Kind 1: the source offers the target peace.
public sealed record OfferPeace(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
{
    public const byte Kind = 1;

    public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

    public override int Icon => 8;

    public override string Name(DiplomaticAction action) => AbilityRules.TranslateResourceString(action.GetEngine(), S: OfferPeaceText);

    public override string LogText(DiplomaticAction action)
    {
        string subject = AbilityRules.TranslateResourceString(action.GetEngine(), S: PeaceOfferText);
        return Outcome(accepted: action.Source == StrategicMap.Current.SeatedPlayer ? AcceptedLine(subject, engine: action.GetEngine()) : AbilityRules.TranslateResourceString(action.GetEngine(), S: GameTables.PeaceText), action: action, proposed: subject, subject: subject, otherwise: "");
    }

    public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
        => text.Exchange(reaction, proposed: relation == DiplomaticRelation.Unknown ? 103 : 154, accepted: 127, rejected: 130);

    public override void Sent(DiplomaticAction action) => Deliver(action);

    public override bool LapsesUnanswered(DiplomaticAction action)
        => Relation(action.Source, second: action.Target) != DiplomaticRelation.Peace;

    public override void Accepted(DiplomaticAction action)
    {
        StrategicMap.Current.Players.SetDiplomaticRelation((sbyte)(byte)action.Source, otherPlayer: (sbyte)(byte)action.Target, relation: DiplomaticRelation.Peace);
        Broadcast(action, exclude: -1, viewer: action.Source);
        RemoveTargetLog(action);
    }

    public override DiplomaticActionReaction RejectReaction(DiplomaticAction action)
        => Relation(action.Source, second: action.Target) != DiplomaticRelation.Peace ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

    public override void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
    {
        string errorText = "";
        sbyte source = action.Source;
        sbyte target = action.Target;
        DiplomaticAction last = StrategicMap.Current.Players.GetPlayers(target).DiplomaticActions.FindLastAction(Kind, reaction: DiplomaticActionReaction.None, source: target, target: source, other: -1);
        if (idealPeace.IndexOf((byte)source) != -1 || (last is not null && StrategicMap.Current.Day - last.Day <= 1))
        {
            _ = StrategicMap.Current.Players.GetPlayers(target).AIControl.CanOfferPeace((byte)source)
                ? action.Accept(errorText)
                : action.Reject(errorText);
        }
        else
        {
            if (StrategicMap.Current.Players.GetPlayers(target).AIControl.CanOfferPeace((byte)source))
            {
                if (StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)source, relations: DiplomaticRelations.Peace | DiplomaticRelations.Alliance) + 1 < StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)source, relations: DiplomaticRelations.War))
                {
                    last = StrategicMap.Current.Players.GetPlayers(target).DiplomaticActions.FindLastAction(Kind, reaction: DiplomaticActionReaction.Reject, source, target, other: -1);
                    if (last is null || StrategicMap.Current.Day - last.Day > 8)
                    {
                        // { Original bug: escrow gold is counted twice. }
                        _ = StrategicMap.Current.Random(3) == 1 && StrategicMap.Current.Random(250) + 100 < action.EscrowGold + action.EscrowGold
                            ? action.Accept(errorText)
                            : action.Reject(errorText);
                    }
                    else
                    {
                        _ = action.Reject(errorText);
                    }
                }
                else
                {
                    _ = action.Reject(errorText);
                }
            }
            else
            {
                _ = action.Reject(errorText);
            }
        }
    }

    public override void RefusedAI(DiplomaticAction action)
    {
        DiplomaticRelation diplomaticRelation = Relation(action.Source, second: action.Target);
        if (diplomaticRelation is not (DiplomaticRelation.Peace or DiplomaticRelation.War))
        {
            DiplomaticAction last = StrategicMap.Current.Players.GetPlayers(action.Source).CreateDiplomaticAction(new DeclareWar(), action.Target, other: -1);
            if (last is not null)
            {
                string errorText = "";
                _ = last.Send(ref errorText);
                _ = last.Release();
            }
        }
    }

    public override int RelationModifier(DiplomaticAction action, sbyte player, sbyte race)
    {
        if (Relation(action.Source, second: action.Target) != DiplomaticRelation.Peace)
        {
            if (player == action.Source)
            {
                return RaceModifier(action.Target, race, modifier: 10);
            }

            if (player == action.Target)
            {
                return RaceModifier(action.Source, race, modifier: 10);
            }
        }

        return 0;
    }

    internal static string OfferPeaceText { get; } = "Offer peace";
    internal static string PeaceOfferText { get; } = "Peace offer";
}
