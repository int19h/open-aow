using Aow.Engine.Collections;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

public abstract partial record DiplomaticProposal
{
    private static readonly Catalog<byte, Func<int, int, DiplomaticProposal>> s_kinds = new("diplomatic proposal");

    // The message lines (property 27): the default text of the kind, the sender's text, or the
    // reply's text once the target answers. The copies in the players' action lists hold none.
    public IReadOnlyList<string> Message { get; init; } = [];

    // The kind, gold and mana the original format stores.
    public abstract (byte Kind, int Gold, int Mana) ToFields();

    // The proposal the stored kind, gold and mana name. A kind no proposal registered reads as
    // UnknownProposal, which writes the three fields back unchanged.
    public static DiplomaticProposal FromFields(byte kind, int gold, int mana) =>
        s_kinds.TryGet(kind, out var create) ? create(gold, mana) : new UnknownProposal(kind, gold, mana);

    // Registers the proposal of a kind. A second registration of a kind throws.
    internal static void Register(byte kind, Func<int, int, DiplomaticProposal> create) => s_kinds.Add(kind, create);

    // The kinds with a registered proposal.
    internal static bool IsRegistered(byte kind) => s_kinds.TryGet(kind, out _);

    // The message as TStrings.Text joins it.
    public string MessageText => string.Concat(Message.Select(static line => line + "\r\n"));

    // The lines of Text.
    public static IReadOnlyList<string> Lines(StringCollection text)
    {
        var lines = new string[text.Count];
        for (int I = 0; I < lines.Length; I++)
        {
            lines[I] = text[I];
        }

        return lines;
    }

    // The lines TStrings.Text splits Text into.
    public static IReadOnlyList<string> Lines(string text)
    {
        var list = new StringList();
        list.Text = text;
        var lines = Lines(list);
        list?.Dispose();
        return lines;
    }

    public virtual bool Equals(DiplomaticProposal other) =>
        ReferenceEquals(this, other) || other is not null && EqualityContract == other.EqualityContract && Message.SequenceEqual(other.Message);

    public override int GetHashCode() => HashCode.Combine(EqualityContract, Message.Count);

    // The proposal with the gold and mana the sender adds. A kind that carries none ignores them.
    public virtual DiplomaticProposal WithOffer(int gold, int mana) => this;

    // How the relations window composes and shows the proposal (AoWRelations.pas:811, E3EventDialogs.pas:194).
    public virtual ProposalView View => ProposalView.None;

    // The relations window's icon for the proposal, -1 for none.
    public virtual int Icon => -1;

    // The message claims to come from the other player, not from the source (AoWRelations.pas:768,
    // 882, E3EventDialogs.pas:194, Diplomacy.pas:1228, 1333).
    public virtual bool SignedAsOtherPlayer => false;

    // The line of the actions list (Diplomacy.pas:505).
    public virtual string Name(DiplomaticAction action) => "";

    // The logbook line (Diplomacy.pas:1265).
    public virtual string LogText(DiplomaticAction action) => "DiplomaticActionEventLog";

    // The sample played when the proposal enters a logbook (Diplomacy.pas:1208).
    public virtual string LoggedSample => "SFX\\EVENTS\\MESSAGE.WAV";

    // The logbook's middle icon (Diplomacy.pas:1353).
    public virtual int MiddleIcon(DiplomaticAction action) => 0x16;

    // Fills Text with the kind's default message for Reaction (Diplomacy.pas:1420). Relation is
    // the relation of the source to the target.
    public virtual void DefaultMessage(DiplomaticMessageText text, DiplomaticActionReaction reaction, DiplomaticRelation relation)
    {
    }

    // The check the kind adds once the source can pay the offer (Diplomacy.pas:1055).
    public virtual bool CanSend(DiplomaticAction action, ref string errorText) => true;

    // What sending does once the offer sits in escrow (Diplomacy.pas:992).
    public virtual void Sent(DiplomaticAction action)
    {
    }

    // Whether the proposal, still unanswered when its source's turn comes round, lapses as
    // ignored (Diplomacy.pas:613).
    public virtual bool LapsesUnanswered(DiplomaticAction action) => false;

    // What acceptance does once the escrow went to the target and the race relations moved (Diplomacy.pas:826).
    public virtual void Accepted(DiplomaticAction action)
    {
    }

    // The reaction a rejection records (Diplomacy.pas:950).
    public virtual DiplomaticActionReaction RejectReaction(DiplomaticAction action) => DiplomaticActionReaction.Reject;

    // The AI target's answer (Diplomacy.pas:722). Relation is the relation of the source to the
    // target, IdealPeace and IdealWar the target's ideal partners.
    public virtual void AnswerAI(DiplomaticAction action, sbyte player, DiplomaticRelation relation, ByteList idealPeace, ByteList idealWar)
    {
    }

    // The AI source's step once the target rejected or ignored the proposal (Diplomacy.pas:722).
    public virtual void RefusedAI(DiplomaticAction action)
    {
    }

    // How the proposal moves the relation of Race to Player (Diplomacy.pas:527).
    public virtual int RelationModifier(DiplomaticAction action, sbyte player, sbyte race) => 0;

