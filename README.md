# OpenAoW

A source port of Age of Wonders (1999, version 1.36) to C# on .NET, built from the recovered
Delphi 3 source of the game. Desktop first, including Linux, with mobile platforms planned after
the desktop release.

The port ships none of the game's files. It needs an installed copy of the original game (the GOG
release is the reference) and reads its images, sounds, dictionaries, maps and saves in place.

State: the translated game plays on Windows and Linux, and the idiomatic rewrite is under way.
`docs/design.md` holds the decisions, `docs/specs/` the specifications, `docs/milestones.md` the
order of work and `docs/process.md` how the work is done. The .NET solution is under `src/`.

The plan has two stages. The first translates the recovered source mechanically and proves the
result against the rebuilt original, turn for turn on seeded games and pixel for pixel on rendered
frames. The second rewrites the translated tree into idiomatic C# piece by piece, with the same
oracle after every step.

License: GNU General Public License version 3 (`LICENSE`). The license covers the port's own code
and documents, not the original game's files, which the port never contains.

## Native libraries

The music player, libopenmpt, comes from vcpkg and is built from source on first use, never
checked in. `vcpkg.json` pins it. From the repository root, once:

```
vcpkg install
```

This writes `vcpkg_installed/`, which the build copies next to the game. Visual Studio ships
vcpkg (`VC\vcpkg\vcpkg.exe` under its install directory), and on Linux the vcpkg checkout
works the same way with the `x64-linux-dynamic` triplet. Without the libraries the game builds and
runs silent.

## Running

```
dotnet run --project src/Game -- --game-root <install directory> [--scale 2] [--fullscreen] [--mute]
```

Saves and settings go to a separate user directory, never into the install:
`%LOCALAPPDATA%\OpenAoW` on Windows, `$XDG_DATA_HOME/openaow` (by default
`~/.local/share/openaow`) on Linux. `--user-data <directory>` picks another one.

## Linux

SDL3 comes with the `ppy.SDL3-CS` package. For music, install the distribution's libopenmpt
(`libopenmpt0` on Debian and Ubuntu) or build it with vcpkg as above. The game loads
`libopenmpt.so.0`.

Under WSL with WSLg and systemd, `/run/user/1000` has no `wayland-0` socket and `/tmp/.X11-unix`
is empty, so SDL finds no display. X11 works after linking WSLg's socket:

```
sudo mkdir -p /tmp/.X11-unix && sudo ln -sf /mnt/wslg/.X11-unix/X0 /tmp/.X11-unix/X0
DISPLAY=:0 SDL_VIDEO_DRIVER=x11 dotnet run --project src/Game -- --game-root "/mnt/c/GOG Games/Age of Wonders"
```

Over WSLg's Wayland socket (`XDG_RUNTIME_DIR=/mnt/wslg/runtime-dir`) the game runs but its window
does not appear yet (#54).
