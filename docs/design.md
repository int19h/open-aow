# OpenAoW design

OpenAoW is a source port of Age of Wonders (1999, v1.36) to modern desktop platforms, with mobile
as a later goal. It uses the original game files and ships none of them. The source comes from the
recovery project in the `aow` repository (upstream `C:\git\int19h\aow`, the user's working copy
`D:\git\aow`, which the port reads). That repository
holds Delphi 3 source that rebuilds the shipped game with the original compiler. The rebuilt game is
the oracle: the reference whose behavior the port must reproduce.

This document records the decisions made so far and the questions that are still open. It is a
living document. Update it when a decision changes.

The port has two stages with two styles of code. The first stage translates the recovered source
mechanically, by a tool, so that no implementer spends time on a rewrite a program can do, and
proves the result against the oracle. The second stage rewrites that code into idiomatic C#, one
piece at a time, with the oracle as the safety net. Section 3 covers the first stage and section 9
the second. Everything the first stage builds for its own needs is temporary.

## 1. What the source looks like

The recovered source is about 340,000 lines of Pascal in 16 packages. The numbers below come from a
survey of `D:\git\aow\src\delphi3` on 2026-10-24. They are approximate.

| area | packages | lines | role |
|---|---|---|---|
| object model and persistence | Enginep (Engine, EList, FMemStr, zlib, PlugInM) | 15,000 | base classes, property tables, storage streams, the save container |
| map engine | HSEPack | 13,000 | hex and isometric map, terrain, roads, bridges, map editing |
| game rules | AoWEPACK | 100,000 | units, cities, spells, abilities, events, AI, the campaign |
| tactical combat | AoWTCPCK | 12,000 | the combat map and its rules |
| game UI toolkit | aowInt | 11,000 | self-drawn windows, menus, lists and buttons on the game's own surfaces |
| game tools | AOWTools | 1,300 | the book window, turn button, start menu |
| graphics | Gfxepack, Dcpack, Adcpack | 16,000 | DirectDraw surfaces, software blitters, display control |
| image library | Ilpack | 14,000 | the ILB image format, sprites, RLE sprites, image editors |
| sound, video, network | SoundP, VideoP, Network, aowDPlay, aowFXpck | 8,000 | DirectSound, Galaxy music, Video for Windows, DirectPlay |
| programs | AoW.exe, AoWEd.exe | 42,000 | the game's screens and host window, the map editor's forms |

Facts that shape the port:

- The rules packages are plain object code. There are 1,785 classes over all modules. The rules use
  1,790 published properties, which drive the engine's own `ReadWrite` persistence and the editor's
  property grids, not Delphi form streaming.
- The platform layer is small and isolated. DirectDraw appears only in `GFXE.pas` and `DisplayC.pas`.
  Inline assembly is 330 routines: 39 blitters in Gfxepack and Ilpack, and 291 small stubs,
  mostly call thunks and constant returns in AoWEPACK, Engine and HSEPack that the recovery wrote to
  reproduce the original code (`specs/transpiler.md` 8.2).
- The in-game UI (aowInt) draws itself onto the game's surfaces. It does not use the VCL (Delphi's
  Windows GUI framework) for drawing or input. The game's 77 screens in AoW.exe are `DataModule` (Delphi `TDataModule`)
  containers that hold aowInt components, laid out in form files (`data/forms/AoW`, 79 files,
  89,000 lines, 1,588 event handlers). AoW.exe has exactly one VCL form, `MainForm` (Delphi `TMainForm`), which hosts
  the display. Section 3 has the boundary in detail.
- The VCL leaks into the rules in a shallow way. `AoWE.pas` imports Forms and Dialogs. The rules
  packages call `ShowMessage` 91 times and `MessageBox` 37 times.
- Floating point is present in the rules. AoWEPACK has 56 uses of floating point types (AI, AI
  groups, combat, cities, global target spells) and AoWTCPCK has 10. The rules call `Sin`, `Cos`,
  `Power`, `Sqrt`, `Ln` and `ArcTan`. `Random` is called 428 times in the rules.
- Three third-party packages (Multilizer localization, Toolbar97, an internet suite) are out of
  scope for recovery. The port replaces them.
- Language features in use: 30 virtual class methods, 761 `with` statements, eight variant records
  (`data/records/records.md`, section 2), 27 interfaces, method pointers for events, sets, pointers
  and PChar in the graphics code.

Game data on disk: `Images/` and `Int/` hold ILB image libraries. `Songs/` holds Impulse Tracker
modules (`.IT`) that `Galaxy.dll` plays. `Sfx/` holds sound effects. `Dict/` holds Multilizer
dictionaries (`.mld`). `Release/` and `Scenario/` hold maps and campaign data. `Save/` holds saves.

## 2. Language: C#

The port is written in C# 15 on .NET 11. SDK 11.0.100-rc.3 is installed since 2026-10-25 and the
release is weeks away. C# 15 is the default language version for `net11.0` projects, and the
features the port uses (`union` types, collection expression arguments, labeled `break` and
`continue`, extension indexers, static abstract interface members) need no preview flag.
`tests/toolchain/cs15probe` (private repository, section 8) compiles and runs each of them, and
`closed` hierarchies besides, and is the first thing to run after an SDK update. The port declares
no `closed` type (section 9). The memory safety preview is not used.

Why C#. It maps onto Delphi 3 more closely than any other broadly supported language. Classes,
virtual methods, properties, interfaces and `try/finally/except` map directly. Method pointers
become delegates. Sets become a bitset struct. Records become value types with `ref` access where
Delphi passed them by reference. The RTTI-driven persistence is explicit code in the source and
stays explicit. It runs on Windows, macOS, Linux, Android and iOS. It has a large contributor pool
and the strongest modding ecosystem of any managed language.

