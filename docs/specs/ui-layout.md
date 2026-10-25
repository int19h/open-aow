# UI layout specification

State: draft, 2026-10-25. Covers how the 79 form files of `AoW.exe` become the port's user
interface objects: the text form grammar, the component and property model, the loading order and
the resolution of references, handler binding, the translator components, and the contract between
the loaded objects and the frame loop. The drawing of the controls themselves is transpiled code
(the aowInt toolkit, `../design.md` section 3) and is not specified here; `graphics.md` has the
surfaces it draws into and `platform.md` has the input events it receives. Facts come from
`data/forms/AoW` and `src/delphi3` of the `aow` repository, from the Delphi 3 VCL source
(`C:\Tools\Delphi3\SOURCE\VCL\CLASSES.PAS`, `FORMS.PAS`) and from the RTTI dump
`data/rtti/aowInt.dpl.txt`, as read on 2026-10-25.

## 1. Decisions

1. The form files stay data. The port carries the 79 text files as they are
   (`src/Forms/AoW/<Module>.txt`, copied from the recovery's `data/forms/AoW`, which are complete
   and lossless, `../design.md` open question 6) and loads them at startup with a reader that
   follows the VCL streaming rules of section 4. One reader replaces 89,294 lines of generated
   construction code, the layouts stay editable by data mods, and the setter order the original's
   streaming produced is reproduced by construction rather than by hand.
2. No reflection. The reader binds property names, component classes and handler names through
   tables the transpiler generates from the `published` sections of the component classes and the
   data module classes (section 5). This keeps the port ahead-of-time compilable (iOS, `../design.md`
   section 6) and makes a misspelled name in a form file a load error with a location.
3. The loading order is the original's: the `Application.CreateForm` sequence of `AoW.dpr`
   (section 4.1), one module after another, each module's references to earlier modules resolved
   when the module finishes loading, exactly as `TReader` and `GlobalFixupReferences` do it. The
   order decides the sibling order of windows with equal priority (section 4.4), which decides what
   draws on top of what.
4. Two roots are not data modules and are not loaded from their form files: `TMainForm` is the
   platform host (`graphics.md` section 7) and `TRegistrationDlg`, a VCL dialog for the CD
   registration check with plain `TLabel`, `TEdit` and `TButton` controls, is dropped with the
   check (`CanStartGame` in `AoW.dpr` always passes in the port).

## 2. What the form files hold

`AoW.exe` has 79 form resources, one per `TDataModule` descendant (77) plus `TMainForm` and
`TRegistrationDlg`. `tools/dfm2txt.py` of the recovery wrote them as Delphi text form files, and
the recovery confirmed on 2026-10-25 that they round trip byte for byte through Delphi's own
converter. Sizes run from 22 lines (`TCMapEvents`) to 6,285 (`ScenSetup`), 89,294 lines in all.
Counts over the 79 files:

| what | count |
|---|---|
| `object` lines (components, roots included) | 2,947 |
| event handler assignments (`OnX = Name`) | 1,588, to 716 distinct methods |
| references to a component of another module (`Module.Name`) | 4,545: IntGfxMod 2,690, FontModule 1,184, General 638, InfoBanner 14, UnitBanner 9, TitleWindow 8, TBWindow 2 |
| references to a component of the same module | 2,642 `AOWWindow`, 96 `VScrollBar`, 76 `WinManager` (all `General.Manager`, cross-module), 17 `TabPanel`, 10 `OwnerMenu`, 2 `OwnerMenuBar`, 563 `ToolTip` (`General.ToolTip`) |
| string lists | 539: `ToolTipText.Strings` 437, `TargetsData` 62 (translators), `Items.Strings` 40 |
| sets | 4 (`Options` of the display and the video player, two empty `Font.Style`) |
| binary blocks | 3 (`Data` of three `TInterfaceIL` components in `IntGfxMod`: an empty version 4 library stored inline) |
| characters written as `#nnn` | 111 (`ColorChangeChar = #0`, `PassWordChar = #0`) |
| floats, string concatenations with `+`, `inherited` or `inline` objects, collections | none |

