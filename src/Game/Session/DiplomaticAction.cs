#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Commands;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Session;

// RefCount, Day, Source, Target, OtherPlayer, Reaction, EscrowGold, EscrowMana, EventLogID and the
// proposal (AoWE.DiplomaticProposals.cs), stored as Kind, Gold, Mana and Message
public partial class DiplomaticAction(CustomNode? owner) : EngineObject(owner), IOwned<DiplomaticActionList>, IOwnedSink<DiplomaticActionList>
{
    // A reference list holds the action and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the cast never fails.
    public new DiplomaticActionList Owner => GetOwner<DiplomaticActionList>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is DiplomaticActionList;

    void IOwnedSink<DiplomaticActionList>.SetOwner(DiplomaticActionList owner) => SetOwner(owner);

    // Diplomacy.pas:495
    public new const int ClassId = 131_666;
    private int _refCount = 1;

    public int Day { get; set; }
    public PlayerId? Source { get; set; } = PlayerId.Independent;
    public PlayerId? Target { get; set; } = PlayerId.Independent;
    // The census finds it written for every kind, 255 for the kinds that name no other player.
    public PlayerId? OtherPlayer { get; set; } = PlayerId.Independent;
    public DiplomaticActionReaction Reaction { get; set; }

    public DiplomaticProposal Proposal { get; set; } = new DeclareWar();

    public int EscrowGold { get; set; }
    public int EscrowMana { get; set; }
    public int EventLogId { get; set; }

    // Diplomacy.pas:596
    // The proposal's kind is one stored byte (rwData), the discriminator of the case mapping.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        (byte kind, int gold, int mana) = Proposal.ToFields();
        DelphiStringList message = [];
        foreach (string line in Proposal.Message)
        {
            _ = message.Add(line);
        }

