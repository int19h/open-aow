# Own save and map formats (milestone 9.7)

State: implemented, 2026-11-06. Revised 2026-10-30 with `9.7-second-opinion.md` F1 to F12,
`9.7-roadmap-amendments.md` and the driver's transition decision (private `data/rewrite/`). The
review's product snapshot is `main` at `41dd67a7e3958181bda692c5fa50144bfa2e3e30`. Each brief
records its actual base. The census is reconciled after C10 before the game switches.

Implemented. The private census 9.7-0 ran on 2026-10-30. Brief 9.7-1 (formats library, converter
and `aowconv`) merged on 2026-11-05 (PR #94), the private oracle adapter 9.7-2 the same day, and
the game switch 9.7-3 on 2026-11-06 (PR #111, main `fb52e0c`). Brief 9.7-4 read this document
against the code of `fb52e0c` and the gate run on its tree: sections 1 to 6 name the code as it
is, with the deviations of the briefs written in place, section 7 records the briefs, section 8
stays the disposition of the second opinion, and section 9 holds the result and its evidence.

Covers `docs/milestones.md` row 9.7, `docs/design.md` sections 5 and 9 and
`docs/specs/persistence.md`. The user's decisions stand: canonical JSON as the
primary save and map format, always pretty printed for hand editing, map field
rows as arrays on single lines, original extensions, and conversion without
creating game objects. The original-format image ships as a transitional engine
backend. Section 6 defines its cost and exit, completed at the latest before any
persistence interface becomes stable mod API in milestone 10.

New milestone row. Work: "Versioned, canonical JSON saves and maps, written pretty
printed, original-format import and export, and the gate through a strict converter".
Done: "All in-scope corpus files satisfy G1, original-representable canonical
documents satisfy G2, own documents satisfy G-own, the game writes JSON for the
in-scope families, and O3 and the scenarios layer hold through strict export".
G1 compares CFS headers and inflated payloads, or exact non-CFS files, as section
3.3 defines. It does not promise identical outer compressed bytes.

## 1. Scope and evidence

### 1.1 File profiles

`GameFiles` takes an explicit profile from each caller. It selects the original
container, engine class table, allowed root family, header and checksum rules,
and document kind. An extension alone does not establish a profile.

Six original profiles exist (`FileProfile.All` in `src/Formats/Original/FileProfile.cs`), one
virtual class each, so a new profile joins without a switch. The kinds are the eight of the
table. A `.hsm` file names its profile through the root class id of its HSM header
(`GameFiles.ProfileFor`): a `TAoWHSMap` root is `world-hsm`, a `TAoWCombatMap` root is
`tactical-hsm`.

| family | extension and kind | profile | engine table and root | 9.7 status |
| --- | --- | --- | --- | --- |
| World saves | `.asg`, `save` | `world-hsm` | game, `TAoWHSMap`, HSM header, CFS or raw | Convert. The game writes JSON |
| Custom maps | `.hsm`, `map` | `world-hsm` | game, `TAoWHSMap`, HSM header, CFS or raw | Convert. The game has no map writer, since the editor is out of scope |
| Tactical maps | `.hsm` in `TCMaps`, `map` | `tactical-hsm` | tactical, `TAoWCombatMap`, HSM header, CFS or raw | Convert with tactical registration context |
| Campaign maps | `.csm`, `campaign-map` | `world-hsm` | game, `TAoWHSMap`, HSM header, CFS or raw | Convert |
| Campaign definitions | `.cam`, `campaign` | `campaign` | campaign, `TAoWCampaign`, stored object with class identity | Convert |
| Campaign saves | `.acg`, `campaign-save` | `campaign` | campaign, `TAoWCampaign`, with an optional embedded compressed HSM image | Convert. The game writes JSON |
| Scenario sets | `.hss`, `scenario-set` | `scenario-set` | game, `TAoWHSSet`, 52-byte HSS header, object body and trailing CRC-32 | Convert this file, not its external resources. `SaveHSS` takes the profile and has no caller in the product |
| Hero lists | `.ahl`, `hero-list` | `hero-list` | game, `THeroLibrary`, written object without class id | Convert. The game writes JSON |
| Item lists | `.ail`, `item-list` | `item-list` | game, `TItemLibrary`, written object without class id | Convert. The game writes JSON |
| Game-set resources | 12 shipped `.pfs` files | none | written objects, CRC-wrapped objects and separate `GENERAL.PFS` flag/string layout | Inventory only, original paths retained until milestone 10 |
| E-mail game index | `EmailIn/egames.pfs` | none | written object and external CRC bookkeeping | Inventory only, original path retained |

The census fixes exact root types, headers, checksums, read entries and write
entries from code. Unclassified entries block the switch. Redirecting generic
`ReadFromFile`, `WriteToFile` or CRC methods never implicitly converts excluded
PFS files. Their callers retain original byte behavior and rules CRC calculations.
In the code the game set loads through `ReadFromFileCRC` and `WriteToFileCRC`
(`AoWE.cs`), which take no profile and never reach the file boundary of 4.1.

ILB images, dictionaries, settings, network packets, localization exports and
other assets are outside 9.7. Editor and networking features retain their own
milestones. Existing in-scope writers emit JSON even without an editor. Preview
and listing understand both formats for in-scope profiles.

### 1.2 Code evidence

The numbered protocol includes conditional properties, dynamic ranges, records
and callbacks. At the review snapshot, `HSEngine.MapFields.cs:57-90` adapts dimensions
to bytes and writes fields sequentially in property 6. `Engine.cs:12531` reads
fixed byte counts for `rwData`. Some payload readers swallow exceptions, and
`rwChildren` places `csKeepIndex` children at stored slots. Validation precedes them.

`AoWE.cs:94472-94490` stores campaign map bytes and a size. `SaveCampaign` fills
them through `SaveHSMtoStream`. Nested compressed bytes belong to the outer
campaign's exact payload. `Engine.cs:14629-14651` and recovered
`Engine.pas:2185-2206` store `rwStrings` as byte length plus text block. The
count-of-prefixed-strings description in product persistence section 1.4 was stale until 9.7-4
corrected it.

The independent comparator is `tools/oracle/asg.py` `ModelReader`, backed by
recovery metadata rather than the product codec. Its public reader requires an
HSM header. Native load itself changes state, as persistence M1b records.
Conversion never invokes that load or constructs a game object.

### 1.3 Census and pinned corpus

The old baseline is 556 physical files: 23 custom maps, 41 campaign maps, 2 campaign
definitions, `Release/Release.hss`, 2 release hero lists, 4 saves, 59 tactical maps,
7 oracle inputs and 417 native autosaves under `D:/Build/oracle`. These are census
inputs, not verified completion evidence. The 417 native archive saves and 177
behavior-gate saves have separate provenance and counts.

9.7-0 writes `data/rewrite/9.7-0/census.json` and its Markdown summary `census.md`. It pins
source-relative identity, kind, profile, size, SHA-256 and provenance. Physical
files, nested objects and synthetic fixtures have separate counts. Excluded PFS
files and their callers are inventoried, not counted as conversion passes.
Verification never silently refreshes the accepted manifest from checked inputs.

Add coverage for missing families and encodings: empty and nonempty hero and item
lists, a nonempty campaign save, nested CFS, levels 0 to 3, raw HSM, HSS headers
and checksum failures, truncation, unknown classes and properties, present defaults,
stored nulls and every layout case of section 2.5. Synthetic fixtures supplement
original files with separate counts. No game data enters the product repository.

The accepted census pins 766 files: the 556 in scope, equal to the baseline (delta 0), and 210
inventory only (the 12 shipped PFS files and 198 rebuilt archive saves). By profile and kind the
556 are `world-hsm` 492 (`save` 428, `campaign-map` 41, `map` 23), `tactical-hsm` 59,
`campaign` 2, `hero-list` 2 and `scenario-set` 1. No original item list or campaign save exists in
the install or the oracle, so those families and the encodings above are synthetic fixtures of
`aowconv check` (`src/Convert/FixtureCases.cs`, 85 cases at the gate). The census was refreshed
over the product tree of each later brief with `census.py --refresh` and reviewed against the
accepted one. The last refresh is over tree `89a686b`, the tree of main `fb52e0c`, and its corpus
pins are unchanged.

Every persistence site has a rule or named exclusion. Inventory inheritance,
literal/computed ids, dynamic ranges, conditional calls, storage modes,
`OpenPropertyStream`, callbacks, raw byte counts, embedded class resolution, direct
table reads and all oracle file readers. Identify overlaps, floating-point fields
and opaque payload boundaries and reference content. A literal-id grep is insufficient.

## 2. The own format

### 2.1 Encoding and formatting

Own documents are UTF-8 without a BOM, LF endings, two-space indentation and one
final LF. Both writers always emit pretty printed, uncompressed JSON. Members
occupy separate lines and ordinary objects and arrays expand. Empty containers
are `{}` and `[]`. Readers accept bounded user-compressed gzip documents. No
writer emits gzip.

The only formatting exception is `Fields[y][x]`. Coordinates are zero-based,
rows increase in engine y order and elements in engine x order. There are exactly
`Height` rows of `Width` objects. Each entire row occupies one line, including
compact nested objects and sprite lists, with one space after colons and commas.
No new reference mechanism is introduced. The writer (`src/Formats/Json/JsonWriter.cs`) writes
any member named `Fields` that holds an array of arrays as rows, so a parsed document writes back
canonically without the decoder's layout marks.

Integers use invariant decimal without leading plus signs or zeroes. Integer
negative zero normalizes to `0`. Hex has lowercase digits and even length. Strings
escape quotes, backslashes and controls, using standard short escapes for
backspace, tab, LF, form feed and CR, and lowercase Unicode escapes for other
controls and U+007F to U+009F. Other valid characters remain literal UTF-8. Unpaired
surrogates fail. Floating-point payloads use exact bits as section 2.3 specifies.

### 2.2 Envelope, schema and names

```json
{
  "$format": "OpenAoW",
  "$version": 1,
  "$kind": "save",
  "$container": {
    "profile": "world-hsm",
    "compression": "cfs",
    "level": 2
  },
  "root": {
    "$class": "TAoWHSMap",
    "Day": 12,
    "DefendersSeed": 123456789,
    "InitialRandomSeed": 123456789
  }
}
```

This illustrates the envelope, not a complete playable save. Kinds come from
section 1.1. Both writers always emit `$container`. Its profile identifies container
and engine context. `compression` is `cfs` or `none`. `level` exists only for CFS
and is 0, 1, 2 or 3. Raw HSM and CFS level 0 remain distinct. New game HSM images
use CFS level 2. Unwrapped profiles use `none`.

Profiles specify further named header fields under `header`, with identical
presence rules in both writers. Fields read by previews or logic remain visible.
Reserved header bytes and noncanonical encodings use validated layout residue.
Derived checksums are recomputed, not independently editable. Import verifies them.
In version 1 an HSM header has `header.Reserved` only when its reserved integer is not 0,
and a scenario set has `header.Name`, the set name of up to 40 bytes. The HSS CRC-32 is
recomputed on export.

`src/Formats/schema.json` is embedded source. The census proposal is reviewed
against call sites before acceptance, then hand-owned. Classes record stable
names, parents and per-engine id mappings. Properties record names, legacy ids,
codecs, size rules, call conditions, storage modes, absence/default/presence rules,
record fields and canonical ordinals. Objects specify allowed families. Reference
properties and list/record elements name domains, null sentinels and missing-target
rules.

What version 1 holds: 970 classes, generated once by the private
`tools/formats/schema-build` from the 9.7-0 census proposal, the class baseline
`src/Units.Tests/EClassBaseline.json`, registration reachability and corpus order evidence, then
hand-owned. `python -B tools/formats/schema-build/regenerate.py` reproduces it. A class records
its name, class id, parent, the engine tables that enlist it (game, tactical, campaign) and,
for three classes whose corpus order differs from inherited `ReadWrite` order (`TAoWHSMap`,
`THero`, `TLeader`), an explicit order. A property records its name, id, codec and, where they
apply, its size, its object class or family, its callback payload, its dynamic id range and its
slot base. Absence needs no schema default: an absent member is an absent property in the engine
image, and the engine's reader applies its own default, constructor-backed `rwData` defaults
included. Version 1 annotates no reference domain in the schema. The unit validators of 2.7 name
the one domain the game checks (`player-slot`), and the census marks the other reference
candidates for review. Two classes carry case tables (2.3). `legacyShapes` holds the one named
legacy rule, `LEGACY_VALLEY01_SPRITE` (section 9).

`src/Formats/versions.json` pins the current version (1) with the schema's SHA-256, the codec
contracts (`property-table-1`, `cfs-1`, `hsm-header-1`, `hss-header-1`, `stored-object-1`,
`written-object-1`, `cp1252-frozen-1`), the mapping contracts (`DiplomaticProposal-1`,
`ProductionOrder-1`), `unitMappings`, the persisted classes whose game load needs a unit mapping
(`TAoWHSMap`, `TDiplomaticAction`, `TMapContainer`, `TProductionItem`), and `flatMappings`, the
kind classes that keep a flat mapping, empty in version 1. `SchemaTests` fails when the schema's
hash differs from the pinned one.

Initial names use fields without `F`, checked against recovery metadata for local
arguments, with declaring-class prefixes for collisions. Persisted class, member
and case names survive C# renames. Cases use explicit registration keys, not
`Type.Name`. Descriptors differ from per-engine enlistment. Resolve ids through
the profile's actual engine table and reject ambiguity. Completeness checks invoke
no creation delegates and demand no global id uniqueness across all descriptors.
Ordinary members use names. Opaque compatibility bytes and unknown-case payloads
retain numbers where lossless conversion requires them.

### 2.3 Values and presence

| original operation | document and rule |
| --- | --- |
| Integer, byte, word and shortint, including `Ex` | Integer with exact storage range. Version 1 declares no enum or reference domain in the schema |
| `rwBool` | `true` for presence. Explicit `false` normalizes to absence |
| `rwString` | String through frozen `Cp1252`. `IsMapped` checks every character. Strict input refuses unmapped characters with locations |
| `rwStrings` | Array of lines only when the block rule below reproduces exact bytes, otherwise `{ "$text": "..." }` |
| `rwStringsEx` | `lines` and signed 32-bit `objects` arrays of equal length. Noncanonical text uses `$text` plus `objects`, with decoded line count checked |
| `rwData` | Named fields with explicit layout and `$padding` for nonzero padding, otherwise a classified exact lowercase hex payload. Version 1 maps no record to named fields: every `rwData` value is exact hex, and `$padding` does not occur |
| Floating-point field | `{ "$bits": "..." }` with exact original little-endian bytes at declared width, retaining negative zero, infinities and NaN payloads. Version 1 has one, `TAIPlayerControl.DefenseStrengthMultiplier` |
| Overlapping record | One authoritative schema field set. Other views derive from it. Uninterpreted bits stay exact, never independently editable aliases |
| `rwStreamProc` | Typed arrays or codec-specific objects. Bit lists retain `default`, stored `count` and exact `bits`, without capacity rounding. Other identified payloads stay hex until a decoder exists |
| `rwEObject` | Object with fixed class from schema context, no editable `$class` |
| `rwCreateEObject` | Object with `$class`, `null` for stored class id 0, absent for absent property |
| `rwChildren` | `Children` in semantic traversal order. Every identity-bearing child slot has explicit `$slot` |
| Map property 6 | `Fields[y][x]`, fixed field class from the level profile |
| Dynamic sprites | `Sprites` in original table order with `$class` and explicit identity metadata where a dynamic id carries identity |
| Campaign map stream | `Map`, a nested document with `$container` and compressed-stream residue when nonempty. Empty and absent states remain distinct under the profile. `TAoWCampaign` ids 5 (size) and 6 (bytes) form the one member |
| Production item and diplomatic action | `$case`, the case name of the registered kind, then that case's fields. Values the case does not keep go to `$residue`. An unregistered key reads as the case `Unknown`, which keeps the discriminator as `Kind` and every value |
| Absent property | Absent member, with documented reader default at game load. A present default value stays present through conversion |

For `rwStrings`, an empty array encodes a zero-length block. A nonempty array
encodes each element followed by CR LF, including the last element. Elements
contain no CR, LF or NUL. An empty string element encodes one empty terminated
line. `$text` covers blocks not reproduced by this rule, including NULs and other
newline conventions. The prefix is a 32-bit byte length, not a line count.
`rwStringsEx` appends per-line 32-bit object values after that block. Its codec
retains all values and rejects inconsistent lengths. Fixtures distinguish absent
properties, empty blocks, empty lines and present defaults.

Unknown property ids, classes and ordinary members fail with class, path and
location. Duplicate JSON members and unknown reserved members fail too. Opaque
payloads are classifications, not an escape for unknown ids. Unknown production
and diplomatic cases have an explicit form retaining discriminator, values and
property presence. They never become known cases by dropping data.

`$residue` holds typed meaningful fields outside a mapped case, distinct from
encoding residue. Census-discovered values also remain in the 9.6 model, even
when a case does not use them. The pinned corpus expects zero unexpected
production or diplomatic case residues. New residue fails that expectation and
requires mapping review. Converter residue cannot preserve a value through game
load if the model discards it.

The 9.6 census found such a value only in the other player of a diplomatic action, which the
model keeps in the common part of `TDiplomaticAction` (`unions-and-kinds.md` 3.7), so no case
field carries it. A registered kind drops any other stray value on read (roadmap X20), so a
document whose `$residue` reaches the game loses it at the next save. The case tables live in
`schema.json` (`ProductionOrder`: keys 1, 2 and 10 to 14, `DiplomaticProposal`: kinds 0 to 9).
`CaseCodec` (`src/Formats/Original/CaseCodec.cs`) turns the stored members into the case and
back, and asks the unit side, `ProductionOrderCases` and `DiplomaticProposalCases` in
`src/Units/FormatMappings.cs`, what the kind writes back through `FromFields` and `ToFields`. A
document never keeps a case field the kind drops. An unknown case name, a field the case does
not keep and a registered key under `Unknown` are refused, and a mapped class whose unit side
is not registered fails with `E_MAPPING`. `aowconv check` counts the `$residue` members of every
imported document and finds 0.

### 2.4 Graph identity and canonical order

The stored graph is a tree of owned children, embedded and created objects. Shared
children retain existing skip rules. Original cross references stay integers,
annotated with domains (in version 1 by the unit validators of 2.7, not by the
schema). Instance and class-kind identities differ. Identity-bearing
slots are explicit, so deleting an item never renumbers survivors. The schema
marks other arrays whose position is identity. Duplicate slots and ids fail.

Preserve child and sprite order wherever iteration affects behavior. Do not sort
those arrays by slot for appearance. Unordered collections use stable schema keys
and reject duplicate keys. A new hash collection does not inherit arbitrary
runtime iteration order.

Reserved members have this total order wherever permitted by schema: `$format`,
`$version`, `$kind`, `$container`, `$mods`, `$class`, `$case`, `$slot`, `$ext`,
`$residue`, `$padding`, `$bits`, `$text`, `$layout`. `$mods` and `$ext` are reserved
for later versions, not accepted version 1 members. Envelope `root` follows
reserved members. Ordinary properties follow frozen schema ordinals, initially
inherited `ReadWrite` order with base fields first. Mapped record fields have their
own ordinals. Future extension maps sort keys by ordinal Unicode scalar value.
This is format ordering, not game-text collation.

Refactoring does not change JSON order. An order change requires a version and
migration. No timestamp, host path, tool version or process identity enters a
document. The same persisted state, including semantic order and retained residue,
writes the same bytes.

### 2.5 Encoding residue and edits

The default original encoding uses short entries then long entries, each in
declared legacy write order, data without gaps and short string prefixes below
255. `$layout` records differences needed for G1. Its versioned structure has
`codec`, `fingerprint` and `parts`. Parts have categories, bounded locations and
exact bytes or encoding parameters. Categories cover table form/order, value
prefixes, gaps, overlaps, header extensions and nested compression. Named record
padding remains `$padding`, not a duplicate editable value. The codec is
`property-table-1`, and the code names six categories (`src/Formats/Original/Layout.cs`):
`table-form-order` (a stored table that the writer does not make from the data order, which
also places overlapping values at their stored offsets), `gaps`, `value-prefixes`,
`value-encoding`, `header-extensions` and `nested-compression`.

The fingerprint is SHA-256 over the versioned decoded subtree and relevant profile
metadata, excluding encoding residue. Parts are untrusted. Validate bounds,
ownership and agreement with visible content before use. A fingerprint match
alone never authorizes arbitrary bytes. Reconstruct and verify that the image
decodes to the visible tree. No full-image shortcut overrides edited values.

Every nested compressed map retains its compressed bytes as a validated part,
even when otherwise canonical. This preserves exact bytes inside an uncompressed
campaign. Game-produced nested streams follow the same rule. Top-level CFS
compressed bytes are outside G1 and are not retained for it.

Content edits invalidate affected parts. Rebuild tables, lengths, checksums and
ancestor offsets from the edited tree. Preserve unaffected parts only after
validation. Normalization reports discarded encoding and retains semantic
`$residue`. Incompatible semantic residue after a class or case change fails
unless a named mapping resolves it. Stale residue never restores an old value.
Invalid or out-of-bounds residue is an error, not silent cleanup.

`normalize` preserves valid archival residue and reports invalidated encoding.
`to-original --exact-layout` refuses stale or missing parts needed for a claimed
exact reconstruction. Normal edited export rebuilds encoding and reports loss
of old encoding identity, not loss of game data.

Fixtures cover long prefixes for short strings, zero-size aliases such as
Tutorial's property 57, nonzero padding, header extensions, gaps, changed lengths,
removed properties, class/case changes and nested compression. The census records
categories and counts. Game-written documents are not promised to lack `$layout`.

The `aowconv check` fixtures cover the long prefix of a short string, Tutorial's property 57,
long table entries with a gap after a Boolean, a noncanonical table order, the HSM and HSS
header extensions, an edit that invalidates the layout and an export that rebuilds it,
`--exact-layout` refusing a stale layout, a part that does not fit, and a campaign save whose
nested map keeps its compressed bytes until an edit recompresses it and rebuilds its size.
Nonzero padding has no fixture, since version 1 maps no record (2.3). A class change is
`GameSwitchTests.AllowedClassChangeValidatesTheNewClassFields`. Over the pinned corpus the
residue is 3142 parts in 61 files: `table-form-order` 3136, `gaps` 5, `header-extensions` 1.
`tactical-hsm` holds 2999 parts in 52 files, because 52 of the 59 tactical maps store field
sprites with nonsequential dynamic ids that 1.36 ignores on read. `world-hsm` holds 142 parts in
8 files (the 7 prepared oracle inputs and one install map that writes `TAoWHSMap` id 31 after
id 30), and `scenario-set` 1 part in its one file. The check expectation file pins these counts.

### 2.6 Versioning and code page

`$version` is a positive integer. Writers emit the current one. Readers retain
historical decoder contracts, parse under the declared version, migrate through
pure tree transforms in `src/Formats/Migrations`, then validate the current result.
Newer versions fail with both numbers. Opening never rewrites an input file.

Changes to schema meaning, order or mapping behavior bump the version and supply
migrations. Cosmetic source changes do not. Additions use identity migrations only
when old presence and defaults retain their meaning. New required fields or
changed defaults need explicit transforms. Stable names do not follow C# names.

Migrations create no game object, execute no rules and read no clock, filesystem
or mod assembly. They preserve semantic residue and source provenance, and
validate or invalidate encoding residue. Historical golden documents cover whole
chains, renamed classes/cases, defaults and both residue types.

`versions.json` pins normalized schema hashes and mapping contract identifiers.
A test checks them against the current version. Golden binary/document pairs pin
behavior too, since unchanged mapping names do not prove unchanged code. Mapping
implementation changes receive format-change review.

Version 1 is the only version, so `MigrationChain.Default` holds no step and
`src/Formats/Migrations` holds the chain type only. `MigrationTests` run a test chain from
version 1 to 3 over the golden hero lists of `src/Formats.Tests/Golden`, check that every step
runs and names the values it synthesizes, and that a newer version fails with both numbers. The
production order and diplomatic proposal mappings landed inside 9.7-3, before any game wrote a
document, so they are part of version 1 and no flat-field migration exists.

9.7-1 uses `Aow.Rtl.Cp1252` from 9.5-P1b, including `IsMapped`, never `AnsiString`.
It does not wait for 9.5-1. The table is frozen from 9.7-1 onward. Fixtures cover
all 256 bytes, the five undefined-byte mappings, NULs and 9.5's binary-valued
strings. Strict JSON input refuses unmapped characters. Explicit lossy conversion
reports every replacement and is outside G2. This strengthens the roadmap's
report-only wording without changing 9.5 font/input normalization.

### 2.7 Validation before game load

Original import validates container and codec structure without game behavior.
Game load additionally validates the migrated tree against read-only content
context before destructive engine load steps. Formats owns structural validation.
Units supplies data-only family, ownership, reference and content validators.
The converter never constructs game objects.

| edit or inconsistency | game-load result |
| --- | --- |
| Remove unreferenced child | Accept if owner permits absence, retaining surviving identities |
| Remove referenced child | Refuse at referring value and name target, unless field explicitly permits absence |
| Unknown `$class` | Refuse with name and location |
| Known incompatible `$class` | Refuse against root, owner or property family |
| Allowed class change | Validate new inherited and own fields, never reinterpret old fields by numeric coincidence |
| Duplicate slot, id or JSON member | Refuse and locate both declarations |
| Dimensions or rows disagree | Refuse before allocation, including wrong row lengths and coordinates |
| Invalid storage range, enum or reference value | Refuse with applicable constraint |
| Remove property | Apply schema absence rule, including constructor-backed `rwData` defaults rather than assumed zero |
| Required child missing, invalid owner or dangling reference | Refuse before engine image loading |
| Unknown ordinary or reserved member | Refuse, including mistyped names |

Validation protects loading and execution invariants, not strategic sense. It
never guesses a repair. Legacy exceptions and compatibility normalization are
narrow named rules backed by original fixtures, not blanket integer exemptions.

What runs in version 1. Structural validation is the strict image encoding of
`GameFiles.ExportImage`: unknown members, classes and properties, incompatible families,
duplicate slots, dimensions and row lengths, storage ranges and mistyped reserved members fail
there. The game validators are `FormatMapping.Validators`, one `unit-content` validator over the
rules that `ClassRegistry` registers in its third block: `TAoWHSMap.CurrentPlayer` and
`TAoWHSMap.TurnOrder` name stored slots of the `player-slot` domain (`root.Players.Children`), a
`TAoWHSMap` needs its `TMapContainer` child, and a `TMapContainer` holds as many `TMapLevel`
children as its `LevelCount`. A nested document, a campaign's `Map`, validates in its own scope.
No other reference, ownership or content rule exists yet. `GameSwitchTests` load a shipped save
edited eleven ways (removed referenced child, unknown and incompatible class, duplicate slot,
row length, dimensions, storage range, unknown member, mistyped reserved member, missing
required child, dangling turn order) and check each refusal's code and location and that the
live map and the source file stay as they were. A removed unreferenced child is accepted, and a
removed `rwData` property keeps the constructor default.

Initial limits are 256 MiB decompressed bytes per file including embedded streams,
depth 128, 4 million decoded nodes, 1 million items per collection and 16 MiB
decoded bytes per string. Check arithmetic for sizes, offsets and dimension
products before allocation. 9.7-0 reports maxima. Any required limit adjustment
is explicit and fixed before 9.7-1 fixtures are accepted. Limit errors are located
and never yield partial loads.

Preflight validation and conversion errors leave the current game and source
file intact. Preflight finishes before existing routines clear the map or set.
Later rule-execution errors are distinct and never fall back to the original bytes.
This contract does not claim rollback of arbitrary game execution.

`LoadHSM`, `LoadHSMEx`, the previews, `LoadHSS`, `TAoWEngine.LoadCampaign` and
`AoWWinMan.LoadCampaign` keep the current state on a refusal. The file dialog's
`LoadSelectedGame` closes the current campaign and map only after the preview of the chosen
save passed its preflight, and for a campaign save it leaves closing to the campaign loaders
and starts only a newly loaded map. The dialog change has no unit test, since the dialog needs
the UI data module.

### 2.8 Diagnostics

Parsing retains spans for names, values and array items, including valid JSON
that later fails. Migrations carry provenance. Diagnostics include file, JSON
path, one-based line and column, stable code, rejected value and constraint.
Missing members point to owners. Dangling references point to the reference and
name its domain. Synthesized values name the migration and nearest original
owner location. Gzip locations explicitly refer to decompressed text. Binary
errors give byte offset and profile, never fabricated line numbers.

```text
Save/edited.asg:42:19 E_REFERENCE root.Players[1].LeaderId
Leader id 17 is absent from root.Leaders. Use an existing id or the documented null value.
```

That path illustrates the contract, not a fixed member. Game, CLI and logs share
a diagnostic type. Fixtures pin syntax, valid-JSON semantic and migrated-field
errors exactly.

The type is `FormatDiagnostic` (`src/Formats/Diagnostics.cs`), with 35 stable codes from
`E_SYNTAX` to `E_UNKNOWN_FORMAT`. Its text is `file:line:column CODE path` for a JSON location,
`file:0xoffset CODE path [profile]` for a binary one, with `(decompressed)` after the location
of an inflated or gunzipped input, then the message and the rejected value, the constraint
and the migration where they exist. A dangling reference reads, for example,
`The player-slot 1 is absent from root.Players.Children. Use an existing one.` The game logs
the same text in its file log (4.2), and a refused load raises `EGameFileError` with it.

### 2.9 Content detection

Keep original extensions. Classification yields own JSON, gzip own JSON, supported
original profile, malformed own input, unsupported version, unsupported kind or
unknown format. It never retries binary after a JSON error.

Readers accept one leading UTF-8 BOM and JSON whitespace, arbitrary member order,
then exactly one object with `$format: "OpenAoW"`. Comments, trailing commas,
duplicates and non-whitespace trailing content fail. Writers remove BOMs. A first
non-whitespace `{` identifies a JSON candidate even when truncated. Gzip magic
selects bounded decompression followed only by own-JSON parsing. Missing or wrong
markers, unsupported kinds and unsupported versions never fall back to binary.

CFS magic selects a validated original container. Unwrapped originals require a
matching profile. Extensions select candidates, but ambiguous headerless input
requires caller context or CLI `--kind` and `--profile`. Invalid candidates are
unknown or corrupt, not guessed. Kind, profile and root family must agree. G2
retains that same context.

### 2.10 Hand edits and canonicalization

These fragments illustrate normalization. Full fixtures carry schema context.
Editing canonical `"Day": 12` to `"Day": 13` changes only that value through the
document codec, apart from invalidated residue. Input members `DefendersSeed`
then `Day` return to schema order `Day` then `DefendersSeed`, with whitespace and
escapes normalized.

Removing slot 5 from identity-bearing children with slots 2, 5 and 9 leaves slots
2 and 9, not 0 and 1. References to slot 5 fail unless their policy permits absence.
This differs from JSON array positional indices.

An illustrative two-by-two field grid is:

```json
"Fields": [
  [{"Terrain": 1}, {"Terrain": 2}],
  [{"Terrain": 3}, {"Terrain": 4}]
]
```

Editing the second cell in row zero to `{"Terrain": 7}` rewrites that row line.
The other row stays identical. Removing a cell is a dimension error. `Terrain`
is illustrative, with actual names fixed by the census. Codec normalization is
idempotent. Native load changes and game actions prevent any promise of a minimal
diff after game load/save. Canonical ordering makes real changes visible.

## 3. Converter architecture and contracts

### 3.1 Projects and registration

- `src/Formats`, `Aow.Formats`, references only `Aow.Rtl`. It contains the versioned
  document model, profiles, schema, original codecs, transitional image adapter,
  validation interfaces, writer, migrations and `GameFiles`. Engine-image generation
  and original export are separate operations. Images containing port state are
  not original-game exports.
- Unit-dependent mappings live in `Aow.Units`, registered by persisted name in
  the third block of `ClassRegistry`'s one module initializer. Descriptors come
  first, kinds second, format mappings third, one registration call per mapped
  class in one form. Units references Formats with no reverse reference.
- `TProductionItem` maps to `Order` with stable `$case`, through
  `ProductionOrder.ToFields` and `FromFields`. `TDiplomaticAction` uses its proposal
  twins. They arrive with 9.6-6 and 9.6-7. Registration invokes no creation delegate
  and draws no random number or instance id. `ClassRegistryTests` checks every
  schema class requiring a unit mapping. In the code the item object itself carries
  `$case` and the case fields (2.3), and `FormatMapping.Register` in `ClassRegistry`'s third
  block registers four classes: `TAoWHSMap` and `TMapContainer` with content rules,
  `TProductionItem` and `TDiplomaticAction` with their case units. The test checks that the
  registered classes are exactly `unitMappings` of `versions.json`, that every rule names
  members of its schema class, that no `flatMappings` class has a line, and that every class
  with a case table has its case unit, which `CaseMappings` then holds for the converter.
- Units supplies data-only game validation. Built-in mappings run without engine
  object construction. Future mod fragments remain declarative, and conversion
  never loads mod assemblies.
- `src/Convert`, `Aow.Convert`, executable `aowconv`, references Units for built-in
  mapping registration. It calls `SetErrorMode` with the repository's required
  flags first, opens no window and plays no sound. New test hosts follow the same
  process rule.
- `src/Formats.Tests` holds codec and contract fixtures. Units tests cover real
  registrations, game validation and integration when 9.7-3 wires them.

9.7-1 needs no Units edit. It tests validation interfaces with data fixtures and
uses its accepted flat mappings. Unit mappings and the Units project reference
enter with integration. Later mappings bump the format version and migrate the
earlier flat documents.

9.7-3 wired the Units side. `Aow.Formats` references only `Aow.Rtl`, `Aow.Units` references
`Aow.Formats`, and `Aow.Convert` references `Aow.Rtl`, `Aow.Formats` and `Aow.Units`. `aowconv`
runs the `Aow.Units` module initializer before any command, so a mapped class never converts
without its mapping, and `check` runs the unit validators over every imported document. The
case mappings landed in version 1 with 9.7-3 (2.6).

### 3.2 CLI and safe output

```text
aowconv to-own <in> <out> [--kind <kind>] [--profile <profile>] [--overwrite]
aowconv to-original <in> <out> [--exact-layout] [--allow-lossy] [--overwrite]
aowconv to-own --dir <in> --out <dir> [--profiles <manifest>] [--overwrite]
aowconv to-original --dir <in> --out <dir> [--allow-lossy] [--overwrite]
aowconv normalize <in> <out> [--overwrite]
aowconv check <file or directory>... [--expect <census.json>]
aowconv info <file> [--kind <kind>] [--profile <profile>]
```

`to-original` is strict. It refuses unrepresentable state and names its paths
before writing. It never drops port properties, mod references or unmapped text
and reports success. `--allow-lossy` permits only schema-defined projections,
reports every dropped or replaced value and returns code 3 when loss occurs.
Essential state without a coherent defined projection still fails. The oracle
never uses this option. `--exact-layout` conflicts with lossy conversion and
requires the retained exact-layout claim to remain valid. It does not promise
outer zlib bytes excluded by G1.

`normalize` parses, migrates, validates structure and writes canonical JSON. It
reports encoding invalidation and retains semantic residue. It does not assert
playability without game-content context. `info` reports profile, kind, version,
representability, residue and validation coverage. `check` separates G1, eligible
G2, G-own and supported G3. Unavailable checks are reported with reasons, not passes.

Version 1 defines no schema projection, so `--allow-lossy` is accepted and never loses, and
exit code 3 does not occur. `check` prints the summary lines of 5.2, a `Validation:` line for
the unit validators, the pinned census files, per-profile counts, layout parts by category, the
legacy rules that fired, a `G3:` line that names the independent reader and the profiles
without one, and timed `Measure:` lines. With `--expect` and the check expectation file it
fails on any drift of the pinned layout counts. Section 9 quotes its output at the gate.

Exit codes: 0 complete success, 1 comparison difference, 2 error, 3 successful
explicit lossy conversion. Errors outrank differences, which outrank losses in
batch results. Enumeration is sorted, each file is converted once per direction
in one process, and every result is reported. Ambiguous directory inputs need a
profile manifest, never a guessed kind.

CLI output refuses existing destinations unless `--overwrite` is given. It
replaces only after complete valid serialization. Refuse input/output identity,
case aliases, overlapping directory trees and resolved symlink collisions.
Normalization writes a separate output, never edits its input in place. Batches
report completed and failed files without claiming a directory-wide transaction.

### 3.3 Guarantees

Let I(F, P) import original file F under profile context P, E(D, P) strictly export
document D, and N(D) parse, migrate and write canonical JSON preserving valid
residue. P includes kind and root context absent from headerless original files.

- G1: for every supported, structurally valid original F under P,
  `B(E(I(F, P), P)) = B(F)`. For CFS, B is the exact five-byte prefix plus inflated
  payload. For non-CFS, B is the entire file. There are no timer masks. Raw HSM
  and CFS level 0 differ. Unknown or malformed inputs are errors, not counted passes.
- Embedded compression is not recursively normalized by B. Validated nested
  compressed bytes reproduce the exact outer campaign. Editing a nested map
  invalidates that residue and recomputes sizes and outer checksums.
- G2: for every current-version canonical original-representable D,
  `I(E(D, P), P)` has exactly D's bytes under the same context. Both writers share
  metadata and residue rules. G2 excludes lossy export, obsolete versions before
  migration and hand edits before normalization.
- G-own: `N(N(D)) = N(D)` for every valid supported own document, including future
  port-only state. It never passes through original export. Game load/save is a
  separate behavior check because loading changes state.
- G3: the independent reader finds model equality for supported original families.
  Existing `asg.ModelReader` covers HSM world and tactical payloads with its
  registered schema. Other profiles explicitly report model checking unavailable
  until an independent reader exists. G1 still covers every in-scope profile.
  `aowconv` runs no G3. The gate runs it in the model mode of the oracle and scenarios
  comparisons (177 saves and 92 states, all `world-hsm` saves). `campaign`,
  `scenario-set`, `hero-list` and `item-list` have no independent reader, so G3 is unavailable
  for them (section 9).

The corpus and generated documents receive every eligible check. Unknown classes,
properties and invalid checksums fail rather than becoming untyped catch-all
residue. The census names supported legacy exceptions. Canonical documents accepted
as representable satisfy G2, including defaults, empty containers and nested images.

## 4. How the game uses the format

### 4.1 Preflight and the transitional backend

For in-scope profiles, `GameFiles` is the only file boundary. Original loads read
through `IRtlHost.Files`, import to a document, validate for game context, export
for unmasked G1 comparison, then produce the engine image. Own loads classify,
parse, migrate, validate and produce the engine image. No conversion step creates
game objects. Existing engine loading starts only after preflight succeeds.

G1 mismatch, unknown members and conversion failures are load errors. There is
no automatic original-byte fallback or recovery bypass in 9.7. Report the first
differing offset for reconstructed-original mismatches. Preflight failures preserve
the current game and source file.

Saves convert the engine-written image to a document, validate and finish
serialization before replacement. `ReadWrite`, binary streams and in-memory
images remain the engine protocol for 9.7. `LoadHSMfromStream` and
`SaveHSMtoStream` remain internal image operations for campaigns. Internal port
images do not pass for original exports, which undergo strict representability
checks separately.

In-scope callers of `LoadHSM`, `LoadHSMEx`, `CreatePreviewHSM`, `SaveHSM`, `LoadHSS`,
`RetrieveFromFile`, `StoreInFile`, `ReadFromFile`, `WriteToFile` and CRC forms take
profiles. Census coverage includes direct header listing such as `GetHSSetFiles`,
previews and nested campaigns. Excluded PFS callers retain their original paths.
New direct opens of in-scope files must enter this inventory and boundary.

In the code the boundary has two halves. `GameBoundary` in `src/Formats/GameBoundary.cs`
converts: `PrepareLoad` takes the file's bytes and a caller profile and returns the engine
image or the refusal, and `PrepareSave` imports the engine-written image, validates the
document, checks that the document's own image equals the engine's, and returns the canonical
bytes. `FileBoundary` in `src/Units/FileBoundary.cs` reads and writes through
`IRtlHost.Files`, runs the unit validators, logs and caches previews. A caller profile
(`GameProfile`) names the original profile, the kinds it accepts and the kind it writes:
`world-map` (`world-hsm`, kinds `save`, `map` and `campaign-map`, writes `save`),
`tactical-map`, `campaign` (writes `campaign-save`), `scenario-set`, `hero-list` and
`item-list`. `LoadHSM`, `LoadHSMEx`, `CreatePreviewHSM`, `LoadPreviewHSM`, `SaveHSM`, `LoadHSS`,
`SaveHSS`, `RetrieveFromFile`, `StoreInFile`, `ReadFromFile` and `WriteToFile` take a
`GameProfile` parameter, and every product caller passes one. `FileBoundary.Load` returns null
on a refusal and `LoadOrRaise` raises `EGameFileError` for the routines whose original raised on
a failed open. `GetHSSetFiles` reads its 52 header bytes through `FileBoundary.ListingHeader`.

The CRC forms run over the engine image, so an original and its own document give one CRC.
`FileBoundary.ImageCrc32` serves the map CRCs of the multiplayer setup (`AoWSetup.cs`
`ValidateMap` and `hPickMap`), and the host sends the engine image of its map, so the client's
CRC of the received stream agrees. `FileBoundary.ImageTailInt32` reads the stored CRC of
`RELEASE.HSS` for `TAoWHSSet.RetrieveHSSCRC`. The PFS CRCs keep their original routines.

### 4.2 User data and atomic replacement

Keep existing relative paths: `Save/`, `User/`, `Scenario/Custom/`,
`Scenario/Campaign/`, `TCMaps/`, `EmailIn/`, `EmailOut/` and `DEBUG/` where used.
The root is read only, the user layer shadows it by case-insensitive relative
path and listings merge. Opening, listing or previewing never copies shipped data.

Retain `%LOCALAPPDATA%/OpenAoW` on Windows, `$XDG_DATA_HOME/openaow` with the
existing XDG fallback on Linux, and `~/Library/Application Support/OpenAoW` on
macOS. Preserve host overrides and the oracle's single-directory mode. Resolve
casing, aliases and future mod layers through the file service, not a second
Formats lookup.

Original saves copied to the user layer load normally. Saving under the same
name replaces that user copy with JSON. State this in user-facing instructions.
Original-game files come from explicit export.

After serialization and validation, write a sibling temporary through the file
service, finish and close it, then replace atomically. Failure retains the old
save and cleans up the temporary. Add the replacement primitive and host support
to 9.7-3. Do not truncate before conversion. Inject serialization, write and replace
failures in tests, including replacement of original-format user saves.

In the code `FileBoundary.Replace` writes `<name>.openaow-tmp` through the file service,
flushes it to the disk (`FileStream.Flush(true)`), closes it and calls
`IFileSystem.ReplaceFile`, which moves the temporary over the target in one step. A file
service without the primitive returns false and the save is refused. `LayeredFileSystem`
resolves both paths in the user layer, so the move stays on one volume and a shipped file is
shadowed, never written (`platform.md` 7). `GameSwitchTests` inject a serialization, a write
and a replacement failure, replace an original-format user save with its document, shadow a
shipped file and refuse a file service without replacement. The user-facing statement that a
save under an original's name becomes JSON is not written yet (section 9).

Hosts gain `--file-log <path>`. Opens and writes record logical path, resolved
layer, profile, kind, detected format, version, G1 result or not-applicable reason,
validation outcome and error code. Physical paths belong in diagnostics/logs,
never documents. Failed preflight operations are logged too.

`OpenAoW` (`src/Game`) and the Headless host take `--file-log <path>` and write one JSON line per
open, preview, listing, CRC read and write (`FileLogRecord.ToJson`): `operation`, `path` (as the
engine names the file), `layer` (`root`, `user` or `single` in the layered file service), `profile`, `use` (the caller
profile), `kind`, `format`, `version`, `g1` (`equal`, `different at 0x...` or `not applicable:`
and the reason), `validation` (`passed`, `refused`, `not reached` or not applicable), `error`,
`diagnostic` and `cached`. A header listing of an original logs its open with G1 and validation
not applicable.

### 4.3 Previews and resource use

Measure size, latency and peak memory for the largest map, largest oracle save
and a nonempty campaign. Record the multiple live representations during load,
gate time before/after and pacing results. Conversion never runs per frame.

9.7-3 measures the 23-map scenario listing. Above one second on the recorded
machine, add a session preview cache and repeat the measurement. Identity includes
resolved layer, profile, schema version and content hash, or a change token proven
to detect every write and layer change. Path, length and write time alone do not
suffice. Without a reliable token, re-read and hash before reuse. Final load
validates actual bytes regardless of preview. Same-length hand edits and new
shadowing invalidate the cache. A corrupt preview reports that file and leaves
other entries usable.

The listing of the 23 maps of `Scenario/Custom` through the preview preflight took 3.2 s for
the originals and 3.3 to 3.5 s for their own documents on the recording machine (9.7-3, with
another implementer run on the same machine), so the session cache is in: `FileBoundary.Previews`,
a `PreviewCache` bounded by 64 MiB of image bytes that drops the oldest entry first. Its key is
the resolved layer, the path, the profile, the schema version and the SHA-256 of the bytes just
read. No change token is trusted, so every lookup reads and hashes the file. Only successful
preflights enter it, and previews, listings and CRC reads use it, never a final open. With the cache
the first pass took 3.8 s for originals and 2.5 s for own documents, the second 5 ms and 53 ms,
with 23 entries and a peak of 138 to 160 MiB. `PreviewCacheTests` cover the second preview, the
final open, a same-length hand edit, new shadowing, a forty-file listing, the byte bound and a
corrupt preview. Section 9 records the load and save measurements.

## 5. Gate and evidence

### 5.1 Independent oracle boundary

Native references remain untouched and go directly to Python readers. Only port
own documents undergo strict `aowconv to-original`, once in a batch per run
directory, into a separate output directory. A manifest links source path and
SHA-256, kind and profile to output path and SHA-256, recording zero losses.
Expected originals are never imported and re-exported for comparison.

Resolve artifacts once for every comparison and diagnostic reader. Inventory
`compare_saves`, `normalized_save`, `state_fields`, `compare_runs`' direct
`inflate` for `raw_equal`, `check_scenarios_75.py`'s direct source/state reads,
frame-state readers and all other save readers. Adapt only the actual own-document
side. Native scenario sources and expected states stay native. Never reuse stale
outputs without matching source hashes and current conversion contracts.

Keep `asg.ModelReader` independent of Formats. Retain coverage reports and require
zero unknown fields for promised model coverage. Opaque bytes have explicit entries.
Shared recovery metadata does not prove JSON naming. Add mapping anchors with
expected original ids and bytes reviewed independently of the product schema.
Edits to seeds, cases, strings, defaults, slots and map coordinates reach the
independently specified fields. Never generate expected values from the mapping
being tested. Check game behavior where relevant.

G1 uses unmasked B equality. Behavior byte comparison retains today's timer
normalization for root ids 0x37 and 0x3A and existing listed normalizations, all
reported. These are separate measures. No new mask hides port state or conversion
loss. Pixel comparisons stay independent of save conversion.

Before export, require every expected port file to exist as current own JSON,
with no port-only state in the unmodded oracle. Errors, unknown fields, loss,
skipped artifacts and incomplete runs fail. Negative checks change a seed,
remove a required child and introduce port-only state. They fail at comparison,
validation or strict export respectively, never vanish through normalization.

The private `tools/oracle/artifacts.py` (9.7-2) does this. It detects own documents by content,
stages them, runs one strict `aowconv to-original --dir` batch per run directory into
`<run>.export/original` and writes `<run>.export/manifest.json` with each source and output
SHA-256, kind, profile, version and zero losses, plus the SHA-256 of `aowconv.dll`,
`Aow.Formats.dll` and `Aow.Rtl.dll`. A later resolution reuses outputs only when every hash
matches. A refused batch writes `failure.json` and leaves nothing readable. `asg.inflate` and
`ModelReader` refuse an own document, so a reader that skips the resolution fails. Since 9.7-3
the gate passes `--require-own` in the oracle, frames, language-frames and scenarios layers.
Two of the negative checks differ from the plan above: the removed child is a map field, refused
at validation with `E_DIMENSIONS`, and the port-only state is refused while `aowconv` reads the
JSON (`E_UNKNOWN_MEMBER`), before export starts, since the version 1 schema has no port-feature
member. The mapping anchors take their ids and bytes from the engine's `ReadWrite` methods, the
case anchors included (a `Migrate` order, an `OfferPeace` proposal and an unknown kind).

Own-document resumption checks load actual own saves through the game. Compare
subsequent states against established native reload behavior, including known
reload effects. Where current references do not establish it, record a separate
native reload fixture under the harness rules. An uninterrupted native run does
not prove a reload. Keep existing resumption cases and add own cases with separate
counts. Codec G2 is not game resumption evidence.

`tools/oracle/own_resumption.py` (9.7-3) takes the native evidence the oracle already has: every
native run loaded its prepared input, an original save, and played on. Each of the seven cases
converts that input with `aowconv to-own`, checks that the strict export reproduces it under
unmasked G1, loads the own document through the Headless host, plays three turns and compares
the four day saves with the native run's in byte and model modes. A port-written own save loaded
in the middle of a run had no native evidence, because the native runs never reload. The native
reload fixture (`tools/oracle/README.md` in the private repository) was recorded on 2026-11-06:
the original game loads `small50-original-a/day-05.asg` and plays three turns, with the harness's
isolation and the registry restored. The port, loading its own day-05 document with the view size
of that native run (416 by 408), writes days 06 to 08 equal to it in byte and model modes. The
view size matters because the map view follows play and its scroll origin and player 0's last view
location are saved. The original takes its window state from the user's settings, so the harness
pins it for every native reference (issue #112, closed).

### 5.2 Summaries and assertions

The accepted census fixes N original physical files, S synthetic cases, R layout
parts and F files with residue, per-profile counts and supported G3 counts. The
old 556 is a baseline, not the final count. Explain additions individually and
check pinned hashes as well as counts.

```text
Corpus: N original files, N G1 equal, N imported documents G2 equal, 0 unknown properties
Fixtures: S cases, all expected results matched
Own: all eligible documents G-own equal, 0 strict export losses
Layout: R parts in F original files, 0 unexpected case residues
Schema: 0 unclassified sites, 0 registration ambiguities
```

Reports substitute actual command output for symbols and enumerate G3 checked
and unavailable families. Counts reduced by skipped inputs fail.

| layer | assertion after 9.7 |
| --- | --- |
| Product | Zero warnings. Formats tests cover codec pairs, canonical bytes, edits, diagnostics, limits, migrations and schema hashes. Units tests cover profiles, mappings and game validation |
| Persistence | Existing three summary lines retained, plus census-based summaries above. Unmasked G1 for every in-scope original, G2 for eligible documents, G-own for all own fixtures, explicit G3 coverage |
| Oracle | Seven scenarios, 177 of 177 saves are current own documents. Strict exports lose nothing and equal native saves in byte and model modes. All 177 satisfy G2 and G-own |
| Frames and language-frames | Existing frame counts and masks unchanged. Every in-scope actual state is own JSON, compared through strict export. Pixels remain independently compared |
| Scenarios | `Scenarios: PASS`, 23 shipped scenarios, 92 complete states equal in byte/model modes and 92 frames equal. Native source-map identity retained and every original open passes G1 |
| Integration | Zero fallback loads. Valid edits preserve identity. Invalid preflight leaves the live game intact. Failed writes preserve prior files. Hand edits and changed layers invalidate previews |
| Additional own checks | Separate exact counts for native-backed own resumptions, mapping anchors and negative adapter checks, without replacing or reducing the 177-save or 92-state measures |

`tools/formats/schema_check.py` verifies the entire section 1.3 inventory. Dynamic
and conditional sites name reviewed rules. Drift, unexplained size changes and
unclassified readers fail. Missing G3 support never reduces G1 coverage.

Section 9 gives the lines each layer printed at the gate. Two assertions of the table have no
separate line there: G2 and G-own of the 177 game-written oracle saves (the layer exports them
strictly and compares the exports), and the mapping anchors and negative checks, which run as
cases of `test_compare.py` and `test_model_compare.py` inside the oracle layer's test totals.

## 6. Transition cost, exit and adjacent specifications

### 6.1 What 9.7 retains

The driver chooses the binary-image bridge for 9.7. It bounds the initial engine
change and retains current adapters, but every own load/save still passes through
Delphi widths, property numbers, call order, record layout and stream behavior.
It also costs conversion time and memory. JSON alone removes none of these
internal constraints and does not make persisted `rwData` records plain.

9.6 adapters remain in `ReadWrite`, as 9.6-6 does for production orders.
`ToFields`, `FromFields` and proposal twins remain the original-kind bridge.
Documents name stable cases and their fields. Unknown cases retain original
discriminators and presence. A mapping merged after 9.7-3 adds a version and
migration from flat fields. Amend 9.6 assumption 2 to name both the converter
and transitional adapter as owners of numeric legacy knowledge.

The mappings merged inside 9.7-3, so version 1 holds them and no migration from flat fields
exists. 9.7-4 amended assumption 2 in `unions-and-kinds.md` 4 and the private
`9.6-spec-draft.md` 4. The cost is measurable: a load holds the file bytes,
the inflated payload, the JSON tree, the engine image, the engine's memory stream copy and
then the game objects, and a save adds the engine image, its JSON tree, the re-encoded image of
the check and the UTF-8 text (section 9).

A persisted array resize before retirement requires an original-size projection,
extra-entry storage, absence defaults, shrinking behavior and reference remapping.
Extra entries survive own save/reload. Token events sharing images need compatible
handling too. Essential state is never silently truncated for original export.
Strict export refuses it. This is the concrete cost behind X28 and issue #72.

### 6.2 Port state and mods

Property ids `0x40000000` to `0x4FFFFFFF` are port features. Mod property ids start
at `0x50000000`. Unmodded baseline gameplay writes neither range. The `rw*Ex`
zero rule omits extension state and rules identity when empty, so the unmodded
oracle needs no extension mask. Own documents name this state.
No code writes either range in 9.7, and the version 1 schema has no member there, so a
document member for one is an unknown member and refused.

The stronger F5 and F10 contract overrides roadmap amendment 4 and X15: strict
export refuses unrepresentable state rather than dropping it with a report or
masking it. Only explicit schema-defined lossy export drops values, reports every
loss and stays outside G2 and the gate. Reserved ids are not a claim that original
games accept them.

Future mod persistence adds qualified class, case and resource names under a
version bump. Original references remain integers. Domain annotations start in
9.7 and later cover names in properties, lists, records and payloads. Decode a
reference-bearing payload before allowing mod ids into it. The facade assigns
numeric ids and registers fragments. Mods never choose numbers or call internal
catalogs directly.

Future `$mods` entries are rules dependencies even when a mod only patched stats
and no object names it. Missing or incompatible gameplay dependencies refuse
normal loading. Any explicit recovery action is separate and reports changed
rules. Port migrations precede mod migrations in dependency/load order, with
stable names and source provenance. Conversion never executes mod assemblies.
These contracts supersede the weaker missing-mod rule in mods-design section 5.2.

PFS conversion remains milestone 10 game-set work, including CRC semantics and
non-property data. It is not already shipped by 9.7. The race lift adds port-owned
race data without altering shipped files, a stable race reference domain, and
array and bit-field widening before a 14th race is accepted. Fixed player count
is a scope choice, not a logical consequence of G1.

### 6.3 Exit from the Delphi image

Complete this exit at the latest before any persistence interface becomes stable
mod API in milestone 10. It is a dependency of that API and its SDK baseline,
not an unowned cleanup after milestone 10. 9.7 ships the transition and specifies
the exit, without claiming its implementation complete.

1. Define a typed persistence tree API with stable names, identity, presence,
   defaults, references and extensible records/lists. Give it a direct document
   backend. Renaming numeric `rw*` methods alone is insufficient.
2. Move original ids, widths, raw layouts, order and compression into legacy codec
   adapters over the tree. Replace raw persisted record copies with explicit
   field serialization. Retain unknown original values and semantic residue
   without imposing legacy layout on C# types.
3. Migrate engine serializers class by class. Move own load/save onto the direct
   backend, preserving construction and load semantics in the engine. Retain
   historical document decoders/migrations and independent original import/export.
   Conversion still creates no game objects.
4. Separate token-event transport from disk persistence where they share images.
   Keep explicit legacy packet adapters until networking changes them. Transport
   widths never silently limit new own state.
5. Finish migration before marking an interface stable. Prove extended arrays and
   richer cases beyond original layouts, lossless own reload, strict refusal of
   unrepresentable exports, historical migrations, G1/G2/G-own and the native gate.
   Inventory checks show no Delphi binary image on the own disk path and no raw
   legacy persistence requirement in the stable mod interface.

Milestone 10 planning allocates named implementation briefs for these steps before
publishing a persistence API baseline. Until then `ReadWrite`, numeric `rw*` and
raw `rwData` are transitional reachable APIs, not stable contracts. This replaces
mods-design's proposal to stabilize them and the roadmap's deferral of named
persistence until after milestone 10. 9.6 is not blocked by exit implementation.
Mod work that still uses the bridge carries the stated temporary costs.

None of the five steps is implemented. The private roadmap allocates them as briefs PE-1 to
PE-5 in its phase 8, before 10.4 publishes the API baseline, and `milestones.md` section 10
names them as that step's prerequisite.

### 6.4 Cross-document reconciliation

9.7-4 records these resolutions in the named documents. Its brief names exact
paths before editing them.

| document | resolution | recorded by 9.7-4 in |
| --- | --- | --- |
| 9.6 specification | Current brief numbers, stable case names, lossless unknown cases, meaningful unused values in the model, numeric knowledge in the transitional adapter as well as converter | private `data/rewrite/9.6-spec-draft.md` 4 and 6, product `unions-and-kinds.md` 4 |
| Construction form | Third `ClassRegistry` block and completeness checks. Descriptors before kinds before mappings. The mod facade owns allocation rather than mod module initializers | private `data/rewrite/construction-form.md` 2.4 and 2.6 |
| 9.5 specification | P1b supplies frozen `Cp1252` and `IsMapped`. Engine byte boundaries remain during transition. Strict JSON refusal does not change font/input normalization | private `data/rewrite/9.5-spec-draft.md` 2.7, product `strings.md` 2.7 |
| Mods design | PFS stays milestone 10 work. Reference annotations, qualified names, declared rules dependencies, strict export and the API exit prerequisite apply | private `data/rewrite/mods-design.md` 1, 4.3, 5.1 to 5.3 and 7 |
| Roadmap | Apply X12, X19, X24, split ranges and zero rule of X15, and C10 dependency. Strengthen export over X15, limit X28 adapters to transition, move exit before stable milestone 10 persistence API | private `data/rewrite/roadmap.md` X12, X15, X19, X24, X28, phases 1, 6 and 8, sections 3 and 4 |
| Product persistence | Correct `rwStrings`, specify profiles and G1 domains, distinguish JSON disk format from retained image, defer plain persisted records until explicit adapters exist | `persistence.md` 1 and 5 |
| Product testing | Independent Python model reads, untouched native references, strict actual-side export, exact corpus checks versus timer-normalized behavior, own resumptions | `testing.md` 3.2 |
| Product design, platform and milestones | Primary JSON for scoped families, PFS exclusion, safe user writes, extensions, export rules, transition cost and exit | `../design.md` 5 and 9, `platform.md` 7, `../milestones.md` row 9.7 and section 10 |

These stronger contracts resolve conflicts. Older wording in another document
never authorizes automatic loss, stable numeric persistence API or an oracle bypass.

## 7. Brief sequence and done measures

Each implementation brief records command output, file scope and actual base.
The commands here are future acceptance work, not evidence from revising this
specification. Product implementation briefs keep `python tools/check.py` passing,
build with zero warnings and run relevant tests. The 13 gate layers retain their
established counts, with new fixture counts reported separately. A changed
established count is a finding, not an automatic pass.

| brief | scope | done beyond the existing gate |
| --- | --- | --- |
| 9.7-0 census | Private `tools/formats/census.py`, schema draft, `9.7-census.json` and Markdown summary. Profile, site, oracle-reader, reference-domain, opaque-data and residue inventories, pinned corpus and fixture plan | `python -B tools/formats/census.py --out data/rewrite` reports baseline files by kind/hash, explains additions to 556, inventories 12 shipped PFS files and the e-mail list as excluded, lists zero unexplained sites and registration ambiguities, and identifies missing fixtures. Reports data maxima and model coverage |
| 9.7-1 formats and CLI | `src/Formats`, `src/Formats.Tests`, `src/Convert`, solution/project entries. No Units edit. Data-only validation interfaces, schema, profiles, codecs, source spans, diagnostics, residue, normalization, migrations and safe CLI output | `dotnet test src/Formats.Tests` passes. `aowconv check --expect data/rewrite/9.7-census.json` over corpus and fixtures prints section 5.2 with actual counts. All profiles pass G1, eligible documents pass G2 and all own fixtures pass G-own. Fixtures pin edits, locations, limits, nested compression and migration chains. Report size, latency and peak memory |
| 9.7-2 oracle adapter | Private oracle readers and tests from census, including `compare.py`, `check_scenarios_75.py`, `test_compare.py`, `test_model_compare.py` and frame-state readers, plus `tools/formats/schema_check.py` and `tools/check.py` | `python -B tools/oracle/test_compare.py` and `python -B tools/oracle/test_model_compare.py` pass with native/own pairs, independent anchors and negative cases. Reference hashes remain unchanged and exports lose nothing. `python tools/check.py` passes on unchanged product with native actuals still supported. Full own-output and game-resumption assertions activate at 9.7-3 |
| 9.7-3 game switch | Profiled Engine/HSEngine and census calls after C10, Units project reference and registration, content validation and tests, atomic replacement in file service and hosts, Headless/Game logs, preview measurement and any required cache. Private native reload fixtures and resumption checks where existing evidence is insufficient | `python tools/check.py` passes with section 5 assertions, 177 current own saves and 92 current own scenario states, zero fallback and zero losses. Valid-edit and every 2.7 refusal test passes before live mutation. Write-failure and cache tests pass. Own resumed play matches established native reload evidence, with separate counts |
| 9.7-4 documents | Product design 5/9, persistence 1/5, testing 3.2, platform 7, milestone row and report. Private adjacent specifications and roadmap rows in 6.4, with exact paths listed in the brief | All dispositions and cross-document resolutions describe implemented code and actual evidence. Reports carry command outputs, counts, supported/unavailable G3 families, PFS exclusion, transition cost and milestone 10 exit prerequisite |

Before 9.7-3, reconcile the early schema and inventory against main after C10 and
9.6-4. Record moved symbols and changed codecs while retaining persisted names.
Never silently regenerate accepted expectations. The reconciled inventory and
actual reference validators form part of 9.7-3 acceptance.

9.7-0 starts alongside other work because it changes no product code. 9.7-1 waits
for 9.7-0 and 9.5-P1b, and merges between whole-tree passes outside the 9.5 freeze.
9.7-2 starts when 9.7-1 has a working strict exporter and merges after it.

9.7-3 starts after 9.7-1, 9.7-2, CF-3 (former 9.4-7), 9.5-1, C10 and 9.6-4 merge.
CF-3 establishes the registry, 9.5-1 moves file entries to `string`, C10 finishes
file/member renames, and 9.6-4 finishes Engine/HSEngine message slots. Work beside
9.6-5 to 9.6-7 requires coordinated disjoint edits. The second merge rebases and
reruns affected checks. Registry and project edits also overlap, so the roadmap's
file-access-only overlap description does not replace inspection of those changes.

Order and proposal mappings land with whichever of 9.7-3, 9.6-6 and 9.6-7 lands
later. A mapping added after the switch carries a version bump and migration from
flat fields. 9.7-4 follows both 9.7-3 and 9.6-7.

How the briefs ran:

| brief | landed | notes |
| --- | --- | --- |
| 9.7-0 census | private `8a2f4d0`, 2026-10-30 | The tool is `tools/rewrite/formats/census.py` and its output `data/rewrite/9.7-0/` (`census.json`, `census.md`, `schema.json`). The corpus reproduced the 556-file baseline with delta 0. The census was refreshed with `--refresh` over the trees of 9.7-3, last over `89a686b` |
| 9.7-1 formats and CLI | PR #94, 2026-11-05, private side `23c9501` | The gate command is `aowconv check --expect data/rewrite/9.7-1/check-expect.json` (since 9.7-3 `check-expect.9.7-3.json`), an expectation file that names the census by path and SHA-256 and pins the layout counts. The schema generator is the private `tools/formats/schema-build` |
| 9.7-2 oracle adapter | private `0c3794c` and the review fixes `bb5b4cc`, 2026-11-05 | `artifacts.py` resolves own actuals through one strict batch per run directory (5.1). The gate kept native actuals until 9.7-3 |
| 9.7-3 game switch | PR #111, 2026-11-06, main `fb52e0c`, private `e933328` to `351deab` | The production order and diplomatic proposal mappings landed here, in version 1, after 9.6-6 and 9.6-7 merged. Review fixes A to H: the file dialog close order (2.7), the unit validators in `aowconv check`, the CRC forms over the engine image (4.1), the preview cache bound (4.3), the flush before the replacement (4.2), the logged header listing (4.2) and two tests of 2.7 |
| 9.7-4 documents | this revision | The report is section 9 |

## 8. Second opinion disposition

| finding | disposition | reason and applied contract |
| --- | --- | --- |
| F1 | Applied | Driver selects transitional image backend for 9.7. Section 6 states costs and a five-step exit before stable milestone 10 persistence API |
| F2 | Applied | Stable slots, structural/content validation, reference rules and preservation on preflight failure in 2.4 and 2.7 |
| F3 | Applied | Source spans, migration provenance and located valid-JSON diagnostics shared by CLI/game in 2.8 |
| F4 | Applied | Value/container residue, verified fingerprints, nested compressed bytes and edit invalidation without semantic loss in 2.5 |
| F5 | Applied | G1, representable G2, G-own and bounded G3 in 3.3. Both writers emit container metadata. Strict export overrides automatic drops |
| F6 | Applied | Pinned scope/corpus in section 1. PFS stays original until milestone 10. Missing profile fixtures supplement the 556 baseline |
| F7 | Applied | Census includes dynamic/conditional operations, real string codecs, raw layouts and per-engine identity |
| F8 | Applied | Row formatting, total order, exact encodings and honest normalization in section 2. Game reload is separate from codec idempotence |
| F9 | Applied | Damaged own input and unsupported versions never fall back. Section 4 removes automatic legacy bypass |
| F10 | Applied | Native references stay independent, all actual readers adapt, losses checked before export, anchors, negative checks and own resumptions in section 5 |
| F11 | Applied | Historical contracts and mapping behavior pinned. P1b is the one frozen encoder, strict refusal replaces report-only text loss |
| F12 | Applied | User roots/layers retained, atomic replacement, memory measurements and reliable cache invalidation in section 4 |

No finding is rejected. Collected roadmap amendments apply with the stronger
export and text-input rules above. The driver's exit deadline supersedes the
roadmap's later named-persistence deferral. Readiness means contracts and brief
dependencies are specified, not that implementation or gate evidence already exists.

## 9. Result

### 9.1 The gate

`python tools/check.py` passed all 13 layers on the tree of main `fb52e0c` (tree `89a686b`,
private gate log `tools/obj/check-3o2it0g3`, 2026-11-06, 1,260 s with 8 jobs). Its summary:

```text
product: PASS (3646 tests, 2497 platform assertions, 36 headless checks, sort dump 2994 lines, Strings check: 0 findings, zero build warnings)
transpiler: PASS (328 tests)
remaining-todos: PASS (remaining TODO checks: 53 passed, 0 failed, movement checks: 27 passed in 13 groups)
forms: PASS (253 tests, reference diff 81 of 81 files in forms-reference.diff)
localization: PASS (L1/L2: PASS, 6683 entries, 100245 lookup checks, 181410 assertions | Behavior: PASS, 1051 assertions across format variants, malformed files, context fallback, native restoration and language settings | Components: PASS, 66900 assertions for TIvDictionary, generated resource startup, object trees, shared form dictionary and reload)
probes: PASS (46 matched)
persistence: PASS (7200 byte fixtures, 200 resumptions, 12 structural/profile checks | Schema: 0 unclassified sites, 0 registration ambiguities (1152 sites, 980 property rules, 43 dynamic sites naming 7 reviewed rules, 110 conditional property sites with reviewed presence and absence rules) | 766 census pins | 306 oracle readers classified | Product sources: 0 of 638 pinned sources differ from D:\git\open-aow and 0 are absent, census base 89a686b | Corpus: 556 original files, 556 G1 equal, 556 imported documents G2 equal, 0 unknown properties | Fixtures: 85 cases, all expected results matched | Own: all eligible documents G-own equal (595 documents), 0 strict export losses | Layout: 3142 parts in 61 original files, 0 unexpected case residues | Validation: 556 imported documents pass the unit validators (unit-content, 4 mapped classes), 0 refused | Expect: layout parts, residue files, categories, per-profile residue, legacy rules and case residues match check-expect.9.7-3.json (b89a84c16793))
oracle: PASS (7 scenarios, 177/177 saves equal to original in byte and model modes, export: 177 own documents, 0 strict export losses, 0 native actual states, Own resumptions: 7 scenarios, 7 own inputs loaded, 28/28 following saves equal to native reload evidence, 0 losses, oracle tests: 48 run, 0 skipped)
frames: PASS (100/100 pixel-identical, zero masks, 61 states and 7 dialog saves equal, export: 68 own documents, 0 strict export losses, 0 native actual states)
audio: PASS (P5 audio: PASS, 214 WAV decoded, 24 songs rendered 5 seconds each to memory, song end and loop, Galaxy fades and samples, 4 muted SDL presets, title startup, missing library, 460 assertions)
pacing: PASS (300 frames, 32.895..33.108 ms, no catch-up after 70.218 ms)
language-frames: PASS (L3/L4, 7/7 frames pixel-identical, German complete map state equal, English Undead rendered, German glyphs present, export: 1 own documents, 0 strict export losses, 0 native actual states)
scenarios: PASS (Scenarios: PASS, 23 shipped maps started through menus, shipped titles and descriptions verified, human first player, 3 completed turns each, 92 complete states equal (both), 92 frames equal, zero masks | Scenarios export: 92 of 92 actual states own documents, 92 strict exports, 0 losses, 0 native actuals, 92 expected states and 23 source maps native)
```

The product layer's 3,646 tests are `Rtl.Tests` 338, `Units.Tests` 3,033 and `Formats.Tests` 275.
The persistence layer ran `schema_check.py`:

```text
Schema: 0 unclassified sites, 0 registration ambiguities (1152 sites, 980 property rules, 43 dynamic sites naming 7 reviewed rules, 110 conditional property sites with reviewed presence and absence rules)
Pins: 766 of 766 census files present with their pinned size and SHA-256 (556 in scope, 210 inventory only), census 22b21ce4ad56
Oracle readers: 306 call sites in 23 files, 0 unclassified or drifted (native-only 142, not-a-reader 8, original-only 30, pinned-native-product 3, resolved 28, test 95), 9.7-0 census listed 306
Product sources: 0 of 638 pinned sources differ from D:\git\open-aow and 0 are absent, census base 89a686b
schema_check: PASS
```

and `aowconv check --expect data/rewrite/9.7-1/check-expect.9.7-3.json`:

```text
Corpus: 556 original files, 556 G1 equal, 556 imported documents G2 equal, 0 unknown properties
Fixtures: 85 cases, all expected results matched
Own: all eligible documents G-own equal (595 documents), 0 strict export losses
Layout: 3142 parts in 61 original files, 0 unexpected case residues
Validation: 556 imported documents pass the unit validators (unit-content, 4 mapped classes), 0 refused
Schema: 0 unclassified sites (census 22b21ce4ad56), 0 registration ambiguities in 3 engine tables (measured at schema load, 970 classes, schema 5ecbbc56c94a)
Pinned: 556 of 556 census files present with their pinned SHA-256
Profile campaign: 2 files, 2 G1, 2 G2, 2 G-own, 0 layout parts in 0 files
Profile hero-list: 2 files, 2 G1, 2 G2, 2 G-own, 0 layout parts in 0 files
Profile scenario-set: 1 files, 1 G1, 1 G2, 1 G-own, 1 layout parts in 1 files
Profile tactical-hsm: 59 files, 59 G1, 59 G2, 59 G-own, 2999 layout parts in 52 files
Profile world-hsm: 492 files, 492 G1, 492 G2, 492 G-own, 142 layout parts in 8 files
Layout parts by category: gaps 5, header-extensions 1, table-form-order 3136
Legacy rules: LEGACY_VALLEY01_SPRITE 1 in 1 file(s) (install:Scenario/Campaign/Valley01.CSM)
G3: unavailable in aowconv. The independent reader is tools/oracle/asg.py ModelReader (world-hsm and tactical-hsm payloads), run by the 9.7-2 gate. Other profiles have no independent reader yet.
Measure: install:Release/Release.hss (scenario-set), 1060096 bytes, 1060096 payload bytes, own JSON 2222594 bytes, import 16 ms, export 33 ms
Measure: oracle:original/large/large-original-a/day-20.asg (world-hsm), 116037 bytes, 908397 payload bytes, own JSON 4127313 bytes, import 404 ms, export 162 ms
Measure: oracle:original/large/large-original-b/day-20.asg (world-hsm), 116037 bytes, 908397 payload bytes, own JSON 4127313 bytes, import 402 ms, export 163 ms
Expect: layout parts, residue files, categories, per-profile residue, legacy rules and case residues match check-expect.9.7-3.json (b89a84c16793)
```

The oracle layer's per-scenario lines are `Oracle small: 51/51 saves equal in byte and model modes,
list normalizations [], 51 own documents exported with 0 losses, 0 native actuals` and the same
with 21 of 21 for `large`, `underground`, `large_igor`, `heroes`, `magic` and `water`, then
`Oracle export: 177 of 177 actual saves own documents, 177 strict exports, 0 losses, 0 native
actuals`. `own_resumption.py` printed `Own resumption <scenario>: 4 day saves from an own input,
all equal to the native run in byte and model modes` for each of the seven, then `Own
resumptions: 7 scenarios, 7 own inputs loaded, 28/28 following saves equal to native reload
evidence, 0 losses`. `test_compare.py` ran 21 tests and `test_model_compare.py` 27, all OK.

What holds. Every in-scope corpus file, 556 of 556, round trips byte for byte under unmasked G1,
and every imported document satisfies G2. The game writes only own documents: 177 oracle saves,
92 scenario states, 68 frame states and dialog saves and 1 language-frames state are own JSON,
and their strict exports lose nothing and equal the native references. The boundary has no
fallback path (4.1), and no layer found a native actual state. The own resumptions are a
separate count: 7 own inputs loaded, 28 following saves equal.

### 9.2 G3 families

| profile | G3 |
| --- | --- |
| `world-hsm` | Supported. `ModelReader` reads the 177 oracle saves and the 92 scenario states in model mode, all equal |
| `tactical-hsm` | `ModelReader` reads tactical payloads, but no gate layer runs G3 over the 59 tactical maps. G1 and G2 cover them |
| `campaign` | Unavailable: no independent reader. G1, G2 and G-own cover the 2 campaign definitions and the campaign save fixtures |
| `scenario-set` | Unavailable: no independent reader. G1, G2 and G-own cover `Release.hss` |
| `hero-list` | Unavailable: no independent reader. G1, G2 and G-own cover the 2 release lists and the fixtures |
| `item-list` | Unavailable: no independent reader. No original exists, the fixtures cover the family |

### 9.3 Measurements

9.7-1 measured `aowconv info` in Release, one process per file, cold JIT:

| file | original | payload | own JSON | import | write | parse | export | peak working set |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| `Scenario/Campaign/Valley Highman.csm` (largest map) | 82330 | 781077 | 3037782 | 378 ms | 35 ms | 164 ms | 158 ms | 122.2 MiB |
| `Scenario/Custom/After the Fall.HSM` (largest custom map) | 78016 | 754710 | 2988622 | 381 ms | 36 ms | 193 ms | 131 ms | 134.1 MiB |
| oracle `large-original-a/day-20.asg` (largest save) | 116037 | 908392 | 4126756 | 421 ms | 46 ms | 203 ms | 149 ms | 149.4 MiB |
| `Keepers.CAM` with After the Fall as its map (`.acg` fixture) | 144292 | 144292 | 3355024 | 477 ms | 35 ms | 164 ms | 238 ms | 140.3 MiB |

9.7-3 measured the game's loads and saves through the boundary with the Release scratch tools,
on the same machine while another implementer run was active:

| file | original | image | own JSON | original load | own load | save | peak |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| `Valley Highman.csm` (largest map) | 82330 | 781077 | 3037782 | 502 ms | 308 ms | 539 ms | 186.3 MiB |
| `After the Fall.HSM` | 78016 | 754710 | 2988622 | 503 ms | 265 ms | 529 ms | 176.8 MiB |
| `large-original-a/day-20.asg` (largest oracle save) | 116037 | 908392 | 4126756 | 597 ms | 375 ms | 571 ms | 209.0 MiB |
| `Keepers.CAM` with After the Fall (`.acg`) | 146672 | 146672 | 3359784 | 680 ms | 409 ms | 649 ms | 230.3 MiB |
| `Release.hss` | 1060096 | 1060096 | 2222594 | 108 ms | 47 ms | 45 ms | 122.0 MiB |

A world map or save as an own document is 3.9 to 4.5 times the size of its engine image and 36
to 38 times the size of its compressed original. The listing and the preview cache are in 4.3, the live representations in
6.1. Pacing at the gate: 300 frames at 32.895 to 33.108 ms. The last gate before the switch with
13 layers passing (`tools/obj/check-418vf8mt`, 2026-11-05, native actuals) took 1,178 s and
paced at 32.988 to 33.273 ms. The switch gate took 1,260 s. Both gates ran 8 jobs on a machine
in use, so the difference is not attributed.

### 9.4 The Valley01 rule

Sprite 51 of the map field at x 74, y 91 on level 0 of `Scenario/Campaign/Valley01.CSM`
(inflated offset 0x41565) stores class id 132462, `TFlagMessageEventResource` in the 1.36 engine
table, with five short entries: id 1 (4 bytes), ids 3 and 4 (one byte each, 74 and 91, the field's
own coordinates) and ids 10 and 12 (one byte each, 0). It is the only resource-class sprite in the
install and oracle files. The shape is a map object image of an earlier build. 1.36 reads it
through `TMapObjectResource.ReadWrite` and never reads ids 10 and 12. Rejecting it makes the first
map of the shipped Valley campaign unloadable through the converter, so 9.7-1 accepted it under one
named rule, `LEGACY_VALLEY01_SPRITE` in `schema.json` `legacyShapes`: only for a
`TFlagMessageEventResource` stored as a `TMapField` sprite, the class decodes with the fixed legacy
shape `ResourceID` (id 1, i32), `LegacyXhx` (id 3, u8), `LegacyYhx` (id 4, u8), `LegacyProperty10`
(id 10, u8) and `LegacyProperty12` (id 12, u8). Any other id in that context is an unknown
property, and the class keeps its 1.36 shape elsewhere. Export writes the legacy shape back byte
for byte, so G1 holds and the engine reads the image the original reads. The check expectation
pins the rule to exactly one occurrence in that file.

### 9.5 What 9.7 leaves

- PFS. The 12 shipped `.pfs` game set files and `EmailIn/egames.pfs` keep the original format and
  their original paths through `ReadFromFileCRC` and `WriteToFileCRC`. They are inventoried by
  the census and excluded from conversion and from every count above. Their conversion, CRC
  semantics included, is milestone 10 work (step 10.1, 6.2).
- The transition cost (6.1). Every own load and save passes through the Delphi image:
  property numbers, widths, call order, record layouts and the stream code stay the engine's
  protocol, every `rwData` record stays exact hex, and conversion adds the time and memory of
section 9.3.
  A persisted array keeps its original size until an adapter of X28 gives its extra entries a
  port-features property.
- The exit (6.3). A typed persistence tree with a direct document backend, the legacy codecs as
  adapters, the serializers migrated class by class, the transport separated from disk
  persistence, and the proof. It is not implemented. It is a prerequisite of milestone 10: no
  persistence interface becomes stable mod API before it, and the private roadmap allocates it
  as briefs PE-1 to PE-5 before step 10.4 publishes the API baseline.

### 9.6 Open items

- The user-facing statement that a save under an original's name becomes JSON (4.2) is not in
  the product's `README.md`.
- `aowconv info` prints `Game content validation arrives with 9.7-3.` and runs no unit
  validator. `check` and the game run them.
- Version 1 maps no `rwData` record to named fields, annotates no reference domain in the schema
  and defines no lossy projection (2.2, 2.3, 3.2).
- The file dialog's close order (2.7) has no unit test.
