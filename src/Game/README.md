# Aow.Game

The game starts the 77 layout modules in AoW.dpr order, creates TMainForm, loads
RELEASE.HSS and draws the title scene through General.Manager and TSdlDC. Audio
uses the null backend. The SDL window presents a streaming RGB555 texture with
integer scaling. Alt+Enter toggles desktop fullscreen.

```
dotnet run --no-build --project src/Game -- --game-root "D:\Games\GOG\Age of Wonders" --scale 1
```

Files and typed JSON settings go to the user data directory. `--user-data PATH`
selects that directory. `--single-directory PATH` selects one directory for both
layers. `--fullscreen` overrides the saved display mode.

Automated rendering uses the headless presenter. It initializes neither SDL video
nor audio and writes tightly packed, top-down RGB555LE rows plus JSON metadata.
The title reference numbers presentations, including the initial paint of the
cleared back surface. Presentation 60 contains title scene update 59.

```text
dotnet run --no-build --project src/Game -- --fixture title-60 --fixtures D:\git\open-aow-private\tests\frames\fixtures.toml --out D:\git\open-aow-private\tools\oracle\obj\3.4-title
dotnet run --no-build --project src/Game -- --smoke-test
dotnet run --no-build --project src/Game -- --verify --source-root D:\git\aow\src\delphi3
```

The fixture mode reads its dimensions, pointer, seed, clock and registry inputs
from the TOML file. It currently accepts title scenes. Scripted map and tactical
fixtures remain outside this milestone. The selected frame is recorded through
the same display controller and frame loop as the interactive run.

`--verify` includes memory-only display and presenter checks. `--verify-sdl`
exercises SDL with its dummy video driver and hidden window.