The property that appears most is the dotted sub-object `Alignment.*` (12 properties of
`TAOWAlignment`, written on almost every control). The component classes used, with the unit that
defines them:

| count | class | unit | treatment |
|---:|---|---|---|
| 679 | `TAOWLabel` | aowInt | transpiled toolkit |
| 445 | `TAOWPanel` | aowInt | |
| 402 | `TAOWImage` | aowInt | |
| 388 | `TAOWButton` | aowInt | |
| 194 | `TAOWCheckBox` | aowInt | |
| 170 | `TAoWFrame` | aowInt | |
| 97, 12 | `TAOWVScrollBar`, `TAOWHScrollBar` | aowInt | |
| 75, 75 | `TAOWMemo`, `TAOWListBox` | aowInt | |
| 56 | `TAOWWindow` | aowInt | |
| 46 | `TInterfaceIL` | aowInt (`TImageLibrary` descendant) | |
| 33, 28, 24 | `TAOWComboBox`, `TAOWEdit`, `TAoWGauge` | aowInt | |
| 13, 13, 6, 2 | `TaowUserDrawBox`, `TAOWMenu`, `TAOWTabPanel`, `TAOWMenuBar` | aowInt | |
| 1, 1 | `TAOWWinManager`, `TAOWToolTip` | aowInt | in `General` |
| 62 | `TIvTranslator` | Localize (Multilizer) | the port's translator component, `localization.md` |
| 12, 3 | `TImageLibraryFont`, `TImageLibrary` | ImageLib | transpiled (`FontModule`, `IntGfxMod`) |
| 3, 3, 2, 2, 2, 2 | `TAoWBookWindow`, `TAoWArnoLine`, `TStartMenu`, `TScanner`, `TAoWUserWindow`, `TAOWTurnButton` | AOWTools | transpiled |
| 1, 1 | `TAoWMapControl`, `TSetupControl` | AoWEPACK | transpiled rules components |
| 1 | `TAoWCombatMapControl` | AoWTCPCK | transpiled |
| 1 | `THSMapViewer` | HSEPack | transpiled (in `MWindow`) |
| 1, 1, 1 | `TDisplay`, `TWindows16DC`, `TDDrawFullScr16DC` | DisplayC | `MainForm` only, the host |
| 1 | `TVideoPlayer` | VideoP | `IntroScene`, `platform.md` |
| 1 | `TGDPlay` | aowDPlay | `SetupEvents`, networking stub until `networking.md` |
| 1, 1 | `TmsSMTP`, `TmsMessage` | internet suite | `SMTPDlg`, play-by-email stub |
| 2, 2, 2 | `TLabel`, `TEdit`, `TButton` | VCL | `RegistrationDlg` only, dropped |

Every other `object` is a root: the 77 data module classes of `AoW/*.pas`, `TMainForm` and
`TRegistrationDlg`.

## 3. The text form grammar

The reader parses this grammar, which is the subset of Delphi's text form format that
`dfm2txt.py` writes (no `inherited`, `inline`, collections or `item` blocks occur):

```
form      := object
object    := 'object' Name ':' ClassName NL { property | object } 'end' NL
property  := Path '=' value NL
Path      := Ident { '.' Ident }                 -- 'Alignment.AlignWidth', 'ToolTipText.Strings'
value     := Integer                             -- decimal, optionally negative
           | String                              -- 'text' with '' for a quote, and #nnn runs outside quotes
           | Ident                               -- enum member, True/False, a constant (crDefault, clBlack), a component reference
           | Ident '.' Ident                     -- a component of another module
           | '[' [ Ident { ',' Ident } ] ']'     -- a set
           | '(' NL { String NL } ')'            -- a string list, one string per line
           | '{' NL { HexLine NL } '}'           -- binary data
```

