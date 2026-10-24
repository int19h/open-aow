# OpenAoW design

OpenAoW is a source port of Age of Wonders (1999, v1.36) to modern desktop platforms, with mobile
as a later goal. It uses the original game files and ships none of them. The source comes from the
recovery project in the `aow` repository (`D:\git\aow`, pulled from `C:\git\aow`). That repository
holds Delphi 3 source that rebuilds the shipped game with the original compiler. The rebuilt game is
the oracle: the reference whose behavior the port must reproduce.

This document records the decisions made so far and the questions that are still open. It is a
living document. Update it when a decision changes.

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
  Inline assembly is 212 blocks in 28 files, almost all in the blitters of Gfxepack and Ilpack.
- The in-game UI (aowInt) draws itself onto the game's surfaces. It does not use the VCL (Delphi's
  Windows GUI framework) for drawing or input. The game's 77 screens in AoW.exe are `TDataModule`
  containers that hold aowInt components, laid out in form files (`notes/forms/AoW`, 79 files,
  89,000 lines, 1,588 event handlers). AoW.exe has exactly one VCL form, `TMainForm`, which hosts
  the display. Section 3 has the boundary in detail.
- The VCL leaks into the rules in a shallow way. `AoWE.pas` imports Forms and Dialogs. The rules
  packages call `ShowMessage` 91 times and `MessageBox` 37 times.
- Floating point is present in the rules. AoWEPACK has 56 uses of floating point types (AI, AI
  groups, combat, cities, global target spells) and AoWTCPCK has 10. The rules call `Sin`, `Cos`,
  `Power`, `Sqrt`, `Ln` and `ArcTan`. `Random` is called 428 times in the rules.
- Three third-party packages (Multilizer localization, Toolbar97, an internet suite) are out of
  scope for recovery. The port replaces them.
- Language features in use: 30 virtual class methods, 761 `with` statements, 539 variant records,
  27 interfaces, method pointers for events, sets, pointers and PChar in the graphics code.

Game data on disk: `Images/` and `Int/` hold ILB image libraries. `Songs/` holds Impulse Tracker
modules (`.IT`) that `Galaxy.dll` plays. `Sfx/` holds sound effects. `Dict/` holds Multilizer
dictionaries (`.mld`). `Release/` and `Scenario/` hold maps and campaign data. `Save/` holds saves.

## 2. Language: C#

The port is written in C# on .NET 8 or later.

Why C#. It maps onto Delphi 3 more closely than any other broadly supported language. Classes,
virtual methods, properties, interfaces and `try/finally/except` map directly. Method pointers
become delegates. Sets become flags enums or a bitset struct. Records become structs. Pointers
become `unsafe` code or `Span<T>`. The RTTI-driven persistence becomes attributes with reflection
or a source generator. It runs on Windows,
macOS, Linux, Android and iOS. It has a large contributor pool and the strongest modding ecosystem
of any managed language.

Alternatives considered:

- Free Pascal. The lowest-risk route to a running cross-platform build: the recovered source
  compiles in Delphi mode already, and the diff against the oracle stays trivial. Rejected for the
  stated goals of moddability and a long-lived community project, which want a modern language and
  tooling. Free Pascal stays useful as a second compiler during the port.
- Rust and C++. Rejected: ownership and manual memory management map poorly onto a managed object
  graph of 1,785 classes.
- Java and Kotlin. Rejected: no structs, no unsafe memory, no sets, weaker game tooling.

Two Delphi features need a design, not a mapping:

1. Class references (`class of T`) with virtual class methods. Each game class gets one metaclass
   object (a singleton descriptor). A virtual class method becomes a virtual instance method on the
   descriptor. A variable of type `TFooClass` becomes a reference to a descriptor. `ClassType`,
   `InheritsFrom` and `NewInstance` live on the descriptor. The engine already has a class registry
   for persistence class ids, so this fits.
2. Variant records. The ones used as real byte unions become structs with explicit layout. The ones
   used as tagged unions by convention become ordinary structs or classes. The transpiler needs a
   per-type decision list for this.

## 3. Transpile strategy

We write our own transpiler. Delphi 3 has a small grammar: no generics, no anonymous methods, no
overloading. Every recovered unit parses with Free Pascal in Delphi mode, so a hand-written parser
is a bounded job. Commercial converters produce code that needs a rewrite anyway.

The transpile is regenerable. The recovery in `aow` is still being refined, so the port must be able
to take an updated source and produce updated C# at any time. The rules are:

- Generated C# is never edited by hand. A hand-written override replaces a generated unit, class or
  method and lives in a separate file, in the pattern the `aow` repository uses for `src/model/hand`.
