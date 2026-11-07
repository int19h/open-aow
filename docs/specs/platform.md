# Platform specification

State: draft, 2026-10-25. Covers the host that runs the transpiled game: process startup and the
main form's role, the window and the presenter's SDL side, input mapping, the clock, audio (music
and sound effects), the intro video, files and paths, settings in place of the registry, message
boxes, the `Application` stub, the networking transport hook, plug-ins, and the headless host.
The graphics contract the host fulfils is `graphics.md` sections 7 and 8; the loaded UI it feeds
input to is `ui-layout.md`; the RTL services it implements are `rtl-shim.md`'s `IRtlHost`. Facts
come from `src/delphi3` of the `aow` repository and the GOG install (`D:\Games\GOG\Age of Wonders`)
as read on 2026-10-25. Section 7 gained the replacement primitive and the file log of milestone
9.7 on 2026-11-06 (brief 9.7-4).

## 1. Decisions

1. SDL3 through a thin binding, one window, one thread. The original runs everything on the main
   thread in a synchronous frame loop (`graphics.md` 7.2), and the host keeps that: SDL events are
   pumped where the original called `Application.ProcessMessages`, audio mixing is the only other
   thread and it never touches game state.
2. The game root is a directory the user points the port at (the GOG or CD install), or a zip of it
   on mobile. The port never writes into it: a user data directory overlays it for saves, user
   lists, e-mail folders and logs (section 7). File names are resolved case-insensitively with
   either separator, because the source spells the same directories three ways.
3. Audio is rewritten behind the original's own abstraction. `Sound.pas` already defines an
   abstract `TSoundSystem` with one backend, `GalSndS.pas` over `Galaxy.dll`; the port supplies a
   second backend, `TSdlSoundSystem`, that mixes WAV samples in C# and plays the Impulse Tracker
   songs through libopenmpt, with the Galaxy backend's volume, panning, fade and loop semantics
   (section 5).
4. The intro video is optional. `INTRO.AVI` is Indeo Video 3.2 (`IV32`), a codec no managed library
   decodes. The host plays it when an FFmpeg library with the `indeo3` decoder is available at run
   time and otherwise behaves as if `Skip Intro` were set. Nothing else in the game is video.
5. Settings replace the registry one to one: the 40 values `TAoWRegistry` reads and writes under
   `\Software\Triumph Studios\Age of Wonders\<Section>` become entries of a settings file in the
   user data directory, keyed `Section/Name`, read and written through the shim's `TRegistry`
   (`rtl-shim.md`). File associations and the DirectPlay lobby registration are no-ops.
6. The CD check and the single-instance mapping go. `CanStartGame` returns `True`; the memory check
   with it is meaningless today. The `dplobby` command line start is ignored until networking
   exists.

## 2. Startup and the main form

`AoW.dpr` (`ui-layout.md` 4.1) is transcribed as `Program.Main`: set the title, create the 77 data
modules in order, create the main form, run. `TMainForm` is the host object. Its handlers are
transcribed, with the Windows parts replaced:

| handler | what it does | host replacement |
|---|---|---|
| `FormCreate` | `RegisterTacticalCombat`; client size 640 by 480; registers the lobby server; loads `RELEASE\RELEASE.HSS` (the scenario set); debug window when debug mode; `DXDC.UseVideoMemory` from detail level; window state and position from settings; sound settings from the registry (section 5.1); `DD.SetCooperativeLevel`; `InitializeSoundSystem('Galaxy', ...)` when sound quality is not 0, then music and SFX volume; video size and mode from settings, full screen when the mode says so; `General.Manager` size from the back surface; multiplayer names from settings; `RestoreWindowLayout`; `SetCurrentDir(StartupDirectory)`; combat font | the lobby registration, `DXDC`, `WinDC` and `DD` lines fall away or address `TSdlDC` and the fullscreen flag; `InitializeSoundSystem('SDL', ...)`; the debug window becomes a log |
| `FormShow` | `ParamStr(1)`: `dplobby` starts the lobby, an existing `.asg` or `.acg` queues `DeferredSetupAction := 2` behind the title scene, else splash (demo), intro (`IntroScene.Execute`) unless debug or `Skip Intro`, else the title scene | the same, with the first argument of the port's command line as `ParamStr(1)` |
| `DisplayUpdateFrame` | the frame procedure (`graphics.md` 7.2) | transcribed |
| `DisplayMouseDown`, `DisplayMouseMove`, `DisplayMouseUp`, `DisplayDblClick`, `DisplayKeyDown`, `DisplayKeyPress` | forward to `General.Manager` (`CheckMouseDown` and the others) with modal checks | called by the event pump (section 3) |
| `DisplayResizeBackSurface`, `DisplaySurfaceDataLost`, `DisplayActivateDC`, `DisplaySwapDC` | manager size and layout reset, video mode bookkeeping | transcribed against `TSdlDC` |
| `FormCloseQuery` | asks the map to exit (`ExitGame(egmTerminate)`), refuses while in combat with a confirm dialog | the window close event asks the same question |
| `FormDestroy` | releases sound, saves the layout and every setting | the same on exit |

