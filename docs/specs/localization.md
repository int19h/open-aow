# Localization specification

State: draft, 2026-10-25. Covers the Multilizer dictionaries (`Dict\*.mld`), the engine's three
translation paths, the translator components on the forms, what the shipped English game does with
all of it, and how the port reproduces that while making the four other languages available. The
file format itself is documented in `data/localization/mld-format.md` of the private repository
(`../design.md` section 8), reversed from the shipped `Localize.dpl` on 2026-10-25 with a reference
parser in `tools/localization/mld_dump.py` and the content analysis in
`data/localization/mld-analysis.md`, both there too. The recovered Localize units
(`IvDictio`, `IvBinDic`, `IvAMulti`, `IvMulti`, 303 lines) are stubs with empty bodies, so this part
of the port is written from that format document, not transpiled.

## 1. Decisions

1. The port implements the dictionary and the translator itself: a reader for the format, the
   lookup the original performs (an exact, case-sensitive binary search over the loaded column),
   and the component walk the translator performs after a form loads. These replace the four stub
   units under their original class names (`TIvDictionary`, `TIvBinaryDictionary`,
   `TIvCustomTranslator`, `TIvTranslator`), so the transpiled callers (`TAoWDictionary`,
   `TAoWEngine`, the 62 translator components) compile unchanged.
2. English is exact by construction. Across the 6,683 entries of the eight dictionaries exactly one
   English translation differs from its source, `Undead(plural)` to `Undead` in `ResStr.mld`, and
   `TranslateRStr` hard-codes the same substitution for language 0. The port loads `ResStr.mld` at
   startup as the original does and translates through it; the result for English players is the
   source text everywhere plus that one substitution, which is what the shipped game shows.
3. The four other languages are offered as a setting. The shipped code forces English twice
   (`TAoWEngine.Create` maps every language to 1, `TAoWRegistry.GetLanguage` returns `alEnglish`
   after reading the setting), although the dictionaries carry complete German, French, Italian and
   Spanish columns and the map, campaign and ability loaders already pass the registry's language
   through. The port keeps the forced behavior as the default and adds a `Language` setting that,
   when it is not English, replaces the two forcing points by overrides with the natural mapping.
   This is a deliberate extension outside the oracle; it changes no save and no English frame.
4. Text is `string` with one code page 1252 char per byte (`strings.md` 2.1, `rtl-shim.md` 1.2),
   so the dictionaries, the resource strings and the fonts' glyph indices keep their bytes. The
   dictionaries are bytes on disk, and the reader decodes each string through `Cp1252.Decode`. A
   dictionary stored as UTF-16 enters through `Cp1252.Normalize`. The order of section 3 is the
   exact byte comparison through `Cp1252`: the sort of the entries passes `Cp1252.Comparer`, and the
   binary search calls `Cp1252.Compare`. This differs from UTF-16 ordinal order for the chars of
   bytes 128 to 159, which the eight files use (`strings.md` 2.4). A test of the `localization`
   layer asserts that the sorted entries of each file are in `Cp1252.Compare` order. Decided
   2026-10-26 with `../design.md` open question 1 and implemented by brief 9.5-1 (2026-11-05).

## 2. The dictionaries

