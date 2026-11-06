# Unions and kind families (milestone 9.6)

Amended by roadmap.md, 2026-10-30

State: draft, revised 2026-10-30 after the user's review of `bd279cd`. Covers `docs/milestones.md`
row 9.6 (done measure O3) and `docs/design.md` sections 2 and 9. Base: `main` at `64a05be`, with
9.4-1, 9.4-2, 9.4-5, 9.4-6, 9.3b-14 and 9.5-P1 merged and 9.4-3 in flight. Two directions of the
user from 2026-10-30 shape it. Saves and maps get the port's own format behind a round-trip
converter (milestone 9.7), and the oracle compares the converter's original-format output byte for
byte. And `union` serves only data that is closed by nature: behavior that varies by kind,
presentation included, lives in virtual methods on the kind, so that a mod adds a kind without
touching a screen.

Implemented. Briefs 9.6-1 to 9.6-7 merged on 2026-11-05 (PRs #99 and #103 to #108, main
`a4bc2a3`), after the private census 9.6-0 (`data/rewrite/9.6-census.md`). Brief 9.6-8 read this
document against the code the same day and recorded the decisions the briefs took (roadmap row
42): section 1 stays the survey before the milestone, sections 3 and 4 name the code as it is,
sections 2 and 5 to 7 stay the rule and the plan the briefs ran under, with the amendments of
2026-11-05, and section 8 holds the result.

## 1. Where the tree stands

Surveyed on main with `git grep` and a field-offset scan of every explicit-layout struct.