- Nobody refactors generated output until the upstream source is frozen. Refactoring happens on the
  C# side after the oracle tests pass, and it moves code out of the generated set into hand-written
  code one piece at a time.
- The transpiler lives in this repository, with the Delphi source as an input path, and runs in one
  command.

What gets transpiled and what gets rewritten. The list is per unit, because several packages mix
rules code with editor dialogs. It was reviewed against `notes/modules.md` and the unit imports on
2026-10-24.

| set | units | lines | treatment |
|---|---|---|---|
| engine | Engine, inc/HashTab, EList, FMemStr, PlugInM | 11,900 | transpile. The clipboard and popup menu members of `TEComponent` get stub overrides |
| zlib | zlib.pas and the C object files it links | 500 | rewrite: the unit is a binding to C code, see open question 5 |
| engine, design time | ImgLComp, EResGrid, DebMDlg | 1,800 | drop (editor property grids and a debug dialog) |
| image library | ImageLib core classes, HashTabU | 8,800 | transpile. 11 routines with inline assembly get hand-written overrides |
| image library, design time | ImageLib panels, grids, manager list boxes, property editor; ILSpin, SubImgEd, SImgSEd, ILEdGrid, ILEdForm, PerfDlg, ImgSetEd | 4,100 | drop (editor) |
| map engine | HSEngine, IsoHex, ILTer, Terrain, Hexagon, Road, Bridge, BorderHx, RndHex, HSLib | 7,900 | transpile. 7 hex coordinate routines with inline assembly get overrides |
| map engine, editor | HSMEdit, HSSEdit, TerCBox, ProgDlg, HSEdMod, HSEdData | 2,100 | later, with the editor |
| rules | AoWEPACK (all units), AoWTCPCK (all units) | 112,000 | transpile |
| game UI toolkit | aowInt, except AoWDialogForm and AOWInterfaceEditor; AOWTools | 11,900 | transpile. The two excluded units are design-time only |
| game screens | AoW.exe: the 77 data module units, E1, E2 and E3 flow units | 27,000 | transpile. The form files become layout data, see below |
| host window | AoW.exe: AoWUnit (`TMainForm`) | 500 | rewrite as the platform host |
| sound, rules side | Sound (TSoundSystem, TSFX, TSFXLibrary, TSonglist) | 1,600 | transpile, minus the manager list boxes |
| sound, backend | GalSndS, Galaxy, DSound | 1,000 | rewrite: a new `TSoundSystem` descendant on SDL audio and a module player |
| sound, editor | SFXEdDlg, SpinEdit | 300 | drop |
| graphics | GFXE, GFXDiag, DisplayC | 16,100 | rewrite on the new surface layer, see the contract below |
| zoom display | Adcpack | 400 | drop: nothing in the game or the editor imports it |
| effects | aowFX | 300 | transpile (pure particle code) |
| network, protocol | NetworkE | 1,800 | transpile (token protocol on `TEObject`) |
| network, transport | aowDPlay | 2,100 | rewrite on sockets |
| video | Video, VFW | 1,000 | rewrite (one `TVideoPlayer` component, intro and ending only) |
| editor | AoWEd.exe (88 units), ILToolsP | 15,200 | later, separate project |
| third party | Localize, Vcladdon, ims | | replace, see below |

Where aowInt ends and AoW.exe begins. aowInt is a widget toolkit of `TComponent` descendants
(`TAoWComponent`, `TAOWControl`, `TAOWWindow` and about 25 controls) that draw onto a `TSurface`
and take mouse and key events from the host. Its only VCL dependencies are the `TComponent`
streaming base, the types `TShiftState`, `TMouseButton`, `TColor` and `TCursor`, three uses of
`Application` and the clipboard in `AoWEdit`. All of those go to the RTL shim. AoW.exe adds three
things on top of the toolkit:

1. The 77 screens. Each is a `TDataModule` descendant whose components (679 labels, 445 panels, 402
   images, 388 buttons and so on) are declared in a form file and bound to handler methods by name.
   The form files reference two shared modules for fonts and interface images (`FontModule`, 1,184
   references, and `IntGfxMod`, 2,690 references) and hold 62 Multilizer translator components.
2. The flow units (E1GameFlow, E2GameFlow, E3EventDialogs, E2WindowLayout, the Globals units),
   which open and close screens and route event log entries to dialogs.
3. `TMainForm`, the one VCL form. It owns the `TDisplay` control, switches between the full screen
   and the windowed display controller, draws the cursor, scrolls the map and routes mouse and key
   events into the toolkit.

The screens and flow units are transpiled like any other code. The form files are converted once
into a layout format the port loads at run time, or into generated construction code (open
question 7). A loader creates the components, sets the properties, resolves the references to the
shared modules and binds the handlers. `TMainForm` is rewritten as the platform
host, since everything it does is window and input plumbing.