Two styles. Generated code (section 3) is a calque of the Delphi: 1-based byte strings, `Ptr<T>`
over arrays, metaclass objects, split constructors, `with` blocks as scoped locals. That is accepted
because it is temporary, with one rule: the scaffolding it needs lives in shim types, base classes
and extension methods, never as a pattern woven through the game logic, so that a later rewrite of
a method replaces the method and leaves nothing behind. Hand-written code, which is the rewritten
layers of section 3, every override and every refactoring pass of section 9, uses the current
language: `record` and `record struct`, pattern matching and `switch` expressions, collection
expressions and spreads, string interpolation, `List<T>` and `Dictionary<K, V>`, `Span<T>`, `ref`
locals and returns, inline arrays and `ref struct` where performance asks for them, `union` types
for data closed by nature, virtual methods on the kind for behavior that varies by kind (section
9), and generics with static abstract interface members where the Delphi code dispatched on class
references.

Alternatives considered:

- Free Pascal. The lowest-risk route to a running cross-platform build: the recovered source
  compiles in Delphi mode already, and the diff against the oracle stays trivial. Rejected for the
  stated goals of moddability and a long-lived community project, which want a modern language and
  tooling. Free Pascal stays useful as a second compiler during the port.
- Rust and C++. Rejected: ownership and manual memory management map poorly onto a managed object
  graph of 1,785 classes.
- Java and Kotlin. Rejected: no structs, no unsafe memory, no sets, weaker game tooling.

Two Delphi features need a design, not a mapping:

1. Class references (`class of T`) with virtual class methods. Questions about a run-time class
   (`is`, `ClassName`, equality, `InheritsFrom`) use `System.Type`. Dispatch on a class (virtual
   class methods, virtual constructors through a class reference, 15 `class of` types) uses a
   metaclass object that exists only for the classes that need one: 1,014 of 1,307 under the
   survey of 2026-10-25, almost all through `EngineObject.ClassId` (Delphi `TEObject`), with nothing emitted per class
   beyond the class methods the source declares (`specs/transpiler.md` 4.4). In Delphi a virtual
   class method called on an instance dispatches on the instance's run-time class, which the
   survey found at 222 sites, so instances reach their metaclass through `ClassType`. In the second
   stage the metaclass objects give way to generics with static abstract interface members or to
   the engine's own class registry.
2. Records and variant records. Records are structs (value semantics as in Delphi, `ref` where
   Delphi passed by reference). The survey of 2026-10-25 (`data/records/`) found 155 record types
   and 51 address-taking sites on 20 of them: 17 are synchronous aliases that live for one call
   and become cells, the rest are array elements, allocated blocks and views, each with a
   mechanical mapping (`specs/transpiler.md` 4.3). Of the eight variant records, six are byte
   unions and became explicit-layout structs in the first stage. `EngineMessage` (Delphi `TEMsg`) and `ShowInfo` (Delphi `TShowInfo`) overlay a
   reference with an integer, and the first stage gave that slot the shim's integer-or-reference
   value, `IntOrRef`. Milestone 9.6 (`specs/unions-and-kinds.md`) settled all eight. `MapCoordinate` (Delphi `TXYL`),
   `Entry` (Delphi `TPaletteEntry`) and `DefenseRequirements` (Delphi `TDefenseRequirements`) stay explicit-layout structs, since game logic reads
   both views. The three combat-info buffers of `AoWTC` became plain `AbilityCombatInfo` (Delphi `TAbilityCombatInfo`) locals,
   because their second arm is never read. The message slots of `EngineMessage` and `MapFieldMessage` (Delphi `TMapFieldMsg`) became
   the union `MsgArg` and the blend slot of `ShowInfo` the union `BlendValue` (section 9).

### Names

Issue 161 fixed the port's names.

A type is `Foo`. An interface is `IFoo`. A generic parameter is the only name that keeps a `T` prefix, written `TFoo`. An exception type is `FooException`. An enum member carries no type prefix. A digit stays in the member when the digit is the meaning, as in `Bits8` and `Rgb555`. The `Delphi` prefix marks a type that is not fully ported. `DelphiList` is the only such type.

A name that clashes with a .NET type is a reason to use the .NET type. A distinct class keeps a short name only when every use refers to the port's type. `Object` and `List` are out of that set. `Component` stays, because the port does not use `System.ComponentModel.Component`.

Own-format documents store the current names. The loader accepts an older stored type or enum member name through `src/Rtl/Delphi/names.csv` and an older property name through the property's `oldNames` in `src/Formats/schema.json`. A name that comes from an original game file stays the Delphi string. Form class names in the language dictionaries are that case.

A Delphi citation in these documents keeps the Delphi name. That is a Delphi unit, a Delphi routine, a quoted Delphi source line, or a `.pas` or `.dfm` reference. Where prose names a port type and the Delphi name helps a reader find the source, the C# name comes first and the Delphi name follows once in parentheses.

Names follow the .NET identifier-names guide. A type, a member and a constant use PascalCase. A parameter and a local use camelCase. A private instance field uses `_camelCase` and a private static field uses `s_camelCase`. Names are whole words. The short words kept in this project are Aow, Hex, Info, Id, IO, UI, AI, Tcp, Cpu and the file format names. A two-letter acronym is uppercase. A longer acronym is Pascal-cased.

