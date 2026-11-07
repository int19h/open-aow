using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.EventLog;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;

// What a diplomatic action proposes: war, peace, an alliance, a message, a threat, a tribute, or a
// suggestion about a third player. Each kind holds its own fields and does what the switches over
// the action type did, arm by arm (Diplomacy.pas, Player.PAS, AoWRelations.pas and
// E3EventDialogs.pas). The original format stores a proposal as the one-byte kind (TDiplomaticAction
// property 25) with gold, mana and the message (properties 23, 24 and 27), and ToFields and
// FromFields convert between the two. A mod proposal registers a kind of its own and overrides these.

namespace Aow.Units.AoWE
{
    // A view key of the relations window. Instances are the keys, the name serves the reports.
    public sealed class ProposalView(string name)
    {
        public string Name => name;

        public override string ToString() => name;

        // The window has nothing to show.
        public static readonly ProposalView None = new("None");
        // Sent at once and announced to every player who knows both sides (declare war, break alliance).
        public static readonly ProposalView Notice = new("Notice");
        // Sent at once to the target alone (threaten to attack).
        public static readonly ProposalView Threat = new("Threat");
        // The sender adds gold and mana, the target accepts or rejects.
        public static readonly ProposalView Offer = new("Offer");
        // The sender writes the text.
        public static readonly ProposalView Message = new("Message");
    }

    public abstract record DiplomaticProposal
    {
        private static readonly Catalog<byte, Func<int, int, DiplomaticProposal>> Kinds = new("diplomatic proposal");

        // The message lines (property 27): the default text of the kind, the sender's text, or the
        // reply's text once the target answers. The copies in the players' action lists hold none.
        public IReadOnlyList<string> Message { get; init; } = [];

        // The kind, gold and mana the original format stores.
        public abstract (byte Kind, int Gold, int Mana) ToFields();

        // The proposal the stored kind, gold and mana name. A kind no proposal registered reads as
        // UnknownProposal, which writes the three fields back unchanged.
        public static DiplomaticProposal FromFields(byte Kind, int Gold, int Mana) =>
            Kinds.TryGet(Kind, out var create) ? create(Gold, Mana) : new UnknownProposal(Kind, Gold, Mana);

        // Registers the proposal of a kind. A second registration of a kind throws.
        internal static void Register(byte Kind, Func<int, int, DiplomaticProposal> Create) => Kinds.Add(Kind, Create);

        // The kinds with a registered proposal.
        internal static bool IsRegistered(byte Kind) => Kinds.TryGet(Kind, out _);

        // The message as TStrings.Text joins it.
        public string MessageText => string.Concat(Message.Select(static line => line + "\r\n"));

        // The lines of Text.
        public static IReadOnlyList<string> Lines(TStrings Text)
        {
            var lines = new string[Text.Count];
            for (int I = 0; I < lines.Length; I++)
            {
                lines[I] = Text[I];
            }

            return lines;
        }

        // The lines TStrings.Text splits Text into.
        public static IReadOnlyList<string> Lines(string Text)
        {
            var list = new TStringList();
            list.Text = Text;
            var lines = Lines(list);
            list.Free();
            return lines;
        }

        public virtual bool Equals(DiplomaticProposal other) =>
            ReferenceEquals(this, other) || other is not null && EqualityContract == other.EqualityContract && Message.SequenceEqual(other.Message);

        public override int GetHashCode() => HashCode.Combine(EqualityContract, Message.Count);

        // The proposal with the gold and mana the sender adds. A kind that carries none ignores them.
        public virtual DiplomaticProposal WithOffer(int Gold, int Mana) => this;

        // How the relations window composes and shows the proposal (AoWRelations.pas:811, E3EventDialogs.pas:194).
        public virtual ProposalView View => ProposalView.None;

        // The relations window's icon for the proposal, -1 for none.
        public virtual int Icon => -1;

        // The message claims to come from the other player, not from the source (AoWRelations.pas:768,
        // 882, E3EventDialogs.pas:194, Diplomacy.pas:1228, 1333).
        public virtual bool SignedAsOtherPlayer => false;

