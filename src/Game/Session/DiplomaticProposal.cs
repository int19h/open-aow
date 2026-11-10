using Aow.Engine.Collections;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

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
    public static IReadOnlyList<string> Lines(StringCollection Text)
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
        var list = new StringList();
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
    public virtual string Name(DiplomaticAction Action) => "";

    // The logbook line (Diplomacy.pas:1265).
    public virtual string LogText(DiplomaticAction Action) => "DiplomaticActionEventLog";

    // The sample played when the proposal enters a logbook (Diplomacy.pas:1208).
    public virtual string LoggedSample => "SFX\\EVENTS\\MESSAGE.WAV";

    // The logbook's middle icon (Diplomacy.pas:1353).
    public virtual int MiddleIcon(DiplomaticAction Action) => 0x16;

    // Fills Text with the kind's default message for Reaction (Diplomacy.pas:1420). Relation is
    // the relation of the source to the target.
    public virtual void DefaultMessage(DiplomaticMessageText Text, DiplomaticActionReaction Reaction, DiplomaticRelation Relation)
    {
    }

    // The check the kind adds once the source can pay the offer (Diplomacy.pas:1055).
    public virtual bool CanSend(DiplomaticAction Action, ref string ErrorText) => true;

    // What sending does once the offer sits in escrow (Diplomacy.pas:992).
    public virtual void Sent(DiplomaticAction Action)
    {
    }

    // Whether the proposal, still unanswered when its source's turn comes round, lapses as
    // ignored (Diplomacy.pas:613).
    public virtual bool LapsesUnanswered(DiplomaticAction Action) => false;

    // What acceptance does once the escrow went to the target and the race relations moved (Diplomacy.pas:826).
    public virtual void Accepted(DiplomaticAction Action)
    {
    }

    // The reaction a rejection records (Diplomacy.pas:950).
    public virtual DiplomaticActionReaction RejectReaction(DiplomaticAction Action) => DiplomaticActionReaction.Reject;

    // The AI target's answer (Diplomacy.pas:722). Relation is the relation of the source to the
    // target, IdealPeace and IdealWar the target's ideal partners.
    public virtual void AnswerAI(DiplomaticAction Action, sbyte Player, DiplomaticRelation Relation, ByteList IdealPeace, ByteList IdealWar)
    {
    }

    // The AI source's step once the target rejected or ignored the proposal (Diplomacy.pas:722).
    public virtual void RefusedAI(DiplomaticAction Action)
    {
    }

    // How the proposal moves the relation of Race to Player (Diplomacy.pas:527).
    public virtual int RelationModifier(DiplomaticAction Action, sbyte Player, sbyte Race) => 0;

    // Modifier, scaled by how Race regards the race of Other, or Modifier itself for Other's own race.
    protected static int RaceModifier(sbyte Other, sbyte Race, int Modifier)
    {
        if (AoWHSMap.Current.FPlayers.GetPlayers(Other).FPlayerRace != Race)
        {
            var players = AoWHSMap.Current.FPlayers.GetPlayers(Other);
            return Round((double)((AoWHSMap.Current.FRaces.GetRace(Race).GetRelationValue(players.FPlayerRace) - 50) * Modifier) * 0.01);
        }

        return Modifier;
    }

    // The relation of First to Second.
    protected static DiplomaticRelation Relation(sbyte First, sbyte Second) =>
        AoWHSMap.Current.FPlayers.GetDiplomaticRelation((sbyte)(byte)First, (sbyte)(byte)Second);

    // Shows Log at once when Player is seated.
    protected static void ShowTo(EventLog Log, sbyte Player)
    {
        if (Player == AoWHSMap.Current.FSeatedPlayer)
        {
            Log.Execute(false, ExecuteTrigger.Event, null);
        }
    }

    // Announces the action to every player who knows both sides but Exclude, and shows it to Viewer.
    protected static void Broadcast(DiplomaticAction Action, sbyte Exclude, sbyte Viewer)
    {
        var Log = Action.CreateEventLog();
        BroadcastDiplomaticEvent(Log, Exclude);
        ShowTo(Log, Viewer);
        Log.Release();
    }

    // Delivers the proposal to the target's logbook (Diplomacy.pas:992, the arm of the kinds that wait for an answer).
    protected static void Deliver(DiplomaticAction Action)
    {
        AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).PlayerSeen((sbyte)(byte)Action.FSource);
        if (Action.FSource == AoWHSMap.Current.FSeatedPlayer)
        {
            var p5MessageSent = AbilityRules.TranslateRStr(P5MessageSentRStr);
            AoWHSMap.Current.ShowMessage(p5MessageSent);
        }

        var Log = Action.CreateEventLog();
        AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.AddEvent(Log);
        ShowTo(Log, Action.FTarget);
        Log.Release();
        AoWHSMap.Current.FDiplomaticRelationsChangedEventsStore.Raise(Action);
    }

    // Drops the target's logbook entry of the answered proposal.
    protected static void RemoveTargetLog(DiplomaticAction Action)
    {
        var Log = (DiplomaticActionEventLog)AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.FindID(Action.FEventLogID);
        if (Log != null)
        {
            Log.SetOwner(null);
        }
    }

    // Moves the target's logbook entry, now holding the answer, to the source and shows it there.
    protected static void ReturnToSource(DiplomaticAction Action)
    {
        var Log = (DiplomaticActionEventLog)AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).FEventLogbook.FindID(Action.FEventLogID);
        Log.SetDiplomaticAction(Action);
        AoWHSMap.Current.FPlayers.GetPlayers(Action.FSource).FEventLogbook.AddEvent(Log);
        ShowTo(Log, Action.FSource);
    }

    // The logbook line of an answered kind: Proposed while unanswered, else the answer about Subject.
    protected static string Outcome(DiplomaticAction Action, string Proposed, string Subject, string Accepted, string Otherwise) => Action.FReaction switch
    {
        DiplomaticActionReaction.None => Proposed,
        DiplomaticActionReaction.Reject => AbilityRules.Format(AbilityRules.TranslateRStr(P5XRejectedRStr), [Subject]),
        DiplomaticActionReaction.Accept => Accepted,
        DiplomaticActionReaction.Ignore => AbilityRules.Format(AbilityRules.TranslateRStr(P5XIgnoredRStr), [Subject]),
        _ => Otherwise,
    };

    // The accepted line about Subject.
    protected static string AcceptedLine(string Subject) => AbilityRules.Format(AbilityRules.TranslateRStr(P5XAcceptedRStr), [Subject]);

    private static readonly string P5MessageSentRStr = "Message sent";
    internal static readonly string P5XRejectedRStr = "%s rejected";
    internal static readonly string P5XAcceptedRStr = "%s accepted";
    internal static readonly string P5XIgnoredRStr = "%s ignored";

    private static void BroadcastDiplomaticEvent(DiplomaticActionEventLog Log, sbyte Exclude)
    {
        int i = 0;
        for (int num = AoWHSMap.Current.FPlayers.Count - 1; i <= num; i++)
        {
            Player players = AoWHSMap.Current.FPlayers.GetPlayers(i);
            if (players.FStatus == Player.Status.Playing && i != Exclude && (i == Log.FAction.FSource || i == Log.FAction.FTarget || (AoWHSMap.Current.FPlayers.GetPlayers(i).GetFirstDaySeen((sbyte)(byte)Log.FAction.FSource) != -1 && AoWHSMap.Current.FPlayers.GetPlayers(i).GetFirstDaySeen((sbyte)(byte)Log.FAction.FTarget) != -1)))
            {
                EventLog tEventLog = (EventLog)Log.Copy(GameTables.AoWEngine);
                players.FEventLogbook.AddEvent(tEventLog);
                tEventLog.Release();
            }
        }
    }
}