A ported type drops the Delphi T prefix and any recovery prefix. A generic parameter stays `TFoo`. The Delphi prefix still marks a type that is not fully ported. A type uses the CLR name when that name is the meaning, as in `Int32` and `SByte`. A nested type does not repeat its owner. A nested enum whose name a member of its class needs moves out beside the class as `FooBar` (class `Foo`, member `Foo.Bar`), so no member takes a `Current` prefix. A metaclass keeps `Class` at the end of its name. A type name has no `Abstract` prefix. The hex and road bases keep the names issue 200 owns, which are `AbstractHexagon`, `AbstractBorderHexagon`, `AbstractRoad`, `AbstractHexagonResource`, `AbstractBorderHexagonResource` and `AbstractRoadResource`.

## 3. Transpile strategy

We write our own transpiler. Delphi 3 has a small grammar: no generics, no anonymous methods, no
overloading. Every recovered unit parses with Free Pascal in Delphi mode, so a hand-written parser
is a bounded job. Commercial converters produce code that needs a rewrite anyway.

The hand-over took place on 2026-10-27 after the oracle, frames, scenarios and audio passed.
The C# units are now source, edited by hand. Every edit keeps `python tools/check.py` passing
from the private repository. Transpilation remains available only for reference diffs of upstream
changes outside the product repository. The recovery is at 96 percent byte matching, the remaining functions were verified
by hand, by automated computer-only games and by play, and only minor bugs are expected to remain.
So the port does not wait for upstream to freeze: it translates the source as it stands when the
transpiler is ready. The rules are:

- Until milestone 2.4 (`milestones.md`, the headless oracle passes), generated C# is never edited by
  hand. A hand-written override replaces a generated unit, class or method and lives in a separate
  file, in the pattern the `aow` repository uses for `src/model/hand`. An upstream change is taken
  by regenerating.
- After that, the C# tree is the source of truth and the second stage (section 9) begins. An
  upstream change is ported by hand: the transpiler is run on the changed unit and its output is the
  reference diff, never a replacement of the tree. The transpiler stays in the repository for that.
- The transpiler lives in the private repository, with the Delphi source as an input path, and
  runs with `emit --out <dir>` into a directory outside the product repository.

What gets transpiled and what gets rewritten. The list is per unit, because several packages mix
rules code with editor dialogs. It was reviewed against `doc/modules.md` and the unit imports on
2026-10-24.

| set | units | lines | treatment |
|---|---|---|---|
| engine | Engine, inc/HashTab, EList, FMemStr, PlugInM | 11,900 | transpile. The clipboard and popup menu members of `EngineComponent` (Delphi `TEComponent`) get stub overrides |
| zlib | zlib.pas and the C object files it links | 500 | rewrite: the unit is a binding to C code, see open question 5 |
| engine, design time | ImgLComp, EResGrid, DebMDlg | 1,800 | drop (editor property grids and a debug dialog) |
| image library | ImageLib core classes, HashTabU | 8,800 | transpile. 11 routines with inline assembly get hand-written overrides |
| image library, design time | ImageLib panels, grids, manager list boxes, property editor; ILSpin, SubImgEd, SImgSEd, ILEdGrid, ILEdForm, PerfDlg, ImgSetEd | 4,100 | drop (editor) |
| map engine | HSEngine, IsoHex, ILTer, Terrain, Hexagon, Road, Bridge, BorderHx, RndHex, HSLib | 7,900 | transpile. 7 hex coordinate routines with inline assembly get overrides |
| map engine, editor | HSMEdit, HSSEdit, TerCBox, ProgDlg, HSEdMod, HSEdData | 2,100 | later, with the editor |
| rules | AoWEPACK (all units), AoWTCPCK (all units) | 112,000 | transpile |
| game UI toolkit | aowInt, except AoWDialogForm and AOWInterfaceEditor; AOWTools | 11,900 | transpile. The two excluded units are design-time only |
| game screens | AoW.exe: the 77 data module units, E1, E2 and E3 flow units | 27,000 | transpile. The form files become layout data, see below |
| host window | AoW.exe: AoWUnit (`MainForm`) | 500 | rewrite as the platform host |
| sound, rules side | Sound (`SoundSystem` (Delphi `TSoundSystem`), `Sfx` (Delphi `TSFX`), `SfxLibrary` (Delphi `TSFXLibrary`), `SongList` (Delphi `TSonglist`)) | 1,600 | transpile, minus the manager list boxes |
| sound, backend | GalSndS, Galaxy, DSound | 1,000 | rewrite: a new `SoundSystem` descendant on SDL audio and a module player |
| sound, editor | SFXEdDlg, SpinEdit | 300 | drop |
| graphics | GFXE, GFXDiag, DisplayC | 16,100 | transpile the Pascal surface, clipping, palette and blend table code of GFXE and the display control's loop of DisplayC; rewrite the 28 assembler blitters; replace the DIB and DirectDraw surfaces with memory surfaces of the same names and the eight display controllers with one SDL presenter; drop GFXDiag and the DirectDraw binding. `specs/graphics.md` |
| zoom display | Adcpack | 400 | drop: nothing in the game or the editor imports it |
| ability plug-in | AbilityP (AbilPack) | 500 | drop: a `Plugin` (Delphi `TPlugIn`) package that only `LoadExternalPlugIns` could load from a `PLUGINS` directory, which the shipped game does not have; neither executable loads it statically (`data/initialization-order.json`) |
| effects | aowFX | 300 | transpile (pure particle code) |
| network, protocol | NetworkE | 1,800 | transpile (token protocol on `EngineObject`) |
| network, transport | aowDPlay | 2,100 | rewrite on sockets |
| video | Video, VFW | 1,000 | rewrite (one `VideoPlayer` (Delphi `TVideoPlayer`) component, intro and ending only) |
| editor | AoWEd.exe (88 units), ILToolsP | 15,200 | later, separate project |
| third party | Localize, Vcladdon, ims | | replace, see below |