| category | count on main |
|---|---|
| Delphi variant records (`data/records/records.md`) | 8: `TXYL`, `TPaletteEntry`, `TDefenseRequirements`, `TEMsg`, `TShowInfo` and three anonymous combat-info buffers (four C# structs) |
| explicit-layout structs | 106, of which 7 have overlapping fields: `TXYL` (`HSEngine.cs:4815`), `TPaletteEntry` (`GFXE.cs:1273`), `TDefenseRequirements` (`AoWE.cs:99830`), the buffers (`AoWTC.cs:66, 75, 84, 93`) |
| `IntOrRef` slots | `TEMsg.Payload` and `Param[3]` (`Engine.cs:6077`), `TMapFieldMsg.Param[4]` and `Result` (`HSEngine.cs:5292`), `TShowInfo.Payload` (`ImageLib.cs:2474`), `THashItem.Key` (`Engine.cs:8682`, `HashTabU.cs:15`) |
| byte views | `Unsafe.As` on 291 lines (211 views over `TXYL`), `MemoryMarshal` on 102 lines |
| `TVarRec` | `src/Rtl/Values.cs`, 335 `Format` lines in 49 Units files, one Units signature (`AoWE.cs:103537`) |
| engine messages | 81 `MsgProc` overrides, 216 `MsgID ==` tests over 73 literal ids, 306 `Param` uses (80 `As<T>()`, 94 `.Value`), 59 Units files. The 10 typed payload objects of 9.2 ride in `Param` slots |
| rules families (descendants) | `TSpell` 117, `TAbility` 83, `TTokenEvent` 63, `TEventLog` 43, `TStructure` 41, `TCombatAction` 36, `TAIGroup` 18, `TAIPlayerAction` 14, `TMoveOnEvent` 3, `TFlagEvent` 2 |
| type cascades | event logs: 28 arms in `TheMapExecuteEventLog` (`aowMapEvents.cs:348`) and 2 in the AI's `TProcessEventlogAIPA` (`AoWE.cs:69037`). Structures: 15 arms in `ShowStructureInfo` (`AoWInfoWin.cs:510`), 8 in `MainBtnClick` (`AoWInfoWin.cs:379`), 4 in `TheMapStructureDblClick` (`aowMapEvents.cs:1071`) |
| class id dispatch | 1 `switch` on `EClass.ClassID`, 17 structure ids to tactical map names (`AoWTC.cs:12332`). 32 id comparisons and 152 `FindOwnedChild(id)` lines are queries, not dispatch |
| kind-tagged objects | `TProductionItem` (`AoWE.cs:1273`): `FProductionID` in {1, 2, 10 to 14} decides what `FParameter` means, 10 switches, 20 comparisons, 6 files. `TDiplomaticAction` (`AoWE.cs:95552`): `FActionType` (10 kinds) decides which fields matter, 14 switches in 3 files |

Facts that decide the design:

- Spells, abilities, tokens, combat actions and map events already dispatch through virtual
  methods. The other `switch` statements (585 in all) select code by an enum over the same fields.
- `TEMsg` and `TMapFieldMsg` never reach a stream or a packet. `TShowInfo` reaches the ILB files.
  `TProductionItem` and `TDiplomaticAction` are saved, sit in maps and travel in token events.
- `design.md` section 1 says "539 variant records", section 2 says eight. Eight is right.

## 2. What C# 15 provides

Probed on 2026-10-30 against SDK 11.0.100-rc.3:

- `union U(A, B)` is a struct over one `object? Value` with a conversion per case. Value-type cases
  box. Switch expressions over it are checked (CS8509), `default(U)` matches `null`.
- A custom union is a `[Union]` struct implementing `IUnion` whose one-parameter constructors name
  the cases, with `HasValue` and one `TryGetValue(out T)` per case. Patterns go through
  `TryGetValue` (the probe's `Value` threw and every match still succeeded), conversions work in
  collection expressions and `params ReadOnlySpan<U>`. No boxing.
- `closed class C` makes `C` abstract and forbids subclasses in another assembly (CS9382).
- Exhaustiveness is checked for switch expressions only, never for switch statements.

Rule for 9.6: a `union` for data closed by nature, in the custom form when a case is a value type,
since each sits on a frequent path. Behavior that varies by kind is a virtual method on the kind.
Where the behavior is presentation, the virtual returns a description (a view key) that the screen
renders, so no screen code enters a rules class. 9.6 declares no `closed` type: no family in the
tree is closed by nature, and `closed` bars code mods. After 9.6 no cascade over a family's types
remains in screen code.

## 3. Decisions

| candidate | closed by nature? | decision |
|---|---|---|
| `TXYL`, `TPaletteEntry`, the byte views | yes, byte layouts | stay explicit-layout, game logic reads both views |
| `TDefenseRequirements` | yes | stays, `Strength` aliases element 7 of `DamageStrengths` |
| the four combat-info buffers | | removed, the second arm is never read. They are plain `TAbilityCombatInfo` locals |
| `TVarRec` | yes, the cases of `Format` | custom union `FormatArg` (3.1) |
| `TEMsg` and `TMapFieldMsg` slots | yes, int or reference | custom union `MsgArg` (3.2) |
| `TShowInfo` blend slot | yes, percentage or table | custom union `BlendValue` (3.3) |
| `THashItem.Key` | | stays `IntOrRef` until the collections pass gives each hash list a typed key |
| message kinds | no | stay. 73 bare literal ids. A message hierarchy needs named ids first, a follow-up |
| event logs | no, mods add events | virtual `View` and `ExecuteAI` (3.4) |
| structures | no, the first thing a mod adds | virtual panel, actions and tactical map name (3.5) |
| production kinds | no, a new city action is plausible | open `ProductionOrder` records with virtual behavior (3.6) |
| diplomatic kinds | no, new options are plausible | open `DiplomaticProposal` records with virtual behavior (3.7) |
| spells, abilities, tokens, combat actions, map events, AI | no | stay, already virtual |
| id comparisons, `FindOwnedChild`, behavior enums | | stay, queries for one class and enum tags select code |

### 3.1 FormatArg

```csharp
[Union] public readonly struct FormatArg : IUnion     // src/Rtl/FormatArg.cs
{
    // cases int, bool, char, double, string, TObject, PointerArg (a record struct over uint)
    private readonly long bits; private readonly object? reference; private readonly Case kind;
}
public static string Format(string? format, params ReadOnlySpan<FormatArg> args);
```

Brief 9.6-1 (PR #99). The `Format` calls compile unchanged. There is one implicit conversion per
case and one from a Delphi `Char` byte through `Cp1252.Char`, because the compiler prefers a
user-defined conversion to the union's own. With a lone `byte` operator a constant 1 turns into a
Char, and one operator per source type keeps the case each argument had under `TVarRec`. Both a
Delphi `Char` byte and a string-element `char` select the Char case (roadmap X23). A nil `TObject`
is the union's null case, not an Object case holding null (9.6-1 review), since a type pattern on a
custom union does not test for null. `Format` rejects it as it rejects any argument of the wrong
kind. `Format`, which every `CreateFmt` calls, reads each argument with one switch expression with
no discard arm, so a new case fails the build. The `Class` case went with CF-3, and the `PChar` case
is dropped, since nothing passes a `Ptr<byte>` to `Format`, which the build proves. `TVarRec`,
`VarKind` and `ExceptionText` are deleted. `rtl-shim.md` 1.6 lists the cases and conversions, 3.3
what each conversion character takes.

### 3.2 MsgArg

```csharp
[Union] public readonly struct MsgArg : IUnion, IEquatable<MsgArg>   // src/Rtl/MsgArg.cs, cases int and TObject
{
    public int Int { get; }                     // the int, 0 when empty, EInvalidCast on a reference
    public T? As<T>() where T : class;          // the reference as T, null otherwise
    // ==, != and Equals: the equality of the original 32-bit word
}
[InlineArray(Arity)] public struct TEMsgParams { public const int Arity = 3; }          // Engine.cs
[InlineArray(Arity)] public struct TMapFieldMsgParams { public const int Arity = 4; }   // HSEngine.cs
```

Brief 9.6-4 (PR #105). `TEMsg.Param` is a `TEMsgParams` and `TMapFieldMsg.Param` a
`TMapFieldMsgParams`, inline arrays of `MsgArg` named by their role with the arity of the message
protocol as a named constant, never a generated `FixedArray_MsgArg_N` (roadmap X31). These are kept
protocol layouts, not content counts. `TEMsg.Payload` and `TMapFieldMsg.Result` are `MsgArg`, and
the `Engine`, `Obj` and `Value` accessors are gone. A nil reference is the union's null, as the
original slot holds 0 for it.

What changes against `IntOrRef`: a reference no longer offers its `InstanceId` as an integer, and
`Int` on a reference throws. A compile-time scan of all 511 slot uses found no reference read as an
integer. The census's three mixed reads, `Value` of a reference in `TMapFieldMsg.Result`, were the
answered test of `TMapField.SendMapFieldMsg`, which compares the result before and after each
handler. It compares with `MsgArg` equality, the equality of the original word: the same integer or
the same object are equal, the empty slot, the integer 0 and the nil reference are all the word 0,
and a nonzero integer never equals a reference. So an integer answer and a reference answer stop
the loop alike, a reference the caller preset stays and every handler runs, and a handler that
writes 0 over 0 has not answered, as in the original. `MsgArgTests` cover the union,
`MessageSlotTests` the arity, the dispatcher and each slot's cases, and
`NoStorageMethodTakesAMessageRecord` the absence of a message record from every `rw*` and `Read`
method.

### 3.3 TShowInfo

```csharp
[Union] public readonly struct BlendValue : IUnion    // src/Units/GFXE.BlendValue.cs, cases int (percentage) and TBlendTable
{
    public int Percentage { get; }                    // the percentage, 0 when empty, EInvalidCast on a table
}
public struct TShowInfo { public TShowMode ShowMode; public TBlendMode BlendMode; public BlendValue Blend; }   // ImageLib.cs
```

Brief 9.6-5 (PR #106). `BlendValue` sits beside the table type in the units, not in the shim. A
nil table is the union's null. `BlendMode` stays a field: it picks one of five tables for a
percentage, and `TLibraryImage.ShowEx` reads the image's own mode with the caller's value
(`transpiler.md` 4.3). The `bmUser` arm of `Show`, `ShowStretched` and `ShowEx` matches
`Blend is TBlendTable table`, the others read `Blend.Percentage`, which throws on a table. A
`bmUser` image read from a file carries an int and draws nothing, as before. Only `ImageLib.cs` and
`AoWTitleScene.cs` read or write the slot, so the 25 more files of section 6 did not exist. The
8-byte image record keeps its layout, and a table writes 0 where the first stage wrote an instance
id, which no written file held. The brief's round trip over the installed libraries compared 11,419
records of 23,677 images in 538 libraries, all equal, and found no `bmUser` image in the data.

### 3.4 Event logs

```csharp
public sealed class EventView(string name) { public string Name => name; /* None, Acknowledge and 34 dialog views */ }
public partial class TEventLog                             // EventLog.Views.cs
{
    public virtual EventView View() => EventView.None;     // what the map events screen shows
    public virtual void ExecuteAI(sbyte Player) { }         // what an AI player does with it
}
public partial class TMapEvents                            // aowMapEvents.cs
{
    public static readonly Catalog<EventView, Action<TMapEvents, TEventLog, bool, TExecuteDoneEvent>> EventRenderers;
}
```

Brief 9.6-3 (PR #104). `TEventLog` stays open. `EventLog.Views.cs` declares the two virtuals with
every override, 28 of `View` and two of `ExecuteAI`, and names no screen. A renderer takes the
screen, the event, the immediate mode and the done callback, so registration needs no screen
instance. Every original renderer but the two production screens (`CityProduction`,
`ProductionPlace`) uses the callback, and three of them (`Tutorial`, `FlagMessage`, `Help`) take
the mode. `ClassRegistry.cs` adds the 35 original renderers with the catalog's `Add`, which
rejects a duplicate key, and the mod facade of milestone 10 adds mod renderers. No separate
`RegisterView` exists. `TheMapExecuteEventLog` hides the transient windows as before and calls the
renderer of `Event.View()`. `None` has no renderer, so nothing runs and `Done` is not called, as
for the 11 kinds the cascade skipped. `Acknowledge` calls `Done(Event)`, the old `TUserEventLog`
arm. The overrides read the state the cascade consulted (the negotiation target and joining hero,
the seated player at game over, the player magic mode, the army view's selection, the prisoner
hero) with the same queries at the same moment. `TProcessEventlogAIPA` calls
`Log.ExecuteAI(player)`, overridden by `THeroEmergeEventLog` and `TDiplomaticActionEventLog`.

Arm order mattered only where an earlier arm caught a subclass. No arm tested a base before one of
its subclasses, so the first match was the nearest handled ancestor, which is what a virtual
resolves to. `TEnterCaveEventLog`, `TPlayerConnectionEventLog`, `TRazeStructureEventLog` and
`TRestartGameEventLog` inherit the `TMessageEventLog` override, `TTutorialMessageEventLog` and
`TFlagMessageEventLog` override `THelpMessageEventLog`'s, and `TJoinEventLog`,
`TProductionEventLog` and the `TTextEventLog` line keep `None` beneath overriding subclasses.
`EventLogViewTests` replay the old cascade over all 44 event log classes with built state.

### 3.5 Structures

Brief 9.6-2 (PR #103). Four dispatch sites became virtuals on `TStructure`, declared with every
override in `Structure.Views.cs`, which names no screen. Keys are `StructureView` values, the open
pattern of `EventView`: `None`, the two screens `CityScreen` and `ProductionScreen`, and 19 panels.
The info window keeps two static catalogs, `TIWindow.StructurePanels` and
`TIWindow.StructureButtons` (`Action<TIWindow, TStructure>`), and the map screen one,
`TMapEvents.StructureScreens` (`Action<TMapEvents, TStructure>`). Renderers take their screen as an
argument, `ClassRegistry.cs` adds the 23 original renderers with `Add`, and both view briefs build
on CF-2's `Catalog`.

| virtual | replaces | returns |
|---|---|---|
| `InfoPanel(sbyte Viewer)` | `ShowStructureInfo` | the panel key. A structure under construction or razed shows that state first (the protected `StatePanel`), else the kind's panel, `Generic` by default. `TPlayerStructure` shows its owner, and every player in debug mode, the protected virtual `OwnerPanel()`, and anyone else `OtherPlayer`. `TCity` shows the war panel to every player in war mode |
| `InfoButton(sbyte Viewer)` | `MainBtnClick` | the screen to open (city, production place), or `None` after running the kind's own action (cave, teleport, exploration site, wizard's tower, arena, altar) |
| `DoubleClick(sbyte Viewer)` | `TheMapStructureDblClick` | the same, with the old ownership and `CanDblClick` gates |
| `CombatMapName(CombatMapSite Site)` | the class id switch of the tactical map generator | the tactical map file stem, `Blank000` by default |

`CombatMapSite` carries the generator state the old arms read and wrote, so `CombatMapName` stays
free of generator code (amended by 9.6-2 on 2026-11-05). The kind reads whether the combat is a
site combat, the random terrain mode and its neighbours (`NeighborHolds(Direction, Kind)`), and
reports back the random terrain mode, whether the map takes the combat's items
(`TakesCombatItems`) and the neighbour tags it fixes (`FixTag`). The generator applies them after
the call. The bridge, no structure, stays in the generator in the original lookup order.

The class id switch matched exact ids, while a virtual is inherited. Each override sits on the
class whose id the switch named, and `NoStructureInheritsAnotherClassMapName` checks that no
structure class inherits a map name from a class above it. `StructureViewTests` replay copies of
the old cascades and the old switch over all 42 structure classes of the `EClass` table and every
state they consulted, including the random seed after the site map draw.

### 3.6 Production orders

```csharp
public abstract record ProductionOrder                                   // AoWE.ProductionOrders.cs
{
    public abstract (int Id, int Parameter, int GroupId) ToFields();
    public static ProductionOrder FromFields(int Id, int Parameter, int GroupId);  // registry, else UnknownOrder
    internal static void Register(int Id, Func<int, int, int, ProductionOrder> Create);
    public virtual string Name(TProductionControl Control);              // and 16 more virtuals
    public virtual void Execute(TProductionControl Control);             // inside the control's update bracket
    public virtual void Executed(TProductionControl Control);            // after the bracket closed
}
public sealed record ProduceUnit(int Index, int GroupId) : ProductionOrder { public const int Kind = 1; /* overrides */ }
// ResearchSpell(Index) 2, Trade(TurnsTotal) 10, Upgrade 11, Fortify 12, Migrate(Race) 13, Loot 14,
// UnknownOrder(Id, Parameter, GroupId)
```

Brief 9.6-6 (PR #107). The kinds are open, so the shape is an open abstract record carrying the
behavior. The 17 virtuals are `CityAction`, `UnitProductionIndex`, `Name`, `NameProgressive`,
`CanExecute`, `Execute`, `Executed`, `Complete`, `Completed`, `Cancel`, `Loopable`, `LocksQueue`,
`ActivityImage` for a city and for a production place, `CitySequence`, `RealmActivity` and
`ActivityLines`. Starting and finishing an order are void pairs, because the old arms ran partly
inside the control's `BeginUpdate` and `EndUpdate` bracket and partly after it.
`TProductionControl.ExecuteProduction` checks `CanExecute`, calls `Execute` inside the bracket and
`Executed` after `EndUpdate`. The turn's end calls `Complete` when the order's turns run out and
`Completed` while the order still runs with no turns left. What varies by control (city or
production place) is virtual on `TProductionControl`, which gained `ProductionCity()` and
`MagicSphere()` in place of the per-control arms, so each arm of the switches over the id or its
mapped `TCityAction` moved into its order class and calls the control's members. The brief removed
30 switches across `AoWE.cs`, `City.cs`, `PrdPlace.cs`, `AoWCityScreen.cs`, `AoWInfoWin.cs`,
`AoWProdScreen.cs` and `AoWRealm.cs`, and the screens take names and images from the order.

The city screen keeps its own button table: `SelectAction`, `CityBtn1Click`, `CityBtn2Click` and
`ActivityImage(TCityAction)` in `AoWCityScreen.cs` select a button's panel and handler, and they
include Raze, which is no order. The table stays for milestone 10, where a mod's new city action
needs a button (amended 2026-11-05 after 9.6-6). `QueueLBChange` feeds an order's `CityAction`
into `SelectAction`.

The order registry is a `Catalog<int, ...>`, whose `Add` rejects a duplicate id and whose `TryGet`
serves `FromFields`. `ClassRegistry.cs` registers the seven original kinds. `TProductionItem`
keeps its node identity and holds `Order` in place of the three fields, `UnknownOrder(0, 0, 0)`
until it is set or read. Finding an equal item (`TProductionQueue.IndexOf`) is record equality.

### 3.7 Diplomatic proposals

```csharp
public abstract record DiplomaticProposal                                // AoWE.DiplomaticProposals.cs
{
    public IReadOnlyList<string> Message { get; init; }                 // property 27, in every kind
    public abstract (byte Kind, int Gold, int Mana) ToFields();
    public static DiplomaticProposal FromFields(byte Kind, int Gold, int Mana);   // registry, else UnknownProposal
    internal static void Register(byte Kind, Func<int, int, DiplomaticProposal> Create);
    public virtual ProposalView View { get; }                           // and 16 more virtuals
}
public abstract record OfferProposal(int Gold, int Mana) : DiplomaticProposal;   // the five kinds with an offer
// DeclareWar 0, OfferPeace 1, ProposeAlliance 2, SendMessage 3, ThreatenToAttack 4, MakeTribute 5,
// BreakAlliance 6, SendMessageAsY 7, BreakAllianceWithY 8, DeclareWarToY 9, UnknownProposal(Kind, Gold, Mana)
```

Brief 9.6-7 (PR #108). `TDiplomaticAction` keeps its class id and the common part: reference
count, day, source, target, other player, reaction, escrow gold and mana, and event log id. It
holds a `DiplomaticProposal`, the pattern of 3.6 keyed by the one-byte kind with a
`Catalog<byte, ...>` that rejects duplicates. `OfferProposal(Gold, Mana)` is the base of the five
kinds that carry an offer (`OfferPeace`, `ProposeAlliance`, `MakeTribute`, `BreakAllianceWithY`,
`DeclareWarToY`). Two fields that section 3 gave the kinds stay common, per roadmap X20 and the
census. `FOtherPlayer` stays a field of `TDiplomaticAction`, since the census finds it written for
every kind, 255 for the kinds that name no other player. `Message` is a member of the
`DiplomaticProposal` base, which every kind carries. No subclass of `TDiplomaticAction` per kind
exists, because the original format names one class id for every kind. `TDiplomaticActionType`
is deleted.

The 17 virtuals do what the 25 replaced switches and branches in `AoWE.cs`, `AoWRelations.cs` and
`E3EventDialogs.cs` did: `WithOffer`, `View`, `Icon`, `SignedAsOtherPlayer`, `Name`, `LogText`,
`LoggedSample`, `MiddleIcon`, `DefaultMessage`, `CanSend`, `Sent`, `LapsesUnanswered`, `Accepted`,
`RejectReaction`, `AnswerAI`, `RefusedAI` and `RelationModifier`. The screens branch on the
proposal's `ProposalView` (`None`, `Notice`, `Threat`, `Offer`, `Message`), one branch per view and
none per kind, with no renderer table until milestone 10 gives a mod's proposal a view of its own.
The relations window enters the player's offer into the proposal at send time with `WithOffer`.
`ClassRegistry.cs` registers the ten original kinds.

## 4. Persistence and the converter boundary

The layout of the files (property numbers, offsets, the one-byte kind) binds only the code that
reads and writes the original format. Their information content binds the model: the oracle
compares the converter's output byte for byte, so the model holds every value the original writes,
including one left in a field its kind does not use. The census fixes each case's fields from the
saves, maps and gate runs.

| family | boundary | adapter |
|---|---|---|
| `FormatArg`, `MsgArg` | none | never streamed or sent. `MessageSlotTests.NoStorageMethodTakesAMessageRecord` checks that no `rw*` or `Read` method takes a message record |
| `TShowInfo` | ILB image, 8 bytes | `ReadImage` and `WriteImage` stay in the struct: mode bytes at 0 and 1, padding at 2, the percentage at 4. Reading yields the percentage case. A table writes 0 where the original wrote a process address no reader uses. ILB files are game data, outside 9.7 |
| event logs, structures | none | virtuals add no state, class ids unchanged |
| `ProductionOrder` | `TProductionItem.ReadWrite` ids 20 to 22, in saves, maps and `TProductionTE` | below |
| `DiplomaticProposal` | `TDiplomaticAction.ReadWrite` ids 23 to 25 and 27 (gold, mana, kind, message), in saves and `TDiplomaticActionTE`. Id 22, the other player, stays in the common part (3.7) | the same pattern, the kind as the one-byte `rwData` at id 25 and the message from the proposal's `Message` |

```csharp
public override void ReadWrite(TEStorageStream Stream)
{
    base.ReadWrite(Stream);
    var (id, parameter, groupId) = Order.ToFields();
    Stream.rwInteger(ref id, 20);
    Stream.rwInteger(ref parameter, 21);
    Stream.rwInteger(ref groupId, 22);
    if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
        Order = ProductionOrder.FromFields(id, parameter, groupId);
}
```

The code is `TProductionItem.ReadWrite` as it stands. `TDiplomaticAction.ReadWrite` writes
properties 20 to 31 the same way. Calls, ids, order and the zero rule (`persistence.md` 1.4) stay as
they were. `ProductionOrderTests` and `DiplomaticProposalTests` compare the written bytes with bytes
recorded on main before the change, for every property set the census found per kind, for the kinds
the census did not meet as the source builds them and for ids no kind registers, alone and inside
`TProductionTE`, or inside `TDiplomaticActionTE` and `TDiplomaticActionEventLog`. Original kinds
register in `ClassRegistry`, not at unit initialization, and the same tests check that every
original kind is registered and that a second registration throws. An unregistered id reads as
`UnknownOrder` or `UnknownProposal` and writes back unchanged. The loader assigns mod order and
proposal ids through the milestone 10.4 facade, and the internal registries take those assigned ids.
The view and kind registries build on CF-2's `Catalog`.

A registered kind holds only its own fields (roadmap X20). It drops a value that a file holds in a
field the kind does not use, so that value writes back as 0, which the zero rule omits. The census
fixed each kind's fields and found no such value in the 401 files it read, apart from the other
player, which it finds written for every kind and which stays in the common part of
`TDiplomaticAction` (3.7). X20 makes the converter's `$residue` the net for values outside the
census, with no residue expected on production items and diplomatic actions, so that a census miss
fails the gate.

What 9.6 assumes from 9.7:

1. Until 9.7 lands, the original format is the primary format and the adapters live in `ReadWrite`.
   Since 9.7 the own format is primary on disk, and the adapters stay in `ReadWrite`, since the
   engine image remains the engine's protocol (`own-formats.md` 6.1).
2. The converter keeps class ids as object identity and the property numbering inside itself.
   During the transition both the converter (its schema and case tables) and the transitional
   adapter in `ReadWrite` own that numeric legacy knowledge, until the exit of `own-formats.md`
   6.3 moves it into legacy codecs alone.
3. Its per-class mapping calls `ToFields` and the registries' `FromFields` unchanged. Brief 9.7-3
   adds the mappings of `TProductionItem` and `TDiplomaticAction` (`own-formats.md` 3.1). The 9.6
   briefs ran before it merged and left them out. 9.7-3 added them in version 1 of the format: a
   document names the kind as `$case` by its registration key, holds the kind's own fields, and an
   unregistered id reads as the case `Unknown`, which keeps the id and every value.
4. The port's own format names order and proposal types and writes only their fields. The census
   tells 9.7 which original fields carry no information.
5. The oracle compares converter output in byte mode, and the scenarios layer loads the shipped
   maps through the converter. 9.6 changes no check of the gate.

Nothing in the decisions is blocked only by the save or map layout. The numbered flat fields of
production items and diplomatic actions argued against their redesign, and section 3 designs both
for the converter boundary.

## 5. What stays bit for bit, and the proof

| invariant | proof |
|---|---|
| save and map bytes of production items and diplomatic actions | `persistence` (7,200 byte fixtures), `oracle` (177 saves, byte and model), `scenarios` (92 states, the shipped maps), per-kind byte tests against bytes recorded on main |
| network packet layout | token events serialize through the same `ReadWrite` behind a stand-in transport: the per-kind tests on `TProductionTE` and `TDiplomaticActionTE` |
| class ids, sizes, parents | the 9.4 `EClass` baseline test, unchanged |
| instance id sequence | `MsgArg` allocates no id and `THashItem.Key` keeps `IntOrRef`. `oracle`, and `frames` through the animation phase that reads `InstanceId` |
| which dialog, panel or action a kind gets | tests that the view virtuals return, for every concrete kind and every state the cascades test, the view of a table reviewed against today's cascades, and that `CombatMapName` equals the old switch, copied into the test, for every structure class in the `EClass` table |
| message delivery, `Format` argument order | code paths unchanged. `oracle`, `scenarios`, probes `format` and `strings` |
| frame pixels | `frames` 100 of 100, `language-frames` 7 of 7, `scenarios` 92 frames |
| allocation profile | `pacing` (300 frames at 33 ms) |

The 13 layers of `tools/check.py` keep their counts. A count that moves is a finding, not a pass.

## 6. Brief sequence

| brief | scope | done beyond O3 |
|---|---|---|
| 9.6-0 census and probe | private only. `cs15probe` gains the section 2 cases. `tools/rewrite/union-census` lists the property ids present per production and diplomacy kind in oracle saves, scenario states, shipped maps and campaigns. A throwaway instrumented build logs each message slot's case written and read through the oracle and scenario runs. Output `data/rewrite/9.6-census.json` and `.md` | the probe prints its line, the census covers every message slot and every kind |
| 9.6-1 `FormatArg` | `src/Rtl` (six files and new `FormatArg.cs`), five `Rtl.Tests` files, `AoWE.cs:103537` | `git grep -n "TVarRec\|VarKind" -- src` is empty, `StringFormatTests` cover every case and conversion |
| 9.6-2 structure views | new `Structure.Views.cs`, `AoWInfoWin.cs`, `aowMapEvents.cs` (double click), `AoWTC.cs` (the id switch), tests | no type test on a structure kind in the three screen methods, no switch on `EClass.ClassID`, the view and map name tests pass |
| 9.6-3 event log views | new `EventLog.Views.cs`, `aowMapEvents.cs`, `AoWE.cs` (`TProcessEventlogAIPA`), tests | both cascades hold no type test, the view tests pass |
| 9.6-4 `MsgArg`, buffers | `Engine.cs`, `HSEngine.cs`, `FixedArrays.cs`, new `src/Rtl/MsgArg.cs`, `CoreTests.cs`, the 59 Units message files, `AoWTC.cs` | `IntOrRef` remains only in `THashItem`, `Ptr.cs` and their tests, the overlap scan finds 3 structs |
| 9.6-5 `TShowInfo` | `ImageLib.cs`, 25 more Units files, `GraphicsConstructionTests.cs` | no `IntOrRef` in `ImageLib.cs`, the ILB round trip test passes |
| 9.6-6 production orders | new `AoWE.ProductionOrders.cs`, `AoWE.cs`, `City.cs`, `PrdPlace.cs`, `AoWCityScreen.cs`, `AoWInfoWin.cs`, `AoWProdScreen.cs`, tests | `FProductionID` and `TProductionItem.FParameter` appear nowhere, no switch over a production id or its city action outside the order classes, apart from the city screen's button table (`SelectAction`, the two button handlers and `ActivityImage(TCityAction)` in `AoWCityScreen.cs`, which select a button's panel and handler and include Raze, no order), kept for milestone 10 where a mod's new action needs a button (amended 2026-11-05 after 9.6-6), the byte tests pass |
| 9.6-7 diplomatic proposals | new `AoWE.DiplomaticProposals.cs`, `AoWE.cs`, `AoWE.Assembly.cs`, `AoWRelations.cs`, `E3EventDialogs.cs`, tests | no switch on `FActionType`, the byte tests pass |
| 9.6-8 documents | this document, `design.md` sections 1, 2, 6 and 9, `rtl-shim.md` 1.5 and 1.6, `transpiler.md` 4.3, `persistence.md` 5, `milestones.md` row 9.6 reworded to unions for closed data and virtual dispatch for kind families | the documents name the code as it is |

Overrides go into the new partial files, which keeps the view briefs out of the unit files 9.6-4
rewrites. 9.6-0 runs now, beside 9.4 and 9.5, since it changes nothing in the product. 9.6-1 and
9.6-2 touch disjoint files and run in parallel worktrees. 9.6-3 waits for both (`AoWE.cs`
with 9.6-1, `aowMapEvents.cs` with 9.6-2). 9.6-4 waits for 9.6-3 and the census. 9.6-5, 9.6-6 and
9.6-7 all edit `AoWE.cs`, and 9.6-5 also shares `AoWCityScreen.cs` and `AoWProdScreen.cs` with 9.6-6
and `E3EventDialogs.cs` with 9.6-7, so they go in that order. 9.6-8 comes last.

Dependencies. All product briefs start after CF-3 (the former 9.4-7), 9.5-1 and C10 merge.
CF-3 deletes the `Meta` conversion of `TVarRec`, and 9.4-3 and 9.4-4 edit `AoWE.cs`. 9.5-1 freezes `src/` and makes
`TVarRec.Str` a `string` and keeps `Chr` a `char`, with both `byte` and `char` Char-case
arguments, which `FormatArg` takes over. Delphi `Char` remains `byte` until a later pass.
9.6 readies screens and persistence for new kinds, and registering a kind from another assembly is the modding work's.

## 7. Risks and the evidence each brief needs beyond the gate

1. 9.6-0: the instrumented build leaks into the product. Evidence: the 9.6-4 diff has no logging.
2. 9.6-1: an argument changes case (a Delphi `Char` passed as a number, a `char` from 9.5).
   Evidence: `StringFormatTests` over every case and conversion character, including Char-case
   arguments from both `byte` and `char`, and the `format` probe.
3. 9.6-2 and 9.6-3: an override on the wrong class changes what a kind shows, or a decision moved
   out of a screen reads different state. Evidence: the view tests of section 5, a reflection test
   that prints each kind's resolved view for the report, the map name test over the `EClass` table,
   and review that the new partial files name no dialog or screen type.
4. 9.6-4: a reader used the integer view of a reference or read a slot of another kind. Evidence:
   no mixed slot in the census, `Int` throwing on a reference through the whole gate run, reviewed
   reader code for each message id the gate never reaches, and for the buffers the cited lines
   showing that each `GetCombatInfo` override writes only `TAbilityCombatInfo` fields.
5. 9.6-5: the cross-record read in `ShowEx`. Evidence: a test that reads every ILB in `Images` and
   `Int`, writes each image's 8-byte record back, compares with the file and counts `bmUser` images.
6. 9.6-6 and 9.6-7: stale fields, step-by-step construction, arms that read control state.
   Evidence: the census, a check through the gate run that `FromFields` then `ToFields` returns the
   triple read, the per-kind byte tests, a test per UI flow that edits an existing item
   (`AoWProdScreen.cs:615, 808, 1023`, the `E3EventDialogs` editor), and a report table pairing
   each old arm with its new method.
7. A switch over a union written as a statement escapes the check of section 2. Evidence: review.
8. Transpiler reference diffs keep the old shapes. Evidence: the transpiler layer passes, and the
   9.6-8 report records the divergence. `transpiler.md` 4.3 lists it, shape by shape.
9. The SDK is a release candidate. Evidence: the extended `cs15probe` runs first in every brief.

## 8. Result

The seven product briefs merged on 2026-11-05, each after a review at high effort and a full run
of the private gate:

| brief | PR | what holds |
|---|---|---|
| 9.6-1 `FormatArg` | #99 | `git grep -n "TVarRec\|VarKind" -- src` prints nothing. 180 new tests select every case by its conversion and meet every case with every conversion character |
| 9.6-2 structure views | #103 | no type test on a structure kind in the three screen methods, no switch on a class id. `StructureViewTests` replay the old cascades and the old switch over all 42 structure classes |
| 9.6-3 event log views | #104 | neither cascade holds a type test. `EventLogViewTests` replay the old cascade over all 44 event log classes |
| 9.6-4 `MsgArg`, buffers | #105 | `IntOrRef` remains only in `THashItem`, `Ptr.cs` and their tests. The overlap scan finds 3 structs (`TXYL`, `TPaletteEntry`, `TDefenseRequirements`) |
| 9.6-5 `TShowInfo` | #106 | no `IntOrRef` in `ImageLib.cs`. The ILB round trip compares 11,419 records in 538 libraries, all equal |
| 9.6-6 production orders | #107 | `git grep FProductionID -- src` finds only a test comment that names the old layout. The byte tests and the replayed switches pass |
| 9.6-7 diplomatic proposals | #108 | `git grep FActionType -- src` finds only a test comment that names the old layout. The byte tests and the replayed presentation switches pass |

The gate of main `a4bc2a3`, the last brief merged, passed all 13 layers: the product layer runs
3,595 tests (`Rtl.Tests` 339, `Units.Tests` 2,986, `Formats.Tests` 270) with zero build warnings
and `Strings check: 0 findings`, the oracle has 177 of 177 saves equal in byte and model modes,
frames 100 of 100 are pixel-identical, and the scenarios layer has 92 states and 92 frames equal.
The transpiler layer passes its 328 tests with the reference emitter's old shapes
(`transpiler.md` 4.3).

Two items of the plan were not needed. The 25 more Units files of 9.6-5 did not exist (3.3), and
the PChar case of `FormatArg` went because nothing passed a `Ptr<byte>` (3.1).
