using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

// What a diplomatic action proposes: war, peace, an alliance, a message, a threat, a tribute, or a
// suggestion about a third player. Each kind holds its own fields and does what the switches over
// the action type did, arm by arm (Diplomacy.pas, Player.PAS, AoWRelations.pas and
// E3EventDialogs.pas). The original format stores a proposal as the one-byte kind (TDiplomaticAction
// property 25) with gold, mana and the message (properties 23, 24 and 27), and ToFields and
// FromFields convert between the two. A mod proposal registers a kind of its own and overrides these.

namespace Aow.Game.Session
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

    public abstract partial record DiplomaticProposal
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
            list?.Dispose();
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
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Other).FPlayerRace != Race)
            {
                var players = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Other);
                return Round((double)((Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(Race).GetRelationValue(players.FPlayerRace) - 50) * Modifier) * 0.01);
            }

            return Modifier;
        }

        // The relation of First to Second.
        protected static TDiplomaticRelation Relation(sbyte First, sbyte Second) =>
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelation((sbyte)(byte)First, (sbyte)(byte)Second);

        // Shows Log at once when Player is seated.
        protected static void ShowTo(TEventLog Log, sbyte Player)
        {
            if (Player == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
            {
                Log.Execute(false, TExecuteTrigger.etEvent, null);
            }
        }

        // Announces the action to every player who knows both sides but Exclude, and shows it to Viewer.
        protected static void Broadcast(TDiplomaticAction Action, sbyte Exclude, sbyte Viewer)
        {
            var Log = Action.CreateEventLog();
            Aow.Game.Session.DiplomaticProposal.BroadcastDiplomaticEvent(Log, Exclude);
            ShowTo(Log, Viewer);
            Log.Release();
        }

        // Delivers the proposal to the target's logbook (Diplomacy.pas:992, the arm of the kinds that wait for an answer).
        protected static void Deliver(TDiplomaticAction Action)
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).PlayerSeen((sbyte)(byte)Action.FSource);
            if (Action.FSource == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
            {
                var p5MessageSent = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.DiplomaticProposal.P5MessageSentRStr);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(p5MessageSent);
            }

            var Log = Action.CreateEventLog();
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.AddEvent(Log);
            ShowTo(Log, Action.FTarget);
            Log.Release();
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDiplomaticRelationsChangedEventsStore.Raise(Action);
        }

        // Drops the target's logbook entry of the answered proposal.
        protected static void RemoveTargetLog(TDiplomaticAction Action)
        {
            var Log = (TDiplomaticActionEventLog)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.FindID(Action.FEventLogID);
            if (Log != null)
            {
                Log.SetOwner(null);
            }
        }

        // Moves the target's logbook entry, now holding the answer, to the source and shows it there.
        protected static void ReturnToSource(TDiplomaticAction Action)
        {
            var Log = (TDiplomaticActionEventLog)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.FindID(Action.FEventLogID);
            Log.SetDiplomaticAction(Action);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FSource).FEventLogbook.AddEvent(Log);
            ShowTo(Log, Action.FSource);
        }

        // The logbook line of an answered kind: Proposed while unanswered, else the answer about Subject.
        protected static string Outcome(TDiplomaticAction Action, string Proposed, string Subject, string Accepted, string Otherwise) => Action.FReaction switch
        {
            TDiplomaticActionReaction.darNone => Proposed,
            TDiplomaticActionReaction.darReject => Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.DiplomaticProposal.P5XRejectedRStr), [Subject]),
            TDiplomaticActionReaction.darAccept => Accepted,
            TDiplomaticActionReaction.darIgnore => Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.DiplomaticProposal.P5XIgnoredRStr), [Subject]),
            _ => Otherwise,
        };

        // The accepted line about Subject.
        protected static string AcceptedLine(string Subject) => Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.DiplomaticProposal.P5XAcceptedRStr), [Subject]);

        private static readonly string P5MessageSentRStr = "Message sent";
        internal static readonly string P5XRejectedRStr = "%s rejected";
        internal static readonly string P5XAcceptedRStr = "%s accepted";
        internal static readonly string P5XIgnoredRStr = "%s ignored";

        private static void BroadcastDiplomaticEvent(global::Aow.Game.Events.TDiplomaticActionEventLog Log, sbyte Exclude)
        {
            int i = 0;
            for (int num = global::Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Count - 1; i <= num; i++)
            {
                global::Aow.Game.Session.TPlayer players = global::Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(i);
                if (players.FStatus == global::Aow.Game.Session.TPlayerStatus.psPlaying && i != Exclude && (i == Log.FAction.FSource || i == Log.FAction.FTarget || (global::Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(i).GetFirstDaySeen((sbyte)(byte)Log.FAction.FSource) != -1 && global::Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(i).GetFirstDaySeen((sbyte)(byte)Log.FAction.FTarget) != -1)))
                {
                    global::Aow.Game.Events.TEventLog tEventLog = (global::Aow.Game.Events.TEventLog)Log.Copy(global::Aow.Game.Magic.Abilities.GameTables.AoWEngine);
                    players.FEventLogbook.AddEvent(tEventLog);
                    tEventLog.Release();
                }
            }
        }
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
    public sealed partial record DeclareWar : DiplomaticProposal
    {
        public const byte Kind = 0;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Notice;

        public override int Icon => 9;

        public override string Name(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.P5DeclareWarRStr);

        public override string LogText(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.P5WarRStr);

        public override int MiddleIcon(TDiplomaticAction Action) => 0x15;

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Proposing(Relation == TDiplomaticRelation.drUnknown ? 151 : 100);

        public override void Sent(TDiplomaticAction Action)
        {
            Action.ApplyRelationModifiers();
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).PlayerSeen((sbyte)(byte)Action.FSource);
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drWar);
            if (Action.FSource == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
            {
                var p5WarDeclared = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.DeclareWar.P5WarDeclaredRStr);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(p5WarDeclared);
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

            if (Player == Action.FTarget && Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drWar && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FSource).FPlayerRace == Race)
            {
                return -20;
            }

            return 0;
        }

        private static readonly string P5WarDeclaredRStr = "War declared";
    }

    // Kind 1: the source offers the target peace.
    public sealed partial record OfferPeace(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 1;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 8;

        public override string Name(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.OfferPeace.P5OfferPeaceRStr);

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.OfferPeace.P5PeaceOfferRStr);
            var accepted = Action.FSource == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer ? AcceptedLine(subject) : Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.P5PeaceRStr);
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
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drPeace);
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
            var Last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Target).FDiplomaticActions.FindLastAction(Kind, TDiplomaticActionReaction.darNone, Target, Source, -1);
            if (IdealPeace.IndexOf((byte)Source) != -1 || (Last != null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay - Last.FDay <= 1))
            {
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Target).FAIControl.CanOfferPeace((byte)Source))
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
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Target).FAIControl.CanOfferPeace((byte)Source))
                {
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Source, TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drPeace, TDiplomaticRelation.drAlliance)) + 1 < Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Source, TSet<TDiplomaticRelation>.Of(TDiplomaticRelation.drWar)))
                    {
                        Last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Target).FDiplomaticActions.FindLastAction(Kind, TDiplomaticActionReaction.darReject, Source, Target, -1);
                        if (Last == null || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay - Last.FDay > 8)
                        {
                            // { Original bug: escrow gold is counted twice. }
                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(3) == 1 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(250) + 100 < Action.FEscrowGold + Action.FEscrowGold)
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
                var Last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FSource).CreateDiplomaticAction(new DeclareWar(), Action.FTarget, -1);
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

        internal static readonly string P5OfferPeaceRStr = "Offer peace";
        internal static readonly string P5PeaceOfferRStr = "Peace offer";
    }

    // Kind 2: the source proposes an alliance to the target.
    public sealed partial record ProposeAlliance(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 2;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 10;

        public override string Name(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.ProposeAlliance.P5ProposeAllianceRStr);

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.ProposeAlliance.P5AllianceProposalRStr);
            var accepted = Action.FSource == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer ? AcceptedLine(subject) : Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.ProposeAlliance.P5MadeAllianceRStr);
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
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drAlliance);
            Broadcast(Action, -1, Action.FSource);
            RemoveTargetLog(Action);
        }

        public override TDiplomaticActionReaction RejectReaction(TDiplomaticAction Action) =>
            Relation(Action.FSource, Action.FTarget) != TDiplomaticRelation.drAlliance ? TDiplomaticActionReaction.darReject : TDiplomaticActionReaction.darIgnore;

        public override void AnswerAI(TDiplomaticAction Action, sbyte Player, TDiplomaticRelation Relation, TByteList IdealPeace, TByteList IdealWar)
        {
            string ErrorText = "";
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FAIControl.CanProposeAlliance((byte)Action.FSource))
            {
                RandSeed = (int)(((uint)(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FInitialRandomSeed + Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay + Action.FSource + Action.FTarget)) >> 2);
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

        internal static readonly string P5ProposeAllianceRStr = "Propose alliance";
        internal static readonly string P5AllianceProposalRStr = "Alliance proposal";
        internal static readonly string P5MadeAllianceRStr = "Made alliance";
    }
}

