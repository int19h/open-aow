#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
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
        return Outcome(accepted: action.Source == action.GetEngine().StrategicMap.SeatedPlayer ? AcceptedLine(subject, engine: action.GetEngine()) : AbilityRules.TranslateResourceString(action.GetEngine(), S: GameTables.PeaceText), action: action, proposed: subject, subject: subject, otherwise: "");
    }

    public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

    public override void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
        => text.Exchange(reaction, proposed: relation == DiplomaticRelation.Unknown ? 103 : 154, accepted: 127, rejected: 130);

    public override void Sent(DiplomaticAction action) => Deliver(action);

    public override bool LapsesUnanswered(DiplomaticAction action)
        => Relation(action.Source, second: action.Target, engine: action.GetEngine()!) != DiplomaticRelation.Peace;

    public override void Accepted(DiplomaticAction action)
    {
        action.GetEngine().StrategicMap.Players.SetDiplomaticRelation(PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(action.Source)), otherPlayer: PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(action.Target)), relation: DiplomaticRelation.Peace);
        Broadcast(action, exclude: -1, viewer: action.Source);
        RemoveTargetLog(action);
    }

    public override DiplomaticActionReaction RejectReaction(DiplomaticAction action)
        => Relation(action.Source, second: action.Target, engine: action.GetEngine()!) != DiplomaticRelation.Peace ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

    public override void AnswerAI(DiplomaticAction action, PlayerId? player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
    {
        string errorText = "";
        int source = PlayerIdExtensions.ToOriginal(action.Source);
        int target = PlayerIdExtensions.ToOriginal(action.Target);
        DiplomaticAction last = action.GetEngine().StrategicMap.Players.GetPlayers(target).DiplomaticActions.FindLastAction(Kind, reaction: DiplomaticActionReaction.None, source: PlayerId.FromOriginal(target), target: PlayerId.FromOriginal(source), other: -1);
        if (idealPeace.IndexOf((byte)source) != -1 || (last is not null && action.GetEngine().StrategicMap.Day - last.Day <= 1))
        {
            _ = action.GetEngine().StrategicMap.Players.GetPlayers(target).AIControl.CanOfferPeace(PlayerId.FromOriginal((byte)source))
                ? action.Accept(errorText)
                : action.Reject(errorText);
        }
        else
        {
            if (action.GetEngine().StrategicMap.Players.GetPlayers(target).AIControl.CanOfferPeace(PlayerId.FromOriginal((byte)source)))
            {
                StrategicMap strategicMap = action.GetEngine().StrategicMap;
                if (strategicMap.Players.GetDiplomaticRelationCount(PlayerId.FromOriginal((sbyte)(byte)source), relations: DiplomaticRelations.Peace | DiplomaticRelations.Alliance) + 1 < strategicMap.Players.GetDiplomaticRelationCount(PlayerId.FromOriginal((sbyte)(byte)source), relations: DiplomaticRelations.War))
                {
                    last = strategicMap.Players.GetPlayers(target).DiplomaticActions.FindLastAction(Kind, reaction: DiplomaticActionReaction.Reject, source: PlayerId.FromOriginal(source), target: PlayerId.FromOriginal(target), other: -1);
                    if (last is null || action.GetEngine().StrategicMap.Day - last.Day > 8)
                    {
                        // { Original bug: escrow gold is counted twice. }
                        _ = action.GetEngine().StrategicMap.Random(3) == 1 && action.GetEngine().StrategicMap.Random(250) + 100 < action.EscrowGold + action.EscrowGold
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
        DiplomaticRelation diplomaticRelation = Relation(action.Source, second: action.Target, engine: action.GetEngine()!);
        if (diplomaticRelation is not (DiplomaticRelation.Peace or DiplomaticRelation.War))
        {
            DiplomaticAction last = action.GetEngine().StrategicMap.Players.GetPlayers(action.Source).CreateDiplomaticAction(new DeclareWar(), action.Target, other: null);
            if (last is not null)
            {
                string errorText = "";
                _ = last.Send(ref errorText);
                _ = last.Release();
            }
        }
    }

    public override int RelationModifier(DiplomaticAction action, PlayerId? player, int race)
    {
        if (Relation(action.Source, second: action.Target, engine: action.GetEngine()!) != DiplomaticRelation.Peace)
        {
            if (player == action.Source)
            {
                return RaceModifier(action.Target, race: race, modifier: 10, engine: action.GetEngine()!);
            }

            if (player == action.Target)
            {
                return RaceModifier(action.Source, race: race, modifier: 10, engine: action.GetEngine()!);
            }
        }

        return 0;
    }

    internal static string OfferPeaceText { get; } = "Offer peace";
    internal static string PeaceOfferText { get; } = "Peace offer";
}