        // The line of the actions list (Diplomacy.pas:505).
        public virtual string Name(TDiplomaticAction Action) => "";

        // The logbook line (Diplomacy.pas:1265).
        public virtual string LogText(TDiplomaticAction Action) => "TDiplomaticActionEventLog";

        // The sample played when the proposal enters a logbook (Diplomacy.pas:1208).
        public virtual string LoggedSample => "SFX\\EVENTS\\MESSAGE.WAV";

        // The logbook's middle icon (Diplomacy.pas:1353).
        public virtual int MiddleIcon(TDiplomaticAction Action) => 0x16;

        // Fills Text with the kind's default message for Reaction (Diplomacy.pas:1420). Relation is
        // the relation of the source to the target.
        public virtual void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation)
        {
        }

        // The check the kind adds once the source can pay the offer (Diplomacy.pas:1055).
        public virtual bool CanSend(TDiplomaticAction Action, ref string ErrorText) => true;

        // What sending does once the offer sits in escrow (Diplomacy.pas:992).
        public virtual void Sent(TDiplomaticAction Action)
        {
        }

        // Whether the proposal, still unanswered when its source's turn comes round, lapses as
        // ignored (Diplomacy.pas:613).
        public virtual bool LapsesUnanswered(TDiplomaticAction Action) => false;

        // What acceptance does once the escrow went to the target and the race relations moved (Diplomacy.pas:826).
        public virtual void Accepted(TDiplomaticAction Action)
        {
        }

        // The reaction a rejection records (Diplomacy.pas:950).
        public virtual TDiplomaticActionReaction RejectReaction(TDiplomaticAction Action) => TDiplomaticActionReaction.darReject;