Where aowInt ends and AoW.exe begins. aowInt is a widget toolkit of `Component` (Delphi `TComponent`) descendants
(`UIComponent` (Delphi `TAoWComponent`), `Control` (Delphi `TAOWControl`), `Window` (Delphi `TAOWWindow`) and about 25 controls) that draw onto a `Surface` (Delphi `TSurface`)
and take mouse and key events from the host. Its only VCL dependencies are the `Component`
streaming base, the types `ShiftState` (Delphi `TShiftState`), `MouseButton` (Delphi `TMouseButton`), `TColor` and `TCursor`, three uses of
`Application` and the clipboard in `AoWEdit`. All of those go to the RTL shim. AoW.exe adds three
things on top of the toolkit:

1. The 77 screens. Each is a `DataModule` descendant whose components (679 labels, 445 panels, 402
   images, 388 buttons and so on) are declared in a form file and bound to handler methods by name.
   The form files reference two shared modules for fonts and interface images (`UIFonts`, 1,184
   references, and `UIGraphics`, 2,690 references) and hold 62 Multilizer translator components.
2. The flow units (E1GameFlow, E2GameFlow, E3EventDialogs, E2WindowLayout, the Globals units),
   which open and close screens and route event log entries to dialogs.
3. `MainForm`, the one VCL form. It owns the `Display` (Delphi `TDisplay`) control, switches between the full screen
   and the windowed display controller, draws the cursor, scrolls the map and routes mouse and key
   events into the toolkit.

The screens and flow units are transpiled like any other code. The form files are converted once
into a layout format the port loads at run time, or into generated construction code (open
question 7). A loader creates the components, sets the properties, resolves the references to the
shared modules and binds the handlers. `MainForm` is rewritten as the platform
host, since everything it does is window and input plumbing.

The graphics contract. GFXE exports 193 routines and 37 types, but the code that stays uses a
small part. The rules use 34 routines and 7 types, the UI 38 and 7, the image library 42 and 9.
The surface description record `LockedSurface` (Delphi `TSurfaceDesc`) appears 334 times in the rules and is the center of
the contract. The rewritten layer must provide:

- `Surface`, `DataSurface` (Delphi `TDataSurface`) and the surface the display owns, with lock, unlock, resize, restore
  and lost handling.
- The clipped blitters: `BltClipped`, `BltSDRectClipped`, `BltSpriteClipped16`,
  `BltColorKeyClipped` and `BltRect`.
- The 16-bit blend, color key and transparent blitters that ImageLib calls.
- The blend tables and the lookup blend table collection.
- The palette and the 8-bit to 16-bit remaps.
- The display mode list, and the display mode and pixel format queries.

The display controller contract that AoW.exe uses is smaller still. It needs an active drawing
surface, the back surface size, the frame rate, a swap between controllers and the mouse position.
It raises events for frame update, mouse, key, resize, surface loss and activation.

Third-party replacements. Multilizer is reached through one function, `TranslateRStr` (608 calls
in AoWEPACK, 510 in AoW.exe), plus the translator components in the form files, which translate
component properties at load time. The replacement is a string table built from the `Dict/*.mld`
dictionaries, with `TranslateRStr` as the shim entry point and the layout loader doing what the
translator components did. The internet suite (ims) is used only by the play-by-email dialogs
(`SmtpSender` (Delphi `TmsSMTP`), `MailMessage` (Delphi `TmsMessage`), the POP3 and SMTP dialogs), see open question 7. Toolbar97 and SpinEdit
are editor only.

A Delphi RTL shim library in C# provides what the rules code expects: `DelphiList` (Delphi `TList`), `StringList` (Delphi `TStringList`),
`TStream` and `TMemoryStream`, the string routines, `Random` and exceptions. It also provides the
small set of VCL calls the rules make (`ShowMessage`, `MessageBox`). Those route to the game UI.

Semantic traps the transpiler must handle itself, because they decide whether the oracle comparison
can pass:

- `shr` on `Integer` is a logical shift in Delphi. It becomes `(int)((uint)x >> n)`.
- `Random` is a 32-bit linear congruential generator seeded from `RandSeed`. The shim reproduces it
  bit for bit, including `Random(n)` and the floating point `Random`.
- Strings are 1-based byte strings in a Windows code page. The data files depend on this. The shim
  uses a byte string type for anything that touches data, and converts to .NET strings only at the
  UI boundary.
- Integer arithmetic wraps in both languages. Division and modulo truncate toward zero in both.
- Delphi computes floating point in x87 extended precision (80 bits). The port uses `double`.
  Results can differ in the last bits. This is accepted, see section 5.
- `Round` in Delphi rounds half to even. .NET `Math.Round` does the same by default. `Trunc` is a
  cast.
- Sets of up to 64 elements become flags enums or a `ulong`. Larger sets become a bitset struct.
- Class instance size and field offsets do not matter in the port, except where the code reads
  records from data files. Those records get explicit layout.

The transpiler reports what it cannot translate as a `TODO` comment in the output and as a line in a
report file. `translate_readwrite.py` in the `aow` repository does the same. The transpiler is
written in C# (decided 2026-10-26) and builds its output as Roslyn syntax trees, not as text, so
that it targets a typed object model of C# and every construct it emits is well formed by
construction. It lives in the private repository (section 8), because it is temporary by design:
section 3 retires it once the C# tree is the source of truth. `specs/transpiler.md` is its
specification.

