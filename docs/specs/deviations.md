# Deliberate deviations from the original

The port fixes bugs of the original where the user decides so. Bug-for-bug compatibility is not a
goal in itself (decision of 2026-11-26, issue 396). This file lists each deliberate deviation:
the issue, what differs, and how each test layer that compares the port with the original
handles it. A deviation is one of three kinds of handling in a layer:

- **unaffected**: the layer's inputs never reach the difference, or both sides agree on them.
- **emulated**: the layer's hosts run with the test-only switch `AOW_EMULATE_ORIGINAL_BUGS=1`.
  The hosts read it at entry (`GateSwitches.EmulateOriginalBugs`), and the engine's loads take
  it as the default of their load context (`IEnginePersistableFactory.EmulateOriginalBugs`).
  Only the drivers that compare the port with the original set it, each in the one place that
  starts its host process. The game's own loads, the playtest builds and the crash checks run
  without it. Emulation serves a difference that later game state depends on, where a comparer
  rule on one save cannot hold the following days equal.
- **comparer rule**: the comparison ignores or maps the difference. It serves a difference that
  stays inside the compared output and changes no later state.
- **recaptured**: the reference of a fixture is a new capture of the original in a setup where
  both rules give the same result.

## 1. Bit lists read past their stored bits (issue 396)

**Original.** `TBitList.LoadFromStream` raises a stored count to its capacity, a multiple of 32,
and then reads the bit bytes of the raised count (`Engine.pas` 4743 to 4789). A list whose count
does not fill 32 bits, as a first `Put` on an empty list leaves it, takes up to 3 bytes of the data
that follows it in the stream as bits. That is the data of the next member of the owner, or data
outside the owner. The three stored bit lists are `Sign.TextSeen`, `PlayerStructure.VictoryLocations`
and `Player.AIValidAttackPlayers`. A spurious attack bit changes later AI decisions. A spurious
seen bit changes the sign animation of the seated player.

**Port.** A load takes the stored bits. The count is raised to the capacity as `SetCapacity`
raises it on every growth, and the bits past the stored ones keep the default
(`persistence-tree.md`, `BitList`). The own format stores the count and the bits a list holds
(`own-formats.md`). Under the switch the load takes the bytes the original takes: from the image
on an image load (`BitListNode.ReadPast`), and from the owner's following members for
`Player.AIValidAttackPlayers` on a document load (`PersistSupport.BitListOverRead`).

**Census (2026-11-26).** Short lists are in 3 saves of the installed game (13 attack lists in
`Save/After the Fall.asg`, one sign in each of `Save/After the Fall 1.asg` and `2.asg`), in the
oracle starts `campaign_transfer.acg` (one sign), `pbem.asg` and `pbem_source.asg` (one attack
list each), and in 59 of the 171 own saves of the user's 2026-11-26 playtest snapshot (103 signs
in 54 files, 50 attack lists in 15 files). No `VictoryLocations` has a short count.

| layer | handling |
|---|---|
| product tests | The tests pin the game's load (stored bits only) for all three lists and the emulated load with a factory that sets the switch (`PersistEquivalenceTests`, `ListShapeTests`). The persistence equivalence and corpus tests compare two port paths that share `PersistSupport.BitList`, so they run without the switch and prove the game's load |
| persistence | unaffected: G1, G2, the own round trip and the converter check move the stored count and bits, never a load's raised list |
| oracle | emulated: `tools/oracle/run.py` starts each headless host with the switch, so the scenario runs and the own resumptions (which run through `run.py`) load as the original does. The play-by-email and campaign transfer starts hold short lists, and their later days depend on the attack bits. The campaign victory start stores the sign at count 32 (`prepare_campaign_victory.py`), so it loads alike with or without the switch |
| scenarios, frames | emulated: `tools/oracle/verify_frames.py` starts each game host with the switch, because the fixtures compare state with the original's |

## 2. The battle camera follows the seated player's units (issue 346)

**Original.** After the battle AI moves a unit, the camera centres on it when the unit's combat
slot equals the seated player's game player number (`AoWTC.pas` 2073 to 2132). The two numbers are
of different kinds, so the camera follows a unit only when they happen to be equal.

**Port.** The fix compares the combat slot with the seated player's combat slot. View only: no
game state or save changes.

| layer | handling |
|---|---|
| frames | recaptured: the `actions-tactical-auto` fixture is a new native capture of a battle where the seated side's combat slot equals its game player number, so both rules centre the camera on the same units (decision of 2026-11-25). The old fixture differed only in the order edge hexes were first drawn, which picks a decoration variant |
| oracle, scenarios | unaffected: the camera changes no save and no state the comparisons read |

## 3. The item library checks library ids for duplicates (issue 346)

**Original.** `TItemLibrary.AddChild` renumbers an added item when `FindItem(FLibraryID)` finds a
match (`Army.PAS` 4909 to 4913). `FindItem` searches world item ids, so two library items can share
one library id, and an item whose library id equals an unrelated world item id is renumbered for
no reason.

**Port.** `ItemLibrary.AddChild` searches the library's own ids (`FindLibraryId`) and renumbers
only a real duplicate (`ItemLibraryIdTests`).

| layer | handling |
|---|---|
| oracle, scenarios, frames, persistence | unaffected: no compared run adds an item to a library whose library id matches a world item id or another library item (the gate of issue 346 passed with no rule) |
