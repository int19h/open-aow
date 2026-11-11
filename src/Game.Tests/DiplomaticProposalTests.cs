using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Brief 9.6-7: diplomatic proposals against the flat fields and the switches they replaced. The
// bytes were written on main f2575a2 by TDiplomaticAction.ReadWrite with FActionType, FGold, FMana
// and FMessage set as each case lists, alone, inside a TDiplomaticActionTE and inside a
// TDiplomaticActionEventLog. The cases are every property set the 9.6 census found for kinds 0 and
// 1, the fields the source gives kinds 2 to 9, two kinds no proposal registers and a fresh action.
// The Old* methods copy the switches from main f2575a2.
[Collection("Engine")]
[EngineState]
public sealed class DiplomaticProposalTests
{
    public sealed record Case(byte Kind, sbyte Source, sbyte Target, sbyte Other, int Gold, int Mana, byte Reaction,
        int EscrowGold, int EscrowMana, int EventLogId, int Day, string[] Message, string Alone, string Token, string Logged);

    private static readonly Case[] Cases =
    [
        new(0, 2, 1, -1, 0, 0, 0, 0, 0, 19, 4, [],
            "010700520202000714001501160219031A041E051F090201FF00001300000004000000",
            "01070008110200030A0014041524FFFFFFFF520202000714001501160219031A041E051F090201FF0000130000000400000001000000",
            "01070013010200020A00140405000000520202000714001501160219031A041E051F090201FF00001300000004000000"),
        new(0, 4, 8, -1, 0, 0, 0, 0, 0, 0, 6, [],
            "010700520202000614001501160219031A041F050408FF000006000000",
            "01070008110200030A001404151EFFFFFFFF520202000614001501160219031A041F050408FF00000600000001000000",
            "01070013010200020A00140405000000520202000614001501160219031A041F050408FF000006000000"),
        new(0, 6, 11, -1, 0, 0, 0, 0, 0, 16, 7, ["The Dark Elves declare war on the Humans."],
            "010700520202000814001501160219031A041B051E341F38060BFF00002B000000546865204461726B20456C766573206465636C61726520776172206F6E207468652048756D616E732E0D0A1000000007000000",
            "01070008110200030A0014041555FFFFFFFF520202000814001501160219031A041B051E341F38060BFF00002B000000546865204461726B20456C766573206465636C61726520776172206F6E207468652048756D616E732E0D0A100000000700000001000000",
            "01070013010200020A00140405000000520202000814001501160219031A041B051E341F38060BFF00002B000000546865204461726B20456C766573206465636C61726520776172206F6E207468652048756D616E732E0D0A1000000007000000"),
        new(1, 1, 2, -1, 0, 0, 0, 0, 0, 42, 7, [],
            "010700520202000714001501160219031A041E051F090102FF01002A00000007000000",
            "01070008110200030A0014041524FFFFFFFF520202000714001501160219031A041E051F090102FF01002A0000000700000001000000",
            "01070013010200020A00140405000000520202000714001501160219031A041E051F090102FF01002A00000007000000"),
        new(1, 2, 4, -1, 0, 0, 1, 0, 0, 0, 6, [],
            "010700520202000614001501160219031A041F050204FF010106000000",
            "01070008110200030A001404151EFFFFFFFF520202000614001501160219031A041F050204FF01010600000001000000",
            "01070013010200020A00140405000000520202000614001501160219031A041F050204FF010106000000"),
        new(1, 5, 9, -1, 0, 0, 2, 0, 0, 33, 9, ["The Orcs reject the peace offer of the Elves."],
            "010700520202000814001501160219031A041B051E381F3C0509FF01022F000000546865204F7263732072656A65637420746865207065616365206F66666572206F662074686520456C7665732E0D0A2100000009000000",
            "01070008110200030A0014041559FFFFFFFF520202000814001501160219031A041B051E381F3C0509FF01022F000000546865204F7263732072656A65637420746865207065616365206F66666572206F662074686520456C7665732E0D0A210000000900000001000000",
            "01070013010200020A00140405000000520202000814001501160219031A041B051E381F3C0509FF01022F000000546865204F7263732072656A65637420746865207065616365206F66666572206F662074686520456C7665732E0D0A2100000009000000"),
        new(1, 6, 3, -1, 0, 0, 3, 0, 0, 30, 14, ["Peace, friends."],
            "010700520202000814001501160219031A041B051E1A1F1E0603FF01031100000050656163652C20667269656E64732E0D0A1E0000000E000000",
            "01070008110200030A001404153BFFFFFFFF520202000814001501160219031A041B051E1A1F1E0603FF01031100000050656163652C20667269656E64732E0D0A1E0000000E00000001000000",
            "01070013010200020A00140405000000520202000814001501160219031A041B051E1A1F1E0603FF01031100000050656163652C20667269656E64732E0D0A1E0000000E000000"),
        new(1, 1, 2, -1, 100, 50, 0, 100, 50, 0, 8, [],
            "010700520202000A14001501160217031807190B1A0C1C0D1D111F150102FF64000000320000000100640000003200000008000000",
            "01070008110200030A0014041536FFFFFFFF520202000A14001501160217031807190B1A0C1C0D1D111F150102FF6400000032000000010064000000320000000800000001000000",
            "01070013010200020A00140405000000520202000A14001501160217031807190B1A0C1C0D1D111F150102FF64000000320000000100640000003200000008000000"),
        new(2, 3, 4, -1, 200, 0, 1, 0, 0, 12, 15, ["An alliance.", "Second line."],
            "0107005202020009140015011602170319071A081B091E291F2D0304FFC800000002011C000000416E20616C6C69616E63652E0D0A5365636F6E64206C696E652E0D0A0C0000000F000000",
            "01070008110200030A001404154CFFFFFFFF5202020009140015011602170319071A081B091E291F2D0304FFC800000002011C000000416E20616C6C69616E63652E0D0A5365636F6E64206C696E652E0D0A0C0000000F00000001000000",
            "01070013010200020A001404050000005202020009140015011602170319071A081B091E291F2D0304FFC800000002011C000000416E20616C6C69616E63652E0D0A5365636F6E64206C696E652E0D0A0C0000000F000000"),
        new(3, 1, 2, -1, 0, 0, 0, 0, 0, 5, 3, ["Hello", "there"],
            "010700520202000814001501160219031A041B051E171F1B0102FF03000E00000048656C6C6F0D0A74686572650D0A0500000003000000",
            "01070008110200030A0014041538FFFFFFFF520202000814001501160219031A041B051E171F1B0102FF03000E00000048656C6C6F0D0A74686572650D0A050000000300000001000000",
            "01070013010200020A00140405000000520202000814001501160219031A041B051E171F1B0102FF03000E00000048656C6C6F0D0A74686572650D0A0500000003000000"),
        new(4, 2, 5, -1, 0, 0, 0, 0, 0, 0, 8, ["Beware."],
            "010700520202000714001501160219031A041B051F120205FF0400090000004265776172652E0D0A08000000",
            "01070008110200030A001404152DFFFFFFFF520202000714001501160219031A041B051F120205FF0400090000004265776172652E0D0A0800000001000000",
            "01070013010200020A00140405000000520202000714001501160219031A041B051F120205FF0400090000004265776172652E0D0A08000000"),
        new(5, 2, 3, -1, 0, 75, 0, 0, 75, 0, 11, [],
            "0107005202020008140015011602180319071A081D091F0D0203FF4B00000005004B0000000B000000",
            "01070008110200030A001404152AFFFFFFFF5202020008140015011602180319071A081D091F0D0203FF4B00000005004B0000000B00000001000000",
            "01070013010200020A001404050000005202020008140015011602180319071A081D091F0D0203FF4B00000005004B0000000B000000"),
        new(6, 7, 8, -1, 0, 0, 0, 0, 0, 21, 20, [],
            "010700520202000714001501160219031A041E051F090708FF06001500000014000000",
            "01070008110200030A0014041524FFFFFFFF520202000714001501160219031A041E051F090708FF0600150000001400000001000000",
            "01070013010200020A00140405000000520202000714001501160219031A041E051F090708FF06001500000014000000"),
        new(7, 5, 1, 3, 0, 0, 0, 0, 0, 0, 2, ["From someone else."],
            "010700520202000714001501160219031A041B051F1D05010307001400000046726F6D20736F6D656F6E6520656C73652E0D0A02000000",
            "01070008110200030A0014041538FFFFFFFF520202000714001501160219031A041B051F1D05010307001400000046726F6D20736F6D656F6E6520656C73652E0D0A0200000001000000",
            "01070013010200020A00140405000000520202000714001501160219031A041B051F1D05010307001400000046726F6D20736F6D656F6E6520656C73652E0D0A02000000"),
        new(8, 2, 4, 6, 300, 20, 2, 0, 0, 44, 30, [],
            "010700520202000914001501160217031807190B1A0C1E0D1F110204062C0100001400000008022C0000001E000000",
            "01070008110200030A0014041530FFFFFFFF520202000914001501160217031807190B1A0C1E0D1F110204062C0100001400000008022C0000001E00000001000000",
            "01070013010200020A00140405000000520202000914001501160217031807190B1A0C1E0D1F110204062C0100001400000008022C0000001E000000"),
        new(9, 3, 1, 2, 50, 0, 1, 0, 0, 0, 12, ["War on them."],
            "0107005202020008140015011602170319071A081B091F1B0301023200000009010E000000576172206F6E207468656D2E0D0A0C000000",
            "01070008110200030A0014041538FFFFFFFF5202020008140015011602170319071A081B091F1B0301023200000009010E000000576172206F6E207468656D2E0D0A0C00000001000000",
            "01070013010200020A001404050000005202020008140015011602170319071A081B091F1B0301023200000009010E000000576172206F6E207468656D2E0D0A0C000000"),
        new(10, 1, 2, -1, 7, 8, 0, 0, 0, 0, 1, [],
            "010700520202000814001501160217031807190B1A0C1F0D0102FF07000000080000000A0001000000",
            "01070008110200030A001404152AFFFFFFFF520202000814001501160217031807190B1A0C1F0D0102FF07000000080000000A000100000001000000",
            "01070013010200020A00140405000000520202000814001501160217031807190B1A0C1F0D0102FF07000000080000000A0001000000"),
        new(200, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, ["x"],
            "01070052020200051400150119021A031B040102C80003000000780D0A",
            "01070008110200030A001404151EFFFFFFFF52020200051400150119021A031B040102C80003000000780D0A01000000",
            "01070013010200020A0014040500000052020200051400150119021A031B040102C80003000000780D0A"),
        new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, [],
            "010700520202000219001A010000",
            "01070008110200030A001404150FFFFFFFFF520202000219001A01000001000000",
            "01070013010200020A00140405000000520202000219001A010000"),
    ];

    public static readonly TheoryData<int> CaseIndexes = new(Enumerable.Range(0, Cases.Length));

    // The proposal each case reads as, built by hand.
    private static DiplomaticProposal Expected(byte Kind, int Gold, int Mana) => Kind switch
    {
        0 => new DeclareWar(),
        1 => new OfferPeace(Gold, Mana),
        2 => new ProposeAlliance(Gold, Mana),
        3 => new SendMessage(),
        4 => new Player.ThreatenToAttack(),
        5 => new Player.MakeTribute(Gold, Mana),
        6 => new BreakAlliance(),
        7 => new Player.ImpersonatedMessage(),
        8 => new Player.SuggestAllianceBreak(Gold, Mana),
        9 => new Player.SuggestWarDeclaration(Gold, Mana),
        _ => new UnknownProposal(Kind, Gold, Mana),
    };

    private static string Hex(MemoryStream memory) => Convert.ToHexString(memory.ToArray());

    private static MemoryStream Memory(string hex)
    {
        var bytes = Convert.FromHexString(hex);
        var memory = new MemoryStream();
        memory.WriteBuffer(bytes);
        memory.Position = 0;
        return memory;
    }

    private static GameEngine CreateEngine()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        GameEngine engine = new GameEngine(null!);
        engine.Options -= Set<EngineModeElement>.Of(GameEngine.eoVerifyClasses);
        engine.RegisterEClasses([EngineClass.Of<DiplomaticAction>(), EngineClass.Of<DiplomaticActionTokenEvent>(), EngineClass.Of<DiplomaticActionEventLog>()]);
        return engine;
    }

    private static string Write(GameEngine engine, EngineObject value)
    {
        using var memory = new MemoryStream();
        var writer = new EWriteStorageStream(memory, engine, false);
        writer.rwCreateEObject(ref value, null!, 7);
        writer.Dispose();
        return Hex(memory);
    }

    private static T Read<T>(GameEngine engine, string hex) where T : EngineObject
    {
        using var memory = Memory(hex);
        EngineObject result = null!;
        using (var reader = new EReadStorageStream(memory, engine, false)) { reader.rwCreateEObject(ref result, null!, 7); }
        return Assert.IsType<T>(result);
    }

    private static DiplomaticAction Action(Case c) => new(null!)
    {
        FSource = c.Source,
        FTarget = c.Target,
        FOtherPlayer = c.Other,
        FReaction = (DiplomaticActionReaction)c.Reaction,
        FEscrowGold = c.EscrowGold,
        FEscrowMana = c.EscrowMana,
        FEventLogID = c.EventLogId,
        FDay = c.Day,
        Proposal = Expected(c.Kind, c.Gold, c.Mana) with { Message = c.Message },
    };

    private static void AssertCase(Case c, DiplomaticAction action)
    {
        Assert.Equal(Expected(c.Kind, c.Gold, c.Mana) with { Message = c.Message }, action.Proposal);
        Assert.Equal((c.Source, c.Target, c.Other, c.Reaction), (action.FSource, action.FTarget, action.FOtherPlayer, (byte)action.FReaction));
        Assert.Equal((c.EscrowGold, c.EscrowMana, c.EventLogId, c.Day), (action.FEscrowGold, action.FEscrowMana, action.FEventLogID, action.FDay));
    }

    [Theory]
    [MemberData(nameof(CaseIndexes))]
    public void ProposalsWriteTheOldBytes(int Index)
    {
        var c = Cases[Index];
        GameEngine engine = CreateEngine();
        try
        {
            var proposal = DiplomaticProposal.FromFields(c.Kind, c.Gold, c.Mana);
            Assert.Equal(Expected(c.Kind, c.Gold, c.Mana), proposal);
            Assert.Equal((c.Kind, c.Gold, c.Mana), proposal.ToFields());

            var action = Action(c);
            Assert.Equal(c.Alone, Write(engine, action));
            action.Release();

            var te = new DiplomaticActionTokenEvent(null!) { FOperation = 1 };
            action = Action(c);
            te.SetDiplomaticAction(action);
            action.Release();
            Assert.Equal(c.Token, Write(engine, te));
            te.Dispose();

            var log = new DiplomaticActionEventLog(null!) { FDay = 5, FAction = Action(c) };
            Assert.Equal(c.Logged, Write(engine, log));
            log.Dispose();

            var loaded = Read<DiplomaticAction>(engine, c.Alone);
            AssertCase(c, loaded);
            Assert.Equal(c.Alone, Write(engine, loaded));
            loaded.Release();

            var loadedEvent = Read<DiplomaticActionTokenEvent>(engine, c.Token);
            AssertCase(c, loadedEvent.FAction);
            Assert.Equal(c.Token, Write(engine, loadedEvent));
            loadedEvent.Dispose();

            var loadedLog = Read<DiplomaticActionEventLog>(engine, c.Logged);
            AssertCase(c, loadedLog.FAction);
            Assert.Equal(c.Logged, Write(engine, loadedLog));
            loadedLog.Dispose();
        }
        finally
        {
            engine.Dispose();
        }
    }

    [Fact]
    public void EventLogCopiesKeepTheirMessage()
    {
        GameEngine engine = CreateEngine();
        try
        {
            string[] message = ["The Dark Elves declare war on the Humans.", "", "Second line."];
            using var log = new DiplomaticActionEventLog(null!) { FDay = 5 };
            log.FAction = new DiplomaticAction(null!) { FSource = 6, FTarget = 11, FOtherPlayer = -1, FEventLogID = 16, FDay = 7, Proposal = new DeclareWar() with { Message = message } };
            string bytes = Write(engine, log);
            var copy = Read<DiplomaticActionEventLog>(engine, bytes);
            Assert.Equal(message, copy.FAction.Proposal.Message);
            Assert.Equal("The Dark Elves declare war on the Humans.\r\n\r\nSecond line.\r\n", copy.FAction.Proposal.MessageText);
            Assert.Equal(bytes, Write(engine, copy));

            // The copy for an AI player's action list drops the message (Diplomacy.pas:938), the logged copy keeps it.
            var listed = Read<DiplomaticActionEventLog>(engine, bytes);
            listed.FAction.Proposal = listed.FAction.Proposal with { Message = [] };
            Assert.Empty(listed.FAction.Proposal.Message);
            Assert.Equal(message, copy.FAction.Proposal.Message);
            Assert.NotEqual(copy.FAction.Proposal, listed.FAction.Proposal);
            Assert.Equal(new DeclareWar(), listed.FAction.Proposal);
            copy.Dispose();
            listed.Dispose();
        }
        finally
        {
            engine.Dispose();
        }
    }

    [Fact]
    public void UnknownKindsWriteBackUnchanged()
    {
        foreach (var (kind, gold, mana) in new (byte, int, int)[] { (10, 7, 8), (200, 0, 0), (255, -1, 3) })
        {
            var proposal = DiplomaticProposal.FromFields(kind, gold, mana);
            Assert.Equal(new UnknownProposal(kind, gold, mana), proposal);
            Assert.Equal((kind, gold, mana), proposal.ToFields());
            Assert.Same(proposal, proposal.WithOffer(5, 5));
        }
        Assert.Equal(new DeclareWar(), new DiplomaticAction(null!).Proposal);
    }

    // The kinds that carry no offer keep none. The census finds gold and mana on no kind.
    [Fact]
    public void KindsWithoutOfferDropGoldAndMana()
    {
        foreach (byte kind in new byte[] { 0, 3, 4, 6, 7 })
        {
            Assert.Equal((kind, 0, 0), DiplomaticProposal.FromFields(kind, 5, 6).ToFields());
            Assert.Equal((kind, 0, 0), DiplomaticProposal.FromFields(kind, 0, 0).WithOffer(5, 6).ToFields());
        }
        foreach (byte kind in new byte[] { 1, 2, 5, 8, 9 })
        {
            Assert.Equal((kind, 5, 6), DiplomaticProposal.FromFields(kind, 5, 6).ToFields());
            Assert.Equal((kind, 7, 8), DiplomaticProposal.FromFields(kind, 0, 0).WithOffer(7, 8).ToFields());
        }
    }

    [Fact]
    public void OriginalKindsRegisterOnce()
    {
        for (int kind = 0; kind <= 9; kind++)
        {
            Assert.True(DiplomaticProposal.IsRegistered((byte)kind), "kind " + kind);
            Assert.Throws<InvalidOperationException>(() => DiplomaticProposal.Register((byte)kind, static (gold, mana) => new UnknownProposal(0, gold, mana)));
        }
        foreach (byte kind in new byte[] { 10, 11, 200, 255 }) Assert.False(DiplomaticProposal.IsRegistered(kind), "kind " + kind);
    }

    // The editor of the relations window: the message enters on each edit of the memo, the offer
    // when the player sends (AoWRelations.pas:794, 800, 877, 882).
    [Fact]
    public void TheEditorBuildsTheProposal()
    {
        var action = new DiplomaticAction(null!) { FSource = 1, FTarget = 2, FOtherPlayer = -1, Proposal = new OfferPeace() with { Message = ["Default text."] } };
        action.Proposal = action.Proposal with { Message = DiplomaticProposal.Lines("Peace?\r\nPlease.") };
        action.Proposal = action.Proposal.WithOffer(120, 30);
        Assert.Equal(new OfferPeace(120, 30) with { Message = ["Peace?", "Please."] }, action.Proposal);
        var message = new DiplomaticAction(null!) { Proposal = new SendMessage() };
        message.Proposal = message.Proposal.WithOffer(120, 30);
        Assert.Equal(new SendMessage(), message.Proposal);
        action.Release();
        message.Release();
    }

    // AoWRelations.pas:811 ASelectClick and E3EventDialogs.pas:194 ShowDiplomaticEvent: the arm each kind took.
    private static string OldView(byte Kind) => Kind switch
    {
        0 or 6 => "Notice",
        4 => "Threat",
        1 or 2 or 5 or 8 or 9 => "Offer",
        3 or 7 => "Message",
        _ => "None",
    };

    // The icon switches of both screens.
    private static int OldIcon(byte Kind) => Kind switch
    {
        0 => 9,
        1 => 8,
        2 => 10,
        4 => 46,
        5 => 48,
        6 => 8,
        8 or 9 => 47,
        _ => -1,
    };

    // Diplomacy.pas:1208 PlayLoggedSFX.
    private static string OldSample(byte Kind) => Kind switch
    {
        0 or 4 or 6 => "SFX\\EVENTS\\MESSAGE.WAV",
        1 or 2 or 5 => "SFX\\EVENTS\\MESSAGE2.WAV",
        _ => "SFX\\EVENTS\\MESSAGE.WAV",
    };

    // Diplomacy.pas:1353 GetMiddleIcon.
    private static int OldMiddleIcon(byte Kind, byte Reaction) => Kind == 0 || Kind == 9 && Reaction == 1 ? 0x15 : 0x16;

    // Diplomacy.pas:1265 GetText.
    private static string OldText(byte Kind, DiplomaticActionReaction Reaction, bool SourceSeated)
    {
        static string T(string name) => AbilityRules.TranslateRStr(name);
        static string F(string format, string arg) => AbilityRules.Format(T(format), [arg]);
        string Answer(string none, string subject, string accepted, string otherwise) => Reaction switch
        {
            DiplomaticActionReaction.None => none,
            DiplomaticActionReaction.Reject => F(DiplomaticProposal.P5XRejectedRStr, subject),
            DiplomaticActionReaction.Accept => accepted,
            DiplomaticActionReaction.Ignore => F(DiplomaticProposal.P5XIgnoredRStr, subject),
            _ => otherwise,
        };
        return Kind switch
        {
            0 => T(GameTables.P5WarRStr),
            2 => Answer(T(ProposeAlliance.P5AllianceProposalRStr), T(ProposeAlliance.P5AllianceProposalRStr),
                SourceSeated ? F(DiplomaticProposal.P5XAcceptedRStr, T(ProposeAlliance.P5AllianceProposalRStr)) : T(ProposeAlliance.P5MadeAllianceRStr), T(ProposeAlliance.P5AllianceProposalRStr)),
            1 => Answer(T(OfferPeace.P5PeaceOfferRStr), T(OfferPeace.P5PeaceOfferRStr),
                SourceSeated ? F(DiplomaticProposal.P5XAcceptedRStr, T(OfferPeace.P5PeaceOfferRStr)) : T(GameTables.P5PeaceRStr), ""),
            5 => Answer(T(Player.MakeTribute.P5TributeRStr), T(Player.MakeTribute.P5TributeRStr), F(DiplomaticProposal.P5XAcceptedRStr, T(Player.MakeTribute.P5TributeRStr)), ""),
            9 => Answer(T(Player.SuggestWarDeclaration.P5DeclareWarSuggestionRStr), T(GameText.P5DeclareWarRStr), F(DiplomaticProposal.P5XAcceptedRStr, T(GameText.P5DeclareWarRStr)), ""),
            8 => Answer(T(Player.SuggestAllianceBreak.P5BreakAllianceSuggestionRStr), T(GameText.P5BreakAllianceRStr), F(DiplomaticProposal.P5XAcceptedRStr, T(GameText.P5BreakAllianceRStr)), ""),
            6 => T(BreakAlliance.P5AllianceBrokenRStr),
            3 or 7 => T(GameText.P5MessageRStr),
            4 => T(Player.ThreatenToAttack.P5ThreatRStr),
            _ => "DiplomaticActionEventLog",
        };
    }

    // Diplomacy.pas:505 GetName, for the kinds that name no other player.
    private static string OldName(byte Kind) => Kind switch
    {
        0 => AbilityRules.TranslateRStr(GameText.P5DeclareWarRStr),
        1 => AbilityRules.TranslateRStr(OfferPeace.P5OfferPeaceRStr),
        2 => AbilityRules.TranslateRStr(ProposeAlliance.P5ProposeAllianceRStr),
        3 => AbilityRules.TranslateRStr(SendMessage.P5SendMessageRStr),
        4 => AbilityRules.TranslateRStr(Player.ThreatenToAttack.P5ThreatenToAttackRStr),
        5 => AbilityRules.TranslateRStr(Player.MakeTribute.P5MakeTributeRStr),
        6 => AbilityRules.TranslateRStr(GameText.P5BreakAllianceRStr),
        _ => "",
    };

    // Diplomacy.pas:1420 RetrieveDefaultDiplomaticActionMessage, the switch after the races and the alignment.
    private static void OldDefaultMessage(TextControl TextControl, int Alignment, string SourceRace, string TargetRace, string OtherRace,
        byte Action, DiplomaticActionReaction Reaction, DiplomaticRelation Relation, StringCollection Text)
    {
        Text.Clear();
        void Get(int number, string first, string second, string? third = null)
        {
            TextControl.RetrieveText(Alignment + number, Text);
            Text.Text = third is null ? AbilityRules.Format(Text.Text, [first, second]) : AbilityRules.Format(Text.Text, [first, second, third]);
        }
        switch (Action)
        {
            case 0: Get(Relation == DiplomaticRelation.Unknown ? 151 : 100, SourceRace, TargetRace); break;
            case 1:
                if (Reaction == DiplomaticActionReaction.None) Get(Relation == DiplomaticRelation.Unknown ? 103 : 154, SourceRace, TargetRace);
                else if (Reaction == DiplomaticActionReaction.Accept) Get(127, TargetRace, SourceRace);
                else if (Reaction == DiplomaticActionReaction.Reject) Get(130, TargetRace, SourceRace);
                break;
            case 2:
                if (Reaction == DiplomaticActionReaction.None) Get(106, SourceRace, TargetRace);
                else if (Reaction == DiplomaticActionReaction.Accept) Get(121, TargetRace, SourceRace);
                else if (Reaction == DiplomaticActionReaction.Reject) Get(124, TargetRace, SourceRace);
                break;
            case 6: Get(109, SourceRace, TargetRace); break;
            case 4: Get(112, SourceRace, TargetRace); break;
            case 5:
                if (Reaction == DiplomaticActionReaction.None) Get(118, SourceRace, TargetRace);
                else if (Reaction == DiplomaticActionReaction.Accept) Get(133, TargetRace, SourceRace);
                else if (Reaction == DiplomaticActionReaction.Reject) Get(136, TargetRace, SourceRace);
                break;
            case 8:
                if (Reaction == DiplomaticActionReaction.None) Get(160, SourceRace, TargetRace, OtherRace);
                else if (Reaction == DiplomaticActionReaction.Accept) Get(139, TargetRace, SourceRace, OtherRace);
                else if (Reaction == DiplomaticActionReaction.Reject) Get(142, TargetRace, SourceRace, OtherRace);
                break;
            case 9:
                if (Reaction == DiplomaticActionReaction.None) Get(157, SourceRace, TargetRace, OtherRace);
                else if (Reaction == DiplomaticActionReaction.Accept) Get(145, TargetRace, SourceRace, OtherRace);
                else if (Reaction == DiplomaticActionReaction.Reject) Get(148, TargetRace, SourceRace, OtherRace);
                break;
        }
    }

    // Texts 100 to 164, two lines each. The texts the switch formats with the other player's race
    // take three arguments, so a call with two of them throws.
    private static TextControl TextControl()
    {
        int[] withOther = [157, 160, 139, 142, 145, 148];
        var control = new TextControl(null!);
        for (int id = 100; id <= 164; id++)
        {
            var item = control.New();
            item.FID = id;
            bool three = withOther.Any(first => id >= first && id <= first + 2);
            item.FText.Text = "Text " + id + ": %s and %s" + (three ? " about %s" : "") + "\r\nSecond line " + id;
        }
        return control;
    }

    private static readonly byte[] Kinds = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    [Fact]
    public void ProposalsAnswerAsTheOldSwitches()
    {
        var previousMap = AoWHSMap.Current;
        AoWHSMap.Current = new AoWHSMap(null!);
        var control = TextControl();
        try
        {
            int checks = 0;
            foreach (byte kind in Kinds)
            {
                var proposal = DiplomaticProposal.FromFields(kind, 0, 0);
                Assert.Equal(OldView(kind), proposal.View.Name);
                Assert.Equal(OldIcon(kind), proposal.Icon);
                Assert.Equal(kind == 7, proposal.SignedAsOtherPlayer);
                Assert.Equal(OldSample(kind), proposal.LoggedSample);
                if (kind <= 6 || kind == 10) Assert.Equal(OldName(kind), proposal.Name(new DiplomaticAction(null!) { Proposal = proposal }));
                checks += 5;
                string error = "";
                Assert.Equal(kind != 5, proposal.CanSend(new DiplomaticAction(null!) { Proposal = proposal }, ref error));
                Assert.Equal(kind == 5 ? AbilityRules.TranslateRStr(Player.MakeTribute.P5NoGoldManaSpecifiedRStr) : "", error);
                Assert.True(proposal.WithOffer(0, 1).CanSend(new DiplomaticAction(null!), ref error));
                checks += 2;
                foreach (byte reaction in new byte[] { 0, 1, 2, 3, 4 })
                {
                    foreach (sbyte source in new sbyte[] { 0, 1 })
                    {
                        var action = new DiplomaticAction(null!) { FSource = source, FTarget = 2, FReaction = (DiplomaticActionReaction)reaction, Proposal = proposal };
                        string where = proposal + " reaction " + reaction + " source " + source;
                        Assert.True(OldText(kind, (DiplomaticActionReaction)reaction, source == AoWHSMap.Current.FSeatedPlayer) == proposal.LogText(action), where);
                        Assert.True(OldMiddleIcon(kind, reaction) == proposal.MiddleIcon(action), where);
                        checks += 2;
                    }
                    foreach (var relation in new[] { DiplomaticRelation.Peace, DiplomaticRelation.War, DiplomaticRelation.Alliance, DiplomaticRelation.Unknown })
                    {
                        foreach (int alignment in new[] { 0, 1, 2 })
                        {
                            var expected = new StringList();
                            var actual = new StringList();
                            OldDefaultMessage(control, alignment, "Elves", "Orcs", "Dwarves", kind, (DiplomaticActionReaction)reaction, relation, expected);
                            proposal.DefaultMessage(new DiplomaticMessageText(control, alignment, "Elves", "Orcs", "Dwarves", actual), (DiplomaticActionReaction)reaction, relation);
                            Assert.True(expected.Text == actual.Text, proposal + " reaction " + reaction + " relation " + relation + " alignment " + alignment);
                            checks++;
                        }
                    }
                }
            }
            Assert.Equal(Kinds.Length * (7 + 5 * (4 + 12)), checks);
        }
        finally
        {
            control?.Dispose();
            AoWHSMap.Current = previousMap;
        }
    }
}