The recovered source carries byte-matching idioms. Where the original code reached a field through
a pointer in a register, or where a recovered class had no name for a field, the recovery wrote
pointer views (`WorldMap: PAoWHSMap = @AoWE.AoWHSMap`), raw layout views
(`RawObject(Pointer(X)^).Fields[$48 div 4]`, record views with padding fields) and accessor
classes. About 1,000 such constructs exist, almost all in AoWEPACK. The transpiler folds them into
plain field access with the class layouts in `data/classes.json` (`specs/transpiler.md` section 6).
Whether the recovery source itself gets a cleanup pass instead is open question 11.

## 4. Platform layer

The game renders in software into 8-bit and 16-bit paletted surfaces, with RLE sprites and its own
hex and isometric compositor. The port keeps that. The ported graphics layer draws into a byte
framebuffer in C#. The renderer uploads that framebuffer as one texture per frame. This is the
smallest layer that reproduces the original output exactly. A test can compare it pixel for pixel.

Window, input, timing and audio come from SDL3 through a C# binding. SDL3 covers Windows, macOS,
Linux, Android and iOS. MonoGame was considered and is the fallback if SDL alone proves thin. Unity
was rejected for three reasons. Its scene graph and asset pipeline fight an engine that owns its own
object model and surfaces. It is closed source. It still needs an embedded scripting runtime for
mods.

The surface interface stays abstract. A hardware-accelerated renderer with scaling and filtering
can then replace the framebuffer upload later, without changes to the rules or the UI toolkit.

Sound effects are WAV files mixed through SDL audio. Music is Impulse Tracker modules, played by a
module player library (libopenmpt or libxmp through a binding, or a managed player). Video (Video
for Windows) is used for the intro and ending only, and is a later item. Networking replaces
DirectPlay with a plain socket transport behind the existing `NetworkE` interfaces.

Settings move from the Windows registry to a configuration file in the platform's user data
directory. The game root directory is a setting. On mobile, the player supplies the game files as
an archive, and the port unpacks it into app storage. The port never ships game data.

## 5. Testing oracle

The `aow` repository already has the most valuable test asset: seeded computer-only playtests. Their
per-turn saves are compared byte for byte between the original and the rebuilt game
(`tools/playtest.py` and its module docstring). The port inherits that as its oracle. The seeded
inputs and the original's per-day saves are kept under `D:\Build\oracle` (`specs/testing.md` 3.1).

Compatibility with the original's files is one way. The port reads every original format (saves,
maps, scenario sets, campaigns, user lists), because the oracle's inputs and the shipped content
are in them. It is not required to write files the original loads, and the second stage is free to
adopt its own save and map formats behind converting readers for the original ones (section 9). In
the first stage the persistence code is transpiled and writes the original format anyway, which
makes the comparison cheap.

Milestone 9.7 (`specs/own-formats.md`) adopted them. The decisions:

- The own format is primary for the in-scope families: world saves (`.asg`), custom and tactical
  maps (`.hsm`), campaign maps (`.csm`), campaign definitions and saves (`.cam`, `.acg`), the
  scenario set (`.hss`) and the user hero and item lists (`.ahl`, `.ail`). An own document has
  its own extension, an `o` before the original one (`.oasg`, `.ohsm`, `.ocsm`, `.ocam`,
  `.oacg`, `.ohss`, `.oahl`, `.oail`), so it never takes the place of an original in the
  original game's folders and the file dialogs, which show the extension, tell the two apart.
  Content, never the extension, tells a document from an original: an own document under an
  original name still loads.
- A document is canonical JSON: UTF-8 without a byte order mark, LF line ends, always pretty
  printed with two-space indentation for hand editing, members in schema order, with one
  exception: each row of a map's `Fields` takes one line. It carries `$format`, `$version`,
  `$kind` and `$container`, and the same state always writes the same bytes. Readers also accept a
  document the user compressed with gzip.
- The game writes only own documents and reads both formats. An original passes through the
  converter on every open and must reproduce itself byte for byte (G1, unmasked) before the
  engine sees it. A document that fails validation, or an original that fails G1, is a load
  error that leaves the current game and the file as they were. Nothing falls back to the
  original bytes.
- `aowconv` (`src/Convert`, over the library `src/Formats`) converts both ways without creating
  a game object. Its export to the original format is strict: it refuses state the original
  format cannot hold and names it, rather than dropping it. Only an explicit `--allow-lossy`
  drops values, and it reports each loss and stays outside every gate check. Version 1 of the
  format defines no lossy projection.
- User writes are safe. A save is converted, validated and serialized before anything is
  written, then a sibling temporary is written, flushed and moved over the old file in one step
  (`specs/platform.md` 7). A shipped file is never written: a save under its name goes to the
  user layer and shadows it.
- Port-only state takes property ids `0x40000000` to `0x4FFFFFFF`, mod state ids from
  `0x50000000`. A game without mods writes neither range, so the oracle needs no mask for them.
- Out of scope until milestone 10: the game set files (`Release\*.pfs`) and the e-mail game
  index, which keep the original format and paths.

The transition has a cost (`specs/own-formats.md` 6.1). Inside the engine every load and save
still passes through the Delphi image: property numbers, widths, call order, record layouts and
the stream code stay the engine's protocol, and JSON alone lifts none of these constraints.
Conversion costs time and memory: the game loads the largest shipped map in 0.3 to 0.5 s and
holds the file, its payload, a JSON tree and the engine image on the way (`specs/own-formats.md`
6.1 and 9.3). The exit from the image (`specs/own-formats.md` 6.3), a typed persistence tree
with a direct document backend and the legacy codecs as adapters over it, is a prerequisite of
milestone 10: no persistence interface becomes stable mod API before it is done.

