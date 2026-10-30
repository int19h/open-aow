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
| the four combat-info buffers | | removed, the second arm is never read |
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
    // cases: int, bool, char, double, string, TObject?, PointerArg (record struct over uint)
    private readonly long bits; private readonly object? reference; private readonly byte kind;
}
public static string Format(string format, params ReadOnlySpan<FormatArg> args);
```

The 335 `Format` lines compile unchanged, which the probe showed for collection expressions.
`FormatBuf` reads each argument with one switch expression. The `Class` case goes with 9.4-7, and
the `PChar` case stays only if 9.5 leaves a `Ptr<byte>` argument. Both a Delphi `Char` byte
and a string-element `char` select the Char case. The byte overload converts through
`Cp1252.Char`, as `TVarRec(byte)` does after 9.5-P1b.

### 3.2 MsgArg

```csharp
[Union] public readonly struct MsgArg : IUnion        // src/Rtl/MsgArg.cs, cases int and TObject?
{
    public int Int { get; }                           // the int, 0 when empty, EInvalidCast on a reference
    public T? As<T>() where T : class;                // the reference as T, null otherwise
}
```

The `Param` arrays become `[InlineArray]` structs of `MsgArg`, named by their role, each with
a named constant for the protocol arity. Never generate `FixedArray_MsgArg_N`. These are kept
protocol layouts, not content counts. `Payload` and `TMapFieldMsg.Result`
become `MsgArg`, and the `Engine`, `Obj` and `Value` accessors go. The new-turn pun is already an
int read (`AoWE.cs:69539, 95636`). What changes against `IntOrRef`: a reference no longer offers
its `InstanceId` as an integer, and `Int` on a reference throws, so a reader relying on that view
fails in the gate. The census of 9.6-0 lists every message slot with the case written and read.

### 3.3 TShowInfo

```csharp
[Union] public readonly struct BlendValue : IUnion { /* cases int (percentage) and TBlendTable? */ }
public struct TShowInfo { public TShowMode ShowMode; public TBlendMode BlendMode; public BlendValue Blend; }
```

`BlendMode` stays a field: it picks one of five tables for a percentage, and
`TLibraryImage.ShowEx` reads the image's own mode with the caller's payload (`transpiler.md` 4.3).
The `bmUser` arm matches `Blend is TBlendTable table`, the others read `Blend.Percentage`, which
throws on a table. A `bmUser` image read from a file carries an int and draws nothing, as today.

### 3.4 Event logs

`TEventLog` stays open. Two virtuals replace the two cascades over it, declared with every override
in a new `EventLog.Views.cs`:

```csharp
public sealed class EventView(string name) { public string Name => name; /* None, Acknowledge and 34 dialog views */ }
public partial class TEventLog
{
    public virtual EventView View() => EventView.None;     // what the map events screen shows
    public virtual void ExecuteAI(sbyte player) { }         // what an AI player does with it
}
```

`TMapEvents` keeps a static `Catalog<EventView, ...>` from view to renderer, filled with today's
dialog calls. Renderers take the screen as an argument (`Action<TMapEvents, TEventLog>`), so
registration needs no screen instance. `RegisterView` calls its `Add`, which rejects duplicate
keys. Original renderers register in `ClassRegistry`, and the mod facade adds mod renderers.
`TheMapExecuteEventLog` hides the transient windows as today and calls the renderer of `Event.View()`. `None` has no renderer, so nothing runs and `Done` is not called, as for
the 11 kinds the cascade skips. `Acknowledge` calls `Done(Event)`, the `TUserEventLog` arm. The
state the cascade consults (negotiation target, hero present in army info and prisoners, seated
player at game over, player magic mode) is read by the overrides with the same queries at the same
moment. `TProcessEventlogAIPA` calls `Log.ExecuteAI(player)`, overridden by `THeroEmergeEventLog`
and `TDiplomaticActionEventLog`.

Arm order matters only where an earlier arm catches a subclass. No arm tests a base before one of
its subclasses, so the first match is the nearest handled ancestor, which is what a virtual
resolves to. `TEnterCaveEventLog`, `TPlayerConnectionEventLog`, `TRazeStructureEventLog` and
`TRestartGameEventLog` inherit the `TMessageEventLog` override, `TTutorialMessageEventLog` and
`TFlagMessageEventLog` override `THelpMessageEventLog`'s, and `TJoinEventLog`,
`TProductionEventLog` and the `TTextEventLog` line keep `None` beneath overriding subclasses.

### 3.5 Structures

Four dispatch sites become virtuals on `TStructure`, declared with every override in a new
`Structure.Views.cs`. Keys are `StructureView` values, the open pattern of `EventView`, and the
info window and the map screen keep static `Catalog<StructureView, ...>` renderer tables.
Renderers take their screen as an argument, registration calls `Add`, and original entries live
in `ClassRegistry`. Both view briefs build on CF-2's `Catalog`.

| virtual | replaces | returns |
|---|---|---|
| `InfoPanel(sbyte viewer)` | `ShowStructureInfo` | the panel key, `Generic` by default. The ownership, razed and war-mode tests move into `TPlayerStructure` and `TCity` |
| `InfoButton(sbyte viewer)` | `MainBtnClick` | the screen to open (city, production place), or `None` after running the kind's own action (cave, teleport, exploration site, tower, arena, altar) |
| `DoubleClick(sbyte viewer)` | `TheMapStructureDblClick` | the same, with today's ownership and `CanDblClick` gates |
| `CombatMapName()` | the class id switch | the tactical map file stem |

The class id switch matches exact ids: a subclass with an id of its own falls to the default arm,
while a virtual is inherited. The brief lists from the `EClass` table every such subclass below an
overriding class and gives it the default back. The cascades are checked for subclass capture the
same way as the event logs.

### 3.6 Production orders

```csharp
public abstract record ProductionOrder                                   // AoWE.ProductionOrders.cs
{
    public abstract (int Id, int Parameter, int GroupId) ToFields();
    public static ProductionOrder FromFields(int id, int parameter, int groupId);  // registry, else UnknownOrder
    internal static void Register(int id, Func<int, int, int, ProductionOrder> create);
    public virtual string Name(TProductionControl control);              // also NameProgressive, CanExecute,
    public virtual bool Execute(TProductionControl control);             // Loopable, LocksQueue, CityAction, ActivityImage
}
public sealed record ProduceUnit(int Index, int GroupId) : ProductionOrder { /* overrides */ }
// ResearchSpell (2), Trade (10), Upgrade (11), Fortify (12), Migrate (13), Loot (14), UnknownOrder
```

The kinds are open, so the shape is an open abstract record carrying the behavior. What varies by
control (city or production place) is already virtual on `TProductionControl`, so each arm of the
switches over the id or its mapped `TCityAction` moves into its order class and calls the control's
existing members. The screens take names and images from the order. The order registry uses
`Catalog<int, ...>`, whose `Add` rejects duplicate ids and whose `TryGet` serves `FromFields`.
`TProductionItem` keeps its node identity and holds `Order` in place of the three fields. Finding
an equal item
(`AoWE.cs:1406`) is record equality.

### 3.7 Diplomatic proposals

`TDiplomaticAction` keeps its class id and the common part (day, source, target, reaction,
escrow, event log id) and holds an open `DiplomaticProposal`, the pattern of 3.6 keyed by the
one-byte kind with a `Catalog<byte, ...>`. Its internal registration takes the loader-assigned
id for a mod and rejects duplicates. The ten kinds hold their own fields (other player, gold and
mana, message) and override what the 14 switches do today: description text, validity before a reaction, the AI's
acceptance chance, the effect of acceptance. No subclass of `TDiplomaticAction` per kind, because
the original format names one class id for every kind. `E3EventDialogs` builds the proposal when
the player confirms. A field the census finds set for a kind that does not hold it moves into the
common part.

## 4. Persistence and the converter boundary

The layout of the files (property numbers, offsets, the one-byte kind) binds only the code that
reads and writes the original format. Their information content binds the model: the oracle
compares the converter's output byte for byte, so the model holds every value the original writes,
including one left in a field its kind does not use. The census fixes each case's fields from the
saves, maps and gate runs.

| family | boundary | adapter |
|---|---|---|
| `FormatArg`, `MsgArg` | none | never streamed or sent. 9.6-4 adds a check that no `rw*`, `Read` or `Write` call takes a message record |
| `TShowInfo` | ILB image, 8 bytes | `ReadImage` and `WriteImage` stay in the struct: mode bytes at 0 and 1, padding at 2, the int at 4. Reading yields the int case. A table writes 0 where the original wrote a process address no reader uses. ILB files are game data, outside 9.7 |
| event logs, structures | none | virtuals add no state, class ids unchanged |
| `ProductionOrder` | `TProductionItem.ReadWrite` ids 20 to 22, in saves, maps and `TProductionTE` | below |
| `DiplomaticProposal` | `TDiplomaticAction.ReadWrite` ids 22 to 25 and 27, in saves and `TDiplomaticActionTE` | the same pattern, the kind as the one-byte `rwData` at id 25 |

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

Calls, ids, order and the zero rule stay as today. Original kinds register in `ClassRegistry`,
not at unit initialization. An unregistered order id reads as `UnknownOrder` and writes back
unchanged. The loader assigns mod order and proposal ids through the milestone 10.4 facade.
Internal registries take those assigned ids. CF-3's `ClassRegistryTests` checks original kinds
for completeness. The view and kind registries build on CF-2's `Catalog`.

What 9.6 assumes from 9.7:

1. Until 9.7 lands, the original format is the primary format and the adapters live in `ReadWrite`.
2. The converter keeps class ids as object identity and the property numbering inside itself.
3. Its per-class mapping calls `ToFields` and the registries' `FromFields` unchanged.
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
| frame pixels | `frames` 100 of 100, `language-frames` 6 of 6, `scenarios` 92 frames |
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
| 9.6-6 production orders | new `AoWE.ProductionOrders.cs`, `AoWE.cs`, `City.cs`, `PrdPlace.cs`, `AoWCityScreen.cs`, `AoWInfoWin.cs`, `AoWProdScreen.cs`, tests | `FProductionID` and `FParameter` appear nowhere, no switch over a production id or its city action outside the order classes, the byte tests pass |
| 9.6-7 diplomatic proposals | new `AoWE.DiplomaticProposals.cs`, `AoWE.cs`, `AoWE.Assembly.cs`, `AoWRelations.cs`, `E3EventDialogs.cs`, tests | no switch on `FActionType`, the byte tests pass |
| 9.6-8 documents | `design.md` sections 1, 2, 6 and 9, `rtl-shim.md` 1.5 and 1.6, `transpiler.md` 4.3, `persistence.md` 5, `milestones.md` row 9.6 reworded to unions for closed data and virtual dispatch for kind families | the documents name the code as it is |

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
   9.6-8 report records the divergence.
9. The SDK is a release candidate. Evidence: the extended `cs15probe` runs first in every brief.