        Source = persister.OptionalPlayer(name: "Source", value: Source, width: IdWidth.UnsignedByte, fallback: 0);
        Target = persister.OptionalPlayer(name: "Target", value: Target, width: IdWidth.UnsignedByte, fallback: 0);
        OtherPlayer = persister.OptionalPlayer(name: "OtherPlayer", value: OtherPlayer, width: IdWidth.UnsignedByte, fallback: 0);
        gold = persister.Int32("Gold", value: gold);
        mana = persister.Int32("Mana", value: mana);
        Span<byte> kindData = [kind];
        persister.Bytes("Kind", data: kindData);
        kind = kindData[0];
        Reaction = persister.Enum("Reaction", value: Reaction);
        persister.Lines(name: "Message", lines: message);
        // The translate stream's mode holds read, so the original ReadWrite builds the proposal
        // with the translated message there too.
        if (persister.Loading || persister.Translating())
        {
            Proposal = DiplomaticProposal.FromFields(kind, gold, mana) with { Message = DiplomaticProposal.Lines(message) };
        }
        EscrowGold = persister.Int32("EscrowGold", value: EscrowGold);
        EscrowMana = persister.Int32("EscrowMana", value: EscrowMana);
        EventLogId = persister.Int32("EventLogId", value: EventLogId);
        Day = persister.Int32("Day", value: Day);
    }

    // Diplomacy.pas:586
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // Diplomacy.pas:589
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (_refCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // Diplomacy.pas:613
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIdExtensions.ToOriginal(Source) && Reaction == DiplomaticActionReaction.None)
        {
            bool ignore = Proposal.LapsesUnanswered(this);
            if (ignore && (GetEngine().StrategicMap.PlayMode != PlayMode.Simultaneous || GetEngine().StrategicMap.Day > Day + 1))
            {
                Reaction = DiplomaticActionReaction.Ignore;
                StrategicMap strategicMap = GetEngine().StrategicMap;
                EventLog? log = strategicMap.Players.GetPlayers(Target).EventLogbook.FindId(EventLogId);
                Assert(log is DiplomaticActionEventLog, message: "", file: "Diplomacy.pas", line: 697);
                strategicMap.Players.GetPlayers(Source).EventLogbook.AddEvent(log);
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                Player? players = engineStrategicMap.Players.GetPlayers(Source);
                engineStrategicMap.Players.GetPlayers(Source).SetGems(players.Gems + EscrowGold);
                StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                Player? fPlayersPlayers = getPlayersStrategicMap.Players.GetPlayers(Source);
                getPlayersStrategicMap.Players.GetPlayers(Source).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + EscrowMana);
                EscrowGold = 0;
                EscrowMana = 0;
            }
        }
    }

    // Diplomacy.pas:992
    public virtual void ExecuteSend(DiplomaticActionTokenEvent tokenEvent)
    {
        if (Reaction == DiplomaticActionReaction.None)
        {
            (byte _, int gold, int mana) = Proposal.ToFields();
            EscrowGold = gold;
            StrategicMap strategicMap = GetEngine().StrategicMap;
            Player? players = strategicMap.Players.GetPlayers(Source);
            strategicMap.Players.GetPlayers(Source).SetGems(players.Gems - gold);
            EscrowMana = mana;
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            Player? fPlayersPlayers = engineStrategicMap.Players.GetPlayers(Source);
            engineStrategicMap.Players.GetPlayers(Source).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana - mana);
            Proposal.Sent(this);
        }
    }

    // Diplomacy.pas:826
    public virtual void ExecuteAccept(DiplomaticActionTokenEvent tokenEvent)
    {
        Reaction = DiplomaticActionReaction.Accept;
        RetrieveDefaultMessage(Source, target: PlayerIdExtensions.ToOriginal(Target), other: OtherPlayer, Reaction);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Player? players = strategicMap.Players.GetPlayers(Target);
        strategicMap.Players.GetPlayers(Target).SetGems(players.Gems + EscrowGold);
        EscrowGold = 0;
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        Player? fPlayersPlayers = engineStrategicMap.Players.GetPlayers(Target);
        engineStrategicMap.Players.GetPlayers(Target).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + EscrowMana);
        EscrowMana = 0;
        ApplyRelationModifiers();
        Proposal.Accepted(this);
    }

    // Diplomacy.pas:950
    public virtual void ExecuteReject(DiplomaticActionTokenEvent tokenEvent)
    {
        Reaction = Proposal.RejectReaction(this);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Player? players = strategicMap.Players.GetPlayers(Source);
        strategicMap.Players.GetPlayers(Source).SetGems(players.Gems + EscrowGold);
        EscrowGold = 0;
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        Player? fPlayersPlayers = engineStrategicMap.Players.GetPlayers(Source);
        engineStrategicMap.Players.GetPlayers(Source).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + EscrowMana);
        EscrowMana = 0;
        RetrieveDefaultMessage(Source, target: PlayerIdExtensions.ToOriginal(Target), other: OtherPlayer, Reaction);
        DiplomaticActionEventLog log = GetEngine().StrategicMap.Players.GetPlayers(Target).EventLogbook.FindId<DiplomaticActionEventLog>(EventLogId);
        log.SetDiplomaticAction(this);
        GetEngine().StrategicMap.Players.GetPlayers(Source).EventLogbook.AddEvent(log);
    }

    // Diplomacy.pas:722
    public virtual void ExecuteAI(PlayerId? player)
    {
        DiplomaticActionReaction reaction = Reaction;
        if (reaction == DiplomaticActionReaction.None)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            DiplomaticRelation relation = strategicMap.Players.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(Source)), otherPlayer: PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(Target)));
            ByteList idealPeace = [];
            ByteList idealWar = [];
            ByteList peace = [];
            ByteList war = [];
            ByteList unknown = [];
            strategicMap.Players.GetPlayers(Target).AIControl.ListPeaceWarUnknownPlayers(peace, war, unknown);
            strategicMap.Players.GetPlayers(Target).AIControl.ListIdealPeaceWarPlayers(idealPeace, war: idealWar);
            Proposal.AnswerAI(this, player, relation, idealPeace, idealWar);
            idealPeace?.Dispose();
            idealWar?.Dispose();
        }
        else
        {
            if (reaction is DiplomaticActionReaction.Reject or DiplomaticActionReaction.Ignore)
            {
                Proposal.RefusedAI(this);
            }
        }
    }

    // Diplomacy.pas:1055
    public virtual bool CanSend(ref string errorText)
    {
        (byte _, int gold, int mana) = Proposal.ToFields();
        bool result;
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Players.GetPlayers(Source).Gems >= gold)
        {
            if (strategicMap.Players.GetPlayers(Source).MagicControl.Mana >= mana)
            {
                return Proposal.CanSend(this, ref errorText);
            }
            else
            {
                result = false;
                errorText = AbilityRules.TranslateResourceString(GetEngine(), S: s_notEnoughManaText);
            }
        }
        else
        {
            result = false;
            errorText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.TributeNotEnoughGoldText);
        }

        return result;
    }

    // Diplomacy.pas:1081
    public virtual bool Send(ref string errorText)
    {
        if (CanSend(ref errorText) && !GetEngine().StrategicMap.Players.GetPlayers(Source).GetBusy())
        {
            Day = GetEngine().StrategicMap.Day;
            DiplomaticActionTokenEvent tokenEvent = new(null) { Holder = CreationHolder() };
            tokenEvent.SetDiplomaticAction(this);
            tokenEvent.Operation = 0;
            GetEngine().StrategicMap.TokenManager.TokenControls[Source].AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:920
    public virtual bool CanAccept(string errorText) => true;

    // Diplomacy.pas:923
    public virtual bool Accept(string errorText)
    {
        if (CanAccept(errorText) && !GetEngine().StrategicMap.Players.GetPlayers(Target).GetBusy())
        {
            DiplomaticActionTokenEvent tokenEvent = new(null) { Holder = CreationHolder() };
            tokenEvent.SetDiplomaticAction(this);
            tokenEvent.Operation = 1;
            GetEngine().StrategicMap.TokenManager.TokenControls[Target].AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:974
    public virtual bool CanReject(string errorText) => true;

    // Diplomacy.pas:977
    public virtual bool Reject(string errorText)
    {
        if (CanReject(errorText) && !GetEngine().StrategicMap.Players.GetPlayers(Target).GetBusy())
        {
            DiplomaticActionTokenEvent tokenEvent = new(null) { Holder = CreationHolder() };
            tokenEvent.SetDiplomaticAction(this);
            tokenEvent.Operation = 2;
            GetEngine().StrategicMap.TokenManager.TokenControls[Target].AddEvent(tokenEvent);
            _ = tokenEvent.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:505
    public string GetName(PlayerId? player) => Proposal.Name(this);

    // Diplomacy.pas:527
    public int GetRelationModifier(PlayerId? player, int race) => Proposal.RelationModifier(this, player, race);

    // Diplomacy.pas:707
    public DiplomaticActionEventLog CreateEventLog()
    {
        DiplomaticActionEventLog log = new(null)
        {
            Holder = CreationHolder(),
            Day = GetEngine().StrategicMap.Day
        };
        log.SetDiplomaticAction(this);
        return log;
    }

    // Diplomacy.pas:938
    public void UpdateAIDiplomaticActionsLog(PlayerId? player)
    {
        Player? players = GetEngine().StrategicMap.Players.GetPlayers(player);
        if (players.PlayerType is (>= PlayerType.Cpu1 and <= PlayerType.Cpu3) or PlayerType.Cpu4 or PlayerType.Cpu5)
        {
            DiplomaticAction action = Copy<DiplomaticAction>(GetEngine());
            action.Proposal = action.Proposal with { Message = [] };
            _ = GetEngine().StrategicMap.Players.GetPlayers(player).DiplomaticActions.AddChild(action);
            _ = action.Release();
        }
    }

    private static readonly string s_notEnoughManaText = "Not enough mana.";

    private static void RetrieveDefaultDiplomaticActionMessage(PlayerId? source, PlayerId? target, int other, DiplomaticProposal proposal, DiplomaticActionReaction reaction, DelphiStringCollection text, GameEngine? engine)
    {
        text.Clear();
        TextControl fTextControl = engine.GamePack.TextControl;
        StrategicMap strategicMap = engine.StrategicMap;
        DiplomaticRelation diplomaticRelation = strategicMap.Players.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(source)), otherPlayer: PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(target)));
        Player? players = strategicMap.Players.GetPlayers(source);
        string sourceRace = AbilityRules.TranslateResourceString(engine, S: GameTables.RaceNamesPluralTexts[players.PlayerRace]);
        Player? players2 = engine.StrategicMap.Players.GetPlayers(target);
        string targetRace = AbilityRules.TranslateResourceString(engine, S: GameTables.RaceNamesPluralTexts[players2.PlayerRace]);
        string otherRace;
        if (other > 0)
        {
            Player? players3 = engine.StrategicMap.Players.GetPlayers(other);
            otherRace = AbilityRules.TranslateResourceString(engine, S: GameTables.RaceNamesPluralTexts[players3.PlayerRace]);
        }
        else
        {
            otherRace = "";
        }
        Player? players4 = engine.StrategicMap.Players.GetPlayers(source);
        Alignment alignment = engine.GamePack.RaceResources.GetRaceResource(players4.PlayerRace).Alignment;
        proposal.DefaultMessage(new DiplomaticMessageText(alignment: (alignment is not (Alignment.PureGood or Alignment.Good)) ? ((alignment is not (Alignment.Evil or Alignment.PureEvil))
            ? 1
            : 2)
            : 0, textControl: fTextControl, sourceRace: sourceRace, targetRace: targetRace, otherRace: otherRace, text: text), reaction, relation: diplomaticRelation);
    }
}
