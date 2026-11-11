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
// proposal (AoWE.DiplomaticProposals.cs), stored as ActionType, Gold, Mana and Message
public partial class DiplomaticAction : EngineObject
{
    // Diplomacy.pas:495
    public new const int ClassID = 131666;

    // Diplomacy.pas:482
    public DiplomaticAction(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    public int FRefCount;
    public int FDay;
    public sbyte FSource;
    public sbyte FTarget;
    // The census finds it written for every kind, 255 for the kinds that name no other player.
    public sbyte FOtherPlayer;
    public DiplomaticActionReaction FReaction;
    public DiplomaticProposal Proposal = new DeclareWar();
    public int FEscrowGold;
    public int FEscrowMana;
    public int FEventLogID;

    // Diplomacy.pas:596
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        var (kind, gold, mana) = Proposal.ToFields();
        var message = new StringList();
        foreach (var line in Proposal.Message)
        {
            message.Add(line);
        }

        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FSource), 20);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FTarget), 21);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FOtherPlayer), 22);
        Stream.rwInteger(ref gold, 23);
        Stream.rwInteger(ref mana, 24);
        Stream.rwData(Bytes.Of(ref kind, 1), 1, 25);
        Stream.rwData(Bytes.Of(ref FReaction, 1), 1, 26);
        Stream.rwStrings(message, 27);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Proposal = DiplomaticProposal.FromFields(kind, gold, mana) with { Message = DiplomaticProposal.Lines(message) };
        }

        message?.Dispose();
        Stream.rwInteger(ref FEscrowGold, 28);
        Stream.rwInteger(ref FEscrowMana, 29);
        Stream.rwInteger(ref FEventLogID, 30);
        Stream.rwInteger(ref FDay, 31);
    }

    // Diplomacy.pas:586
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Diplomacy.pas:589
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Diplomacy.pas:613
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        bool Ignore;
        EventLog Log;
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FSource && FReaction == DiplomaticActionReaction.None)
        {
            Ignore = Proposal.LapsesUnanswered(this);
            if (Ignore && (AoWHSMap.Current.FPlayMode != PlayMode.Simultaneous || AoWHSMap.Current.FDay > FDay + 1))
            {
                FReaction = DiplomaticActionReaction.Ignore;
                Log = AoWHSMap.Current.FPlayers.GetPlayers(FTarget).FEventLogbook.FindID(FEventLogID);
                Assert(Log is DiplomaticActionEventLog, "", "Diplomacy.pas", 697);
                AoWHSMap.Current.FPlayers.GetPlayers(FSource).FEventLogbook.AddEvent(Log);
                var players = AoWHSMap.Current.FPlayers.GetPlayers(FSource);
                AoWHSMap.Current.FPlayers.GetPlayers(FSource).SetGems(players.FGems + FEscrowGold);
                var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FSource);
                AoWHSMap.Current.FPlayers.GetPlayers(FSource).FMagicControl.SetMana(fPlayersPlayers.FMagicControl.FMana + FEscrowMana);
                FEscrowGold = 0;
                FEscrowMana = 0;
            }
        }
    }

    // Diplomacy.pas:992
    public virtual void ExecuteSend(DiplomaticActionTokenEvent TE)
    {
        if (FReaction == DiplomaticActionReaction.None)
        {
            var (_, gold, mana) = Proposal.ToFields();
            FEscrowGold = gold;
            var players = AoWHSMap.Current.FPlayers.GetPlayers(FSource);
            AoWHSMap.Current.FPlayers.GetPlayers(FSource).SetGems(players.FGems - gold);
            FEscrowMana = mana;
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FSource);
            AoWHSMap.Current.FPlayers.GetPlayers(FSource).FMagicControl.SetMana(fPlayersPlayers.FMagicControl.FMana - mana);
            Proposal.Sent(this);
        }
    }

    // Diplomacy.pas:826
    public virtual void ExecuteAccept(DiplomaticActionTokenEvent TE)
    {
        FReaction = DiplomaticActionReaction.Accept;
        RetrieveDefaultMessage(FSource, FTarget, FOtherPlayer, FReaction);
        var players = AoWHSMap.Current.FPlayers.GetPlayers(FTarget);
        AoWHSMap.Current.FPlayers.GetPlayers(FTarget).SetGems(players.FGems + FEscrowGold);
        FEscrowGold = 0;
        var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FTarget);
        AoWHSMap.Current.FPlayers.GetPlayers(FTarget).FMagicControl.SetMana(fPlayersPlayers.FMagicControl.FMana + FEscrowMana);
        FEscrowMana = 0;
        ApplyRelationModifiers();
        Proposal.Accepted(this);
    }

    // Diplomacy.pas:950
    public virtual void ExecuteReject(DiplomaticActionTokenEvent TE)
    {
        DiplomaticActionEventLog Log;
        FReaction = Proposal.RejectReaction(this);
        var players = AoWHSMap.Current.FPlayers.GetPlayers(FSource);
        AoWHSMap.Current.FPlayers.GetPlayers(FSource).SetGems(players.FGems + FEscrowGold);
        FEscrowGold = 0;
        var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(FSource);
        AoWHSMap.Current.FPlayers.GetPlayers(FSource).FMagicControl.SetMana(fPlayersPlayers.FMagicControl.FMana + FEscrowMana);
        FEscrowMana = 0;
        RetrieveDefaultMessage(FSource, FTarget, FOtherPlayer, FReaction);
        Log = (DiplomaticActionEventLog)AoWHSMap.Current.FPlayers.GetPlayers(FTarget).FEventLogbook.FindID(FEventLogID);
        Log.SetDiplomaticAction(this);
        AoWHSMap.Current.FPlayers.GetPlayers(FSource).FEventLogbook.AddEvent(Log);
    }

    // Diplomacy.pas:722
    public virtual void ExecuteAI(sbyte Player)
    {
        ByteList Peace;
        ByteList War;
        ByteList Unknown;
        ByteList IdealPeace;
        ByteList IdealWar;
        DiplomaticRelation Relation;
        DiplomaticActionReaction Reaction;
        Reaction = FReaction;
        if (Reaction == DiplomaticActionReaction.None)
        {
            Relation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation((sbyte)(byte)FSource, (sbyte)(byte)FTarget);
            IdealPeace = new ByteList();
            IdealWar = new ByteList();
            Peace = new ByteList();
            War = new ByteList();
            Unknown = new ByteList();
            AoWHSMap.Current.FPlayers.GetPlayers(FTarget).FAIControl.ListPeaceWarUnknownPlayers(Peace, War, Unknown);
            AoWHSMap.Current.FPlayers.GetPlayers(FTarget).FAIControl.ListIdealPeaceWarPlayers(IdealPeace, IdealWar);
            Proposal.AnswerAI(this, Player, Relation, IdealPeace, IdealWar);
            Peace?.Dispose();
            War?.Dispose();
            Unknown?.Dispose();
            IdealPeace?.Dispose();
            IdealWar?.Dispose();
        }
        else
        {
            if (Set<DiplomaticActionReaction>.Of(DiplomaticActionReaction.Reject, DiplomaticActionReaction.Ignore).Contains(Reaction))
            {
                Proposal.RefusedAI(this);
            }
        }
    }

    // Diplomacy.pas:1055
    public virtual bool CanSend(ref string ErrorText)
    {
        bool Result;
        var (_, gold, mana) = Proposal.ToFields();
        if (AoWHSMap.Current.FPlayers.GetPlayers(FSource).FGems >= gold)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FSource).FMagicControl.FMana >= mana)
            {
                Result = Proposal.CanSend(this, ref ErrorText);
            }
            else
            {
                Result = false;
                ErrorText = AbilityRules.TranslateRStr(P5NotEnoughManaRStr);
            }
        }
        else
        {
            Result = false;
            ErrorText = AbilityRules.TranslateRStr(GameText.P5NotEnoughGoldRStr);
        }

        return Result;
    }

    // Diplomacy.pas:1081
    public virtual bool Send(ref string ErrorText)
    {
        bool Result;
        DiplomaticActionTokenEvent TE;
        if (CanSend(ref ErrorText) && !AoWHSMap.Current.FPlayers.GetPlayers(FSource).GetBusy())
        {
            FDay = AoWHSMap.Current.FDay;
            TE = new DiplomaticActionTokenEvent(null);
            TE.SetDiplomaticAction(this);
            TE.FOperation = 0;
            AoWHSMap.Current.FTokenManager.TokenControls[FSource].AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:920
    public virtual bool CanAccept(ref string ErrorText) => true;

    // Diplomacy.pas:923
    public virtual bool Accept(ref string ErrorText)
    {
        bool Result;
        DiplomaticActionTokenEvent TE;
        if (CanAccept(ref ErrorText) && !AoWHSMap.Current.FPlayers.GetPlayers(FTarget).GetBusy())
        {
            TE = new DiplomaticActionTokenEvent(null);
            TE.SetDiplomaticAction(this);
            TE.FOperation = 1;
            AoWHSMap.Current.FTokenManager.TokenControls[FTarget].AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:974
    public virtual bool CanReject(ref string ErrorText) => true;

    // Diplomacy.pas:977
    public virtual bool Reject(ref string ErrorText)
    {
        bool Result;
        DiplomaticActionTokenEvent TE;
        if (CanReject(ref ErrorText) && !AoWHSMap.Current.FPlayers.GetPlayers(FTarget).GetBusy())
        {
            TE = new DiplomaticActionTokenEvent(null);
            TE.SetDiplomaticAction(this);
            TE.FOperation = 2;
            AoWHSMap.Current.FTokenManager.TokenControls[FTarget].AddEvent(TE);
            TE.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:505
    public string GetName(sbyte Player) => Proposal.Name(this);

    // Diplomacy.pas:527
    public int GetRelationModifier(sbyte Player, sbyte Race) => Proposal.RelationModifier(this, Player, Race);

    // Diplomacy.pas:707
    public DiplomaticActionEventLog CreateEventLog()
    {
        DiplomaticActionEventLog Log;
        Log = new DiplomaticActionEventLog(null);
        Log.FDay = AoWHSMap.Current.FDay;
        Log.SetDiplomaticAction(this);
        return Log;
    }

    // Diplomacy.pas:938
    public void UpdateAIDiplomaticActionsLog(sbyte Player)
    {
        DiplomaticAction Action;
        var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        if (Set<PlayerType>.Of().WithRange(PlayerType.CPU1, PlayerType.CPU3).WithRange(PlayerType.CPU4, PlayerType.CPU5).Contains(players.FPlayerType))
        {
            Action = (DiplomaticAction)Copy(GameTables.AoWEngine);
            Action.Proposal = Action.Proposal with { Message = [] };
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FDiplomaticActions.AddChild(Action);
            Action.Release();
        }
    }

    private static readonly string P5NotEnoughManaRStr = "Not enough mana.";

    private static void RetrieveDefaultDiplomaticActionMessage(sbyte Source, sbyte Target, sbyte Other, DiplomaticProposal Proposal, DiplomaticActionReaction Reaction, StringCollection Text)
    {
        Text.Clear();
        TextControl fTextControl = AoWHSSet.Current.FTextControl;
        DiplomaticRelation diplomaticRelation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Source, Target);
        Player players = AoWHSMap.Current.FPlayers.GetPlayers(Source);
        string sourceRace = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[players.FPlayerRace]);
        Player players2 = AoWHSMap.Current.FPlayers.GetPlayers(Target);
        string targetRace = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[players2.FPlayerRace]);
        string otherRace;
        if (Other > 0)
        {
            Player players3 = AoWHSMap.Current.FPlayers.GetPlayers(Other);
            otherRace = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[players3.FPlayerRace]);
        }
        else
        {
            otherRace = "";
        }
        Player players4 = AoWHSMap.Current.FPlayers.GetPlayers(Source);
        Alignment alignment = AoWHSSet.Current.FRaceResources.GetRaceResource(players4.FPlayerRace).GetAlignment();
        int alignment2 = ((!Set<Alignment>.Of(new Alignment[2]
        {
            Alignment.PureGood,
            Alignment.Good
        }).Contains(alignment)) ? ((!Set<Alignment>.Of(new Alignment[2]
        {
            Alignment.Evil,
            Alignment.PureEvil
        }).Contains(alignment)) ? 1 : 2) : 0);
        Proposal.DefaultMessage(new DiplomaticMessageText(fTextControl, alignment2, sourceRace, targetRace, otherRace, Text), Reaction, diplomaticRelation);
    }

    // intf_P5_08.inc:143

    // intf_P5_08.inc:143

}