Strings are code page 1252 bytes, as everything in the game; `#nnn` is a byte value. A string
value whose `Path` ends in `.Strings` is a list, and a string list value is read into the `TStrings`
object the property's getter returns, as `TStrings.DefineProperties` does in the VCL; the
property's setter is not called. `TargetsData` is a list property the translator component defines
itself (section 6). Binary data belongs to the component's own `DefineProperties` (here only
`TCustomImageLibrary.Data`, the inline library).

The root's own properties come first (`Left`, `Top`, `Height`, `Width` of the data module's design
window, which the port ignores, and the module's `OnCreate` and `OnDestroy`), then its components
in design order. Every component's `Left` and `Top` are design-time positions of its icon in the
module window and are ignored too; the positions that matter are `WinLeft`, `WinTop`, `WinWidth`,
`WinHeight` and the `Alignment` rules.

## 4. Loading

### 4.1 Order of modules

`AoW.dpr` creates the modules in this order, each with `Application.CreateForm` and each assigned
to its global variable: `IntGfxMod`, `FontModule`, `General`, `TargetSelectionDlg`, `SetupEvents`,
`MapEvents`, `TCMapEvents`, `TitleWindow`, `PopupWindows`, `TBWindow`, `SWindow`, `MWindow`,
`IWindow`, `DialogWin`, `ControlWin`, `UnitBanner`, `UnitWindow`, `ChatWindow`, `MultiWindow`,
`FileDlgWindow`, `PInfoW`, `ProgressDialog`, `SoundDlg`, `VideoDialog`, `FastCombatWindow`,
`PyreScreen`, `NewTurnDlg`, `ArmyEventDlg`, `CityJoinDlg`, `ItemBanner`, `CityRebelDlg`,
`SelFactionWin`, `MsgReceiveDlg`, `HeroInfoDlg`, `PartyJoinDlg`, `ArmyInfoDlg`, `WorldMap`,
`TCUnit`, `TCScanner`, `ScenSetup`, `TCFCDialog`, `RelWindow`, `UnitSelectionDlg`, `ArenaDlg`,
`ConstructDlg`, `MagicWin`, `WizardsTowerDlg`, `PowerDlg`, `SpellBook`, `LeaderSetupWin`,
`DeformFaceDlg`, `HeroUpgradeDlg`, `EmailGameSettingsDlg`, `CastUnitSpellDlg`, `DisjunctionDlg`,
`HeroJoinDlg`, `ResultsWin`, `SynchronizeDlg`, `CityScreen`, `ProductionScreen`, `RealmWin`,
`CampaignTransferDlg`, `TitleScene`, `IntroScene`, `SendEmailGameDlg`, `EmailSettingsDlg`,
`POP3Dlg`, `SMTPDlg`, `HelpWindow`, `CampaignJoinDlg`, `ItemsFoundDlg`, `GamePlayDialog`,
`PasswordDlg`, `InfoBanner`, `MessageDlg`, `TutorialWindow`, `CampaignSettingsDlg`, and last
`MainForm`, then `Application.Run`. Some globals are aliased after creation (`E3Globals.ItemBanner`,
`E3Globals.DisjunctionDlg`, `E2Globals.IntroScene`). The port's startup (`platform.md`) runs the same
list; the transpiled `AoW.dpr` body is its source. The three shared resource modules come first
because every other module references them, and `General` holds the window manager every window
registers with.

### 4.2 Loading one module

`TDataModule.Create` calls `InitInheritedComponent`, which finds the form resource by class name
and reads it with `TReader.ReadRootComponent`, then fires `OnCreate`. The reader's steps, which the
port's `FormLoader.Load(moduleClass)` reproduces:

1. Create the root (the transpiled data module class) and set its `Name` to the object name of the
   file (`FindUniqueName` adds a suffix when a module of that name already exists, which does not
   happen in the game).
2. Read the root's properties, then its components. For each `object`: look the class up in the
   component class registry, construct it with the root as `Owner` (`TComponent.Create(Owner)`
   inserts it into the owner's component list, which is what `FindComponent` searches), set its
   `Name`, assign it to the root's field of that name (the published fields of the data module
   class, section 5.2), read its properties, then its own child objects (none occur in these files:
   the aowInt tree is built by the `AOWWindow` and `WinManager` properties, not by nesting).