        // The AI target's answer (Diplomacy.pas:722). Relation is the relation of the source to the
        // target, IdealPeace and IdealWar the target's ideal partners.
        public virtual void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar)
        {
        }

        // The AI source's step once the target rejected or ignored the proposal (Diplomacy.pas:722).
        public virtual void RefusedAI(TDiplomaticAction Action)
        {
        }

        // How the proposal moves the relation of Race to Player (Diplomacy.pas:527).
        public virtual int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race) => 0;

        // Modifier, scaled by how Race regards the race of Other, or Modifier itself for Other's own race.
        protected static int RaceModifier(sbyte Other, sbyte Race, int Modifier)
        {
            if (AoWHSMap.FPlayers.GetPlayers(Other).FPlayerRace != Race)
            {
                var players = AoWHSMap.FPlayers.GetPlayers(Other);
                return Round((double)((AoWHSMap.FRaces.GetRace(Race).GetRelationValue(players.FPlayerRace) - 50) * Modifier) * 0.01);
            }

            return Modifier;
        }

        // The relation of First to Second.
        protected static TDiplomaticRelation Relation(sbyte First, sbyte Second) =>
            AoWHSMap.FPlayers.GetDiplomaticRelation((sbyte)(byte)First, (sbyte)(byte)Second);

        // Shows Log at once when Player is seated.
        protected static void ShowTo(TEventLog Log, sbyte Player)
        {
            if (Player == AoWHSMap.FSeatedPlayer)
            {
                Log.Execute(false, TExecuteTrigger.etEvent, null);
            }
        }

        // Announces the action to every player who knows both sides but Exclude, and shows it to Viewer.
        protected static void Broadcast(TDiplomaticAction Action, sbyte Exclude, sbyte Viewer)
        {
            var Log = Action.CreateEventLog();
            BroadcastDiplomaticEvent(Log, Exclude);
            ShowTo(Log, Viewer);
            Log.Release();
        }

        // Delivers the proposal to the target's logbook (Diplomacy.pas:992, the arm of the kinds that wait for an answer).
        protected static void Deliver(TDiplomaticAction Action)
        {
            AoWHSMap.FPlayers.GetPlayers(Action.FTarget).PlayerSeen((sbyte)(byte)Action.FSource);
            if (Action.FSource == AoWHSMap.FSeatedPlayer)
            {
                var p5MessageSent = TranslateRStr(P5MessageSentRStr);
                AoWHSMap.ShowMessage(p5MessageSent);
            }

            var Log = Action.CreateEventLog();
            AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.AddEvent(Log);
            ShowTo(Log, Action.FTarget);
            Log.Release();
            AoWHSMap.FDiplomaticRelationsChangedEvents.TriggerNotifyEvents(Action);
        }

        // Drops the target's logbook entry of the answered proposal.
        protected static void RemoveTargetLog(TDiplomaticAction Action)
        {
            var Log = (TDiplomaticActionEventLog)AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.FindID(Action.FEventLogID);
            if (Log != null)
            {
                Log.SetOwner(null);
            }
        }

        // Moves the target's logbook entry, now holding the answer, to the source and shows it there.
        protected static void ReturnToSource(TDiplomaticAction Action)
        {
            var Log = (TDiplomaticActionEventLog)AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.FindID(Action.FEventLogID);
            Log.SetDiplomaticAction(Action);
            AoWHSMap.FPlayers.GetPlayers(Action.FSource).FEventLogbook.AddEvent(Log);
            ShowTo(Log, Action.FSource);
        }

        // The logbook line of an answered kind: Proposed while unanswered, else the answer about Subject.
        protected static string Outcome(TDiplomaticAction Action, string Proposed, string Subject, string Accepted, string Otherwise) => Action.FReaction switch
        {
            TDiplomaticActionReaction.darNone => Proposed,
            TDiplomaticActionReaction.darReject => AoWEUnit.Format(TranslateRStr(P5XRejectedRStr), [Subject]),
            TDiplomaticActionReaction.darAccept => Accepted,
            TDiplomaticActionReaction.darIgnore => AoWEUnit.Format(TranslateRStr(P5XIgnoredRStr), [Subject]),
            _ => Otherwise,
        };

        // The accepted line about Subject.
        protected static string AcceptedLine(string Subject) => AoWEUnit.Format(TranslateRStr(P5XAcceptedRStr), [Subject]);
    }

    // A kind no proposal registered. It does what the default arms did and keeps the three fields.
    public sealed record UnknownProposal(byte Kind, int Gold, int Mana) : DiplomaticProposal
    {
        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);
    }

    // A kind whose sender adds gold and mana and whose target accepts or rejects it.
    public abstract record OfferProposal(int Gold, int Mana) : DiplomaticProposal
    {
        public override DiplomaticProposal WithOffer(int Gold, int Mana) => this with { Gold = Gold, Mana = Mana };

        public override ProposalView View => ProposalView.Offer;
    }

    // Kind 0: the source declares war on the target.
    public sealed record DeclareWar : DiplomaticProposal
    {
        public const byte Kind = 0;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Notice;

        public override int Icon => 9;

        public override string Name(TDiplomaticAction Action) => TranslateRStr(P5DeclareWarRStr);

        public override string LogText(TDiplomaticAction Action) => TranslateRStr(P5WarRStr);

        public override int MiddleIcon(TDiplomaticAction Action) => 0x15;

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Proposing(Relation == TDiplomaticRelation.drUnknown ? 151 : 100);

        public override void Sent(TDiplomaticAction Action)
        {
            Action.ApplyRelationModifiers();
            AoWHSMap.FPlayers.GetPlayers(Action.FTarget).PlayerSeen((sbyte)(byte)Action.FSource);
            AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drWar);
            if (Action.FSource == AoWHSMap.FSeatedPlayer)
            {
                var p5WarDeclared = TranslateRStr(P5WarDeclaredRStr);
                AoWHSMap.ShowMessage(p5WarDeclared);
            }

            Broadcast(Action, -1, Action.FTarget);
        }

        public override void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar) =>
            Action.UpdateAIDiplomaticActionsLog(Player);

        public override int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race)
        {
            if (Player == Action.FSource && Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drWar)
            {
                int Modifier = Relation(Action.FSource, Action.FTarget) == TDiplomaticRelation.drAlliance ? -60 : -20;
                return RaceModifier(Action.FTarget, Race, Modifier);
            }

            if (Player == Action.FTarget && Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drWar && AoWHSMap.FPlayers.GetPlayers(Action.FSource).FPlayerRace == Race)
            {
                return -20;
            }

            return 0;
        }
    }

    // Kind 1: the source offers the target peace.
    public sealed record OfferPeace(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 1;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 8;

        public override string Name(TDiplomaticAction Action) => TranslateRStr(P5OfferPeaceRStr);

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = TranslateRStr(P5PeaceOfferRStr);
            var accepted = Action.FSource == AoWHSMap.FSeatedPlayer ? AcceptedLine(subject) : TranslateRStr(P5PeaceRStr);
            return Outcome(Action, subject, subject, accepted, "");
        }

        public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Exchange(Reaction, Relation == TDiplomaticRelation.drUnknown ? 103 : 154, 127, 130);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(TDiplomaticAction Action) =>
            Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drPeace;

        public override void Accepted(TDiplomaticAction Action)
        {
            AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drPeace);
            Broadcast(Action, -1, Action.FSource);
            RemoveTargetLog(Action);
        }

        public override TDiplomaticActionReaction RejectReaction(TDiplomaticAction Action) =>
            Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drPeace ? TDiplomaticActionReaction.darReject : TDiplomaticActionReaction.darIgnore;

        public override void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar)
        {
            string ErrorText = "";
            sbyte Source = Action.FSource;
            sbyte Target = Action.FTarget;
            var Last = AoWHSMap.FPlayers.GetPlayers(Target).FDiplomaticActions.FindLastAction(Kind, TDiplomaticActionReaction.darNone, Target, Source, -1);
            if (IdealPeace.IndexOf((byte)Source) != -1 || (Last != null && AoWHSMap.FDay - Last.FDay <= 1))
            {
                if (AoWHSMap.FPlayers.GetPlayers(Target).FAIControl.CanOfferPeace((byte)Source))
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
                if (AoWHSMap.FPlayers.GetPlayers(Target).FAIControl.CanOfferPeace((byte)Source))
                {
                    if (AoWHSMap.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Source, TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drPeace, TDiplomaticRelation.drAlliance)) + 1 < AoWHSMap.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Source, TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drWar)))
                    {
                        Last = AoWHSMap.FPlayers.GetPlayers(Target).FDiplomaticActions.FindLastAction(Kind, TDiplomaticActionReaction.darReject, Source, Target, -1);
                        if (Last == null || AoWHSMap.FDay - Last.FDay > 8)
                        {
                            // { Original bug: escrow gold is counted twice. }
                            if (AoWHSMap.Random(3) == 1 && AoWHSMap.Random(250) + 100 < Action.FEscrowGold + Action.FEscrowGold)
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

        public override void RefusedAI(TDiplomaticAction Action)
        {
            string ErrorText = "";
            var diplomaticRelation = Relation(Action.FSource, Action.FTarget);
            if (!TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drPeace, TDiplomaticRelation.drWar).Contains(diplomaticRelation))
            {
                var Last = AoWHSMap.FPlayers.GetPlayers(Action.FSource).CreateDiplomaticAction(new DeclareWar(), Action.FTarget, -1);
                if (Last != null)
                {
                    Last.Send(ref ErrorText);
                    Last.Release();
                }
            }
        }

        public override int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race)
        {
            if (Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drPeace)
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
    }

    // Kind 2: the source proposes an alliance to the target.
    public sealed record ProposeAlliance(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 2;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 10;

        public override string Name(TDiplomaticAction Action) => TranslateRStr(P5ProposeAllianceRStr);

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = TranslateRStr(P5AllianceProposalRStr);
            var accepted = Action.FSource == AoWHSMap.FSeatedPlayer ? AcceptedLine(subject) : TranslateRStr(P5MadeAllianceRStr);
            return Outcome(Action, subject, subject, accepted, subject);
        }

        public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 106, 121, 124);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(TDiplomaticAction Action) =>
            Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drAlliance;

        public override void Accepted(TDiplomaticAction Action)
        {
            AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drAlliance);
            Broadcast(Action, -1, Action.FSource);
            RemoveTargetLog(Action);
        }

        public override TDiplomaticActionReaction RejectReaction(TDiplomaticAction Action) =>
            Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drAlliance ? TDiplomaticActionReaction.darReject : TDiplomaticActionReaction.darIgnore;

        public override void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar)
        {
            string ErrorText = "";
            if (AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FAIControl.CanProposeAlliance((byte)Action.FSource))
            {
                RandSeed = (int)(((uint)(AoWHSMap.FInitialRandomSeed + AoWHSMap.FDay + Action.FSource + Action.FTarget)) >> 2);
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

        public override int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race)
        {
            if (Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drAlliance)
            {
                int Modifier = Relation(Action.FSource, Action.FTarget) == TDiplomaticRelation.drWar ? 30 : 20;
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
    }

    // Kind 3: the source writes to the target.
    public sealed record SendMessage : DiplomaticProposal
    {
        public const byte Kind = 3;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Message;

        public override string Name(TDiplomaticAction Action) => TranslateRStr(P5SendMessageRStr);

        public override string LogText(TDiplomaticAction Action) => TranslateRStr(P5MessageRStr);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);
    }

    // Kind 4: the source threatens to attack the target.
    public sealed record ThreatenToAttack : DiplomaticProposal
    {
        public const byte Kind = 4;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Threat;

        public override int Icon => 46;

        public override string Name(TDiplomaticAction Action) => TranslateRStr(P5ThreatenToAttackRStr);

        public override string LogText(TDiplomaticAction Action) => TranslateRStr(P5ThreatRStr);

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Proposing(112);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);
    }

    // Kind 5: the source pays the target a tribute.
    public sealed record MakeTribute(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 5;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 48;

        public override string Name(TDiplomaticAction Action) => TranslateRStr(P5MakeTributeRStr);

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = TranslateRStr(P5TributeRStr);
            return Outcome(Action, subject, subject, AcceptedLine(subject), "");
        }

        public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 118, 133, 136);

        public override bool CanSend(TDiplomaticAction Action, ref string ErrorText)
        {
            if (Gold == 0 && Mana == 0)
            {
                ErrorText = TranslateRStr(P5NoGoldManaSpecifiedRStr);
                return false;
            }

            return true;
        }

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(TDiplomaticAction Action) => true;

        public override void Accepted(TDiplomaticAction Action) => ReturnToSource(Action);

        public override void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar)
        {
            string ErrorText = "";
            if (TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drPeace, TDiplomaticRelation.drAlliance).Contains(Relation))
            {
                Action.Accept(ref ErrorText);
            }
            else
            {
                if (AoWHSMap.Random(200) + 50 < Gold + Mana)
                {
                    Action.Accept(ref ErrorText);
                }
                else
                {
                    Action.Reject(ref ErrorText);
                }
            }
        }
    }

    // Kind 6: the source ends its alliance with the target.
    public sealed record BreakAlliance : DiplomaticProposal
    {
        public const byte Kind = 6;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Notice;

        public override int Icon => 8;

        public override string Name(TDiplomaticAction Action) => TranslateRStr(P5BreakAllianceRStr);

        public override string LogText(TDiplomaticAction Action) => TranslateRStr(P5AllianceBrokenRStr);

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Proposing(109);

        public override void Sent(TDiplomaticAction Action)
        {
            Action.ApplyRelationModifiers();
            AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drPeace);
            if (Action.FSource == AoWHSMap.FSeatedPlayer)
            {
                var p5AllianceBroken = TranslateRStr(P5AllianceBrokenRStr);
                AoWHSMap.ShowMessage(p5AllianceBroken);
            }

            Broadcast(Action, -1, Action.FTarget);
        }

        public override int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race) =>
            Player == Action.FSource && Relation(Action.FSource, Action.FTarget) == TDiplomaticRelation.drAlliance
                ? RaceModifier(Action.FTarget, Race, -40)
                : 0;
    }

    // Kind 7: the source writes to the target in the name of the other player, offered between human
    // players to a source of race 5 whose hero emerges (Player.PAS:1226).
    public sealed record SendMessageAsY : DiplomaticProposal
    {
        public const byte Kind = 7;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Message;

        public override bool SignedAsOtherPlayer => true;

        public override string Name(TDiplomaticAction Action)
        {
            var name = AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return AoWEUnit.Format(TranslateRStr(P5SendMessageAsXRStr), [name]);
        }

        public override string LogText(TDiplomaticAction Action) => TranslateRStr(P5MessageRStr);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);
    }

    // Kind 8: the source asks the target to end its alliance with the other player.
    public sealed record BreakAllianceWithY(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 8;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(TDiplomaticAction Action)
        {
            var name = AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return AoWEUnit.Format(TranslateRStr(P5SuggestAllianceBreakWithYRStr), [name]);
        }

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = TranslateRStr(P5BreakAllianceRStr);
            return Outcome(Action, TranslateRStr(P5BreakAllianceSuggestionRStr), subject, AcceptedLine(subject), "");
        }

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 160, 139, 142, true);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(TDiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) != TDiplomaticRelation.drAlliance;

        public override void Accepted(TDiplomaticAction Action)
        {
            if (Relation(Action.FTarget, Action.FOtherPlayer) == TDiplomaticRelation.drAlliance)
            {
                AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FTarget, (sbyte)(byte)Action.FOtherPlayer, TDiplomaticRelation.drPeace);
                var broken = AoWHSMap.FPlayers.GetPlayers(Action.FTarget).CreateDiplomaticAction(new BreakAlliance(), Action.FOtherPlayer, -1);
                if (broken != null)
                {
                    Broadcast(broken, Action.FSource, Action.FOtherPlayer);
                    broken.Release();
                }
            }

            ReturnToSource(Action);
        }

        public override TDiplomaticActionReaction RejectReaction(TDiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) == TDiplomaticRelation.drAlliance ? TDiplomaticActionReaction.darReject : TDiplomaticActionReaction.darIgnore;

        public override void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar)
        {
            string ErrorText = "";
            if (DiplomaticProposal.Relation(Action.FTarget, Action.FOtherPlayer) != TDiplomaticRelation.drAlliance)
            {
                Action.Accept(ref ErrorText);
            }
            else
            {
                Action.Reject(ref ErrorText);
            }
        }

        public override int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race) =>
            Player == Action.FTarget && Relation(Action.FTarget, Action.FOtherPlayer) == TDiplomaticRelation.drAlliance
                ? RaceModifier(Action.FOtherPlayer, Race, -40)
                : 0;
    }

    // Kind 9: the source asks the target to declare war on the other player.
    public sealed record DeclareWarToY(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 9;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(TDiplomaticAction Action)
        {
            var name = AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return AoWEUnit.Format(TranslateRStr(P5SuggestWarWithYRStr), [name]);
        }

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = TranslateRStr(P5DeclareWarRStr);
            return Outcome(Action, TranslateRStr(P5DeclareWarSuggestionRStr), subject, AcceptedLine(subject), "");
        }

        public override int MiddleIcon(TDiplomaticAction Action) =>
            Action.FReaction == TDiplomaticActionReaction.darAccept ? 0x15 : 0x16;

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 157, 145, 148, true);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        public override bool LapsesUnanswered(TDiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) != TDiplomaticRelation.drWar;

        public override void Accepted(TDiplomaticAction Action)
        {
            if (Relation(Action.FTarget, Action.FOtherPlayer) != TDiplomaticRelation.drWar)
            {
                AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).PlayerSeen((sbyte)(byte)Action.FTarget);
                AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FTarget, (sbyte)(byte)Action.FOtherPlayer, TDiplomaticRelation.drWar);
                var war = AoWHSMap.FPlayers.GetPlayers(Action.FTarget).CreateDiplomaticAction(new DeclareWar(), Action.FOtherPlayer, -1);
                if (war != null)
                {
                    Broadcast(war, Action.FSource, Action.FOtherPlayer);
                    war.Release();
                }
            }

            ReturnToSource(Action);
        }

        public override TDiplomaticActionReaction RejectReaction(TDiplomaticAction Action) =>
            Relation(Action.FTarget, Action.FOtherPlayer) != TDiplomaticRelation.drWar ? TDiplomaticActionReaction.darReject : TDiplomaticActionReaction.darIgnore;

        public override void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar)
        {
            string ErrorText = "";
            if (DiplomaticProposal.Relation(Action.FTarget, Action.FOtherPlayer) == TDiplomaticRelation.drWar)
            {
                Action.Accept(ref ErrorText);
            }
            else
            {
                if (IdealWar.IndexOf((byte)Action.FOtherPlayer) != -1)
                {
                    if (AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FAIControl.CanDeclareWar((byte)Action.FOtherPlayer))
                    {
                        if (AoWHSMap.Random(5) == 1 && AoWHSMap.Random(550) + 250 < Action.FEscrowGold + Action.FEscrowMana)
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
        }

        public override int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race)
        {
            if (Player == Action.FTarget && Relation(Action.FTarget, Action.FOtherPlayer) != TDiplomaticRelation.drWar)
            {
                int Modifier = Relation(Action.FTarget, Action.FOtherPlayer) == TDiplomaticRelation.drAlliance ? -60 : -20;
                return RaceModifier(Action.FOtherPlayer, Race, Modifier);
            }

            return 0;
        }
    }

    // The default message being written (Diplomacy.pas:1420): the text control, the source's
    // alignment row and the plural race names of source, target and other player.
    public sealed class DiplomaticMessageText(TTextControl textControl, int alignment, string sourceRace, string targetRace, string otherRace, TStrings text)
    {
        // Text Number of the alignment row, the source's races first.
        public void Proposing(int Number, bool AboutOther = false)
        {
            textControl.RetrieveText(alignment + Number, text);
            text.Text = AboutOther
                ? AoWEUnit.Format(text.Text, [sourceRace, targetRace, otherRace])
                : AoWEUnit.Format(text.Text, [sourceRace, targetRace]);
        }

        // Text Number of the alignment row, the target's races first.
        public void Replying(int Number, bool AboutOther = false)
        {
            textControl.RetrieveText(alignment + Number, text);
            text.Text = AboutOther
                ? AoWEUnit.Format(text.Text, [targetRace, sourceRace, otherRace])
                : AoWEUnit.Format(text.Text, [targetRace, sourceRace]);
        }

        // The proposal's text while unanswered, the reply's once accepted or rejected, none once ignored.
        public void Exchange(TDiplomaticActionReaction Reaction, int Proposed, int Accepted, int Rejected, bool AboutOther = false)
        {
            if (Reaction == TDiplomaticActionReaction.darNone)
            {
                Proposing(Proposed, AboutOther);
            }
            else if (Reaction == TDiplomaticActionReaction.darAccept)
            {
                Replying(Accepted, AboutOther);
            }
            else if (Reaction == TDiplomaticActionReaction.darReject)
            {
                Replying(Rejected, AboutOther);
            }
        }
    }

    public partial class TDiplomaticAction
    {
        // Replaces the message with the default text of the kind for Reaction (Player.PAS:1188,
        // Diplomacy.pas:826, 950).
        public void RetrieveDefaultMessage(sbyte Source, sbyte Target, sbyte Other, TDiplomaticActionReaction Reaction)
        {
            var text = new TStringList();
            RetrieveDefaultDiplomaticActionMessage(Source, Target, Other, Proposal, Reaction, text);
            Proposal = Proposal with { Message = DiplomaticProposal.Lines(text) };
            text.Free();
        }

        // Moves every race's relation to source and target by the proposal's modifiers (Diplomacy.pas:826, 992).
        public void ApplyRelationModifiers()
        {
            AoWHSMap.LockRelationsChanged();
            for (int I = 0, last = AoWHSMap.FRaces.Count - 1; I <= last; I++)
            {
                var playerRelationValue = AoWHSMap.FRaces.GetRace(I).GetPlayerRelationValue(FSource);
                var relationModifier = GetRelationModifier(FSource, (sbyte)I);
                AoWHSMap.FRaces.GetRace(I).SetPlayerRelationValue(FSource, relationModifier + playerRelationValue);
                var playerRelationValue2 = AoWHSMap.FRaces.GetRace(I).GetPlayerRelationValue(FTarget);
                var relationModifier2 = GetRelationModifier(FTarget, (sbyte)I);
                AoWHSMap.FRaces.GetRace(I).SetPlayerRelationValue(FTarget, relationModifier2 + playerRelationValue2);
            }

            AoWHSMap.UnlockRelationsChanged();
        }
    }
}
