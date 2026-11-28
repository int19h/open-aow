#nullable disable warnings
using Aow.Engine;
using Aow.Engine.Nodes;
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
        new(0, Source: 2, Target: 1, Other: -1, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 19, Day: 4, Message: [],
            Alone: "010000520202000714001501160219031A041E051F090201FF00001300000004000000",
            Token: "01000008110200030A0014041524FFFFFFFF520202000714001501160219031A041E051F090201FF0000130000000400000001000000",
            Logged: "01000013010200020A00140405000000520202000714001501160219031A041E051F090201FF00001300000004000000"),
        new(0, Source: 4, Target: 8, Other: -1, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 6, Message: [],
            Alone: "010000520202000614001501160219031A041F050408FF000006000000",
            Token: "01000008110200030A001404151EFFFFFFFF520202000614001501160219031A041F050408FF00000600000001000000",
            Logged: "01000013010200020A00140405000000520202000614001501160219031A041F050408FF000006000000"),
        new(0, Source: 6, Target: 11, Other: -1, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 16, Day: 7, Message: ["The Dark Elves declare war on the Humans."],
            Alone: "010000520202000814001501160219031A041B051E341F38060BFF00002B000000546865204461726B20456C766573206465636C61726520776172206F6E207468652048756D616E732E0D0A1000000007000000",
            Token: "01000008110200030A0014041555FFFFFFFF520202000814001501160219031A041B051E341F38060BFF00002B000000546865204461726B20456C766573206465636C61726520776172206F6E207468652048756D616E732E0D0A100000000700000001000000",
            Logged: "01000013010200020A00140405000000520202000814001501160219031A041B051E341F38060BFF00002B000000546865204461726B20456C766573206465636C61726520776172206F6E207468652048756D616E732E0D0A1000000007000000"),
        new(1, Source: 1, Target: 2, Other: -1, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 42, Day: 7, Message: [],
            Alone: "010000520202000714001501160219031A041E051F090102FF01002A00000007000000",
            Token: "01000008110200030A0014041524FFFFFFFF520202000714001501160219031A041E051F090102FF01002A0000000700000001000000",
            Logged: "01000013010200020A00140405000000520202000714001501160219031A041E051F090102FF01002A00000007000000"),
        new(1, Source: 2, Target: 4, Other: -1, Gold: 0, Mana: 0, Reaction: 1, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 6, Message: [],
            Alone: "010000520202000614001501160219031A041F050204FF010106000000",
            Token: "01000008110200030A001404151EFFFFFFFF520202000614001501160219031A041F050204FF01010600000001000000",
            Logged: "01000013010200020A00140405000000520202000614001501160219031A041F050204FF010106000000"),
        new(1, Source: 5, Target: 9, Other: -1, Gold: 0, Mana: 0, Reaction: 2, EscrowGold: 0, EscrowMana: 0, EventLogId: 33, Day: 9, Message: ["The Orcs reject the peace offer of the Elves."],
            Alone: "010000520202000814001501160219031A041B051E381F3C0509FF01022F000000546865204F7263732072656A65637420746865207065616365206F66666572206F662074686520456C7665732E0D0A2100000009000000",
            Token: "01000008110200030A0014041559FFFFFFFF520202000814001501160219031A041B051E381F3C0509FF01022F000000546865204F7263732072656A65637420746865207065616365206F66666572206F662074686520456C7665732E0D0A210000000900000001000000",
            Logged: "01000013010200020A00140405000000520202000814001501160219031A041B051E381F3C0509FF01022F000000546865204F7263732072656A65637420746865207065616365206F66666572206F662074686520456C7665732E0D0A2100000009000000"),
        new(1, Source: 6, Target: 3, Other: -1, Gold: 0, Mana: 0, Reaction: 3, EscrowGold: 0, EscrowMana: 0, EventLogId: 30, Day: 14, Message: ["Peace, friends."],
            Alone: "010000520202000814001501160219031A041B051E1A1F1E0603FF01031100000050656163652C20667269656E64732E0D0A1E0000000E000000",
            Token: "01000008110200030A001404153BFFFFFFFF520202000814001501160219031A041B051E1A1F1E0603FF01031100000050656163652C20667269656E64732E0D0A1E0000000E00000001000000",
            Logged: "01000013010200020A00140405000000520202000814001501160219031A041B051E1A1F1E0603FF01031100000050656163652C20667269656E64732E0D0A1E0000000E000000"),
        new(1, Source: 1, Target: 2, Other: -1, Gold: 100, Mana: 50, Reaction: 0, EscrowGold: 100, EscrowMana: 50, EventLogId: 0, Day: 8, Message: [],
            Alone: "010000520202000A14001501160217031807190B1A0C1C0D1D111F150102FF64000000320000000100640000003200000008000000",
            Token: "01000008110200030A0014041536FFFFFFFF520202000A14001501160217031807190B1A0C1C0D1D111F150102FF6400000032000000010064000000320000000800000001000000",
            Logged: "01000013010200020A00140405000000520202000A14001501160217031807190B1A0C1C0D1D111F150102FF64000000320000000100640000003200000008000000"),
        new(2, Source: 3, Target: 4, Other: -1, Gold: 200, Mana: 0, Reaction: 1, EscrowGold: 0, EscrowMana: 0, EventLogId: 12, Day: 15, Message: ["An alliance.", "Second line."],
            Alone: "0100005202020009140015011602170319071A081B091E291F2D0304FFC800000002011C000000416E20616C6C69616E63652E0D0A5365636F6E64206C696E652E0D0A0C0000000F000000",
            Token: "01000008110200030A001404154CFFFFFFFF5202020009140015011602170319071A081B091E291F2D0304FFC800000002011C000000416E20616C6C69616E63652E0D0A5365636F6E64206C696E652E0D0A0C0000000F00000001000000",
            Logged: "01000013010200020A001404050000005202020009140015011602170319071A081B091E291F2D0304FFC800000002011C000000416E20616C6C69616E63652E0D0A5365636F6E64206C696E652E0D0A0C0000000F000000"),
        new(3, Source: 1, Target: 2, Other: -1, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 5, Day: 3, Message: ["Hello", "there"],
            Alone: "010000520202000814001501160219031A041B051E171F1B0102FF03000E00000048656C6C6F0D0A74686572650D0A0500000003000000",
            Token: "01000008110200030A0014041538FFFFFFFF520202000814001501160219031A041B051E171F1B0102FF03000E00000048656C6C6F0D0A74686572650D0A050000000300000001000000",
            Logged: "01000013010200020A00140405000000520202000814001501160219031A041B051E171F1B0102FF03000E00000048656C6C6F0D0A74686572650D0A0500000003000000"),
        new(4, Source: 2, Target: 5, Other: -1, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 8, Message: ["Beware."],
            Alone: "010000520202000714001501160219031A041B051F120205FF0400090000004265776172652E0D0A08000000",
            Token: "01000008110200030A001404152DFFFFFFFF520202000714001501160219031A041B051F120205FF0400090000004265776172652E0D0A0800000001000000",
            Logged: "01000013010200020A00140405000000520202000714001501160219031A041B051F120205FF0400090000004265776172652E0D0A08000000"),
        new(5, Source: 2, Target: 3, Other: -1, Gold: 0, Mana: 75, Reaction: 0, EscrowGold: 0, EscrowMana: 75, EventLogId: 0, Day: 11, Message: [],
            Alone: "0100005202020008140015011602180319071A081D091F0D0203FF4B00000005004B0000000B000000",
            Token: "01000008110200030A001404152AFFFFFFFF5202020008140015011602180319071A081D091F0D0203FF4B00000005004B0000000B00000001000000",
            Logged: "01000013010200020A001404050000005202020008140015011602180319071A081D091F0D0203FF4B00000005004B0000000B000000"),
        new(6, Source: 7, Target: 8, Other: -1, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 21, Day: 20, Message: [],
            Alone: "010000520202000714001501160219031A041E051F090708FF06001500000014000000",
            Token: "01000008110200030A0014041524FFFFFFFF520202000714001501160219031A041E051F090708FF0600150000001400000001000000",
            Logged: "01000013010200020A00140405000000520202000714001501160219031A041E051F090708FF06001500000014000000"),
        new(7, Source: 5, Target: 1, Other: 3, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 2, Message: ["From someone else."],
            Alone: "010000520202000714001501160219031A041B051F1D05010307001400000046726F6D20736F6D656F6E6520656C73652E0D0A02000000",
            Token: "01000008110200030A0014041538FFFFFFFF520202000714001501160219031A041B051F1D05010307001400000046726F6D20736F6D656F6E6520656C73652E0D0A0200000001000000",
            Logged: "01000013010200020A00140405000000520202000714001501160219031A041B051F1D05010307001400000046726F6D20736F6D656F6E6520656C73652E0D0A02000000"),
        new(8, Source: 2, Target: 4, Other: 6, Gold: 300, Mana: 20, Reaction: 2, EscrowGold: 0, EscrowMana: 0, EventLogId: 44, Day: 30, Message: [],
            Alone: "010000520202000914001501160217031807190B1A0C1E0D1F110204062C0100001400000008022C0000001E000000",
            Token: "01000008110200030A0014041530FFFFFFFF520202000914001501160217031807190B1A0C1E0D1F110204062C0100001400000008022C0000001E00000001000000",
            Logged: "01000013010200020A00140405000000520202000914001501160217031807190B1A0C1E0D1F110204062C0100001400000008022C0000001E000000"),
        new(9, Source: 3, Target: 1, Other: 2, Gold: 50, Mana: 0, Reaction: 1, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 12, Message: ["War on them."],
            Alone: "0100005202020008140015011602170319071A081B091F1B0301023200000009010E000000576172206F6E207468656D2E0D0A0C000000",
            Token: "01000008110200030A0014041538FFFFFFFF5202020008140015011602170319071A081B091F1B0301023200000009010E000000576172206F6E207468656D2E0D0A0C00000001000000",
            Logged: "01000013010200020A001404050000005202020008140015011602170319071A081B091F1B0301023200000009010E000000576172206F6E207468656D2E0D0A0C000000"),
        new(10, Source: 1, Target: 2, Other: -1, Gold: 7, Mana: 8, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 1, Message: [],
            Alone: "010000520202000814001501160217031807190B1A0C1F0D0102FF07000000080000000A0001000000",
            Token: "01000008110200030A001404152AFFFFFFFF520202000814001501160217031807190B1A0C1F0D0102FF07000000080000000A000100000001000000",
            Logged: "01000013010200020A00140405000000520202000814001501160217031807190B1A0C1F0D0102FF07000000080000000A0001000000"),
        new(200, Source: 1, Target: 2, Other: 0, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 0, Message: ["x"],
            Alone: "01000052020200051400150119021A031B040102C80003000000780D0A",
            Token: "01000008110200030A001404151EFFFFFFFF52020200051400150119021A031B040102C80003000000780D0A01000000",
            Logged: "01000013010200020A0014040500000052020200051400150119021A031B040102C80003000000780D0A"),
        new(0, Source: 0, Target: 0, Other: 0, Gold: 0, Mana: 0, Reaction: 0, EscrowGold: 0, EscrowMana: 0, EventLogId: 0, Day: 0, Message: [],
            Alone: "010000520202000219001A010000",
            Token: "01000008110200030A001404150FFFFFFFFF520202000219001A01000001000000",
            Logged: "01000013010200020A00140405000000520202000219001A010000"),
    ];

    public static TheoryData<int> CaseIndexes { get; } = [.. Enumerable.Range(0, count: s_cases.Length)];

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
        byte[] bytes = Convert.FromHexString(hex);
        MemoryStream memory = new();
        memory.WriteBuffer(bytes);
        memory.Position = 0;
        return memory;
    }

    private static GameEngine CreateEngine()
    {
        GameEngine engine = new(null);
        engine.Options &= ~GameEngine.VerifyClasses;
        engine.RegisterEngineClasses([EngineClass.Of<DiplomaticAction>(), EngineClass.Of<DiplomaticActionTokenEvent>(), EngineClass.Of<DiplomaticActionEventLog>()]);
        return engine;
    }

    private static string Write(GameEngine engine, EngineObject value)
    {
        using MemoryStream memory = new();
        engine.StoreEngineObject(memory, objectValue: value, progress: null);
        return Hex(memory);
    }

    private static Translate Read<Translate>(GameEngine engine, string hex) where Translate : EngineObject
    {
        using MemoryStream memory = Memory(hex);
        return Assert.IsType<Translate>(engine.RetrieveEngineObject(memory, progress: null));
    }

    private static DiplomaticAction Action(Case c) => new(null)
    {
        Holder = EngineFixture.Engine,
        Source = PlayerId.FromOriginal(c.Source),
        Target = PlayerId.FromOriginal(c.Target),
        OtherPlayer = PlayerId.FromOriginal(c.Other),
        Reaction = (DiplomaticActionReaction)c.Reaction,
        EscrowGold = c.EscrowGold,
        EscrowMana = c.EscrowMana,
        EventLogId = c.EventLogId,
        Day = c.Day,
        Proposal = Expected(c.Kind, c.Gold, c.Mana) with { Message = c.Message },
    };

    private static void AssertCase(Case c, DiplomaticAction? action)
    {
        Assert.Equal(Expected(c.Kind, c.Gold, c.Mana) with { Message = c.Message }, action.Proposal);
        Assert.Equal((c.Source, c.Target, c.Other, c.Reaction), ((sbyte)PlayerIdExtensions.ToOriginal(action.Source), (sbyte)PlayerIdExtensions.ToOriginal(action.Target), (sbyte)PlayerIdExtensions.ToOriginal(action.OtherPlayer), (byte)action.Reaction));
        Assert.Equal((c.EscrowGold, c.EscrowMana, c.EventLogId, c.Day), (action.EscrowGold, action.EscrowMana, action.EventLogId, action.Day));
    }

    [Theory]
    [MemberData(nameof(CaseIndexes))]
    public void ProposalsWriteTheOldBytes(int index)
    {
        Case c = s_cases[index];
        GameEngine engine = CreateEngine();
        try
        {
            DiplomaticProposal proposal = DiplomaticProposal.FromFields(c.Kind, c.Gold, c.Mana);
            Assert.Equal(Expected(c.Kind, c.Gold, c.Mana), proposal);
            Assert.Equal((c.Kind, c.Gold, c.Mana), proposal.ToFields());

            DiplomaticAction action = Action(c);
            Assert.Equal(c.Alone, Write(engine, value: action));
            _ = action.Release();

            DiplomaticActionTokenEvent tokenEvent = new(null) { Holder = EngineFixture.Engine, Operation = 1 };
            action = Action(c);
            tokenEvent.SetDiplomaticAction(action);
            _ = action.Release();
            Assert.Equal(c.Token, Write(engine, value: tokenEvent));
            tokenEvent.Dispose();

            DiplomaticActionEventLog log = new(null, holder: EngineFixture.Engine) { Day = 5, Action = Action(c) };
            Assert.Equal(c.Logged, Write(engine, value: log));
            log.Dispose();

            DiplomaticAction loaded = Read<DiplomaticAction>(engine, hex: c.Alone);
            AssertCase(c, action: loaded);
            Assert.Equal(c.Alone, Write(engine, value: loaded));
            _ = loaded.Release();

            DiplomaticActionTokenEvent loadedEvent = Read<DiplomaticActionTokenEvent>(engine, hex: c.Token);
            AssertCase(c, loadedEvent.Action);
            Assert.Equal(c.Token, Write(engine, value: loadedEvent));
            loadedEvent.Dispose();

            DiplomaticActionEventLog loadedLog = Read<DiplomaticActionEventLog>(engine, hex: c.Logged);
            AssertCase(c, loadedLog.Action);
            Assert.Equal(c.Logged, Write(engine, value: loadedLog));
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
            using DiplomaticActionEventLog log = new(null, holder: EngineFixture.Engine) { Day = 5 };
            log.Action = new DiplomaticAction(null) { Holder = EngineFixture.Engine, Source = PlayerId.Player6, Target = PlayerId.Player11, OtherPlayer = null, EventLogId = 16, Day = 7, Proposal = new DeclareWar() with { Message = message } };
            string bytes = Write(engine, value: log);
            DiplomaticActionEventLog copy = Read<DiplomaticActionEventLog>(engine, hex: bytes);
            Assert.Equal(message, copy.Action.Proposal.Message);
            Assert.Equal("The Dark Elves declare war on the Humans.\r\n\r\nSecond line.\r\n", copy.Action.Proposal.MessageText);
            Assert.Equal(bytes, Write(engine, value: copy));

            // The copy for an AI player's action list drops the message (Diplomacy.pas:938), the logged copy keeps it.
            DiplomaticActionEventLog listed = Read<DiplomaticActionEventLog>(engine, hex: bytes);
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
        foreach ((byte kind, int gold, int mana) in new (byte, int, int)[] { (10, 7, 8), (200, 0, 0), (255, -1, 3) })
        {
            DiplomaticProposal proposal = DiplomaticProposal.FromFields(kind, gold, mana);
            Assert.Equal(new UnknownProposal(kind, gold, mana), proposal);
            Assert.Equal((kind, gold, mana), proposal.ToFields());
            Assert.Same(proposal, proposal.WithOffer(5, mana: 5));
        }
        Assert.Equal(new DeclareWar(), new DiplomaticAction(null) { Holder = EngineFixture.Engine }.Proposal);
    }

    // The kinds that carry no offer keep none. The census finds gold and mana on no kind.
    [Fact]
    public void KindsWithoutOfferDropGoldAndMana()
    {
        foreach (byte kind in new byte[] { 0, 3, 4, 6, 7 })
        {
            Assert.Equal((kind, 0, 0), DiplomaticProposal.FromFields(kind, gold: 5, mana: 6).ToFields());
            Assert.Equal((kind, 0, 0), DiplomaticProposal.FromFields(kind, gold: 0, mana: 0).WithOffer(5, mana: 6).ToFields());
        }
        foreach (byte kind in new byte[] { 1, 2, 5, 8, 9 })
        {
            Assert.Equal((kind, 5, 6), DiplomaticProposal.FromFields(kind, gold: 5, mana: 6).ToFields());
            Assert.Equal((kind, 7, 8), DiplomaticProposal.FromFields(kind, gold: 0, mana: 0).WithOffer(7, mana: 8).ToFields());
        }
    }

    [Fact]
    public void OriginalKindsRegisterOnce()
    {
        for (int kind = 0; kind <= 9; kind++)
        {
            Assert.True(DiplomaticProposal.IsRegistered(kind.TruncateToByte()), userMessage: $"kind {kind}");
            _ = Assert.Throws<InvalidOperationException>(() => DiplomaticProposal.Register(kind.TruncateToByte(), static (gold, mana) => new UnknownProposal(0, gold, mana)));
        }
        foreach (byte kind in new byte[] { 10, 11, 200, 255 })
        {
            Assert.False(DiplomaticProposal.IsRegistered(kind), userMessage: $"kind {kind}");
        }
    }

    // The editor of the relations window: the message enters on each edit of the memo, the offer
    // when the player sends (AoWRelations.pas:794, 800, 877, 882).
    [Fact]
    public void TheEditorBuildsTheProposal()
    {
        DiplomaticAction action = new(null) { Holder = EngineFixture.Engine, Source = PlayerId.Player1, Target = PlayerId.Player2, OtherPlayer = null, Proposal = new OfferPeace() with { Message = ["Default text."] } };
        action.Proposal = action.Proposal with { Message = DiplomaticProposal.Lines("Peace?\r\nPlease.") };
        action.Proposal = action.Proposal.WithOffer(120, mana: 30);
        Assert.Equal(new OfferPeace(120, Mana: 30) with { Message = ["Peace?", "Please."] }, action.Proposal);
        DiplomaticAction message = new(null) { Holder = EngineFixture.Engine, Proposal = new SendMessage() };
        message.Proposal = message.Proposal.WithOffer(120, mana: 30);
        Assert.Equal(new SendMessage(), message.Proposal);
        _ = action.Release();
        _ = message.Release();
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
    private static int OldMiddleIcon(byte kind, byte reaction) => kind == 0 || (kind == 9 && reaction == 1) ? 0x15 : 0x16;

    // Diplomacy.pas:1265 GetText.
    private static string OldText(byte kind, DiplomaticActionReaction reaction, bool sourceSeated)
    {
        static string Translate(string name) => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: name);
        static string FormatText(string format, string argument) => AbilityRules.Format(Translate(format), arguments: [argument]);
        string Answer(string none, string subject, string accepted, string otherwise) => reaction switch
        {
            DiplomaticActionReaction.None => none,
            DiplomaticActionReaction.Reject => FormatText(DiplomaticProposal.RejectedFormat, argument: subject),
            DiplomaticActionReaction.Accept => accepted,
            DiplomaticActionReaction.Ignore => FormatText(DiplomaticProposal.IgnoredFormat, argument: subject),
            _ => otherwise,
        };
        return kind switch
        {
            0 => Translate(GameTables.WarText),
            2 => Answer(Translate(ProposeAlliance.AllianceProposalText), subject: Translate(ProposeAlliance.AllianceProposalText),
                accepted: sourceSeated ? FormatText(DiplomaticProposal.AcceptedFormat, argument: Translate(ProposeAlliance.AllianceProposalText)) : Translate(ProposeAlliance.MadeAllianceText), otherwise: Translate(ProposeAlliance.AllianceProposalText)),
            1 => Answer(Translate(OfferPeace.PeaceOfferText), subject: Translate(OfferPeace.PeaceOfferText),
                accepted: sourceSeated ? FormatText(DiplomaticProposal.AcceptedFormat, argument: Translate(OfferPeace.PeaceOfferText)) : Translate(GameTables.PeaceText), otherwise: ""),
            5 => Answer(Translate(Player.MakeTribute.TributeText), subject: Translate(Player.MakeTribute.TributeText), accepted: FormatText(DiplomaticProposal.AcceptedFormat, argument: Translate(Player.MakeTribute.TributeText)), otherwise: ""),
            9 => Answer(Translate(Player.SuggestWarDeclaration.DeclareWarSuggestionText), subject: Translate(GameText.DeclareWarText), accepted: FormatText(DiplomaticProposal.AcceptedFormat, argument: Translate(GameText.DeclareWarText)), otherwise: ""),
            8 => Answer(Translate(Player.SuggestAllianceBreak.BreakAllianceSuggestionText), subject: Translate(GameText.BreakAllianceText), accepted: FormatText(DiplomaticProposal.AcceptedFormat, argument: Translate(GameText.BreakAllianceText)), otherwise: ""),
            6 => Translate(BreakAlliance.AllianceBrokenText),
            3 or 7 => Translate(GameText.DiplomaticMessageText),
            4 => Translate(Player.ThreatenToAttack.ThreatText),
            _ => "DiplomaticActionEventLog",
        };
    }

    // Diplomacy.pas:505 GetName, for the kinds that name no other player.
    private static string OldName(byte kind) => kind switch
    {
        0 => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: GameText.DeclareWarText),
        1 => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: OfferPeace.OfferPeaceText),
        2 => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: ProposeAlliance.ProposeAllianceText),
        3 => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: SendMessage.SendMessageText),
        4 => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: Player.ThreatenToAttack.ThreatenToAttackText),
        5 => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: Player.MakeTribute.MakeTributeText),
        6 => AbilityRules.TranslateResourceString(EngineFixture.Engine, S: GameText.BreakAllianceText),
        _ => "",
    };

    // Diplomacy.pas:1420 RetrieveDefaultDiplomaticActionMessage, the switch after the races and the alignment.
    private static void OldDefaultMessage(TextControl textControl, int alignment, string sourceRace, string targetRace, string otherRace,
        byte action, DiplomaticActionReaction reaction, DiplomaticRelation relation, DelphiStringCollection text)
    {
        text.Clear();
        void Get(int number, string first, string second, string? third = null)
        {
            textControl.RetrieveText(alignment + number, text);
            text.Text = third is null ? AbilityRules.Format(text.Text, arguments: [first, second]) : AbilityRules.Format(text.Text, arguments: [first, second, third]);
        }
        switch (action)
        {
            case 0: Get(relation == DiplomaticRelation.Unknown ? 151 : 100, first: sourceRace, second: targetRace); break;
            case 1:
                if (reaction == DiplomaticActionReaction.None)
                {
                    Get(relation == DiplomaticRelation.Unknown ? 103 : 154, first: sourceRace, second: targetRace);
                }
                else if (reaction == DiplomaticActionReaction.Accept)
                {
                    Get(127, first: targetRace, second: sourceRace);
                }
                else if (reaction == DiplomaticActionReaction.Reject)
                {
                    Get(130, first: targetRace, second: sourceRace);
                }

                break;
            case 2:
                if (reaction == DiplomaticActionReaction.None)
                {
                    Get(106, first: sourceRace, second: targetRace);
                }
                else if (reaction == DiplomaticActionReaction.Accept)
                {
                    Get(121, first: targetRace, second: sourceRace);
                }
                else if (reaction == DiplomaticActionReaction.Reject)
                {
                    Get(124, first: targetRace, second: sourceRace);
                }

                break;
            case 6: Get(109, first: sourceRace, second: targetRace); break;
            case 4: Get(112, first: sourceRace, second: targetRace); break;
            case 5:
                if (reaction == DiplomaticActionReaction.None)
                {
                    Get(118, first: sourceRace, second: targetRace);
                }
                else if (reaction == DiplomaticActionReaction.Accept)
                {
                    Get(133, first: targetRace, second: sourceRace);
                }
                else if (reaction == DiplomaticActionReaction.Reject)
                {
                    Get(136, first: targetRace, second: sourceRace);
                }

                break;
            case 8:
                if (reaction == DiplomaticActionReaction.None)
                {
                    Get(160, first: sourceRace, second: targetRace, third: otherRace);
                }
                else if (reaction == DiplomaticActionReaction.Accept)
                {
                    Get(139, first: targetRace, second: sourceRace, third: otherRace);
                }
                else if (reaction == DiplomaticActionReaction.Reject)
                {
                    Get(142, first: targetRace, second: sourceRace, third: otherRace);
                }

                break;
            case 9:
                if (reaction == DiplomaticActionReaction.None)
                {
                    Get(157, first: sourceRace, second: targetRace, third: otherRace);
                }
                else if (reaction == DiplomaticActionReaction.Accept)
                {
                    Get(145, first: targetRace, second: sourceRace, third: otherRace);
                }
                else if (reaction == DiplomaticActionReaction.Reject)
                {
                    Get(148, first: targetRace, second: sourceRace, third: otherRace);
                }

                break;
            default:
                break;
        }
    }

    // Texts 100 to 164, two lines each. The texts the switch formats with the other player's race
    // take three arguments, so a call with two of them throws.
    private static TextControl TextControl()
    {
        int[] withOther = [157, 160, 139, 142, 145, 148];
        TextControl control = new(null) { Holder = EngineFixture.Engine };
        for (int id = 100; id <= 164; id++)
        {
            TextControlItem item = control.New();
            item.Id = id;
            bool three = withOther.Any(first => id >= first && id <= first + 2);
            item.Text.Text = $"Text {id}: %s and %s{(three ? " about %s" : "")}\r\nSecond line {id}";
        }
        return control;
    }

    private static readonly byte[] s_kinds = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    [Fact]
    public void ProposalsAnswerAsTheOldSwitches()
    {
        IDisposable previousMapScope = EngineFixture.UseMap(StrategicMap.Current);
        EngineFixture.SetMap(new StrategicMap(null, holder: EngineFixture.Engine));
        TextControl control = TextControl();
        try
        {
            int checks = 0;
            foreach (byte kind in s_kinds)
            {
                DiplomaticProposal proposal = DiplomaticProposal.FromFields(kind, gold: 0, mana: 0);
                Assert.Equal(OldView(kind), proposal.View.Name);
                Assert.Equal(OldIcon(kind), proposal.Icon);
                Assert.Equal(kind == 7, actual: proposal.SignedAsOtherPlayer);
                Assert.Equal(OldSample(kind), proposal.LoggedSample);
                if (kind is <= 6 or 10)
                {
                    Assert.Equal(OldName(kind), proposal.Name(new DiplomaticAction(null) { Holder = EngineFixture.Engine, Proposal = proposal }));
                }

                checks += 5;
                string error = "";
                Assert.Equal(kind != 5, actual: proposal.CanSend(new DiplomaticAction(null) { Holder = EngineFixture.Engine, Proposal = proposal }, errorText: ref error));
                Assert.Equal(kind == 5 ? AbilityRules.TranslateResourceString(EngineFixture.Engine, S: Player.MakeTribute.NoGoldManaSpecifiedText) : "", error);
                Assert.True(proposal.WithOffer(0, mana: 1).CanSend(new DiplomaticAction(null) { Holder = EngineFixture.Engine }, errorText: ref error));
                checks += 2;
                foreach (byte reaction in new byte[] { 0, 1, 2, 3, 4 })
                {
                    foreach (sbyte source in new sbyte[] { 0, 1 })
                    {
                        DiplomaticAction action = new(null) { Holder = EngineFixture.Engine, Source = PlayerId.FromOriginal(source), Target = PlayerId.Player2, Reaction = (DiplomaticActionReaction)reaction, Proposal = proposal };
                        string where = $"{proposal} reaction {reaction} source {source}";
                        Assert.True(OldText(kind, reaction: (DiplomaticActionReaction)reaction, sourceSeated: source == PlayerIdExtensions.ToOriginal(EngineFixture.Engine.StrategicMap.SeatedPlayer)) == proposal.LogText(action), userMessage: where);
                        Assert.True(OldMiddleIcon(kind, reaction) == proposal.MiddleIcon(action), userMessage: where);
                        checks += 2;
                    }
                    foreach (DiplomaticRelation relation in new[] { DiplomaticRelation.Peace, DiplomaticRelation.War, DiplomaticRelation.Alliance, DiplomaticRelation.Unknown })
                    {
                        foreach (int alignment in new[] { 0, 1, 2 })
                        {
                            DelphiStringList expected = [];
                            DelphiStringList actual = [];
                            OldDefaultMessage(control, alignment, sourceRace: "Elves", targetRace: "Orcs", otherRace: "Dwarves", action: kind, reaction: (DiplomaticActionReaction)reaction, relation, text: expected);
                            proposal.DefaultMessage(new DiplomaticMessageText(control, alignment, sourceRace: "Elves", targetRace: "Orcs", otherRace: "Dwarves", text: actual), reaction: (DiplomaticActionReaction)reaction, relation);
                            Assert.True(expected.Text == actual.Text, userMessage: $"{proposal} reaction {reaction} relation {relation} alignment {alignment}");
                            checks++;
                        }
                    }
                }
            }
            Assert.Equal(s_kinds.Length * (7 + (5 * (4 + 12))), checks);
        }
        finally
        {
            control?.Dispose();
            previousMapScope.Dispose();
        }
    }
}