The graphics contract. GFXE exports 193 routines and 37 types, but the code that stays uses a
small part. The rules use 34 routines and 7 types, the UI 38 and 7, the image library 42 and 9.
The surface description record `TSurfaceDesc` appears 334 times in the rules and is the center of
the contract. The rewritten layer must provide:

- `TSurface`, `TDataSurface` and the surface the display owns, with lock, unlock, resize, restore
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
(`TmsSMTP`, `TmsMessage`, the POP3 and SMTP dialogs), see open question 7. Toolbar97 and SpinEdit
are editor only.

A Delphi RTL shim library in C# provides what the rules code expects: `TList`, `TStringList`,
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
written in Python 3.11, like the `aow` tooling, with no dependencies outside the standard library.
`specs/transpiler.md` is its specification.

The recovered source carries byte-matching idioms. Where the original code reached a field through
a pointer in a register, or where a recovered class had no name for a field, the recovery wrote
pointer views (`WorldMap: PAoWHSMap = @AoWE.AoWHSMap`), raw layout views
(`TRawObject(Pointer(X)^).Fields[$48 div 4]`, record views with padding fields) and accessor
classes. About 1,000 such constructs exist, almost all in AoWEPACK. The transpiler folds them into
plain field access with the class layouts in `notes/classes.json` (`specs/transpiler.md` section 6).
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
(`tools/playtest.py`, `notes/briefs/compare-behavior.md`). The port inherits that as its oracle.
Save and map compatibility is a requirement anyway, so the oracle costs little extra.

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
5. Maps. The port reads every shipped map and campaign file, and a map saved through the port equals
   the original bytes.

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

A constraint to know about: iOS forbids just-in-time compilation, so C# code mods do not load there.
Data mods and Lua scripts work everywhere. If mobile matters, data and Lua are the portable modding
path, and C# assemblies stay desktop only.

## 7. Open questions

1. Strings. Whether the shim keeps a 1-based byte string type throughout the rules, or converts
   the rules to .NET strings and keeps bytes only in the data layer. The first is safer for the
   oracle, the second is cleaner C#.
2. Hash table iteration order. The engine's `THashList` classes determine iteration order in places.
   The shim must reproduce the original order where the rules iterate, or the saves differ.
3. Multilizer dictionaries. The `Dict/*.mld` files hold the localized strings. The port needs a
   reader for that format or a one-time export to its own string table format.
4. Galaxy music. Whether the `.IT` files play correctly through libopenmpt, or whether Galaxy
   applied effects the port must reproduce.
5. zlib output. The save container holds zlib-compressed streams. The byte for byte comparison of
   saves needs the same deflate output as the original zlib build, which the .NET deflate stream
   does not promise. The options are a managed port of the original zlib, or a comparison on the
   decompressed payloads.
6. Form file conversion. Whether the 79 form files become a layout data format that the port loads
   at run time, or generated C# construction code. Data keeps the layouts editable by mods. Code
   gives compile-time binding of the 1,588 handlers.
7. Play-by-email. Whether the SMTP and POP3 game exchange stays in scope. If it does, the internet
   suite is replaced with the .NET mail classes behind the two dialogs.
8. Editor scope. Whether the map editor is ported at all, and whether it keeps the VCL form layout
   or moves onto the game's UI toolkit.
9. Networking. Whether multiplayer is in scope for the first release, and what the transport is.
10. Mobile input. The UI assumes a mouse and an 800 by 600 display. Touch and small screens need a
    UI pass that is outside the faithful port.
11. Idiom cleanup upstream. The decompile session confirmed on 2026-10-24 that replacing the raw
    layout views and accessor classes with plain field access compiles to identical code, while
    the pointer views to globals and the shape casts reproduce the original code and cannot be
    removed without losing matches. The transpiler normalizes all of them either way. Open:
    whether the recovery source gets the match-neutral cleanup (about 290 casts, 17 fields need
    a visibility change). With the cleanup, the transpiler depends less on layout resolution.
12. Unit initialization order. The original binaries hold the unit initialization tables, and
    `tools/buildgame.py` in the `aow` repository can write them as `initialization-order.json`.
    The file has to be produced once and committed where the transpiler can read it. Request to
    the recovery session, through the user.

## 8. Repository layout (proposed)

```
docs/            design notes, this file
tools/transpile/ the Delphi to C# transpiler and its tests
src/Rtl/         the Delphi RTL shim
src/Generated/   transpiled output, regenerated, never edited
src/Overrides/   hand-written replacements for generated code
src/Platform/    SDL window, input, audio, framebuffer presentation
src/Game/        the game executable
tests/           oracle harness and unit tests
```
