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

    public override string Name(DiplomaticAction Action) => AbilityRules.TranslateRStr(P5OfferPeaceRStr);

    public override string LogText(DiplomaticAction Action)
    {
        var subject = AbilityRules.TranslateRStr(P5PeaceOfferRStr);
        var accepted = Action.FSource == AoWHSMap.Current.FSeatedPlayer ? AcceptedLine(subject) : AbilityRules.TranslateRStr(GameTables.P5PeaceRStr);
        return Outcome(Action, subject, subject, accepted, "");
    }

    public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

    public override void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation) =>
        Text.Exchange(Reaction, Relation == DiplomaticRelation.Unknown ? 103 : 154, 127, 130);

    public override void Sent(DiplomaticAction Action) => Deliver(Action);

    public override bool LapsesUnanswered(DiplomaticAction Action) =>
        Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.Peace;

    public override void Accepted(DiplomaticAction Action)
    {
        AoWHSMap.Current.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, DiplomaticRelation.Peace);
        Broadcast(Action, -1, Action.FSource);
        RemoveTargetLog(Action);
    }

    public override DiplomaticActionReaction RejectReaction(DiplomaticAction Action) =>
        Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.Peace ? DiplomaticActionReaction.Reject : DiplomaticActionReaction.Ignore;

    public override void AnswerAI(DiplomaticAction Action, sbyte Player, DiplomaticRelation Relation, ByteList IdealPeace, ByteList IdealWar)
    {
        string ErrorText = "";
        sbyte Source = Action.FSource;
        sbyte Target = Action.FTarget;
        var Last = AoWHSMap.Current.FPlayers.GetPlayers(Target).FDiplomaticActions.FindLastAction(Kind, DiplomaticActionReaction.None, Target, Source, -1);
        if (IdealPeace.IndexOf((byte)Source) != -1 || (Last != null && AoWHSMap.Current.FDay - Last.FDay <= 1))
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Target).FAIControl.CanOfferPeace((byte)Source))
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
            if (AoWHSMap.Current.FPlayers.GetPlayers(Target).FAIControl.CanOfferPeace((byte)Source))
            {
                if (AoWHSMap.Current.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Source, Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance)) + 1 < AoWHSMap.Current.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Source, Set<DiplomaticRelation>.Of(DiplomaticRelation.War)))
                {
                    Last = AoWHSMap.Current.FPlayers.GetPlayers(Target).FDiplomaticActions.FindLastAction(Kind, DiplomaticActionReaction.Reject, Source, Target, -1);
                    if (Last == null || AoWHSMap.Current.FDay - Last.FDay > 8)
                    {
                        // { Original bug: escrow gold is counted twice. }
                        if (AoWHSMap.Current.Random(3) == 1 && AoWHSMap.Current.Random(250) + 100 < Action.FEscrowGold + Action.FEscrowGold)
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
    }

    public override void RefusedAI(DiplomaticAction Action)
    {
        string ErrorText = "";
        var diplomaticRelation = Relation(Action.FSource, Action.FTarget);
        if (!Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.War).Contains(diplomaticRelation))
        {
            var Last = AoWHSMap.Current.FPlayers.GetPlayers(Action.FSource).CreateDiplomaticAction(new DeclareWar(), Action.FTarget, -1);
            if (Last != null)
            {
                Last.Send(ref ErrorText);
                Last.Release();
            }
        }
    }

    public override int RelationModifier(DiplomaticAction Action, sbyte Player, sbyte Race)
    {
        if (Relation(Action.FSource, Action.FTarget) != DiplomaticRelation.Peace)
        {
            if (Player == Action.FSource)
            {
                return RaceModifier(Action.FTarget, Race, 10);
            }

            if (Player == Action.FTarget)
            {
                return RaceModifier(Action.FSource, Race, 10);
            }
        }

        return 0;
    }

    internal static readonly string P5OfferPeaceRStr = "Offer peace";
    internal static readonly string P5PeaceOfferRStr = "Peace offer";
}
