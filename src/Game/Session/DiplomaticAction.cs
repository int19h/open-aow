// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// RefCount, Day, Source, Target, OtherPlayer, Reaction, EscrowGold, EscrowMana, EventLogID and the
// proposal (AoWE.DiplomaticProposals.cs), stored as Kind, Gold, Mana and Message
public partial class DiplomaticAction : EngineObject, IOwned<DiplomaticActionList>, IOwnedSink<DiplomaticActionList>
{
    // A reference list holds the action and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the cast never fails.
    public new DiplomaticActionList Owner { get => (DiplomaticActionList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is DiplomaticActionList;

    void IOwnedSink<DiplomaticActionList>.SetOwner(DiplomaticActionList owner) => SetOwner(owner);

    // Diplomacy.pas:495
    public new const int ClassId = 131666;

    // Diplomacy.pas:482
    public DiplomaticAction(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
    }

    private int _refCount;
    public int Day;
    public sbyte Source;
    public sbyte Target;
    // The census finds it written for every kind, 255 for the kinds that name no other player.
    public sbyte OtherPlayer;
    public DiplomaticActionReaction Reaction;

    public DiplomaticProposal Proposal { get; set; } = new DeclareWar();

    public int EscrowGold;
    public int EscrowMana;
    public int EventLogId;

    // Diplomacy.pas:596
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        var (kind, gold, mana) = Proposal.ToFields();
        var message = new StringList();
        foreach (var line in Proposal.Message)
        {
            message.Add(line);
        }

        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Source), 20);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Target), 21);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref OtherPlayer), 22);
        stream.Int32(ref gold, 23);
        stream.Int32(ref mana, 24);
        stream.Bytes(Bytes.Of(ref kind, 1), 1, 25);
        stream.Bytes(Bytes.Of(ref Reaction, 1), 1, 26);
        stream.Lines(message, 27);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Proposal = DiplomaticProposal.FromFields(kind, gold, mana) with { Message = DiplomaticProposal.Lines(message) };
        }

        message?.Dispose();
        stream.Int32(ref EscrowGold, 28);
        stream.Int32(ref EscrowMana, 29);
        stream.Int32(ref EventLogId, 30);
        stream.Int32(ref Day, 31);
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
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        bool ignore;
        EventLog log;
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == Source && Reaction == DiplomaticActionReaction.None)
        {
            ignore = Proposal.LapsesUnanswered(this);
            if (ignore && (StrategicMap.Current.PlayMode != PlayMode.Simultaneous || StrategicMap.Current.Day > Day + 1))
            {
                Reaction = DiplomaticActionReaction.Ignore;
                log = StrategicMap.Current.Players.GetPlayers(Target).EventLogbook.FindId(EventLogId);
                Assert(log is DiplomaticActionEventLog, "", "Diplomacy.pas", 697);
                StrategicMap.Current.Players.GetPlayers(Source).EventLogbook.AddEvent(log);
                var players = StrategicMap.Current.Players.GetPlayers(Source);
                StrategicMap.Current.Players.GetPlayers(Source).SetGems(players.Gems + EscrowGold);
                var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Source);
                StrategicMap.Current.Players.GetPlayers(Source).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + EscrowMana);
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
            var (_, gold, mana) = Proposal.ToFields();
            EscrowGold = gold;
            var players = StrategicMap.Current.Players.GetPlayers(Source);
            StrategicMap.Current.Players.GetPlayers(Source).SetGems(players.Gems - gold);
            EscrowMana = mana;
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Source);
            StrategicMap.Current.Players.GetPlayers(Source).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana - mana);
            Proposal.Sent(this);
        }
    }

    // Diplomacy.pas:826
    public virtual void ExecuteAccept(DiplomaticActionTokenEvent tokenEvent)
    {
        Reaction = DiplomaticActionReaction.Accept;
        RetrieveDefaultMessage(Source, Target, OtherPlayer, Reaction);
        var players = StrategicMap.Current.Players.GetPlayers(Target);
        StrategicMap.Current.Players.GetPlayers(Target).SetGems(players.Gems + EscrowGold);
        EscrowGold = 0;
        var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Target);
        StrategicMap.Current.Players.GetPlayers(Target).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + EscrowMana);
        EscrowMana = 0;
        ApplyRelationModifiers();
        Proposal.Accepted(this);
    }

    // Diplomacy.pas:950
    public virtual void ExecuteReject(DiplomaticActionTokenEvent tokenEvent)
    {
        DiplomaticActionEventLog log;
        Reaction = Proposal.RejectReaction(this);
        var players = StrategicMap.Current.Players.GetPlayers(Source);
        StrategicMap.Current.Players.GetPlayers(Source).SetGems(players.Gems + EscrowGold);
        EscrowGold = 0;
        var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(Source);
        StrategicMap.Current.Players.GetPlayers(Source).MagicControl.SetMana(fPlayersPlayers.MagicControl.Mana + EscrowMana);
        EscrowMana = 0;
        RetrieveDefaultMessage(Source, Target, OtherPlayer, Reaction);
        log = (DiplomaticActionEventLog)StrategicMap.Current.Players.GetPlayers(Target).EventLogbook.FindId(EventLogId);
        log.SetDiplomaticAction(this);
        StrategicMap.Current.Players.GetPlayers(Source).EventLogbook.AddEvent(log);
    }

    // Diplomacy.pas:722
    public virtual void ExecuteAI(sbyte player)
    {
        ByteList peace;
        ByteList war;
        ByteList unknown;
        ByteList idealPeace;
        ByteList idealWar;
        DiplomaticRelation relation;
        DiplomaticActionReaction reaction;
        reaction = Reaction;
        if (reaction == DiplomaticActionReaction.None)
        {
            relation = StrategicMap.Current.Players.GetDiplomaticRelation((sbyte)(byte)Source, (sbyte)(byte)Target);
            idealPeace = new ByteList();
            idealWar = new ByteList();
            peace = new ByteList();
            war = new ByteList();
            unknown = new ByteList();
            StrategicMap.Current.Players.GetPlayers(Target).AIControl.ListPeaceWarUnknownPlayers(peace, war, unknown);
            StrategicMap.Current.Players.GetPlayers(Target).AIControl.ListIdealPeaceWarPlayers(idealPeace, idealWar);
            Proposal.AnswerAI(this, player, relation, idealPeace, idealWar);
            peace?.Dispose();
            war?.Dispose();
            unknown?.Dispose();
            idealPeace?.Dispose();
            idealWar?.Dispose();
        }
        else
        {
            if (Set<DiplomaticActionReaction>.Of(DiplomaticActionReaction.Reject, DiplomaticActionReaction.Ignore).Contains(reaction))
            {
                Proposal.RefusedAI(this);
            }
        }
    }

    // Diplomacy.pas:1055
    public virtual bool CanSend(ref string errorText)
    {
        bool result;
        var (_, gold, mana) = Proposal.ToFields();
        if (StrategicMap.Current.Players.GetPlayers(Source).Gems >= gold)
        {
            if (StrategicMap.Current.Players.GetPlayers(Source).MagicControl.Mana >= mana)
            {
                result = Proposal.CanSend(this, ref errorText);
            }
            else
            {
                result = false;
                errorText = AbilityRules.TranslateResourceString(s_notEnoughManaText);
            }
        }
        else
        {
            result = false;
            errorText = AbilityRules.TranslateResourceString(GameText.TributeNotEnoughGoldText);
        }

        return result;
    }

    // Diplomacy.pas:1081
    public virtual bool Send(ref string errorText)
    {
        bool result;
        DiplomaticActionTokenEvent tokenEvent;
        if (CanSend(ref errorText) && !StrategicMap.Current.Players.GetPlayers(Source).GetBusy())
        {
            Day = StrategicMap.Current.Day;
            tokenEvent = new DiplomaticActionTokenEvent(null);
            tokenEvent.SetDiplomaticAction(this);
            tokenEvent.Operation = 0;
            StrategicMap.Current.TokenManager.TokenControls[Source].AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:920
    public virtual bool CanAccept(ref string errorText) => true;

    // Diplomacy.pas:923
    public virtual bool Accept(ref string errorText)
    {
        bool result;
        DiplomaticActionTokenEvent tokenEvent;
        if (CanAccept(ref errorText) && !StrategicMap.Current.Players.GetPlayers(Target).GetBusy())
        {
            tokenEvent = new DiplomaticActionTokenEvent(null);
            tokenEvent.SetDiplomaticAction(this);
            tokenEvent.Operation = 1;
            StrategicMap.Current.TokenManager.TokenControls[Target].AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:974
    public virtual bool CanReject(ref string errorText) => true;

    // Diplomacy.pas:977
    public virtual bool Reject(ref string errorText)
    {
        bool result;
        DiplomaticActionTokenEvent tokenEvent;
        if (CanReject(ref errorText) && !StrategicMap.Current.Players.GetPlayers(Target).GetBusy())
        {
            tokenEvent = new DiplomaticActionTokenEvent(null);
            tokenEvent.SetDiplomaticAction(this);
            tokenEvent.Operation = 2;
            StrategicMap.Current.TokenManager.TokenControls[Target].AddEvent(tokenEvent);
            tokenEvent.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:505
    public string GetName(sbyte player) => Proposal.Name(this);

    // Diplomacy.pas:527
    public int GetRelationModifier(sbyte player, sbyte race) => Proposal.RelationModifier(this, player, race);

    // Diplomacy.pas:707
    public DiplomaticActionEventLog CreateEventLog()
    {
        DiplomaticActionEventLog log;
        log = new DiplomaticActionEventLog(null);
        log._day = StrategicMap.Current.Day;
        log.SetDiplomaticAction(this);
        return log;
    }

    // Diplomacy.pas:938
    public void UpdateAIDiplomaticActionsLog(sbyte player)
    {
        DiplomaticAction action;
        var players = StrategicMap.Current.Players.GetPlayers(player);
        if (Set<PlayerType>.Of().WithRange(PlayerType.Cpu1, PlayerType.Cpu3).WithRange(PlayerType.Cpu4, PlayerType.Cpu5).Contains(players.PlayerType))
        {
            action = (DiplomaticAction)Copy(GameTables.GameEngine);
            action.Proposal = action.Proposal with { Message = [] };
            StrategicMap.Current.Players.GetPlayers(player).DiplomaticActions.AddChild(action);
            action.Release();
        }
    }

    private static readonly string s_notEnoughManaText = "Not enough mana.";

    private static void RetrieveDefaultDiplomaticActionMessage(sbyte source, sbyte target, sbyte other, DiplomaticProposal proposal, DiplomaticActionReaction reaction, StringCollection text)
    {
        text.Clear();
        TextControl fTextControl = GamePack.Current.TextControl;
        DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(source, target);
        Player players = StrategicMap.Current.Players.GetPlayers(source);
        string sourceRace = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[players.PlayerRace]);
        Player players2 = StrategicMap.Current.Players.GetPlayers(target);
        string targetRace = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[players2.PlayerRace]);
        string otherRace;
        if (other > 0)
        {
            Player players3 = StrategicMap.Current.Players.GetPlayers(other);
            otherRace = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[players3.PlayerRace]);
        }
        else
        {
            otherRace = "";
        }
        Player players4 = StrategicMap.Current.Players.GetPlayers(source);
        Alignment alignment = GamePack.Current.RaceResources.GetRaceResource(players4.PlayerRace).GetAlignment();
        int alignment2 = ((!Set<Alignment>.Of(new Alignment[2]
        {
            Alignment.PureGood,
            Alignment.Good
        }).Contains(alignment)) ? ((!Set<Alignment>.Of(new Alignment[2]
        {
            Alignment.Evil,
            Alignment.PureEvil
        }).Contains(alignment)) ? 1 : 2) : 0);
        proposal.DefaultMessage(new DiplomaticMessageText(fTextControl, alignment2, sourceRace, targetRace, otherRace, text), reaction, diplomaticRelation);
    }

    // intf_P5_08.inc:143

    // intf_P5_08.inc:143

}