    // Modifier, scaled by how Race regards the race of Other, or Modifier itself for Other's own race.
    protected static int RaceModifier(sbyte other, sbyte race, int modifier)
    {
        if (StrategicMap.Current.Players.GetPlayers(other).PlayerRace != race)
        {
            var players = StrategicMap.Current.Players.GetPlayers(other);
            return Round((double)((StrategicMap.Current.Races.GetRace(race).GetRelationValue(players.PlayerRace) - 50) * modifier) * 0.01);
        }

        return modifier;
    }

    // The relation of First to Second.
    protected static DiplomaticRelation Relation(sbyte first, sbyte second) =>
        StrategicMap.Current.Players.GetDiplomaticRelation((sbyte)(byte)first, (sbyte)(byte)second);

    // Shows Log at once when Player is seated.
    protected static void ShowTo(EventLog log, sbyte player)
    {
        if (player == StrategicMap.Current.SeatedPlayer)
        {
            log.Execute(false, ExecuteTrigger.Event, null);
        }
    }

    // Announces the action to every player who knows both sides but Exclude, and shows it to Viewer.
    protected static void Broadcast(DiplomaticAction action, sbyte exclude, sbyte viewer)
    {
        var log = action.CreateEventLog();
        BroadcastDiplomaticEvent(log, exclude);
        ShowTo(log, viewer);
        log.Release();
    }

    // Delivers the proposal to the target's logbook (Diplomacy.pas:992, the arm of the kinds that wait for an answer).
    protected static void Deliver(DiplomaticAction action)
    {
        StrategicMap.Current.Players.GetPlayers(action.Target).PlayerSeen((sbyte)(byte)action.Source);
        if (action.Source == StrategicMap.Current.SeatedPlayer)
        {
            var messageSent = AbilityRules.TranslateResourceString(s_messageSentText);
            StrategicMap.Current.ShowMessage(messageSent);
        }

        var log = action.CreateEventLog();
        StrategicMap.Current.Players.GetPlayers(action.Target).EventLogbook.AddEvent(log);
        ShowTo(log, action.Target);
        log.Release();
        StrategicMap.Current.DiplomaticRelationsChangedEventsStore.Raise(action);
    }

    // Drops the target's logbook entry of the answered proposal.
    protected static void RemoveTargetLog(DiplomaticAction action)
    {
        var log = (DiplomaticActionEventLog)StrategicMap.Current.Players.GetPlayers(action.Target).EventLogbook.FindId(action.EventLogId);
        if (log != null)
        {
            log.SetOwner(null);
        }
    }

    // Moves the target's logbook entry, now holding the answer, to the source and shows it there.
    protected static void ReturnToSource(DiplomaticAction action)
    {
        var log = (DiplomaticActionEventLog)StrategicMap.Current.Players.GetPlayers(action.Target).EventLogbook.FindId(action.EventLogId);
        log.SetDiplomaticAction(action);
        StrategicMap.Current.Players.GetPlayers(action.Source).EventLogbook.AddEvent(log);
        ShowTo(log, action.Source);
    }

    // The logbook line of an answered kind: Proposed while unanswered, else the answer about Subject.
    protected static string Outcome(DiplomaticAction action, string proposed, string subject, string accepted, string otherwise) => action.Reaction switch
    {
        DiplomaticActionReaction.None => proposed,
        DiplomaticActionReaction.Reject => AbilityRules.Format(AbilityRules.TranslateResourceString(RejectedFormat), [subject]),
        DiplomaticActionReaction.Accept => accepted,
        DiplomaticActionReaction.Ignore => AbilityRules.Format(AbilityRules.TranslateResourceString(IgnoredFormat), [subject]),
        _ => otherwise,
    };

    // The accepted line about Subject.
    protected static string AcceptedLine(string subject) => AbilityRules.Format(AbilityRules.TranslateResourceString(AcceptedFormat), [subject]);

    private static readonly string s_messageSentText = "Message sent";

    internal static string RejectedFormat { get; } = "%s rejected";
    internal static string AcceptedFormat { get; } = "%s accepted";
    internal static string IgnoredFormat { get; } = "%s ignored";

    private static void BroadcastDiplomaticEvent(DiplomaticActionEventLog log, sbyte exclude)
    {
        int i = 0;
        for (int number = StrategicMap.Current.Players.Count - 1; i <= number; i++)
        {
            Player players = StrategicMap.Current.Players.GetPlayers(i);
            if (players.Status == PlayerStatus.Playing && i != exclude && (i == log.Action.Source || i == log.Action.Target || (StrategicMap.Current.Players.GetPlayers(i).GetFirstDaySeen((sbyte)(byte)log.Action.Source) != -1 && StrategicMap.Current.Players.GetPlayers(i).GetFirstDaySeen((sbyte)(byte)log.Action.Target) != -1)))
            {
                EventLog eventLog = (EventLog)log.Copy(GameTables.GameEngine);
                players.EventLogbook.AddEvent(eventLog);
                eventLog.Release();
            }
        }
    }
}