Comparison goes through the port's own comparator (`specs/testing.md` 3.2), never through raw
bytes. It inflates both sides, so the compression stream is free. It takes a list of masked fields
for the floating point allowance below. It takes a list of the properties whose children are
written in hash table order, and compares those as sets. In the first stage that list is empty,
because the hash table code is transpiled and iterates as the original did. When the second stage
replaces a hash table, the oracle shows whether any game logic depended on its order: if it did,
the state diverges, and the rewrite keeps the order. If only the save order changed, the property
goes on the list.

Since 9.7 the oracle keeps the native references untouched and converts only the port's side: the
port's own documents pass once per run through the strict export, and the comparator reads the
result with its own Python reader, independent of `src/Formats`. The corpus check is exact and
separate: every in-scope original of the pinned corpus (556 files) must satisfy unmasked G1,
while the behavior comparison keeps its listed timer normalization.

Milestones, each with its measure:

1. Persistence round trip. The C# Engine loads a save and writes it back. The bytes are identical.
   This mirrors `tests/roundtrip.pas` in the `aow` repository.
2. Headless rules. HSEPack, AoWEPACK and AoWTCPCK run with a null graphics and sound backend. A
   seeded computer-only game produces saves that equal the Delphi build's saves turn for turn. The
   maps are the ones the recovery used: Mountain Clash, After the Fall, Mine All Mine, Igor's Folly,
   Love thy Neighbor, Rocky Hollow and Blood Isles. `playtest.py` is adapted to drive both builds.
3. Rendering. The real platform layer draws frames that equal the Delphi build's frames pixel for
   pixel, for a fixed set of scenes.
4. Playable game. A player completes a scenario with saves, loads, combat, diplomacy and spells.
5. Maps. The port reads every shipped map and campaign file, and a map loaded and saved through
   the port holds the same content under the comparator.

Floating point is not held to bit equality. The rules use floating point in the AI, combat and city
code, and the port computes it in `double` where Delphi used x87 extended precision. A save that
differs only through a floating point path is accepted once the difference is traced to one. The
harness reports the first turn and the first field that differ, so that each divergence gets looked
at once. Integer paths, `Random` and persistence stay exact.

A known risk for milestone 2: game objects hold sprite and animation references, and the rules
touch them. The null backend has to load the ILB libraries and answer size and frame queries, not
only return empty values.

Unit tests cover the RTL shim against recorded Delphi outputs: `Random` sequences, string routines,
`Round`, set operations and the storage stream.

## 6. Modding and scripting

Moddability is a goal of the port, but it comes after the oracle passes. A faithful port first, then
a refactor toward data, so that every step stays testable against the oracle.

Three tiers, in the order they arrive:

1. Data mods. The published properties already give a typed, named model of everything the engine
   persists. The port exposes that model as text (JSON or a similar format) alongside the binary
   formats. Rules that are hardcoded today as one class per spell or ability (30 combat spell
   classes, 20 unit enchantment ability classes, 18 unit spell classes and so on) are lifted into
   data one family at a time. The oracle confirms after each family that the game plays the same.
2. Code mods. C# assemblies loaded through a public API in an isolated load context. This is the
   model of the RimWorld and Terraria communities, where the C# modding ecosystem is strongest. The
   whole object model is reachable, which is the point.
3. Scripting. An embedded Lua interpreter for map and event scripting in the editor. It is small and
   sandboxable, and it runs on every platform.

Kind families stay open for code mods. Behavior that varies by kind is a virtual method on the kind,
so a mod adds a kind without editing a screen or a switch (section 9). Spells, abilities, tokens,
combat actions, map events and the AI dispatched through virtual methods from the start. Milestone
9.6 (`specs/unions-and-kinds.md`) opened the four families that screens and switches still selected
by type or id:

- Structures and event logs. Each kind returns view keys (`StructureView`, `EventView`) from
  virtual methods declared with their overrides in `Structure.Views.cs` and `EventLog.Views.cs`,
  and the screen looks the renderer of the key up in a static `Catalog` table, so no screen code
  enters a rules class. The tactical map generator asks the structure for its map name.
- Production orders and diplomatic proposals. A production item holds a `ProductionOrder` and a
  diplomatic action a `DiplomaticProposal`, open abstract records whose kinds carry the behavior
  (`AoWE.ProductionOrders.cs`, `AoWE.DiplomaticProposals.cs`). A `Catalog` registry turns the id
  stored in a save into the kind, and an id that no kind registered reads and writes back
  unchanged.

`ClassRegistry.cs` registers the original kinds and renderers. Registration from another assembly,
with the ids the mod loader assigns, is the work of milestone 10 (`milestones.md` section 10).

A constraint to know about: iOS forbids just-in-time compilation, so C# code mods do not load there.
Data mods and Lua scripts work everywhere. If mobile matters, data and Lua are the portable modding
path, and C# assemblies stay desktop only.

## 7. Open questions

1. Strings. Decided on 2026-10-26 and implemented on 2026-11-05 by brief 9.5-1
   (`specs/strings.md`): the first stage kept the 1-based byte string type of the shim throughout
   the rules, since that is what the oracle and the data files assume, and the second stage moved
   the tree to .NET `string` with one code page 1252 char per byte, so lengths and positions equal
   the Delphi bytes, and with bytes only at the data and font boundaries (`specs/rtl-shim.md` 1.2).
   The conversion was one pass over the whole tree (`milestones.md` 9.5), not unit by unit, because
   a tree with both string types needs a conversion at every boundary between a rewritten and an
   unrewritten unit. The oracle, the frame fixtures, the exception census and the sort dump held
   after the pass, and the strings check of the gate keeps the retired shim type, the .NET
   comparison, case and character class calls on game text and the casts between `char` and
   `byte` out of the tree (`specs/testing.md` section 6).