3. A property value is applied at once when it is a scalar, string, set, enumeration, method or
   list. A value that names a component is not applied: it becomes a fixup record (instance,
   property, root name, component name), queued in the module's local list when it has no module
   prefix and in the global list when it has one.
4. When the whole root is read, apply the local fixups in queue order (`DoFixupReferences`): each
   looks the component up in the root by name (`Owner` names the root itself) and calls the
   property's setter with it.
5. Call `Loaded` on every component created (no aowInt or ImageLib class overrides it; the VCL
   base does bookkeeping only).
6. Apply the global fixups in queue order (`GlobalFixupReferences`): for each, find the named module
   among the modules created so far; when it exists, find the component in it and call the setter;
   when it does not exist yet, leave the record queued. Records left from earlier modules are tried
   again here, so a reference to a later module resolves when that module loads.
7. Fire the module's `OnCreate` handler.

In the game every cross-module reference points at a module created earlier (`IntGfxMod`,
`FontModule`, `General` are the first three; `InfoBanner`, `UnitBanner`, `TitleWindow` and `TBWindow`
are referenced by modules created after them), so step 6 resolves everything at the end of the
referencing module's load, and nothing is pending when `MainForm` is created. The loader reports a
record still pending after the last module as an error.

### 4.3 Applying a property

The reader applies a value through the class's property table (section 5.1): the entry gives the
property's type and a typed setter delegate that calls the transpiled property setter, so a
property with a setter method runs the same code the original ran during streaming. Those setters
matter: `SetAOWWindow`, `SetManager`, `SetOwner`, `SetTabPanel`, `SetMenu`, `SetMenuBar`,
`SetVScroll` link the component into the tree; the `SetIL*` setters clear `FValid` so that the
control validates its images on first draw; `SetVisible`, `SetChecked`, `SetIndex`, `SetGText`,
`SetGFont`, `SetFMin`, `SetFMax`, `SetFPos` and the rest update state. Values are converted by
the entry's type: an integer property accepts an integer or a constant identifier (`crDefault`,
`clBlack` and the other `cl*` and `cr*` names of `Graphics` and `Controls`, resolved from a
constant table); an enumeration accepts its member names; a `Char` accepts a one-character string
or `#nnn`; a `Boolean` or `LongBool` accepts `True` and `False`; a set accepts member names; a
`TStrings` property accepts a list through its getter; a class property accepts `nil` (not written
by the game) or a reference (fixup); a method property accepts a handler name (section 5.3).

A property absent from the file keeps the value the constructor gave it. The `default` directive
on `Visible` (`default True`) is the reason `Visible` appears only when it is `False`.

### 4.4 Why the order matters

`TAoWComponent.SetOwner`, reached through `AOWWindow`, `WinManager`, `TabPanel`, `OwnerMenu`
and `OwnerMenuBar`, removes the component from its old owner's `FChildren` and inserts it before
the first child whose `Priority` is greater than its own; equal priorities keep insertion order.
`Priority` is a scalar applied in step 3, before any fixup, so the priority is known when the
fixup inserts. The children of a window are therefore ordered by priority and, within a priority,
by the order of the `AOWWindow` lines in the file; the windows of the manager are ordered by
priority and, within a priority, by module creation order and then file order, because their
`WinManager = General.Manager` fixups run at the end of each module's load. The window manager
draws and dispatches input in children order, so this ordering is visible on screen. The loader
reproduces it by applying fixups in queue order exactly as described, and a unit test asserts the
order for a constructed module.

## 5. Generated tables

The transpiler emits, next to each transpiled unit, the tables the reader needs. They are generated
from the `published` sections, so the source stays the single authority, and the recovery's RTTI
dump (`data/rtti/<module>.txt`, the published properties of every class as the original binaries
declare them, with their types and getter and setter kinds) is the cross-check the transpiler
runs: a property in the dump that the source does not publish, or the reverse, is an error.

