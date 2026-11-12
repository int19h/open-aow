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

    private static readonly Case[] s_cases =
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

    public static readonly TheoryData<int> CaseIndexes = new(Enumerable.Range(0, s_cases.Length));

    // The proposal each case reads as, built by hand.
    private static DiplomaticProposal Expected(byte kind, int gold, int mana) => kind switch
    {
        0 => new DeclareWar(),
        1 => new OfferPeace(gold, mana),
        2 => new ProposeAlliance(gold, mana),
        3 => new SendMessage(),
        4 => new Player.ThreatenToAttack(),
        5 => new Player.MakeTribute(gold, mana),
        6 => new BreakAlliance(),
        7 => new Player.ImpersonatedMessage(),
        8 => new Player.SuggestAllianceBreak(gold, mana),
        9 => new Player.SuggestWarDeclaration(gold, mana),
        _ => new UnknownProposal(kind, gold, mana),
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
        engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
        engine.RegisterEngineClasses([EngineClass.Of<DiplomaticAction>(), EngineClass.Of<DiplomaticActionTokenEvent>(), EngineClass.Of<DiplomaticActionEventLog>()]);
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

    private static Translate Read<Translate>(GameEngine engine, string hex) where Translate : EngineObject
    {
        using var memory = Memory(hex);
        EngineObject result = null!;
        using (var reader = new EReadStorageStream(memory, engine, false)) { reader.rwCreateEObject(ref result, null!, 7); }
        return Assert.IsType<Translate>(result);
    }

    private static DiplomaticAction Action(Case c) => new(null!)
    {
        Source = c.Source,
        Target = c.Target,
        OtherPlayer = c.Other,
        Reaction = (DiplomaticActionReaction)c.Reaction,
        EscrowGold = c.EscrowGold,
        EscrowMana = c.EscrowMana,
        EventLogId = c.EventLogId,
        Day = c.Day,
        Proposal = Expected(c.Kind, c.Gold, c.Mana) with { Message = c.Message },
    };

    private static void AssertCase(Case c, DiplomaticAction action)
    {
        Assert.Equal(Expected(c.Kind, c.Gold, c.Mana) with { Message = c.Message }, action.Proposal);
        Assert.Equal((c.Source, c.Target, c.Other, c.Reaction), (action.Source, action.Target, action.OtherPlayer, (byte)action.Reaction));
        Assert.Equal((c.EscrowGold, c.EscrowMana, c.EventLogId, c.Day), (action.EscrowGold, action.EscrowMana, action.EventLogId, action.Day));
    }

    [Theory]
    [MemberData(nameof(CaseIndexes))]
    public void ProposalsWriteTheOldBytes(int index)
    {
        var c = s_cases[index];
        GameEngine engine = CreateEngine();
        try
        {
            var proposal = DiplomaticProposal.FromFields(c.Kind, c.Gold, c.Mana);
            Assert.Equal(Expected(c.Kind, c.Gold, c.Mana), proposal);
            Assert.Equal((c.Kind, c.Gold, c.Mana), proposal.ToFields());

            var action = Action(c);
            Assert.Equal(c.Alone, Write(engine, action));
            action.Release();

            var tokenEvent = new DiplomaticActionTokenEvent(null!) { Operation = 1 };
            action = Action(c);
            tokenEvent.SetDiplomaticAction(action);
            action.Release();
            Assert.Equal(c.Token, Write(engine, tokenEvent));
            tokenEvent.Dispose();

            var log = new DiplomaticActionEventLog(null!) { _day = 5, Action = Action(c) };
            Assert.Equal(c.Logged, Write(engine, log));
            log.Dispose();

            var loaded = Read<DiplomaticAction>(engine, c.Alone);
            AssertCase(c, loaded);
            Assert.Equal(c.Alone, Write(engine, loaded));
            loaded.Release();

            var loadedEvent = Read<DiplomaticActionTokenEvent>(engine, c.Token);
            AssertCase(c, loadedEvent.Action);
            Assert.Equal(c.Token, Write(engine, loadedEvent));
            loadedEvent.Dispose();

            var loadedLog = Read<DiplomaticActionEventLog>(engine, c.Logged);
            AssertCase(c, loadedLog.Action);
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
            using var log = new DiplomaticActionEventLog(null!) { _day = 5 };
            log.Action = new DiplomaticAction(null!) { Source = 6, Target = 11, OtherPlayer = -1, EventLogId = 16, Day = 7, Proposal = new DeclareWar() with { Message = message } };
            string bytes = Write(engine, log);
            var copy = Read<DiplomaticActionEventLog>(engine, bytes);
            Assert.Equal(message, copy.Action.Proposal.Message);
            Assert.Equal("The Dark Elves declare war on the Humans.\r\n\r\nSecond line.\r\n", copy.Action.Proposal.MessageText);
            Assert.Equal(bytes, Write(engine, copy));

            // The copy for an AI player's action list drops the message (Diplomacy.pas:938), the logged copy keeps it.
            var listed = Read<DiplomaticActionEventLog>(engine, bytes);
            listed.Action.Proposal = listed.Action.Proposal with { Message = [] };
            Assert.Empty(listed.Action.Proposal.Message);
            Assert.Equal(message, copy.Action.Proposal.Message);
            Assert.NotEqual(copy.Action.Proposal, listed.Action.Proposal);
            Assert.Equal(new DeclareWar(), listed.Action.Proposal);
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
        var action = new DiplomaticAction(null!) { Source = 1, Target = 2, OtherPlayer = -1, Proposal = new OfferPeace() with { Message = ["Default text."] } };
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
    private static string OldView(byte kind) => kind switch
    {
        0 or 6 => "Notice",
        4 => "Threat",
        1 or 2 or 5 or 8 or 9 => "Offer",
        3 or 7 => "Message",
        _ => "None",
    };

    // The icon switches of both screens.
    private static int OldIcon(byte kind) => kind switch
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
    private static string OldSample(byte kind) => kind switch
    {
        0 or 4 or 6 => "SFX\\EVENTS\\MESSAGE.WAV",
        1 or 2 or 5 => "SFX\\EVENTS\\MESSAGE2.WAV",
        _ => "SFX\\EVENTS\\MESSAGE.WAV",
    };

    // Diplomacy.pas:1353 GetMiddleIcon.
    private static int OldMiddleIcon(byte kind, byte reaction) => kind == 0 || kind == 9 && reaction == 1 ? 0x15 : 0x16;

    // Diplomacy.pas:1265 GetText.
    private static string OldText(byte kind, DiplomaticActionReaction reaction, bool sourceSeated)
    {
        static string Translate(string name) => AbilityRules.TranslateResourceString(name);
        static string FormatText(string format, string argument) => AbilityRules.Format(Translate(format), [argument]);
        string Answer(string none, string subject, string accepted, string otherwise) => reaction switch
        {
            DiplomaticActionReaction.None => none,
            DiplomaticActionReaction.Reject => FormatText(DiplomaticProposal.s_rejectedFormat, subject),
            DiplomaticActionReaction.Accept => accepted,
            DiplomaticActionReaction.Ignore => FormatText(DiplomaticProposal.s_ignoredFormat, subject),
            _ => otherwise,
        };
        return kind switch
        {
            0 => Translate(GameTables.s_warText),
            2 => Answer(Translate(ProposeAlliance.s_allianceProposalText), Translate(ProposeAlliance.s_allianceProposalText),
                sourceSeated ? FormatText(DiplomaticProposal.s_acceptedFormat, Translate(ProposeAlliance.s_allianceProposalText)) : Translate(ProposeAlliance.s_madeAllianceText), Translate(ProposeAlliance.s_allianceProposalText)),
            1 => Answer(Translate(OfferPeace.s_peaceOfferText), Translate(OfferPeace.s_peaceOfferText),
                sourceSeated ? FormatText(DiplomaticProposal.s_acceptedFormat, Translate(OfferPeace.s_peaceOfferText)) : Translate(GameTables.s_peaceText), ""),
            5 => Answer(Translate(Player.MakeTribute.s_tributeText), Translate(Player.MakeTribute.s_tributeText), FormatText(DiplomaticProposal.s_acceptedFormat, Translate(Player.MakeTribute.s_tributeText)), ""),
            9 => Answer(Translate(Player.SuggestWarDeclaration.s_declareWarSuggestionText), Translate(GameText.s_declareWarText), FormatText(DiplomaticProposal.s_acceptedFormat, Translate(GameText.s_declareWarText)), ""),
            8 => Answer(Translate(Player.SuggestAllianceBreak.s_breakAllianceSuggestionText), Translate(GameText.s_breakAllianceText), FormatText(DiplomaticProposal.s_acceptedFormat, Translate(GameText.s_breakAllianceText)), ""),
            6 => Translate(BreakAlliance.s_allianceBrokenText),
            3 or 7 => Translate(GameText.s_diplomaticMessageText),
            4 => Translate(Player.ThreatenToAttack.s_threatText),
            _ => "DiplomaticActionEventLog",
        };
    }

    // Diplomacy.pas:505 GetName, for the kinds that name no other player.
    private static string OldName(byte kind) => kind switch
    {
        0 => AbilityRules.TranslateResourceString(GameText.s_declareWarText),
        1 => AbilityRules.TranslateResourceString(OfferPeace.s_offerPeaceText),
        2 => AbilityRules.TranslateResourceString(ProposeAlliance.s_proposeAllianceText),
        3 => AbilityRules.TranslateResourceString(SendMessage.s_sendMessageText),
        4 => AbilityRules.TranslateResourceString(Player.ThreatenToAttack.s_threatenToAttackText),
        5 => AbilityRules.TranslateResourceString(Player.MakeTribute.s_makeTributeText),
        6 => AbilityRules.TranslateResourceString(GameText.s_breakAllianceText),
        _ => "",
    };

    // Diplomacy.pas:1420 RetrieveDefaultDiplomaticActionMessage, the switch after the races and the alignment.
    private static void OldDefaultMessage(TextControl textControl, int alignment, string sourceRace, string targetRace, string otherRace,
        byte action, DiplomaticActionReaction reaction, DiplomaticRelation relation, StringCollection text)
    {
        text.Clear();
        void Get(int number, string first, string second, string? third = null)
        {
            textControl.RetrieveText(alignment + number, text);
            text.Text = third is null ? AbilityRules.Format(text.Text, [first, second]) : AbilityRules.Format(text.Text, [first, second, third]);
        }
        switch (action)
        {
            case 0: Get(relation == DiplomaticRelation.Unknown ? 151 : 100, sourceRace, targetRace); break;
            case 1:
                if (reaction == DiplomaticActionReaction.None) Get(relation == DiplomaticRelation.Unknown ? 103 : 154, sourceRace, targetRace);
                else if (reaction == DiplomaticActionReaction.Accept) Get(127, targetRace, sourceRace);
                else if (reaction == DiplomaticActionReaction.Reject) Get(130, targetRace, sourceRace);
                break;
            case 2:
                if (reaction == DiplomaticActionReaction.None) Get(106, sourceRace, targetRace);
                else if (reaction == DiplomaticActionReaction.Accept) Get(121, targetRace, sourceRace);
                else if (reaction == DiplomaticActionReaction.Reject) Get(124, targetRace, sourceRace);
                break;
            case 6: Get(109, sourceRace, targetRace); break;
            case 4: Get(112, sourceRace, targetRace); break;
            case 5:
                if (reaction == DiplomaticActionReaction.None) Get(118, sourceRace, targetRace);
                else if (reaction == DiplomaticActionReaction.Accept) Get(133, targetRace, sourceRace);
                else if (reaction == DiplomaticActionReaction.Reject) Get(136, targetRace, sourceRace);
                break;
            case 8:
                if (reaction == DiplomaticActionReaction.None) Get(160, sourceRace, targetRace, otherRace);
                else if (reaction == DiplomaticActionReaction.Accept) Get(139, targetRace, sourceRace, otherRace);
                else if (reaction == DiplomaticActionReaction.Reject) Get(142, targetRace, sourceRace, otherRace);
                break;
            case 9:
                if (reaction == DiplomaticActionReaction.None) Get(157, sourceRace, targetRace, otherRace);
                else if (reaction == DiplomaticActionReaction.Accept) Get(145, targetRace, sourceRace, otherRace);
                else if (reaction == DiplomaticActionReaction.Reject) Get(148, targetRace, sourceRace, otherRace);
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
            item.Id = id;
            bool three = withOther.Any(first => id >= first && id <= first + 2);
            item.Text.Text = "Text " + id + ": %s and %s" + (three ? " about %s" : "") + "\r\nSecond line " + id;
        }
        return control;
    }

    private static readonly byte[] s_kinds = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    [Fact]
    public void ProposalsAnswerAsTheOldSwitches()
    {
        var previousMap = AoWHSMap.Current;
        AoWHSMap.Current = new AoWHSMap(null!);
        var control = TextControl();
        try
        {
            int checks = 0;
            foreach (byte kind in s_kinds)
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
                Assert.Equal(kind == 5 ? AbilityRules.TranslateResourceString(Player.MakeTribute.s_noGoldManaSpecifiedText) : "", error);
                Assert.True(proposal.WithOffer(0, 1).CanSend(new DiplomaticAction(null!), ref error));
                checks += 2;
                foreach (byte reaction in new byte[] { 0, 1, 2, 3, 4 })
                {
                    foreach (sbyte source in new sbyte[] { 0, 1 })
                    {
                        var action = new DiplomaticAction(null!) { Source = source, Target = 2, Reaction = (DiplomaticActionReaction)reaction, Proposal = proposal };
                        string where = proposal + " reaction " + reaction + " source " + source;
                        Assert.True(OldText(kind, (DiplomaticActionReaction)reaction, source == AoWHSMap.Current.SeatedPlayer) == proposal.LogText(action), where);
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
            Assert.Equal(s_kinds.Length * (7 + 5 * (4 + 12)), checks);
        }
        finally
        {
            control?.Dispose();
            AoWHSMap.Current = previousMap;
        }
    }
}
