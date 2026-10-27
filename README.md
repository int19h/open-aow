# OpenAoW

A source port of Age of Wonders (1999, version 1.36) to C# on .NET, built from the recovered
Delphi 3 source of the game. Desktop first, including Linux, with mobile platforms planned after
the desktop release.

The port ships none of the game's files. It needs an installed copy of the original game (the GOG
release is the reference) and reads its images, sounds, dictionaries, maps and saves in place.

State: design stage. `docs/design.md` holds the decisions, `docs/specs/` the specifications,
`docs/milestones.md` the order of work and `docs/process.md` how the work is done. The source
tree under `src/` appears as the milestones are implemented.

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
vcpkg (`VCcpkgcpkg.exe` under its install directory), and on Linux the vcpkg checkout
works the same way with the `x64-linux-dynamic` triplet. Without the libraries the game builds and
runs silent.