2. Hash table iteration order. Resolved on 2026-10-25. In the first stage the `HashList` (Delphi `THashList`) classes
   are transpiled, so the order is the original's. Issue 150 replaced them with `Dictionary`, and
   no path enumerates those tables. In the second stage the comparator compares
   hash-ordered children as sets (section 5), and the oracle tells which orders game logic depends
   on.
3. Multilizer dictionaries. Decided on 2026-10-25 (`specs/localization.md`): the format is
   reversed (`data/localization/mld-format.md`, parser in `tools/localization/`), the port writes
   the dictionary and translator classes itself in place of the stub units, and the content analysis
   showed that the English column equals the source text in all but one of 6,683 entries, so the
   shipped English game is reproduced by running the same lookup over the same files. German,
   French, Italian and Spanish are complete in the files and become a setting, an extension outside
   the oracle.
4. Galaxy music. Decided on 2026-10-25 (`specs/platform.md` section 5): the 24 `.IT` songs play
   through libopenmpt behind a second `SoundSystem` backend that keeps the Galaxy backend's
   volume, pan, fade and loop semantics. Music rendering is not held to the oracle; it affects
   neither saves nor frames.
5. zlib output. Resolved on 2026-10-25: the comparator compares inflated payloads
   (`specs/persistence.md` 1.1, `specs/testing.md` 3.2). The port uses the .NET zlib stream for
   both directions, and the compressed bytes are never compared.
6. Form file conversion. Decided on 2026-10-25 (`specs/ui-layout.md`): the 79 text form files
   stay data and are loaded at startup by a reader that reproduces the VCL streaming rules,
   including the reference fixup order that decides window stacking. Handler and property binding
   goes through tables the transpiler generates from the `published` sections, so there is no
   reflection. The text files in `data/forms` are the source: the recovery confirmed that they are
   complete (79 for `AoW.exe`, 84 for `AoWEd.exe`), keep every property with binary ones as hex
   blocks, and convert back byte-identically for all 79 game forms.
7. Play-by-email. Decided on 2026-10-25: wanted, and out of scope for the first release. The
   `ims` components get stand-ins that load from the form files and fail every operation
   (`specs/platform.md` section 9). When it comes into scope the internet suite is replaced with
   the .NET mail classes behind the two dialogs.
8. Editor. Decided on 2026-10-25: wanted, and out of scope for the first release. Whether it keeps
   the VCL form layout or moves onto the game's UI toolkit is decided when it is scheduled.
9. Networking. Decided on 2026-10-25: multiplayer is wanted, and out of scope for the first
   release. The engine's transport hook is two calls, `RemotePlayerCommunicationControl.OnSendData` (Delphi `TRemotePCC`) and
   `ReceiveDataPackage`, wired by `aowSetupEvents` to the DirectPlay component, which only
   `AoW.exe` uses (68 sites); the port ships a stand-in that reports no connections until
   `specs/networking.md` exists. The transport will be sockets.