namespace Aow.Game.Engine
{

    // Kind 3: the source writes to the target.
    public sealed partial record SendMessage : DiplomaticProposal
    {
        public const byte Kind = 3;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Message;

        public override string Name(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Engine.SendMessage.P5SendMessageRStr);

        public override string LogText(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.P5MessageRStr);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        internal static readonly string P5SendMessageRStr = "Send Message";
    }
}

namespace Aow.Game.Session
{

    // Kind 4: the source threatens to attack the target.
    public sealed partial record ThreatenToAttack : DiplomaticProposal
    {
        public const byte Kind = 4;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Threat;

        public override int Icon => 46;

        public override string Name(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.ThreatenToAttack.P5ThreatenToAttackRStr);

        public override string LogText(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.ThreatenToAttack.P5ThreatRStr);

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Proposing(112);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        internal static readonly string P5ThreatenToAttackRStr = "Threaten to attack";
        internal static readonly string P5ThreatRStr = "Threat";
    }

    // Kind 5: the source pays the target a tribute.
    public sealed partial record MakeTribute(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 5;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 48;

        public override string Name(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.MakeTribute.P5MakeTributeRStr);

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.MakeTribute.P5TributeRStr);
            return Outcome(Action, subject, subject, AcceptedLine(subject), "");
        }

        public override string LoggedSample => "SFX\\EVENTS\\MESSAGE2.WAV";

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Exchange(Reaction, 118, 133, 136);

        public override bool CanSend(TDiplomaticAction Action, ref string ErrorText)
        {
            if (Gold == 0 && Mana == 0)
            {
                ErrorText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.MakeTribute.P5NoGoldManaSpecifiedRStr);
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
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(200) + 50 < Gold + Mana)
                {
                    Action.Accept(ref ErrorText);
                }
                else
                {
                    Action.Reject(ref ErrorText);
                }
            }
        }

        internal static readonly string P5MakeTributeRStr = "Make tribute";
        internal static readonly string P5NoGoldManaSpecifiedRStr = "No gold/mana specified";
        internal static readonly string P5TributeRStr = "Tribute";
    }

    // Kind 6: the source ends its alliance with the target.
    public sealed partial record BreakAlliance : DiplomaticProposal
    {
        public const byte Kind = 6;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Notice;

        public override int Icon => 8;

        public override string Name(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.P5BreakAllianceRStr);

        public override string LogText(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.BreakAlliance.P5AllianceBrokenRStr);

        public override void DefaultMessage(DiplomaticMessageText Text, TDiplomaticActionReaction Reaction, TDiplomaticRelation Relation) =>
            Text.Proposing(109);

        public override void Sent(TDiplomaticAction Action)
        {
            Action.ApplyRelationModifiers();
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FSource, (sbyte)(byte)Action.FTarget, TDiplomaticRelation.drPeace);
            if (Action.FSource == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
            {
                var p5AllianceBroken = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.BreakAlliance.P5AllianceBrokenRStr);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(p5AllianceBroken);
            }

            Broadcast(Action, -1, Action.FTarget);
        }

        public override int RelationModifier(TDiplomaticAction Action, sbyte Player, sbyte Race) =>
            Player == Action.FSource && Relation(Action.FSource, Action.FTarget) == TDiplomaticRelation.drAlliance
                ? RaceModifier(Action.FTarget, Race, -40)
                : 0;

        internal static readonly string P5AllianceBrokenRStr = "Alliance broken";
    }

    // Kind 7: the source writes to the target in the name of the other player, offered between human
    // players to a source of race 5 whose hero emerges (Player.PAS:1226).
    public sealed partial record SendMessageAsY : DiplomaticProposal
    {
        public const byte Kind = 7;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, 0, 0);

        public override ProposalView View => ProposalView.Message;

        public override bool SignedAsOtherPlayer => true;

        public override string Name(TDiplomaticAction Action)
        {
            var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.SendMessageAsY.P5SendMessageAsXRStr), [name]);
        }

        public override string LogText(TDiplomaticAction Action) => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.P5MessageRStr);

        public override void Sent(TDiplomaticAction Action) => Deliver(Action);

        private static readonly string P5SendMessageAsXRStr = "Send message as %s";
    }

    // Kind 8: the source asks the target to end its alliance with the other player.
    public sealed partial record BreakAllianceWithY(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 8;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(TDiplomaticAction Action)
        {
            var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.BreakAllianceWithY.P5SuggestAllianceBreakWithYRStr), [name]);
        }

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.P5BreakAllianceRStr);
            return Outcome(Action, Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.BreakAllianceWithY.P5BreakAllianceSuggestionRStr), subject, AcceptedLine(subject), "");
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
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FTarget, (sbyte)(byte)Action.FOtherPlayer, TDiplomaticRelation.drPeace);
                var broken = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).CreateDiplomaticAction(new BreakAlliance(), Action.FOtherPlayer, -1);
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

        private static readonly string P5SuggestAllianceBreakWithYRStr = "Suggest alliance break with %s";
        internal static readonly string P5BreakAllianceSuggestionRStr = "Break alliance suggestion";
    }

    // Kind 9: the source asks the target to declare war on the other player.
    public sealed partial record DeclareWarToY(int Gold = 0, int Mana = 0) : OfferProposal(Gold, Mana)
    {
        public const byte Kind = 9;

        public override (byte Kind, int Gold, int Mana) ToFields() => (Kind, Gold, Mana);

        public override int Icon => 47;

        public override string Name(TDiplomaticAction Action)
        {
            var name = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).GetName();
            return Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.DeclareWarToY.P5SuggestWarWithYRStr), [name]);
        }

        public override string LogText(TDiplomaticAction Action)
        {
            var subject = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.P5DeclareWarRStr);
            return Outcome(Action, Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.DeclareWarToY.P5DeclareWarSuggestionRStr), subject, AcceptedLine(subject), "");
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
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FOtherPlayer).PlayerSeen((sbyte)(byte)Action.FTarget);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.SetDiplomaticRelation((sbyte)(byte)Action.FTarget, (sbyte)(byte)Action.FOtherPlayer, TDiplomaticRelation.drWar);
                var war = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).CreateDiplomaticAction(new DeclareWar(), Action.FOtherPlayer, -1);
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
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Action.FTarget).FAIControl.CanDeclareWar((byte)Action.FOtherPlayer))
                    {
                        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(5) == 1 && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(550) + 250 < Action.FEscrowGold + Action.FEscrowMana)
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

        private static readonly string P5SuggestWarWithYRStr = "Suggest war with %s";
        internal static readonly string P5DeclareWarSuggestionRStr = "Declare war suggestion";
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
                ? Aow.Game.Magic.Abilities.AbilityRules.Format(text.Text, [sourceRace, targetRace, otherRace])
                : Aow.Game.Magic.Abilities.AbilityRules.Format(text.Text, [sourceRace, targetRace]);
        }

        // Text Number of the alignment row, the target's races first.
        public void Replying(int Number, bool AboutOther = false)
        {
            textControl.RetrieveText(alignment + Number, text);
            text.Text = AboutOther
                ? Aow.Game.Magic.Abilities.AbilityRules.Format(text.Text, [targetRace, sourceRace, otherRace])
                : Aow.Game.Magic.Abilities.AbilityRules.Format(text.Text, [targetRace, sourceRace]);
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
            using var text = new TStringList();
            Aow.Game.Session.TDiplomaticAction.RetrieveDefaultDiplomaticActionMessage(Source, Target, Other, Proposal, Reaction, text);
            Proposal = Proposal with { Message = DiplomaticProposal.Lines(text) };
        }

        // Moves every race's relation to source and target by the proposal's modifiers (Diplomacy.pas:826, 992).
        public void ApplyRelationModifiers()
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.LockRelationsChanged();
            for (int I = 0, last = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.Count - 1; I <= last; I++)
            {
                var playerRelationValue = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(I).GetPlayerRelationValue(FSource);
                var relationModifier = GetRelationModifier(FSource, (sbyte)I);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(I).SetPlayerRelationValue(FSource, relationModifier + playerRelationValue);
                var playerRelationValue2 = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(I).GetPlayerRelationValue(FTarget);
                var relationModifier2 = GetRelationModifier(FTarget, (sbyte)I);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(I).SetPlayerRelationValue(FTarget, relationModifier2 + playerRelationValue2);
            }

            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.UnlockRelationsChanged();
        }
    }
}