### 5.1 Property tables

For every class with a `published` section: `static readonly PropertyTable Properties`, an ordered
dictionary from property name to an entry `{ Kind, Type, Getter, Setter }`, where `Kind` is one of
`Ordinal`, `Enum`, `Set`, `String`, `Char`, `Class`, `Method`, `Strings`, `SubObject`, and the
accessors are delegates onto the transpiled property. `SubObject` entries (`Alignment`, `Font`)
return the sub-object whose own table takes the rest of a dotted path. A class inherits its
ancestors' entries. aowInt's published properties by type: `Integer` 106, inherited republished 77,
`TInterfaceIL` 31, `TNotifyEvent` 22, `Boolean` 16, `TImageLibraryFont` 10, `TStringList` 9,
`string` 4, component references of six classes 15, `Char` 4, `Byte` 4, enumerations
(`TaowTextAlign`, `TAOWAlignWidth`, `TAOWAlignHeight`, `TFrameType`) 5, `TCursor` 2, `TColor` 2,
`LongBool` 2, and the draw and mouse event types.

### 5.2 Component class registry and module fields

A registry from class name to a constructor delegate `TComponent Create(TComponent owner)` for
every component class that appears in a form file (section 2), filled by each unit's `Initialize`
the way the original's `RegisterClasses` calls did. For every data module class, a table from
component name to a field setter, generated from the published fields (`UnitPnl: TAOWPanel`, and
so on), so that step 2 of the load fills the fields the handlers use.

### 5.3 Handler binding

For every data module class, a table from method name to a factory that returns a delegate of the
requested event type bound to the module instance. The event types in the files: `TNotifyEvent`
(`OnClick` 373, `OnChange` 307, `OnDoubleClick` 92, `OnUpdate` 82, `OnHide` 39, `OnCreate` 23,
`OnDrawBack`'s cousin `OnShow` 15, `OnDestroy` 18, `OnResize` 12, `OnSelect` 10, `OnDrag` 9,
`OnReturn` 3, `OnAnimDone` 3 and the module-defined `OnUnitChanged`, `OnShowMessage`,
`OnSeatedPlayerChanged`), `TAoWOnDrawEvent` (`OnPreDraw` 144, `OnDraw` 129), `TAoWOnDrawBackEvent`
(`OnDrawBack` 22), `TAoWOnDrawItemEvent` (`OnDrawItem` 19, `OnPreDrawItem` 6), `TMouseEvent`
(`OnMouseDown` 135, `OnMouseUp` 3), `TMouseMoveEvent` (14), `TKeyEvent` (`OnKeyDown` 38),
`TKeyPressEvent`, `TAoWOnDrawSurface`.

The recovered handler declarations do not always carry the event's parameters: the binary shows
that many handlers ignore `Sender` and the recovery declared them without it
(`procedure TArmyInfoDlg.CloseBtnClick;` bound to an `OnClick`, marked `{ draft }` because the
parameter list is inferred). Under the register convention an extra argument is harmless at run
time, so the original worked. The factory therefore adapts: when the method's parameter list is a
prefix of the event's, the generated delegate drops the trailing arguments; when the method takes
parameters of other types, the transpiler reports an error. A handler name that no method
matches is a load error (the VCL raised `PropValueError`).

## 6. Translators

Each of 62 modules has a `TIvTranslator` component with `DictionaryName` (`'AoW'` for the game's
own forms) and a `TargetsData` list that names the properties to translate per class (`Items`,
`Text`, `ToolTipText` in `ScenSetup`). In the original the Multilizer component walks the module's
components after loading and replaces those string properties from the dictionary for the active
language, and `GetLanguage` always answers English in the shipped game (`../design.md` open question
3). The port's `TIvTranslator` is a component of the shim with the two properties, a `TargetsData`
reader that keeps the list, and a `Translate` that `localization.md` specifies; until then it does
nothing, which is the shipped behavior for English.

## 7. From loaded objects to the frame