`AoWEngine.StartupDirectory` defaults to `ExtractFilePath(Application.ExeName)` (`Engine.pas:8768`)
and the registry's `Startup Directory` can override it; `Root Directory` is a second registry path
the engine reads for data (`GetRootDirectory`). In the port `Application.ExeName` reports
`<game root>\AoW.exe`, so both default to the game root without touching the transpiled code, and
the two settings stay writable for a user who keeps data elsewhere.

## 3. Window, presenter and input

The window is created at the back surface size times the integer scale (`graphics.md` 7.4), title
from `Application.Title`, resizable with snapping, with `SDL_WINDOW_HIGH_PIXEL_DENSITY` off so that
one back-surface pixel is a whole number of screen pixels. `IDisplayHost` (`graphics.md` section 8)
is implemented over an `SDL_Renderer` with a streaming `SDL_PIXELFORMAT_XRGB1555` texture; `Present`
uploads and draws; `ClientSize` reports the window's client size divided by the scale;
`PointerPosition` maps the pointer through the scale and the letterbox; `HasFocus` is the keyboard
focus flag; `ShowSystemCursor` toggles `SDL_ShowCursor`.

`Present` waits for the vertical blank (`SDL_SetRenderVSync` with interval 1), so a scrolled frame
never tears. The game's limiter (`graphics.md` 7.2) keeps its 33 ms target, which is two refreshes
at 60 Hz. The port's own setting `Video/VSync` (section 8) turns the wait off at 0, and
`--no-vsync` does the same for one run. A driver without vsync logs the fact and presents without it.
The headless host (section 10) has no renderer and is unaffected.

`PumpEvents` is `Application.ProcessMessages`: it drains the SDL queue and translates:

| SDL event | delivered as |
|---|---|
| mouse button down | `DisplayMouseDown(Sender, Button, Shift, X, Y)` with `mbLeft` or `mbRight` (the game tests both, 32 sites each; the middle button is ignored as the original ignored it), `Shift` with `ssLeft` or `ssRight` plus `ssShift`, `ssCtrl`, `ssAlt` from the modifier state, and `ssDouble` on the second press of a double click. A double click is a second press of the same button within the system double-click time (500 ms) and 4 pixels of the first; the VCL then sent `DblClick` before the second `MouseDown`, so the host calls `DisplayDblClick` and then `DisplayMouseDown` with `ssDouble` |
| mouse button up | `DisplayMouseUp` with the same conventions |
| mouse motion | `DisplayMouseMove(Sender, Shift, X, Y)`; when the pointer leaves the image, one `MouseMove([], -1, -1)` as the display control did |
| mouse wheel | ignored; the original had no wheel handling (one `WM_` reference in the game code, none for the wheel). A later option can map it to the scroll bars |
| key down | `DisplayKeyDown(Sender, Key, Shift)` with a Windows virtual key code. The game tests `VK_ESCAPE` (23 sites), the arrows, `VK_SPACE`, `VK_RETURN`, `VK_DELETE`, `VK_PRIOR`, `VK_NEXT` and `VK_F1`, `F3` to `F8`, `F11`, `F12`, and compares letter keys against `Ord('A')` style constants, so the mapping covers the full virtual key set: letters and digits from the SDL keycode (upper-cased ASCII), the rest from a table of SDL scancodes to `VK_*` values. Key repeat is delivered as the original received `WM_KEYDOWN` repeats |
| text input | `DisplayKeyPress(Sender, Key)` with the character converted from UTF-8 to code page 1252; a character outside the code page is dropped. The control characters the VCL delivered through `WM_CHAR` are synthesized from key down: `#13` for Return, `#8` for Backspace, `#27` for Escape, `#9` for Tab |
| window resize, focus gained or lost, close requested | `TCustomDisplay.Resize`, `Application.Active`, `FormCloseQuery` |