10. Mobile input. Later, after the desktop release. What is known: the UI's minimum is 640 by 480
    (`General.Manager` and `DisplayResizeBackSurface` clamp to it) and much of it reflows at other
    sizes, so a touch pass salvages most screens. The input model is a mouse, but the only drag
    gesture is multi-unit selection, which is rare, and the right button is rare too (inverting a
    unit's selection in a party is the known use). A tap maps to a left click, tap and hold to a
    right click, and the drag selection gets its own gesture or a button.
11. Idiom cleanup upstream. The decompile session confirmed on 2026-10-24 that replacing the raw
    layout views and accessor classes with plain field access compiles to identical code, while
    the pointer views to globals and the shape casts reproduce the original code and cannot be
    removed without losing matches. The transpiler normalizes all of them either way. The cleanup
    was not done as of 2026-10-25: it needs a byte-matching worker run, and the recovery delivered
    `data/layout-views.json`, the resolution of every view, instead. The transpiler uses that file
    as its resolution table (`specs/transpiler.md` section 6), so the cleanup is optional and the
    user decides whether it ever runs.
12. Unit initialization order. Resolved on 2026-10-25: `data/initialization-order.json` in the
    `aow` repository holds the tables of both executables and the 21 packages they load, with four
    unresolved entries in `AoW.exe` that lie outside the transpile set (`specs/transpiler.md` 4.8).

## 9. From calque to idiomatic C#

The second stage starts when milestone 2.4 passes and the C# tree becomes the source of truth
(section 3). Its rules:

- One piece at a time, with the oracle run after each. A piece is a unit, a class family or a
  shim type, chosen by payoff: what the next hand-written work has to read and touch comes first.
- The scaffolding of the first stage is retired type by type, each retirement being its own pass
  over the tree: `AnsiString` to `string` with bytes at the data and font boundaries (open question
  1), `Ptr<T>` to arrays and spans, the split constructors to constructors, metaclass objects to
  generics with static abstract interface members or to the engine's own class registry, `with`
  blocks to plain code, the shim's `TVarRec` and the message and blend slots of `IntOrRef` to
  unions (milestone 9.6), the shim's `DelphiList` and `StringList` to `List<T>` and
  `Dictionary<K, V>`, explicit-layout records to plain `record struct` types with explicit
  serialization.
- Behavior that varies by kind goes in virtual methods on the kind, so that families stay open for
  mods. `union` serves only data that is closed by nature, and `closed` is not used, since it bars
  code mods. This is the user's direction of 2026-10-30. Where the behavior is presentation, the
  virtual returns a view key that the screen renders, so no screen code enters a rules class. Byte
  puns stay explicit-layout structs. Milestone 9.6 (`specs/unions-and-kinds.md`) applied the rule:
  - Three unions hold data closed by nature, each a custom `[Union]` struct that stores its value
    cases without boxing: `FormatArg` for the arguments of `Format` (`src/Rtl/Legacy/FormatArg.cs`, in
    place of `TVarRec`), `MsgArg` for the slots of the engine messages `EngineMessage` and `MapFieldMessage`
    (`src/Rtl/Legacy/MsgArg.cs`), and `BlendValue` for the percentage or table of `ShowInfo`
    (`src/Game/Draw/BlendValue.cs`).
  - Four kind families became virtual dispatch (section 6): structure and event log views, whose
    keys the screens render from `Catalog` tables, and production orders and diplomatic proposals,
    open records with `Catalog` registries. No switch on a structure kind, an event log kind, a
    production id or a diplomatic kind remains outside the kind classes, apart from the city
    screen's button table, which selects a button's panel and handler and stays until milestone 10
    gives a mod's new city action its button.
  - `IntOrRef` remains only in `Ptr<T>.Unbox(IntOrRef)`. The hash lists (Delphi `THashList`
    and `TStringHashList`) became `Dictionary<TKey, TValue>` with typed keys in issue 150.
- Formats are free. Once the readers for the original formats exist as converting adapters, the
  port's own save and map formats can change, and the comparator (section 5) reads both. Nothing
  requires that the original loads a file the port wrote. Milestone 9.7 did this
  (`specs/own-formats.md`): the game writes versioned JSON documents and reads originals through
  the converter, `aowconv` exports a document to the original format when the state fits it, and
  the gate compares through that strict export. The engine still works on the original image in
  memory, so the explicit-layout records of the persisted `rwData` properties stay until the exit
  from the image gives each an explicit field adapter, before any persistence interface becomes
  stable mod API (milestone 10).
- Performance work comes after the shape is right. The first stage is allowed to be slower than the
  original (deep copies, boxed pointers, byte strings). The second stage measures before it
  optimizes, and uses `Span<T>`, `ref` and inline arrays where the measurement points.

## 8. Repository layout

Two repositories, decided on 2026-10-26. `D:\git\open-aow` (this one) holds the product: the C#
source and the documents that specify it. `D:\git\open-aow-private` holds what the port builds on
the way and does not ship: the transpiler, the catalogues and surveys with their scripts, the
probes, the oracle tooling and the correspondence with the recovery project. A path in these
documents that starts with `docs/` or `src/` is in this repository. One that starts with
`correspondence/`, `data/`, `tools/` or `tests/` is in the private repository. What exists is
marked. The rest is where the specifications put things.

Since 2026-10-27 the game library is hand-owned source. There is no override manifest. Every
directory under `src` is hand-owned source, and hand edits go through the same review. Layout
source joined that rule with brief C11 of milestone 9.8 (2026-11-05). Until then decision D3
(`data/rewrite/review-9x.md`) regenerated the layout files whole from the private forms generator,
with no hand edits, so that the 9.5 strings pass changed the generator once instead of every
layout. Issue 161 (2026-11-10) named the projects, put each namespace in its own folder and put
each top-level type in its own file. The library project is `src/Game/Aow.Game.csproj` (assembly
`Aow.Game`). It compiles `src/Game` and `src/Engine`. A layout partial sits beside its type and
keeps the `.Layout.cs` suffix. The executable is `src/Aow/Aow.csproj` (assembly `Aow`, namespace
`Aow`). The private transpiler and the forms generator are reference emitters: the transpiler
accepts `emit --out <dir>`, the generator writes into private scratch space, both stay outside
the product tree, and their output serves comparison only and is never copied into `src`.

```
D:\git\open-aow
docs/                 this file, process.md, milestones.md, specs/, briefs/               (exists)
src/Aow/              the game executable, assembly Aow, namespace Aow                platform.md 2
src/Game/             the game library, assembly Aow.Game, namespace Aow.Game
src/Engine/           namespace Aow.Engine, compiled by the game library
src/Game.Tests/       tests of the game library, namespace Aow.Game.Tests
src/Rtl/              the Delphi RTL and VCL shim                                    rtl-shim.md
src/Formats/          the own save and map formats: documents, profiles, codecs      own-formats.md
src/Convert/          aowconv, the converter's command line                          own-formats.md 3.2
src/Graphics/         the rewritten blitters and memory surfaces                     graphics.md
src/Forms/            the form files as data                                         ui-layout.md
src/Platform/         the SDL host: window, input, audio, video, files, settings     platform.md
src/Headless/         the host without a display, for the oracle                    testing.md
src/Rtl.Tests/        xUnit tests of the RTL shim                                    rtl-shim.md 8
src/Formats.Tests/    xUnit tests of the own formats                                 own-formats.md

D:\git\open-aow-private
correspondence/       requests to and replies from the recovery project, dated      (exists)
data/                 catalogues the port built from the recovered source           (exists)
tools/survey/         scripts that build the catalogues, the cross-reference check  (exists)
tools/transpile/      the Delphi to C# transpiler, in C# on Roslyn, and its tests   transpiler.md
tools/oracle/         seeded runs, save and frame comparison                         testing.md, graphics.md
tools/forms/          reference layout generator, form checks, expected-tree dumps  ui-layout.md
tools/localization/   dictionary dump                                                (exists)
tests/probes/         Delphi 3 probe programs and their expected outputs             testing.md 2 (from-recovery/ exists)
tests/toolchain/      the C# 15 feature probe                                        (exists)
tests/frames/         frame fixtures                                                graphics.md 9
tests/forms/          loaded-tree expectations                                      ui-layout.md 8
tests/fixtures/       Delphi-made fixtures for the unit tests                       process.md 6
```