`General.Manager` (`TAOWWinManager`, 640 by 480 in the file, `BackgroundTexture` and
`ForegroundTexture` `IntGfxMod.GenericT` indices 0 and 1, `BorderBottom` 48 for the bottom bar,
`OverlayPriority` 0) owns every window; `DisplayResizeBackSurface` sets its size to the back
surface's, never below 640 by 480, and `General.ResetLayout` re-aligns the windows by their
`Alignment` rules. Each frame `TMainForm.DisplayUpdateFrame` calls `General.Manager.DrawSurface`
with the back surface (`graphics.md` 7.2), which validates, aligns, redraws the dirty rectangles and
blits window back surfaces. Input arrives from the display control's handlers, which the host
calls with the original's argument conventions: `CheckMouseDown(Button, Shift, X, Y)`,
`CheckMouseUp`, `CheckMouseMove(Shift, X, Y)` in back-surface pixels, `CheckKeyDown(Key, Shift)`
with a Windows virtual key code, and `CheckKeyPress(Key)` with a code page 1252 character
(`platform.md` maps SDL events to these). `RestoreWindowLayout` and `General.SaveLayout` read and
write window positions through the registry wrapper (`rtl-shim.md`, settings store).

Nothing in this path is new code except the reader and the tables: the toolkit, the window
manager, the module handlers and the game flow units (`E1GameFlow`, `E2GameFlow`, `E3EventDialogs`
and the others of `AoW/`) are transpiled. The executable's own code is verified to the same standard
as the packages: of its 1,334 functions, 1,258 byte-match the original, 1 is proven equivalent, 30
are not proven and 45 are unsupported by the checker (`D:\Build\tmp\cx_rva\step10\quiet\AoW.plain.json`).
The 75 unverified ones are mostly draw handlers (`U1PreDraw` in six dialogs, `DrawUnit`,
`DrawGraph`, `DrawScene`) and window logic (`TGeneral.ResetLayout`, `RestoreWindowLayout`,
`TCityScreen.ShowCity`, `TUnitWindow.ShowUnit`); the frame oracle (`graphics.md` section 9) is
what vouches for them, and a frame difference that traces into one of them is settled against the
original binary. The `{ draft }` markers and the "stub bodies are placeholders" headers in 43 of
the 92 `AoW/` units predate the matching and are stale.

## 8. Tools and tests

- `tools/forms/check.py`: parses the 79 files with the grammar of section 3 and checks every
  class name against the registry list, every property path against the property tables (exported
  by the transpiler as JSON next to the C#), every handler name against the module's methods, and
  every reference against the file or the named module. It runs in the build.
- `tools/forms/dump.py`: produces, from a form file alone, the expected tree after load: per
  component its class, name, owner link (from `AOWWindow`, `WinManager`, `TabPanel`, `OwnerMenu`),
  `Priority`, `WinLeft`, `WinTop`, `WinWidth`, `WinHeight`, `Visible`, and the children order by the
  rule of section 4.4. `tests/forms/` compares the port's loaded tree with it for all 77 modules.
- A unit test of the loader on a synthetic module with equal priorities, a cross-module reference
  to a later module, a dotted sub-object property, a `#nnn` character, a string list and a handler
  without parameters.

## 9. Done measures

- U1: `check.py` passes on the 79 files with zero errors.
- U2: the port loads the 77 modules in the order of section 4.1 with no pending fixup, and the
  loaded trees equal `dump.py`'s expectation for every module.
- U3: the title scene fixture of `graphics.md` section 9 compares equal, which exercises the
  manager, the title window and the fonts end to end.

## 10. Open points

1. Whether the form files are embedded in the executable as resources (as the original did) or
   shipped as files next to it. Either way they are source assets of the port, not game data, since
   they come from the recovered source tree. The default is embedded.
2. The `AoWEd` form files (84) are out of scope until the editor is.
3. `TGDPlay`, `TmsSMTP` and `TmsMessage` need stand-in classes that load their properties and do
   nothing, so that `SetupEvents` and `SMTPDlg` load; their behavior is `networking.md` and the
   play-by-email decision.