Eight files in `Dict\`, 6 KB to 675 KB. Per `mld-format.md`: a 42-byte header (`MLD`, version 3,
byte order, character set, a flag that says entries carry two context strings), six language
records (Native, en-US, de-DE, fr-FR, it-IT, es-ES, code page 1252), the entries, and a trailer
with description, owner and save time. An entry is eight strings, each a 16-bit length and cp1252
bytes: source text, form class name, component name, then the five language columns; an empty
translation is an empty string. The parser consumes every byte of every file.

| file | entries | used by | what it holds |
|---|---:|---|---|
| `ResStr.mld` | 1,164 | `TAoWEngine` at startup, through `TranslateRStr` (987 calls in AoWEPACK, 514 in AoW, 8 in AoWTCPCK) | the resource strings of the code; 358 of the 368 distinct `resourcestring` values of the recovered source, the rest belong to the editor |
| `Release.mld` | 917 | `TAbilityControl` loading (`Ability.PAS:202`) | ability and release texts |
| `CampMaps.mld`, `CustMaps.mld` | 1,166, 1,107 | `TAoWHSMap.MainReadWrite` on a day-0 read, campaign or custom by `FGameType` | map texts: names, descriptions, messages |
| `Campaign.mld` | 394 | `TAoWCampaign` loading (`Army.PAS:6072`) | campaign texts |
| `AoW.mld` | 1,095 | the 62 `TIvTranslator` components of `AoW.exe`'s forms (`DictionaryName = 'AoW'`) | form texts with form class and component context; all 1,027 context rows match the recovered form files |
| `AoWSetup.mld`, `AoWEd.mld` | 75, 765 | the setup program and the editor | out of scope |

Known defects in the shipped data, which the port reproduces by using the files as they are: two
`ResStr` rows have NUL bytes in place of their first letters and can never match; `AoWEd.mld`
translates a context-free "Cancel" as "Kanada"; `AoW.mld` lacks 40 French, Italian and Spanish
entries of the city screen and party controls and one German entry.

## 3. Lookup

From the disassembly of `Localize.dpl` (`mld-format.md`): `Open` reads the file, keeps the source
text, the two context strings and the selected language's column, and sorts the entries with an
exact byte comparison of the source text (the on-disk order is not used). `Translate(S)` does a
binary search for `S` with the same comparison and returns the translation, or `S` itself when
there is no entry or the translation is empty. Case and trailing spaces matter. When several
entries share a source text, the search returns the one it lands on, so the port's sort and search
are the same algorithm, not a dictionary type: a stable sort by ordinal byte comparison and the
standard binary search that stops at the first equal element it probes, as the disassembly shows
(`mld-format.md` names the comparison and the probe order). The translator's context lookup
searches for source plus form class plus component name first and falls back to the source-only
lookup.

`SetLanguage(n)` selects the column (1 en-US to 5 es-ES; `-1` means automatic, which the shipped
game never reaches because the registry read is overridden). `SetLanguage` before `Open` selects
the column to keep; after `Open` the dictionary reloads.

## 4. The three translation paths in the engine

All three are transpiled code; the port only supplies the dictionary classes under them.

1. Resource strings. `TAoWEngine.TranslateRStr(S)` returns `FDictionary.Translate(S)` when the
   dictionary's language is above 0, otherwise `S`, except that `P2UndeadPluralRStr` becomes
   `P2UndeadRStr`. `FDictionary` is opened on `ResStr.mld` in `UpdateStartupDirectory`. Every
   user-visible string of the rules and the executable passes through it (1,509 call sites).
2. Object texts. `TTranslateStorageStream` (Engine) walks an object tree as a read-mode storage
   stream whose `rwString` and `rwStrings` translate in place instead of reading;
   `TTranslateTextStream` (AoWEPACK) binds it to a dictionary, joining string lists with `<eol>`
   for the lookup and splitting the result on `<eol>` or line feeds. `TranslateEObjectFromFile(Obj,
   File)` opens a `TAoWDictionary` on the file, sets the language from `FRegistry.GetLanguage`
   (`alEnglish` is 1, `alGerman` 2, `alFrench` 3, `alItalian` 4, `alSpanish` 5), translates, and
   closes. Maps call it at the end of a day-0 read, abilities at load, the campaign at load.
3. Form texts. Each module's `TIvTranslator` (`ui-layout.md` section 6) carries `DictionaryName`
   and `TargetsData`, the list of property names to translate per class (`Items`, `Text`,
   `ToolTipText` in the game's forms). After the module loads, `Translate` walks the module's
   components and, for each listed string property, replaces the value by the context lookup
   (source, form class, component name) with the source-only fallback. The dictionary is `AoW.mld`
   opened once per process and shared by the 62 translators.

In the shipped English game paths 1 and 3 are identities except `Undead(plural)`, and path 2 is an
identity, because every English translation equals its source. The port behaves the same by
running the same code over the same files.

## 5. The language setting

`Language` under `General` (`platform.md` section 8) is 0 automatic, 1 English, 2 German, 3
French, 4 Italian, 5 Spanish (`TAoWLanguage`). With the default (English, or automatic) the
transpiled forcing code runs as shipped. With another value two overrides apply:
`TAoWRegistry.GetLanguage` returns the setting, and the language table in `TAoWEngine.Create`
maps each language to its own column. `TranslateEObjectFromFile` and the translators then follow,
because they already read the registry. Two consequences the port states in its settings
description: the fonts of the English release have no glyphs for some accented characters of the
other languages until that is checked (open point 1), and a save written in one language holds
untranslated map texts only, so switching languages later is safe.

The automatic value (`alAuto`, `SetLanguage(-1)`) picks the system language in Multilizer. The port
maps it to English unless the system language is one of the four others.

## 6. What the port implements

`src/Rtl/Host/Localize/`:

- `MldDictionary`: the reader (header, language records, entries, trailer) with the byte-exact
  validation `mld_dump.py check` performs; `Open`, `Close`, `SetLanguage`, `Translate(S)`,
  `TranslateContext(S, Form, Component)` with the sort and search of section 3.
- `TIvDictionary`, `TIvBinaryDictionary`: the component shells the transpiled `TAoWDictionary`
  derives from, with `FFileName`, `FLanguage`, `SetDictionaryName`, `Open`, `Close`, `SetLanguage`,
  `Translate`, backed by one `MldDictionary`. `SetDictionaryName('ResourceStringDictionary')` is the
  name the engine gives its dictionary before `UpdateStartupDirectory` sets the file.
- `TIvCustomTranslator`, `TIvTranslator`: the component the forms create, with `DictionaryName`,
  `TargetsData` (read from the form file's list), and `Translate`, which the form loader calls
  after a module's `OnCreate` as the original translator did when its owner finished loading. A
  dictionary named `'AoW'` resolves to `Dict\AoW.mld` under the startup directory.

## 7. Tests and done measures

- L1: `mld_dump.py check` and the C# reader agree on every entry of the eight files, and the reader
  consumes every byte.
- L2: for every source text of `ResStr.mld`, `AoW.mld`, `Release.mld`, `CampMaps.mld`,
  `CustMaps.mld` and `Campaign.mld`, the port's `Translate` returns the column `mld_dump.py tsv`
  shows for each of the five languages, including the duplicate-source cases, where the expected
  answer is produced by a Python transcription of the same sort and search.
- L3: an English run of the title scene and the first map day shows the source texts (the frame
  fixtures of `graphics.md` section 9 cover this), and `Undead(plural)` renders as `Undead`.
- L4: with `Language` set to German, the title scene fixture renders the German texts of `AoW.mld`
  with no missing glyphs for the fixture's strings.

## 8. Open points

1. Glyph coverage of the ILB fonts for the accented characters of the four other languages. A scan
   of `FontModule`'s libraries against the characters used in the de, fr, it and es columns settles
   it; characters without glyphs render as 5-pixel gaps (`graphics.md` 6.6).
2. Closed on 2026-10-25 (`../correspondence/2026-10-25c-replies-from-recovery.md`): the recovered
   source spelled `P2FireDamageRStr` as "Fire damage" against "Fire Damage" in the binary and in
   `ResStr.mld`, and the recovery corrected it in `aow` commit d898ee4 together with two other
   string table differences its new `tools/rstrcheck.py` found. The transpiler still compares every
   `resourcestring` value of the transpile set with `ResStr.mld`'s source texts and reports a value
   that differs only in case, because the dictionary is a second source the recovery's check does
   not read. Three `AoWEPACK` strings (`CoPublishedByRStr`, `EuropeRStr`, `NorthAmericaRStr`) are
   NUL-filled in the original and never match in either build.
3. The automatic language value's mapping from the host locale.