`ScrollMap` scrolls when the pointer is at the window edge and `Application.Active`; the host's
focus flag is that value. Text input is enabled while the window has focus.

## 4. Clock and pacing

`IClock.ElapsedMilliseconds` is `SDL_GetTicksNS` divided down (the original's
`GetElapsedMilliSeconds` used `QueryPerformanceCounter`); `IClock.WaitMilliseconds` sleeps with
`SDL_DelayPrecise` for all but the last 2 ms of a wait and spins the rest, which the frame limiter
of `graphics.md` 7.2 relies on. `Sleep(0)` and `Sleep(1)` in the frame procedure become a yield and
a 1 ms delay. `Now`, `Date` and `Time` do not occur in the game code; `timeGetTime` occurs only in
the editor's profiling and the DirectPlay ping (`testing.md` 3.3). The headless host's clock is fixed
(`testing.md` 3.2).

## 5. Audio

### 5.1 The contract

`Sound.pas` is transpiled: `TSampleData`, `TSample`, `TSoundSystem` (abstract), `TSongPlayer`,
`TSonglist`, the sample cache (`LockSampleData`, `CreateSampleData`, `PlaySample`, `PlaySampleEx`)
and the unit routines (`InitializeSoundSystem(Name, Settings)`, `ProcessSound`, `SetMusicVolume`,
`FadeMusicVolume`, `PlaySong`, `PlaySongEx`, `StopSong`, `BackupSongStatus`, `RestoreSongStatus`
and the rest). `GalSndS.pas` and `Galaxy.pas` are dropped and `TSdlSoundSystem` registers itself
under the name `'Galaxy'` as well as `'SDL'`, so the transpiled `FormCreate` finds it. The
behavior it must reproduce is the Galaxy backend's, read from `GalSndS.pas`:

| operation | Galaxy backend | port |
|---|---|---|
| `Activate(Settings)` | `glxInit`, sample volume 127, music volume 127, then `SetSoundSettings` | open the device |
| `SetSoundSettings` | output flags mono or stereo, 8 or 16 bit, high quality; `SampleVoices` (16 in the game); `SampleRate` 22050 or 44100; a 200 ms buffer with DirectSound, 20 ms without | an `SDL_AudioStream` at the requested rate and format, 16 voices, buffer about 50 ms. The four quality presets the main form builds from the `Sound Quality` setting (1: mono 8-bit 22050, 2: mono 16-bit 44100, 3: stereo 16-bit 44100, 4: stereo 16-bit 44100 high quality) are kept |
| `SetMusicVolume(V)` | clamps 0..100, scales to 0..127, applies at once unless a fade-in is pending | the same scale, applied to the song's gain |
| `SetSFXVolume(V)` | 0..100 to 0..127 | master gain of the sample voices |
| `FadeMusicVolume(Volume, Speed)`, `PlaySongEx(File, FadeOut, FadeIn, Loop)` | Galaxy ramps the music volume with a speed parameter; a song change with `FadeOut` sets the status `pssWaitingForFadeIn`, ramps the volume to 0, and `Process` starts the new song with `FadeIn` once the ramp reaches 0, restoring the saved order position with `glxControlMusic(1, Order)` | the same state machine, transcribed. The ramp speed unit of Galaxy is not documented; the port ramps linearly over `Speed` frames of its mixer callback and tunes the constant by ear against the original (section 11) |
| `PlaySong` | stops and unloads the current music, opens `BaseSoundEnginePath + FileName`, loads the module, loop flag through `glxControlMusic(3, 0 or 1)`, starts | libopenmpt: load the `.IT`, set repeat count 0 or infinite, render into the mixer |
| `BackupSongStatus`, `RestoreSongStatus` | file name, loop, volume and order position | the module's current order from libopenmpt |
| sample `Play` | panning left 0, center `$4000`, right `$7FFF`, surround `$8000`; volume `Round(V * $7FFF / 100)` or the sample's own; looped samples set the loop over the whole sample; `glxStartSample` on a free voice | a voice with the same gain and pan, surround treated as center |
| `SetVolume`, `SetPanning` on a playing sample | `glxControlSample(Voice, -1, Volume, Pan)` | adjust the voice |
| `Stop`, `GetPlaying` | voice stop and status | the same |
| `TSampleData.Create(Name)` | `glxfOpen(BaseSoundEnginePath + Name)` and `glxLoadSample`: a RIFF WAV file | the WAV reader of section 5.2 |
| `CreateEx(Format, Data, Size)` | a raw PCM buffer with a `WAVEFORMATEX` (the video player's audio) | the same buffer path |

### 5.2 The assets

`Sfx\` holds 214 WAV files (59 MB): 147 stereo 16-bit 44100, 58 mono 16-bit 44100, 6 at 22050 or
odd rates (39062, 28409), 2 mono 8-bit 16000, and one stereo 8-bit 44100 file in mu-law (format tag
7). The reader accepts PCM 8 and 16 bit and mu-law, any rate, and the mixer resamples to the output
rate with linear interpolation. `Songs\` holds 24 Impulse Tracker modules (22 MB; `TITLE.IT` for the
title scene, `Combat\` for battles, `Special\Sunset.it`). The game refers to sounds by relative path
(`'SFX\EVENTS\NEWEVENT.WAV'`, `'Sfx\Int\Pagef1.wav'`, `'Songs\Combat\Defend.IT'`) with inconsistent
case, resolved by the file service of section 7.

### 5.3 Fidelity

Music is not held to the oracle: libopenmpt's rendering of an `.IT` differs from Galaxy's in
interpolation and effects, and `../design.md` open question 4 accepts that. Sound effects are a
mix of the original samples at the original gains and pans, so their content matches up to
resampling. Neither affects the save or frame oracles.

## 6. Video

`Video.pas` (665 lines, Video for Windows) is rewritten as `TVideoPlayer` over an `IVideoDecoder`
with the same public surface: `Open(FileName)`, `Play`, `Stop`, `Pause`, `Resume`, `Update`,
`Show(X, Y, SD)`, `Position`, `EndPosition`, `Status`, `VideoFormat` (width, height), `SoundFormat`,
`Close`. `IntroScene` uses exactly these: it opens `IMAGES\SCENES\INTRO.AVI`, plays, sets `Position
:= -500` (a half-second lead), and per frame calls `Update` and `Show` centered in the back surface,
finishing when `vpsPlaying` leaves the status. The file is 94 MB: 640 by 480, 15 frames per second,
2,033 frames, video `IV32` (Indeo 3.2) at 24 bits, audio PCM stereo 16-bit 22050. The decoder
implementation binds FFmpeg's `libavcodec` (`indeo3`) and `libavformat` when their shared libraries
are present next to the port or on the system path; the frames are converted to the back surface's
555 format with `RemapSurfacePart`'s path, and the audio goes through `TSampleData.CreateEx`. Without
the libraries `Open` returns `False` and `IntroScene` finishes at once, which is what the original did
when the codec was missing.

## 7. Files and paths

`IRtlHost.Files` is a virtual file system with these rules:

1. Two layers: the game root (read only) and the user data directory (read and write). A read looks
   in the user layer first, then the root, so a user file shadows a shipped one of the same
   case-insensitive relative path. A write goes to the user layer, creating directories, and a
   shipped file is never written. Directory listings merge both layers. Reading, listing and
   previewing copy nothing into the user layer. On Windows the user layer defaults to
   `%LOCALAPPDATA%\OpenAoW`, on Linux `$XDG_DATA_HOME/openaow` (`~/.local/share/openaow` when the
   variable is unset or not absolute), on macOS `~/Library/Application Support/OpenAoW`
   (`SdlHost.DefaultUserDirectory`). `--user-data <dir>` picks another user directory, and
   `--single-directory <dir>` puts both layers on one directory, which is how the oracle runs
   reproduce the original's layout (`testing.md`). The Headless host layers the game root under
   the run directory.
2. Case-insensitive names, both separators. The source spells `Int\`, `INT\`, `Sfx\`, `SFX\`,
   `IMAGES\SCENES\INTRO.AVI` against an install that has `Images\Scenes\Intro.avi`. The service
   resolves each path component against the directory's entries with an ordinal case-insensitive
   compare and caches the directory listings. The game root's own tree is read once at startup.
3. An emulated current directory. The game calls `SetCurrentDir` 20 times and `GetCurrentDir` 8
   times: to the startup directory, to `Scenario\Custom\`, `Scenario\Campaign\`, `Int\Scenes`,
   `EmailIn\`, `Songs`, and the file dialog (`AoWFileDialog`) browses with `SetCurrentDir('..')`,
   drive letters and `ExtractFileDrive`. The shim's `SetCurrentDir`, `GetCurrentDir`,
   `ExpandFileName` and `ExtractRelativePath` work on a current directory kept by the service,
   never on the process's. The drive list offered to the file dialog is one virtual drive, the
   game root, on every platform, and `ExtractFileDrive` returns its letter.
4. `FindFirst` and `FindNext` (16 and 9 sites) return entries in upper-cased name order
   (`rtl-shim.md`), merged over both layers, with `faDirectory` honored.
5. The directories the game uses: `Release\` (`RELEASE.HSS`, `GENERAL.PFS`), `Scenario\Custom\`,
   `Scenario\Campaign\`, `TCMaps`, `Save\` (saves and autosaves, user layer), `User\` (`.ahl` hero
   and `.ail` item lists, user layer), `EmailIn\` and `EmailOut\` (play by e-mail, user layer),
   `Images\`, `Int\`, `Dict\`, `Sfx\`, `Songs\`, `DEBUG\` (sync logs when enabled, user layer),
   `PLUGINS` (section 9). `.hsm` maps and `.hss` sets are listed by a recursive scan
   (`HSEngine.pas:4968`) that reads each candidate's header, so every `.hsm` under the scenario
   directories is found regardless of name.
6. Mobile later: the root layer is a zip the user supplies (`../design.md`), read through the same
   interface; the listing and the case-insensitive lookup come from the archive's central directory.

`TFastLoadFileStream` and `TFileStream` open through the service. `DeleteFile` (4 sites: user lists,
e-mail attachments) acts on the user layer only.

Saves, maps, campaigns, the scenario set and the user lists pass through the game's file boundary
(`own-formats.md` 4.1 and 4.2), which layers two services on this one:

- Replacement. `IFileSystem.ReplaceFile(source, target)` moves a finished, closed file over its
  target in one step. The default implementation returns false. `LayeredFileSystem` resolves both
  paths in the user layer, so the move stays on one volume and replaces only the user copy, and
  `SystemIoFileSystem` moves with overwrite. A save converts, validates and serializes the
  document first, then writes the sibling temporary `<name>.openaow-tmp` through the service,
  flushes it to the disk, closes it and replaces the target. A failure at any step keeps the old
  file and removes the temporary, and a service without the primitive refuses the save, so a
  save never truncates the file it replaces. An original save in the user layer saved under its
  own name becomes an own document. A shipped file saved under its name gets a user copy that
  shadows it.
- The file log. `IFileSystem.Layer(path)` names the layer a path resolves to (`root`, `user`,
  `single` or `none` in `LayeredFileSystem`, `input` for the Headless host's command-line save),
  for logs only. `OpenAoW --file-log <path>` and the Headless host's `--file-log <path>` write one
  JSON line per open, preview, header listing, CRC read and write at the boundary, refusals
  included: the operation, the path as the engine names it, the layer, the profile and the
  caller's use, the kind, the detected format, the version, the G1 result or why it does not
  apply, the validation outcome, the error code and diagnostic, and whether a preview came from
  the session cache. Paths appear in the log and in diagnostics, never in a document.

## 8. Settings

`TAoWRegistry` (transpiled) reads and writes these values through `rwInteger`, `rwBool`,
`rwString`, `rwRect` and `rwBinaryData` under `\Software\Triumph Studios\Age of Wonders\<Section>`:

| section | values |
|---|---|
| `General` | `Debug Mode`, `Skip Intro`, `Auto Save`, `Auto Debug Save`, `Fast Movement Enabled`, `Log Token Events`, `Language`, `Root Directory`, `Startup Directory`, `Serial number`, `D`, and the binary blocks `Classic Window Popup Events`, `Simultaneous Window Popup Events`, `Combat Resolve Mode`, `Movement Speed`; plus the generic indexed stores `General\rectangles`, `booleans`, `strings`, `integers` used by the window layout and the multiplayer names |
| `Video` | `Display Mode`, `Display Width`, `Display Height`, `Allow Resolution Switches`, `Shadows Enabled`, `Surface In VMem` |
| `Sound` | `Sound Quality`, `Music Volume`, `SFX Volume`, `Music Enabled`, `Sound Effects Enabled`, `Use DirectSound` |
| `Workspace` | `Detail Level`, `Main Window State`, `Main Window Position`, `Map Window Position` |
| `Email` | `SMTP Server`, `POP3 Server`, `POP3 UserName`, `POP3 Password`, `Local Email Address`, `Attachment Directory`, `Check Email Frequency` |

The shim's `TRegistry` stores them in `settings.json` in the user layer as `Section/Name` with the
value's kind (integer, boolean, string, rectangle, bytes); a missing value reads as the original's
default, which the transpiled getters supply. `AssociateAoWMap` (file association under
`\Software\Classes`), `RegisterDPlayLobbyServer` and `GetDeveloper`'s GUID key are no-ops.
`Display Mode` 1 maps to the fullscreen flag, `Use DirectSound` is read and ignored. `Video/VSync`
is the port's own setting, absent from the original: missing or nonzero means vsync on (section 3).

## 9. Message boxes, plug-ins, networking, e-mail

Message boxes. `ShowMessage` has 173 call sites (AoWEPACK 105, AoW 43, AoWTCPCK 23), `MessageBox`
82 (AoWTCPCK 37, aowInt 36, AOWTools 7, AoWEPACK 2), `MessageDlg` 4. Most report exceptions or
engine errors, some are game messages (`GameSavedRStr`, `NoHeroAnswersYourCallRStr`). All go to
`IDialogService` (`rtl-shim.md`), which the desktop host implements with `SDL_ShowMessageBox`: a
modal call that blocks the game loop exactly as the VCL dialog did, returning the button for the 20
result comparisons. The headless host answers the default button and logs.

Plug-ins. `LoadExternalPlugIns(StartupDirectory + 'PLUGINS')` scans for `.DPL` packages and loads
them with `LoadPackage`. The shipped game has no `PLUGINS` directory, and `AbilityP.dpl` in the game
root is loaded by neither executable (`data/initialization-order.json` lists 21 statically loaded
packages, AbilityP not among them). The port's `LoadExternalPlugIns` scans and finds nothing; the
`TPlugIn` registry stays as transpiled engine code with the one built-in plug-in
(`TAbilityPlugIn` created by `TAoWEngine`), and `AbilityP` and `Adcpack` are outside the transpile
set. Code mods (`../design.md` section 6) get their own entry point later.

Networking. The engine's transport hook is small: `TRemotePCC.OnSendData(Data, Size, SystemID)` sends
a package and `ReceiveDataPackage` takes one in; `aowSetupEvents` wires both to `TGDPlay`
(`aowDPlayComp`, 68 call sites in `AoW`, none in the rules). The host supplies a `TGDPlay` stand-in
whose `GetConnections` returns no service providers, so the multiplayer window shows an empty list
and every other method returns a failure code, until `networking.md` puts a socket transport behind
the same component.

E-mail play. `ims.dpl`'s `TmsSMTP`, `TmsPOP3` and `TmsMessage` components get stand-ins that load
from the form files and fail every operation, pending `../design.md` open question 7.

## 10. The headless host

`src/Headless` (`testing.md` 3.2) implements `IRtlHost` with a fixed clock, a null display (no
`TSdlDC`, `TCustomDisplay` not created, `DisplayUpdateFrame` runs with `DC = nil`), a null sound
system registered under `'Galaxy'` that accepts every call, the scripted dialog service, the same
file service with both layers on the run directory, and settings from a JSON given on the command
line. It shares every line of the game and the shim with the desktop host; only the service
implementations differ.

## 11. Done measures

- P1: the port starts from the GOG data directory on Windows and Linux, plays `TITLE.IT`, shows the
  title scene, and the frame fixture of the title scene (`graphics.md` section 9) compares equal.
- P2: the input mapping table is covered by a unit test for every virtual key the game tests and
  for the double-click synthesis.
- P3: every setting of section 8 round-trips through the shim's `TRegistry` with the type the
  transpiled getter expects, and a missing setting yields the default.
- P4: the file service resolves every path literal of the game code (the 45 most frequent are
  listed by `tools/survey/literals.py`) against the GOG tree, case-insensitively, and the current
  directory emulation passes the file dialog's browse sequence.
- P5: all 214 WAV files load and decode, all 24 songs play for five seconds without error, and the
  fade state machine passes a unit test transcribed from `GalSndS.pas`.
- P6: the intro plays with FFmpeg present and is skipped cleanly without it.

## 12. Open points

1. The Galaxy fade speed unit. Settled by ear against the original (title music fade on game
   start) once the port runs.
2. Whether to bundle FFmpeg on desktop or leave it optional. Optional is the default.
3. Mouse wheel mapping to scroll bars as a convenience, off by default.
4. Mobile input and the zip root are not specified yet beyond the file service's interface.
   `../design.md` open question 10 records what is known: the toolkit's minimum is 640 by 480 and
   it reflows above that, a tap is a left click, tap and hold is a right click, and the one drag
   gesture (multi-unit selection) needs its own gesture or a button. Specified after the desktop
   release.
