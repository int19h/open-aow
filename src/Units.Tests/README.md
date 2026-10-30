# Product construction tests

`Aow.Units.Tests` holds the game-unit, registry, layout and construction tests.
`Aow.Rtl.Tests` holds the RTL and platform-service tests. Both projects belong to
`OpenAow.slnx`. Assembly attributes live in each project's `AssemblyInfo.cs`.

`EngineFixture` starts the real engine over its temporary recording host. The host
confines file access to that root and supplies in-memory settings and dialogs.
Each engine test restores the engine fields and services. The fixture owns the
startup game set, the game-set factory for isolated cases and the shared owner
membership double. Specialized observers retain their insertion timing checks.
The startup recording uses the same host in a fresh process to isolate lazy caches.
No fixture reads `AOW_GAME_ROOT` or the user's installation.

## Readable expectations

The files under `Snapshots/` replace the construction state hashes. Each case has
named fields, child state, owner observations and teardown results. `$jsonText`
expands serialized JSON text into named values. `$lines` preserves the sequence of
recorded lines. `$prefix` and `$value` preserve the success marker and its state.
`$sameAs` shares a complete expectation between cases with identical recordings.
All routes still execute and compare their complete recording. A failure names the
case and the first differing field path.

The original assertions passed on base commit
`cab46c36382414c4e0cb744ecb4ee4bd39479d9a` while their exact inputs were captured.
Every recorded hash resolved to a captured input. The readable replacements then
passed all 3034 tests against the unchanged product code. The project split changes
test namespaces and owner-double names in the recordings. Owner observation fields,
membership, removal counts and exception checks remain present.

`EngineStartupSnapshot.json` and `CombatMapConstructionSnapshot.json` retain the
readable startup and ordering recordings. `ImageClassSnapshot.json` retains the
image registry recording. `EClassBaseline.json` remains the readable class-id table.

## Run and regenerate

From the product root, set `AOW_PRIVATE` to the private checkout for the RTL probe
fixtures. Without it, the RTL tests look for a sibling `open-aow-private` checkout.
Missing fixtures report the resolved root and the variable in the test skip output.

```powershell
$env:AOW_PRIVATE = 'D:\git\open-aow-private'
$env:AOW_GAME_ROOT = 'D:\Games\GOG\Age of Wonders'
$env:AOW_MUTE = '1'
$env:SDL_AUDIODRIVER = 'dummy'
dotnet build OpenAow.slnx -m:1
dotnet test OpenAow.slnx --no-build -m:1 --blame-hang --blame-hang-timeout 90s
```

To regenerate the files under `Snapshots/`, set `AOW_CONSTRUCTION_RECORD` to an
empty scratch directory and run the unit project with the same environment.
The switch writes readable files there in place of snapshot comparisons. Other
assertions still run. Review the output against the checked-in expectations,
copy the accepted files into `Snapshots/`, clear the switch, rebuild and run again.
The three retained recordings outside `Snapshots/` use their existing readers.

```powershell
$env:AOW_CONSTRUCTION_RECORD = Join-Path $env:TEMP 'aow-construction-recordings'
dotnet test src/Units.Tests/Aow.Units.Tests.csproj --no-build -m:1 --blame-hang --blame-hang-timeout 90s
Remove-Item Env:AOW_CONSTRUCTION_RECORD
```

The base count is 3034. The split count is 3036, comprising 148 RTL tests and 2888
unit tests. Two reflection-only absence tests were removed. Three private-root
checks and one engine-root check were added. Guarded-family failure tests remain.
