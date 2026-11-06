# Strings (milestone 9.5)

Amended by roadmap.md, 2026-10-30

State: draft, 2026-10-29, Fable. Revised 2026-10-30 by Claude (Opus) with the amendments of
`9.5-second-opinion.md` (section 7 lists each), ready to brief. Covers the retirement of the shim's
`AnsiString` in favour of .NET `string`, with code page 1252 bytes only at the data and font
boundaries, in one pass over the whole tree (`docs/design.md` open question 1 and section 9,
`docs/milestones.md` row 9.5). Done measure of the row: O3 holds, the frame fixtures hold. The base
of the pass is the commit section 3.5 names.

Implemented. Brief 9.5-1 merged on 2026-11-05 (PR #95, main `c55a631`). It ran on main `8949ebb`,
one merge past the P5 base `b6a1c3d`, which adds the 9.7-1 projects and leaves `src/Rtl` as it was.
Brief 9.5-2 read this document against the code the same day and corrected what the pass changed:
section 1 stays the survey before the pass, sections 2 to 4 name the code after it, and section 8
holds the result. The pass's record (the rule counts, every hand fix by member, the Layout
comparison, the test mapping and every `Cp1252` call site by boundary of 2.4) is
`data/rewrite/strings.md` in the private repository. Milestone 9.6 then replaced `TVarRec` with the
union `FormatArg` (`rtl-shim.md` 1.6) and moved the message and blend slots off `IntOrRef`
(`unions-and-kinds.md`), so where this document names `TVarRec` or those slots it names the code as
9.5 left it.

## 1. Where the tree stands

Surveyed on main, 2026-10-29, with the semantic counts of the tool's dry run over main `d31aa07`
(`strings.md`). `AnsiString` appears 6,338 times in 241 files of `src/Units`, 376 times in
`src/Rtl`, 17 in `src/Platform`, 16 in `src/Layout`, 10 in `src/Game` and `src/Headless`. The
shapes that matter: 3,332 `(AnsiString)"literal"` casts, 455 `AnsiString.FromBytes(...)` calls of
which 384 are a single literal byte (a Delphi `Char` meeting a string), 88 one-based index reads on
a string receiver (52 in Units), 4 `WithChar`, 6 `Resized` or `SetLength`, 14 `.Span` reads in
Units, 21 `FromPChar`, 20 `AsPtr`, 4 `ShortString` images (`ReadShortString`, `WriteShortString`),
523 `AnsiString X = default` locals, 5 `default(AnsiString)`, 14 string arrays, 34 string fields of
structs (12 of them `[InlineArray]` element fields), 17 `ref byte Key` key-press handlers, 27
retained `Ptr<byte>` lines of byte string character walks in `AoWMemo.cs`, `AoWHTMLMemo.cs` and
`AoWRealm.cs` (`ptr.md`). The survey's 1,368 index reads counted list indexers too
(`AOWMenu.cs:1052` `Value[I]` is a `TStringList`). Comparison calls: 43 `CompareText`, 4
`CompareStr`, 4 `AnsiCompareStr`, 31 `UpperCase`, 23 `LowerCase`. Four ordering sites (operators
and `CompareTo`) sit in `src/Rtl`, none in Units, and one implicit comparer sorts the dictionary
(`MldDictionary.cs:152`). `src/Units` holds 146 equality tests against `""` or `default` and 281
`catch (System.Exception)` blocks. No explicit-layout record holds a string. The Units sources are
pure ASCII: every byte above 127 enters through `FromBytes` literals, and the 35 `\x` escapes in the
tree are all below 128. `src/Layout` holds one non-ASCII char, U+00B7
(`SelFactionWin.Layout.cs:128`).

The dry run read 580 files, changes 301, and makes 8,005 rewrites: type 3,101, literal 3,337,
from-bytes 458, default 807, index 88, byte-context 50, boundary 156, compare 4, pattern 4. Its
residue is 89 sites: struct-field 34, as-ptr 20, key-handler 17, span 17, index-zero 1.

Host text enters game strings through the shim's implicit operator: explicit casts at `Rtl.cs:212`
(command line), `MldDictionary.cs:248` (UTF-16 dictionaries), `JsonSettingsStore.cs:31, 49, 74, 81,
84` (settings values), `GraphicsBoundary.cs:126` (registry values) and `HeadlessHost.cs:33, 42`,
and implicit ones at `FilesAndTime.cs:155, 156, 198` (`ExpandFileName`, `GetCurrentDir`,
`FindFirst` names). The other non-literal casts wrap class names, which are ASCII.

The private repository depends on the type too: `tests/localization` (48 uses), `tools/forms`
(5), `tools/oracle/Tests` (2) and the transpiler emitter (38 sites in 7 files, `Emitter.cs:276`
maps `TypeKind.String` to `Aow.Rtl.AnsiString`). The probes layer transpiles the `.pas` probes and
compiles them against the product `src/Rtl`, so the emitter and the shim change together. The
forms generator holds form text as Latin-1 byte chars (`tools/forms/Generator/FormTree.cs:17`,
`Program.cs:41`), and the Python tools read game text with the `cp1252` codec
(`tests/localization/generate.py:15`).

## 2. The decision

### 2.1 The text type and the byte map

Text is `string`. The invariant of the port is that every `char` in a game string is the code page
1252 decoding of exactly one byte (the image), so length and index are the same in both views and
the only thing the pass changes at an index site is the base. Host paths are the one kind of text
outside the image (2.5). One static class, `Aow.Rtl.Cp1252`, owns the map. P1 put it on main
(`38bd17f`), and P1b completes it:

| member | semantics |
|---|---|
| `Char(byte)` | the 256-entry table: 0 to 127 and 160 to 255 are identity, 128 to 159 are the Windows-1252 code points (0x80 to U+20AC and so on), and the five undefined bytes 129, 141, 143, 144, 157 map to U+0081, U+008D, U+008F, U+0090, U+009D. It is a bijection onto 256 chars |
| `Byte(char)` | the inverse of `Char`. Any other char becomes 63 (`?`). This is the port's choice, deterministic and free of best-fit lookalikes: `WideCharToMultiByte` best-fits by default (Ł to L) and gives `?` only with `WC_NO_BEST_FIT_CHARS` |
| `IsMapped(char)` | true exactly for the 256 chars of `Char` (P1b) |
| `Normalize(string)` | `Char(Byte(c))` per char, the entry rule for host text that becomes game text (2.5, P1b) |
| `Decode(ReadOnlySpan<byte>)` | `Char` per byte, no stopping at zero bytes |
| `Encode(string)` | `Byte` per char into a new `byte[]`. A null follows the switch of 2.2 |
| `Text(byte)` | the one-char string of `Char(b)`, from a 256-entry cache |
| `Compare(string, string)` | per position `Key(a[i]) - Key(b[i])`, then the lengths, where `Key(c)` is `Byte(c)` for a mapped char and 256 plus the code otherwise (P1b). Inside the image this is `_LStrCmp` of `SYSTEM.PAS`, and it differs from UTF-16 ordinal order for the 27 chars that bytes 128 to 159 decode to. `Compare(a, b) == 0` holds exactly when `a == b` |
| `Comparer` | an `IComparer<string>` over `Compare`, for every sort, search and sorted collection over game text (P1b) |

Equality is `string ==` (ordinal), which equals byte equality inside the image because the map is a
bijection. Hash codes stay randomized per process as `AnsiString.GetHashCode` already was, and no
game logic depends on them (`TStringHashList.Find` is a linear `AnsiCompareStr` scan).

The table is frozen. P1's test `EveryByteHasTheSpecifiedCharAndInverseAndCachedText` pins it. From
9.7 on it is a format contract (2.7), and a change to it changes every own document.

After P1b the shim's implicit operator and `ToString` go through `Cp1252` (review finding 24), so
the shim and the table are one encoder before the pass. U+0080 to U+009F in a literal or in host
text become `?` under the gate on the old type, and the pass changes no literal's meaning.

### 2.2 Nulls

A game string is never null. Delphi has one value for nil and `''`, and before the
pass `default(AnsiString) == ""` held. In C# `s.Length` on null throws, and `s == ""` on null is
`false` with no exception, so a null that escapes turns an empty test silently false
(`AOWMultiWin.cs:181` `if (call__1 == ((AnsiString)""))`).

- Rule 3 of the tool initializes every string field, auto-property and array element with `""`,
  and rewrites an existing `default` initializer or assignment of a local to `""`. It adds no
  initializer to a local that has none: C# definite assignment already guarantees the local is
  assigned before it is read, and C8 removed those unused initializers (amended 2026-11-04, the
  tool change landed in P5's final run, with a test). Rule 8 turns `x == ""`, `x != ""` and their
  `default` forms into `Rtl.IsEmpty(x)` and its negation, so the 146 empty tests of Units read a
  null as Delphi does and become observable.
- `Rtl.IsEmpty`, `Length(s)`, the RTL text routines and `Cp1252.Encode` read the switch
  `Rtl.StrictStrings`. Off, a null reads as empty, as Delphi reads nil, and that is the game. On, a
  null argument throws `NullStringException`. Game, Headless, HostChecks and Localization.Tests turn
  it on at entry (`GateSwitches.Enter`) when the environment variable `AOW_STRICT_STRINGS` is `1`,
  which `check.py` sets for every command it starts. The `Rtl.Tests` and `Units.Tests` projects turn
  it on in a module initializer. Every gate run is strict.
- Null sources that skip initializers get a reviewed fix each. They are the 34 string fields of
  structs, since `default(S)`, array elements and struct fields of classes skip constructors and
  code resets view structs such as `HeroCtrl.cs:290` `THeroView.Name` with `= default`. Among them
  are the 12 `[InlineArray]` element fields behind the static resource string tables
  (`FixedArrays.cs:11`, `AoWE.cs:106575` `TDamageTypeRStr`). The last source is `IntOrRef.As<T>()`,
  which returns `default` for a non-string (`Values.cs:10`, read at `Engine.cs:9011` and
  `HashTabU.cs:277`).
- The preferred fix for a struct field is a property whose getter reads a null backing field as
  `""`. That is the Delphi reading of a zeroed record, and it holds through every path that skips a
  constructor. A field that code takes by `ref` stays a field and is initialized at each creation
  site. P5 records the form per site (3.2). A product test asserts after unit initialization that
  every element of the static string tables is non-null.
- After the pass. No struct string field is taken by `ref`, so all 20 struct fields of Units, and
  `TSearchRec.Name` and `TVarRec.Str` in `src/Rtl`, are properties that read a null backing field as
  `""`. The 12 `[InlineArray]` string types keep their element field, and their `From` factory fills
  every element with `""` before it copies the given values. The 18 static tables that had no
  initializer start from `From([])`. `IntOrRef.As<string>()` reads `""` for a slot that holds no
  string, and `TRegistry.ReadString` reads `""` for a value that does not exist, as Delphi's does.
  `StaticStringTableTests` checks every static string field, array element and inline-array element
  of `Aow.Units` after `Startup.InitializeAll()`. Nine static tables of `AoWE` are never filled, in
  the port and in the Delphi source, and their elements read `""` as the shim's empty value did.
- `string ==` between two variables, one null and one empty, is false and passes through no RTL
  routine. Nothing but the closed sources above guards that case, which is why the sources are
  closed instead of the readers guarded.
- A null or a range error inside a broad catch leaves no trace by itself. `Rtl.Wrap` turns
  `NullReferenceException` into `EAccessViolation`, `AOWGauge.cs:212` turns any draw
  exception into a message box, and `AoWE.Format` (`AoWE.cs:103449`) returns the raw template on any
  exception. The exception census of section 4 counts first-chance exceptions in every gate host,
  so a swallowed exception still moves a number.

### 2.3 Characters

In 9.5 a Delphi `Char` stays `byte` in variables, fields, parameters, returns, sets and records.
Only a string element is a `char`. This is the model the tool implements (README, "Known limits"),
and 9.6 builds on it.

- A string element that flows into an integral slot goes through `Cp1252.Byte` (the tool's
  `byte-context` rule): a byte local, field, parameter or return, a `switch`, arithmetic, `Ord`,
  `UpCase`, `TSet<byte>.Contains`, and a comparison with a non-constant or with a constant of 128 and
  above. A comparison with a constant below 128 stays, because both sides promote to `int` and agree
  there. A constant pattern below 128 becomes a char literal.
- `Ord(c)` of a string element becomes `Cp1252.Byte(c)`. `Chr(n)` stays and returns `byte`, because
  its result lands in byte locals. Where a Delphi `Char` byte reaches an RTL routine that takes a
  `char` (`StringOfChar` among them), the RTL keeps a `byte` overload that converts through
  `Cp1252.Char`, and P5 lists the routines that need one.
- Key handlers stay `ref byte Key`. `InputMapper.Text` sends only chars with a code page byte, as
  that byte, so the 17 handlers, their `switch (Key)` labels and `C = unchecked((byte)Key)` stay
  correct with no edit. After the pass the mapper sends a char when `Cp1252.IsMapped` holds and its
  byte is at least 32, as `Cp1252.Byte(c)`.
- `TVarRec(byte)` stays the Char case beside `TVarRec(char)`, and `TVarRec.Chr` holds a `char`
  (`TVarRec(byte)` converts through `Cp1252.Char`). 9.6-1's `FormatArg` takes both over.
- An explicit cast between `char` and `byte` is wrong exactly for the 27 chars of bytes 128 to 159:
  `(byte)c` yields the low byte of the code point, `(char)b` yields a C1 control instead of the
  letter. Both compile and no fixture text holds that range, so the check of 3.4 forbids both.

Retyping Delphi `Char` to `char` is a later pass with its own specification, driven by the
transpiler's symbol table.

### 2.4 Per boundary

| boundary | decision |
|---|---|
| Saves, maps, scenario sets, campaigns, user lists (`rwString`, `rwStrings`, `StringToStream`, `SaveStringToStream`, `StringFromStream`, `StringsToStream`, `TStrings.LoadFromStream` and `SaveToStream`) | bytes. The length prefix counts bytes, which equals the char count. `Encode` at the writer, `Decode` at the reader. The storage stream's property values (`FValues`, a `TStringList`) hold strings. Every save-path `Encode` and `Decode` sits inside these routines, and a `ReadWrite` override never calls `Cp1252` itself (3.4) |
| `ShortString` images (ILB `ID`, `HSEngine` `ID` and `Name`, `FileSecurity` `ID`) | bytes. `ReadShortString` decodes, `WriteShortString` encodes and truncates to the image size as today. The `ShortString` class fields that lost their truncation in the transpiler (`AoWSMTPDlg.cs:75` `FSubject`, `E3IMSTypes.cs:16` `FFrom`) are outside 9.5 |
| Dictionaries (`.mld`) | bytes on disk, decoded at load with `Decode`. The sort and the binary search both use `Cp1252.Compare`, because `mld-format.md` names an exact byte comparison and 395 bytes in the 128 to 159 range occur in the eight files. The binary search (`MldDictionary.Search`) calls `Cp1252.Compare`. The sort (`MldDictionary.Read`) is an `OrderBy` that keeps the on-disk order of equal keys and passes `Cp1252.Comparer`, because without a comparer it sorts a `string` key in UTF-16 ordinal order. The context separator byte 127 is the char `\u007F`. UTF-16 dictionaries enter through `Normalize` (2.5) |
| Fonts and glyph indices (`TImageLibraryFont.TextWidth`, `ShowText`, `GetSafe`) | bytes. The glyph index of a char is `Cp1252.Byte(c)`, one table lookup per char, no allocation. A char outside the table draws glyph 63. `CharCase` goes through `UpperCase` and `LowerCase`, ASCII only, as today |
| Text input (`ref byte Key` handlers, `InputMapper`) | bytes, unchanged (2.3). The mapper drops a char with no code page 1252 byte, because no font has its glyph and no save can hold it |
| Network (`TGDChatMessageEvent`, `TGDFileStartMessageEvent`, packets) | strings in the delegates, bytes in the packets through the same stream routines. The transport stand-in reports no connections, so nothing runs |
| CRC and obfuscation (`WriteToFileCRC`, `AoWSetup` CRC, `PlayerC.Encode` XOR) | bytes. The computation runs over `Encode(text)` and the XOR result decodes back, which is exact because the map is a bijection |
| Strings that hold binary values (`AoWE.Encode` password XOR at `AoWE.cs:105048`, `AoWReg.TransformPassword` at `AoWReg.cs:995`, `FileSecurity.EncodeSecurityID`, the zero-filled resource strings at `AoWE.cs:105811`) | strings in 9.5, exact through the bijection, C1 controls and zero chars included. `strings.md` names them as binary for the 9.7 census |
| Registry and settings (`JsonSettingsStore`, `TRegistry` shim) | `string` end to end, no bytes. Values read from the file enter through `Normalize`, except the path keys (2.5) |
| File names and paths (`ExtractFileName` and friends, `FindFirst`, `TFileStream`, `IFileSystem`) | `string` end to end, with the host's chars (2.5). The split rule stays `\` and `:` only, the enumeration order stays upper-cased ordinal UTF-16 (`rtl-shim.md` 3.5). The RTL routines are total over chars outside the table (2.6), so two such names stay distinct in `CompareText`, `AnsiCompareText` and the sorted lists. A host path reaches a save only through `Encode`, where those chars become `?`. This extends the original, whose ANSI file APIs see such a name only through best-fit lookalikes |
| Forms text (`tools/forms`, `src/Layout`) | the forms generator and the emitter decode form text through the table, not through Latin-1, so a form byte from 0x80 to 0x9F reaches the font as its letter (3.3). No form holds one today |
| Dialogs, console, exceptions, `Assert` | `string`. `NullTerminatedText` stays and cuts at `'\0'` |
| Literals | C# string literals. The pass made each cast literal of the shim a plain literal and each `FromBytes` of literal bytes a string literal. No literal of `src/Units` holds a char above 127, and its sources are pure ASCII |
| Python tools that read or write game text | one 256-entry table, `tools/cp1252.py` in the private repository, in place of the `cp1252` codec, which has no mapping for the five undefined bytes and raises on them |

### 2.5 Host text

After the pass two kinds of host text exist, and every entry point is one or the other.

- A path keeps its real chars and passes only through the path routines. `ParamStr(0)` and
  `GetCurrentDir` return absolute host paths, and a game root below a user name in another script
  yields chars outside the table. Normalizing a path breaks every open below it.
- Text that becomes game text enters through `Cp1252.Normalize`, so that fonts, saves and
  comparisons see one byte per char. After P1b, `Normalize` does exactly what the shim's implicit
  operator does, so a text entry point keeps its behavior and only a path entry point changes.

| entry point | kind | after the pass |
|---|---|---|
| command line, `ParamStr` | path | chars kept |
| `GetCurrentDir`, `ExpandFileName`, `FindFirst` names | path | chars kept |
| UTF-16 dictionaries (`MldDictionary`, the reader's `Chars`) | text | `Normalize` |
| settings and registry values read from `settings.json` (`JsonSettingsStore.ReadText`, which `TRegistry.ReadString` reads through the store) | text, except keys whose value the game opens as a path | `Normalize` at read. The path keys `General/Root Directory`, `General/Startup Directory` and `Email/Attachment Directory` keep their chars |
| headless fixture settings (`HeadlessHost.ReadSetting`) | per key | `JsonSettingsStore.ReadText`, the same rule |
| typed text (`InputMapper.Text`) | text | unchanged, only chars with a byte reach the game, as that byte |
| class names | ASCII | unchanged |

The tool lists every entry point under the residue category `host-entry` (3.2), the implicit ones
included, which no grep counts. `strings.md` classifies each as path, text or ASCII. A path that
becomes game text (a file name in a list, a map name in a save) stays a path in memory and meets the
table at the font (`Byte`, glyph 63) and at the stream writer (`Encode`, byte 63).

### 2.6 The RTL routines over `string`

The Pascal routines keep their names and Delphi contracts, because 191 `Copy`, 235 `Delete`, 50
`Insert` and 29 `Pos` sites pass Delphi arguments and hand-changing that arithmetic is the risk the
pass avoids. Only the direct indexer changes base. Every routine is total: a char outside the table
passes through unchanged and keeps two strings that differ apart.

| routine | semantics over `string` |
|---|---|
| `Copy`, `Delete`, `Insert`, `Pos`, `AnsiPos` | one-based, the edge rules of `rtl-shim.md` 2.3 unchanged |
| `X[I]` | becomes `X[I - 1]`, with a literal index folded (`X[1]` to `X[0]`). Out of range throws `IndexOutOfRangeException` where the shim threw `ERangeError`, and `Rtl.Wrap` maps it to `ERangeError`, so translated handlers see the same class |
| `WithChar`, `Resized`, `SetLength` | `Rtl.WithChar(s, i, c)` one-based, `Rtl.Resized(s, n)` padded with `'\0'` |
| `Length`, `IsEmpty` | `Length(s)` and `Rtl.IsEmpty(s)` follow the null switch (2.2). `s.Length` throws on null in both modes |
| `UpperCase`, `LowerCase`, `UpCase` | ASCII letters only, as today. Every other char stays |
| `AnsiUpperCase`, `AnsiLowerCase` | the code page 1252 tables of `SysUtils.cs` applied through `Byte` and `Char` to mapped chars, never `ToUpperInvariant`, which maps µ (0xB5) outside the code page. A char outside the table stays unchanged (P1b, where P1 returned `"?ÓD?"` for `"Łódź"`) |
| `CompareStr`, `<`, `>`, `<=`, `>=` | `Cp1252.Compare`. C# has no ordering operators on `string`, so the tool rewrites the 4 sites it finds (operators and `CompareTo`, all in `src/Rtl`) |
| sorts, searches, sorted collections | `Cp1252.Comparer` (rule 6) |
| `CompareText` | `CompareStr` after ASCII upper-casing |
| `AnsiCompareText`, `AnsiCompareStr` | the fold and collation tables of `rtl-shim.md` 3.2 over bytes, unchanged inside the image. A char outside the table orders after every mapped char by its code, as in `Compare` (P1b). The pinned 15,568 order exceptions stay valid |
| `Trim` family | chars at most 32 at the ends |
| `IntToStr`, `IntToHex`, `StrToInt`, `StrToIntDef` | invariant culture, `Val` acceptance rules unchanged |
| `Format` | the shim's own `FormatBuf` implementation over `string`, never `string.Format`. `TVarRec.Str` is a `string`, `TVarRec.Chr` is a `char`, and `TVarRec(byte)` stays the Char case (2.3) |
| `StringOfChar(char, n)`, `QuotedStr` | as Delphi, with the `byte` overloads of 2.3 where transpiled callers pass a Delphi `Char` |
| `StrPas`, `FromPChar`, `SetString` | `Decode` of a byte span, stopping at the first zero byte where the source was a `PChar` |

The check of 3.4 keeps culture, Unicode-category and zero-based .NET string calls out of game text
from 9.5-1 on.

### 2.7 The table and 9.7

9.7 converts at the file level and keeps the in-memory image. The converter turns an original file
into a document and back, the engine reads only images the converter wrote, and `ReadWrite`, the
transpiled streams and the image stay as they are (9.7 draft section 4). So every boundary of 2.4
stays in the product, and none moves into the converter. `src/Formats` takes `Cp1252` from
`Aow.Rtl`, writes `rwString` through it, and reports a char outside the table with
`Cp1252.IsMapped`, the predicate the input mapper and `Normalize` use too. 9.7-1 does not need the
table `AnsiString.ToString` uses, since `Cp1252` is on main and after P1b the shim delegates to it.
From 9.7 on the table is a format contract and frozen (2.1). The strings that hold binary values
(2.4) round trip through the 9.7 escape rule for U+007F to U+009F, and `strings.md` names them for
the 9.7 census.

## 3. One pass, made safe and reviewable

### 3.1 The tool

The mechanical part is the Roslyn rewrite `tools/rewrite/strings` in the private repository (P3,
its README). It loads a source tree, resolves every expression against the semantic model, and
applies these rules where the receiver or operand is `Aow.Rtl.AnsiString` in the tree before the
rewrite:

1. Type names in declarations, generics, casts and `As<AnsiString>()` become `string`.
2. `(AnsiString)"lit"` becomes `"lit"`, and `(AnsiString)expr` loses the cast. A non-constant
   operand is also a `host-entry` site (3.2). `FromBytes` of literal bytes becomes a string literal.
   `FromBytes(expr)` of one byte becomes `Cp1252.Text(expr)`, of a span or array `Cp1252.Decode`.
3. `default(AnsiString)` and `= default` on a string declaration become `""`. A string field
   without an initializer gets `= ""`, and a local without one keeps none (2.2, amended
   2026-11-04). `new AnsiString[n]` becomes `Rtl.Strings(n)`, which fills with `""`.
4. An element access on a string receiver gets `- 1` on the index, folded for literals and for
   `I + 1`, `1 + I` and `I - k`, with a non-primary operand parenthesized.
5. `.Span` becomes `Cp1252.Encode`, and `FromPChar` becomes `Rtl.StrPas`, which stops at the
   first zero byte. `.AsPtr()` other than the `.Value` read becomes `Ptr<byte>.Unbox(s)`, the
   zero-terminated encoded copy that the emitter produces for a string passed as `PChar` (3.3). The
   site stays in the `as-ptr` residue, since a write through the copy no longer reaches the string
   (amended in P5). `.AsPtr().Value`, which reads the first char or zero, becomes
   `(s.Length == 0 ? '\0' : s[0])`, inside `Cp1252.Byte` where it flows into a byte slot.
   `Rtl.StringBytes(s)` of P2 becomes `Cp1252.Encode(s)`. `.ToString()` on a string receiver is
   dropped. `WithChar` and `Resized` become the `Rtl` calls.
6. Ordering operators and `CompareTo` become `Cp1252.Compare`. Every `OrderBy`,
   `OrderByDescending`, `ThenBy`, `ThenByDescending`, `Order`, `OrderDescending`, `Sort`,
   `BinarySearch`, `Min`, `Max`, `SortedSet`, `SortedDictionary` and `SortedList` whose key or
   element type is `AnsiString` and that passes no comparer gets `Cp1252.Comparer`. Main has one
   such site, `MldDictionary.cs:152`.
7. Constant patterns against a string element with constants below 128 become char literals.
8. `x == ""`, `x != ""`, `x == default` and their mirrored forms on a string become
   `Rtl.IsEmpty(x)` and its negation.

The `byte-context` rule of 2.3 wraps a string element that flows into an integral slot in
`Cp1252.Byte`.

The tool emits `Cp1252.X` and `Rtl.X` and runs the Roslyn simplifier and the parenthesis cleanup
over the nodes it creates. The shortest name that binds stands (`Cp1252` binds unqualified in every
Units file that imports `Aow.Rtl`, `Rtl` often does not, review finding 28), and the counts C7
brought to zero (IDE0004, IDE0047) stay at zero.

Scope: `src` without `src/Layout`, which the pass regenerates (3.3, D3), and without the test files
that pin `AnsiString` itself, which the pass replaces (3.2). The tool runs over the private trees
with another root. It is deterministic and idempotent. Rules 5, 6 and 8 in their full form, the
emitted names and the new residue categories come with brief P3b (5.1).

### 3.2 Residue, the record and the freeze rehearsal

The tool prints a count per rule and a residue list: every site it declined, by category. The
categories after P3b:

| category | meaning |
|---|---|
| `struct-field` | a string field of a struct, fixed as 2.2 says |
| `as-ptr` | an `AsPtr()` on a string other than `.Value` |
| `span` | a `.Span` site. One that writes through the span is a string-as-buffer site of P2 |
| `host-entry` | an explicit or implicit user-defined conversion from `string` to `AnsiString` with a non-constant operand (2.5) |
| `element-other` | a string element whose converted type is neither integral nor `TVarRec`, such as `object` or an interpolation hole, where a number turns into a letter |
| `index-zero`, `member`, `static-member`, `from-bytes-mixed`, `array-rank`, `var-element`, `unresolved` | as the README defines them |

`key-handler` is gone, since handlers stay `ref byte Key` (2.3).

The record is keyed by symbol, not by line. Each residue site and each hand fix carries the
documentation comment id of its containing member and the text of its statement, and the line only
as information. `strings.md` holds the dry-run inventory and the classification. The patch file
`data/rewrite/strings-patch.json` holds the hand fixes, which the tool applies with `--patch`. C9,
C7 and C8 move nearly every line, and a member key survives them. When main moves while the pass is
open, the implementer reruns the tool and the patch reapplies by member. A patch entry whose member
or statement text is gone is reported, never dropped.

Freeze rehearsal (P5). Before the freeze, the compile errors of the rewritten tree are known and
fixed in the record. P5 holds the RTL after the switch on an unmerged product branch: the
`AnsiString` overloads gone, and `TStringList`, `TVarRec`, `IntOrRef`, `TComponent.Name` and
`MldDictionary` on `string`. That is the hand switch behind the tool's informational counts (448
rewrites in `Rtl`, 79 in `Rtl/Localize`), since a mechanical rewrite of `Rtl.cs` turns `StrPas` into
a call to itself. The tool's `--diagnose <rtl>` mode rewrites the tree in memory, compiles it
against that RTL, writes nothing, and prints every compile error by id and member. Known sources:
`null` arguments that turn ambiguous once `IntOrRef` takes `string` beside `TObject?`
(`Values.cs:8-9`), Delphi `Char` bytes that reach routines taking `char` (2.3), and `ref` uses of
struct fields that became properties (2.2). Each error class gets a recorded fix. A fix that repeats
becomes a tool rule under P3b's test discipline, and the rest join the patch. P5 is done when
`--diagnose <rtl> --patch data/rewrite/strings-patch.json` over the base prints zero errors, so
9.5-1 starts from a tree that compiles.

Tests. The test files that pin `AnsiString` itself (`CoreTests.cs` among them, whose line 102
asserts that U+0080 encodes to 128) are not rewritten. The pass deletes them and maps each behavior
they pin to a `string` test, in a table of the 9.5-1 report. The other test files use strings as
data, and the tool rewrites them as it rewrites Units. The 9.5-1 brief states the product layer's
test count after the pass.

### 3.3 Lockstep in the private repository

P4, on a private branch that merges with the pass:

- The transpiler emitter switches outright. `Emitter.cs:276` maps `TypeKind.String` to `string`,
  char to string conversions emit `Cp1252.Text`, literals with `#` codes emit `\u` escapes, `PChar`
  to string conversions emit `Rtl.StrPas` (not `Cp1252.Decode`, which over a `Ptr<byte>` reads past
  the zero terminator, amended 2026-11-05 after the P4 review), string to `PChar` emits a
  zero-terminated copy through `Ptr<byte>.Unbox`, empty tests emit `Rtl.IsEmpty`, and names come
  out in the simplified form of 3.1. The emitter passes no comparer: the one sort over game text,
  `MldDictionary.Read`, is hand-written and passes `Cp1252.Comparer` by hand. The 326
  transpiler tests update their expected text (328 after the merge). There is no dual mode,
  because the first-stage emission has no consumer once the shim type is gone. Probe `.pas`
  sources and the recorded Delphi outputs do not change.
- The forms generator decodes form text through the table, not through Latin-1 (2.4).
- `tools/cp1252.py` replaces the `cp1252` codec in every Python tool that reads or writes game
  text.

The pass converts `tests/localization`, `tools/oracle/Tests` (HostChecks) and `tools/forms` with the
tool and another root, and the report pairs the product branch with the private commit.

Layout (D3). `src/Layout` stays emitter-owned through 9.5-1. The tool skips it. The pass
regenerates it with P4's forms generator and copies it whole, with no hand edit. The report compares
the regenerated files with the tool's output for `src/Layout` (`--out`, 21 rewrites at `d31aa07`),
every difference explained. Two independent routes to one text cross-check P4. P4 runs the
comparison once on its own base, and 9.5-1 runs it again on the pass base. After 9.5-1, brief C11
made `src/Layout` hand-owned on 2026-11-05, and the forms generator became a reference emitter.

### 3.4 Permanent enforcement

The post-check stays after the pass. From 9.5-1 on,
`StringsRewrite --check <product>/src --allow tools/rewrite/strings/allow.txt` runs as a step of the
`product` layer in `check.py`, after the build, with no new layer. It prints
`Strings check: N findings` and fails unless N is 0. A finding is:

- any `AnsiString`, the type the pass retired,
- an explicit conversion from `char` to `byte` or `sbyte`, or to `char` from a non-constant integer
  or from a constant of 128 and above,
- an equality or relational operator between a `char` operand and a `byte` operand,
- a sort, search or sorted collection over strings without a comparer (rule 6),
- a `Cp1252` call inside a `ReadWrite` override (2.4),
- in `src/Units`, `src/Layout` and the text routines of `src/Rtl`: `string.Compare`,
  `CompareOrdinal`, `CompareTo` on `string`, `StringComparer`, `StringComparison`,
  `Comparer<string>.Default`, `ToUpper` and `ToLower` with their invariant forms on `string` and
  `char`, `char.IsLetter`, `IsDigit`, `IsWhiteSpace`, `IsUpper`, `IsLower`, `IsLetterOrDigit`,
  `IsPunctuation` and `IsControl`, `Trim`, `TrimStart` and `TrimEnd` without arguments,
  `IsNullOrWhiteSpace`, `Split` without separators, the `string` overloads of `IndexOf`,
  `LastIndexOf`, `StartsWith` and `EndsWith` without `StringComparison`, `Encoding.Latin1`,
  `Encoding.Default` and `Encoding.GetEncoding`,
- in `src/Units` and `src/Layout` also `Substring`, `IndexOf`, `LastIndexOf`, `Remove` and `Insert`
  on `string`, whose zero-based positions mix badly with the one-based Pascal routines, until a
  later pass retires those routines unit by unit.

The allowlist names members, not lines, by documentation comment id, each group with its reason. In
`tools/rewrite/strings/allow.txt` of the private repository after the pass it holds 31 members:
the host path code (`IFileSystem.ExtractRelativePath`, `SystemIoFileSystem.Resolve`), the
`FindFirst` order of `rtl-shim.md` 3.5, the settings keys and dictionary paths, host text that
never becomes game text (the language tag, the census file name, reflected property names, layout
module names), the table's own construction in `Cp1252`, the ASCII case arithmetic of `UpCase` and
`LowerCase`, the file signatures and JSON escapes of the own formats (9.7-1), and test members that
build bytes from ASCII constants or walk every char. `InvariantGlobalization`
(`Directory.Build.props:11`) removes machine-culture variance, but the banned differences from
Delphi do not depend on culture: UTF-16 ordinal order, `char.IsWhiteSpace` and `string.Trim()`
treating U+00A0 (byte 0xA0) as space where Delphi `Trim` stops at 32, `char.IsLetter` and
`ToUpperInvariant` on the 27 code page letters and on µ. A grep cannot see the casts and mixed
comparisons, which is why the check is semantic.

### 3.5 Order

D5 runs the mechanical passes C9, C7 and C8 before 9.5, after 9.3 and 9.4-7 are merged. C1 runs
first as its own brief. `construction-form.md` carries 9.4-7 and the review's C3 to C5 as CF-1 to
CF-4, and C6 follows CF-4. The base of 9.5-1 is therefore main after C1, CF-1 to CF-4, C6, P2, P6,
C9, C7 and C8a to C8c.

- P2 runs after CF-4, beside C6 and P6, and before C9. It is hand work, and the passes after it
  rerun on its result. It walks zero-based byte spans (`Rtl.StringBytes(S)`, a
  `ReadOnlySpan<byte>` over the shim's bytes) instead of the one-based indexer, so the tool owns the
  only base change.
- P6 runs after C6, which splits the test project, and merges before C9.
- P1b, P3b and P4 touch no Units file and run any time before P5. P1b edits only the string files of
  `src/Rtl` (`AnsiString.cs`, `Cp1252.cs`, `StringRoutines.cs` and the routine files they call).
- P5 finishes on the base, after C8c merges: it repeats the dry run there, and `strings.md` names
  that commit.
- The pass runs alone. No other brief touches `src/` from its start to its merge.
- C10 follows 9.5-1, so the member keys of the record stay stable through the freeze and C10's
  renames read `string` code.
- D3 keeps `src/Layout` emitter-owned through 9.5-1: C7 and C8c leave it alone, the tool skips it,
  and the pass regenerates it (3.3).
- 9.6-0, 9.7-0 and 9.7-1 run beside 9.5 as their drafts say, and 9.7-1 merges outside the freeze.
  The 9.6 product briefs and 9.7-3 wait for 9.5-1.

The tool is semantic and indifferent to the cleanups, so D5 costs 9.5 nothing as long as the tool
emits simplified names (3.1). In return the 9.5 review reads clean code.

## 4. What stays bit-for-bit, and the proof

| invariant | proof | lands |
|---|---|---|
| Save, map and state bytes | `persistence` (7,200 byte fixtures, 200 resumptions), `oracle` (7 scenarios, 177 saves equal in byte and model modes), `scenarios` (92 states equal in both modes), the German map state equality in `language-frames` | existing |
| Frame pixels, including German glyphs and the Weltkarte caption | `frames` (100 of 100, zero masks), `language-frames` (`fixture_glyphs_equal`, `undead_equal`), `scenarios` (92 frames) | existing |
| Glyphs of bytes 128 to 159 | for every byte `b`, `TextWidth` and the glyph `ShowText` draws for the one-char string of `b` equal glyph `b`, for every font of the English and German installs, headless. A `language-frames` fixture whose German text holds a char from bytes 128 to 159 (`„` 0x84, `“` 0x93, the dash of 0x96 or `…` 0x85, taken from the German dictionary), captured natively at volume 0 | P6 |
| Dictionary lookup, context fallback, every entry of the eight files | `localization` (6,683 entries, 100,245 lookup checks, 174,727 assertions), which covers the 395 bytes in the 128 to 159 range. A test asserts that the sorted entry array of each of the eight dictionaries is in `Cp1252.Compare` order | existing, P6 |
| Units sorts over German and English names | sort dump (HostChecks `--sort-dump`): `CompareItemNames`, `CompareAbilityNames`, `P4CompareSpellNames`, `P4CompareSpellLevels`, `CompareTextControlItems` and a sorted `TStringList`, over the English and German names of units, items, abilities, spells and texts of the installed game's set. The dump on the base and after the pass is identical, 2,994 lines | P6, recorded in P5 |
| Null and range errors behind broad catches and `AoWE.Format` | exception census (below). Counts equal the base, `NullStringException` 0. Every gate run is strict (2.2). A product test asserts the static string tables non-null | P6, recorded in P5, test in 9.5-1 |
| Typed text through save and load | a player name typed through `InputMapper.Text` with €, „, ä and ß survives save and load in byte mode (the saved bytes hold 0x80, 0x84, 0xE4 and 0xDF, `TypedNameRoundTripTests`) and in model mode (the oracle's model reader reads a whole save the Headless host wrote with that leader name, `tools/oracle/typed_name.py` in the private repository) | P6 for byte mode. The model-mode read needs the oracle's Python reader over a whole save, so it landed with the oracle tooling in 9.7-2 (deferred 2026-11-05) |
| Host paths outside the table | a test creates files whose names hold chars outside the table in a temporary folder, finds them through `FindFirst`, opens them, and checks that `AnsiUpperCase` keeps them and that `AnsiCompareText` and a sorted `TStringList` with `dupIgnore` tell them apart (`HostPathTests`) | 9.5-1 |
| `AnsiCompareText` and `AnsiCompareStr` order and equality | probe `strings` with the 15,568-pair pin, probe `lists` for the sorted `TStringList`, `SysUtilsTests` | existing |
| `Format`, `IntToStr`, `StrToInt`, `Copy` family edge cases | probes `format` and `strings`, `SysUtilsProbeTests`, P1's `FormatPreservesEveryHighCharAsStringCharByteAndPChar` | existing |
| `FindFirst` order, path splitting | probe `files`, `FilesProbeTests` | existing |
| The byte map itself | `Char` and `Byte` are inverse over all 256 bytes, `Byte` of all 65,536 chars gives the 256 mapped values and 63 elsewhere, `Compare` equals unsigned byte comparison over all 65,536 byte pairs, `AnsiUpperCase` and `AnsiLowerCase` over 256 chars equal the byte tables, `Decode` then `Encode` round-trips random arrays with embedded zeros, `Format("%s")` of chars above 127 | P1 |
| Totality and one encoder | `IsMapped` over 65,536 chars, `Normalize` equals `Char(Byte(c))`, `Compare(a, b) == 0` exactly when `a == b` over random strings with chars outside the table, `Comparer` agrees with `Compare`, case maps keep chars outside the table, each routine under both null modes, the shim's implicit operator equals `Encode` and its `ToString` equals `Decode` | P1b |
| Culture | Game and Headless stop at start when `System.Globalization.Invariant` is off, and a product test asserts the switch in the test process | P6 |
| Pacing | `pacing` (300 frames at 33 ms), which catches an allocation regression in text drawing | existing |

Exception census. Game, Headless, HostChecks and the private hosts of the persistence and
localization layers register `AppDomain.FirstChanceException` at entry when the environment variable
`AOW_EXCEPTION_CENSUS` names a file, and append one line per run: the host, its arguments and the
counts of `NullReferenceException`, `IndexOutOfRangeException`, `ERangeError`, `EAccessViolation`,
`EConvertError` and `NullStringException`. `check.py` sets the variable to a file in each layer's
run directory. P5 records two gate runs on the base in `data/rewrite/9.5-exceptions-base.json`. A
run whose counts differ between the two is listed there with its reason and left out of the
comparison. The census sees an exception at its throw, before `Rtl.Wrap`, a broad catch or
`AoWE.Format` hides it.

Counts. The 13 layers of `tools/check.py` keep their counts from the base, which includes P6's tests
and fixture. In 9.5-1 one count moves: the product test count, to the number the brief states (the
deleted shim tests, their `string` replacements, the host path test, the static table test). Any
other count that moves is a finding, not a pass.

## 5. Briefs and done measures

Commands run from `D:/git/open-aow` in Git Bash unless they start with `python tools/` or name the
tool, which run from `D:/git/open-aow-private`. Every product brief also runs `dotnet build` with
zero warnings and `dotnet test`.

| step | brief | state, waits for | done |
|---|---|---|---|
| 9.5-0 | P1 (Rtl): `Cp1252`, `Rtl.Strings`, `Rtl.WithChar`, `Rtl.Resized`, string overloads of the Pascal routines | merged (`38bd17f`) | done |
| 9.5-0 | P3 (private): the tool, 20 tests, the dry run over `d31aa07` | committed | done (`strings.md`, README report) |
| 9.5-0 | P1b (Rtl): `IsMapped`, `Normalize`, `Comparer`, the total order of `Compare` and the `Ansi*` routines, case maps that keep chars outside the table, `Rtl.IsEmpty`, `Rtl.StrictStrings` with `NullStringException`, the shim's implicit operator and `ToString` through `Cp1252`, the P1b tests of section 4, the shim tests that pinned the C1 path (`CoreTests.cs:102`) updated | waits for nothing, beside every brief that leaves the string files of `src/Rtl` alone | 13 layers pass with base counts except the product test count, which the report states with each added and changed test |
| 9.5-0 | P3b (private): the tool brief of 5.1 | waits for nothing | 5.1 |
| 9.5-0 | P4 (private branch): emitter switch, forms generator through the table, `tools/cp1252.py` (3.3) | after P3b, which gives the tool `--out` | on the branch the transpiler builds and its 326 tests pass with updated expected text, the Layout cross-check of 3.3 lists every difference with its reason, and `git -C D:/git/open-aow-private grep -n "cp1252" -- tools tests` lists no use of the codec outside `tools/cp1252.py` |
| 9.5-0 | P2 (Units): the string-as-buffer sites become `byte[]` while the type is still `AnsiString` (the three `ReadString` shapes, the `Span` reads in `Engine.cs`), and the 27 retained `Ptr<byte>` walks in `AoWMemo.cs`, `AoWHTMLMemo.cs` and `AoWRealm.cs` become loops over `Rtl.StringBytes(S)` | after CF-4, beside C6 and P6, before C9 | 13 layers pass with unchanged counts, none of the 27 lines `ptr.md` retains is left, and a dry run lists no `span` site that writes through a span |
| 9.5-0 | P6 (product and private): the exception census in the hosts and `check.py`, `AOW_STRICT_STRINGS` in `check.py`, HostChecks `--sort-dump <file>`, the glyph index test, the German fixture with a char from bytes 128 to 159, the input round trip, the dictionary order test, the culture guard (section 4) | after C6, merged before C9 | 13 layers pass, with the new counts the report states, and the census file and the sort dump are written on main |
| 9.5-0 | P5 (unmerged product branch and private): the RTL after the switch, the dry run, `--diagnose` and the patch on the base, the base recordings | after C8c (the base), P1b, P3b, P6 | on the base commit, which `strings.md` names: `--diagnose <rtl> --patch data/rewrite/strings-patch.json` prints 0 errors, `strings.md` classifies every `host-entry` site and lists the path keys and the binary-value strings, `data/rewrite/9.5-exceptions-base.json` holds two gate runs, `data/rewrite/9.5-sorts-base.txt` holds the sort dump |
| 9.5-1 | The pass, one implementer, one worktree, `src/` frozen: switch the RTL from the P5 branch, run the tool with `--write --patch` over `src` and the private trees, merge P4 and regenerate `src/Layout`, fix what the build still reports and record each fix by member, replace the shim tests, add the host path and static table tests, wire `--check` into the `product` layer, delete `AnsiString.cs` and `Rtl.StringBytes` | after P5 and P4. Merged 2026-11-05 (PR #95, `c55a631`) | `python tools/check.py` passes all 13 layers with the counts of section 4 and zero warnings, the product summary carries `Strings check: 0 findings`. `git grep -n AnsiString -- src` prints nothing, and in the private repository `tools/` and `tests/` name it only in reports, documents and `tools/rewrite/strings`. The census of every run equals the base and `NullStringException` is 0. HostChecks `--sort-dump` equals `data/rewrite/9.5-sorts-base.txt`. `python tools/rewrite/analyze_9x.py` prints IDE0004 0 and IDE0047 0 over `src/Units`, as after C7. The report holds the Layout comparison, the test mapping table and the product layer's wall time before and after. `strings.md` lists every `Cp1252` call site by boundary of 2.4 |
| 9.5-2 | Documents: `rtl-shim.md` 1.2, 2.3 and 3 rewritten for `string`, `localization.md` decision 4, `design.md` open question 1, `milestones.md` row 9.5, `testing.md` for the strings check, the strict switch and the exception census, and this document against the code (section 8) | after 9.5-1 | the documents name the code as it is |

Loop base shifts, `StringBuilder` in hot concatenations and span-based text drawing are a later
cleanup, measured first, and not part of the done measure. So are retyping Delphi `Char` to `char`
(2.3) and retiring the one-based Pascal routines unit by unit (3.4), each with its own
specification.

### 5.1 Brief P3b: the tool's new rules

Scope: `tools/rewrite/strings` and its tests in the private repository. No product edit. Input:
sections 2.2, 2.3, 2.5, 3.1, 3.2 and 3.4 of this specification and the README.

| item | what the tool does | count or category |
|---|---|---|
| rule 8 | `x == ""`, `x != ""`, `x == default`, `x != default` and the mirrored forms on an `AnsiString` become `Rtl.IsEmpty(x)` and its negation | `empty` |
| rule 6, implicit comparers | the sorts, searches and sorted collections of 3.1 rule 6 over `AnsiString` without a comparer get `Cp1252.Comparer` | `comparer` |
| rule 5, `.AsPtr().Value` | becomes `(s.Length == 0 ? '\0' : s[0])`, inside `Cp1252.Byte` in a byte slot | `boundary` |
| rule 5, `Rtl.StringBytes(s)` | becomes `Cp1252.Encode(s)` | `boundary` |
| host entries | every explicit or implicit user-defined conversion from `string` to `AnsiString` with a non-constant operand, listed by member | residue `host-entry` |
| foreign element slots | a string element converted to a type that is neither integral nor `TVarRec`, listed by member | residue `element-other` |
| key handlers | `ref byte Key` handlers are no longer residue | `key-handler` removed |
| inline array names | rename the 12 generated `FixedArray_Aow_Rtl_AnsiString_N` types and all their references to `FixedArray_string_N` after changing the element type. The non-null static-table test of 2.2 covers their elements | type rename |
| emitted names | `Cp1252.X` and `Rtl.X` through the Roslyn simplifier and the parenthesis cleanup over created nodes | none |
| symbol keys | every residue site and report line carries the documentation comment id of its containing member and its statement text, the line as information | none |
| `--exclude <path>` | repeatable, skips a folder or file (`src/Layout`, the shim test files) | none |
| `--out <dir>` | writes the rewritten files under another root instead of in place | none |
| `--patch <file>` | applies the member-keyed hand fixes of `strings-patch.json` after the rules, reports an entry whose member or statement is gone | none |
| `--diagnose <rtl>` | rewrites in memory, compiles against the RTL source folder given, writes nothing, prints each compile error by id and member and a count per id | none |
| `--check` with `--allow <file>` | the findings of 3.4 over a tree that has no `AnsiString`, the allowlist keyed by member, prints `Strings check: N findings` and exits non-zero unless N is 0 | none |

Done measure: `dotnet test tools/rewrite/strings/Tests/StringsRewrite.Tests.csproj` passes, with a
test of exact expected text for each item above, including inline-array declarations and their
references, and for each finding kind of 3.4, among them a
`(byte)c` cast, a `char` against `byte` comparison, a comparer-less `OrderBy`, a `Cp1252` call in a
`ReadWrite` override, a parameterless `Trim()` and a `Substring` in a Units-shaped file. A dry run
over main writes `strings.md` with the counts `empty` and `comparer` (`comparer` 1, at
`MldDictionary.cs:152`), the categories `host-entry` and `element-other`, no `key-handler`, and every
site keyed by member. The README describes each item. Rules that P5 finds repeating join the tool
later under the same test discipline.

## 6. Risks

1. Silent order changes for bytes 128 to 159. A comparer-less sort or a `string.CompareOrdinal`
   puts € after Ä. Rule 6 covers every implicit comparer, the check of 3.4 forbids new ones, the
   65,536-pair `Compare` test covers the RTL, the dictionary order test covers the eight files, and
   the sort dump covers the Units sorts over real names.
2. Char and byte confusion. A Delphi `Char` stays `byte` (2.3), and every place a string element
   meets it goes through `Cp1252.Byte`. The check forbids the casts and mixed comparisons that
   compile and fail only from 128 to 159. The glyph test and the German fixture prove that range at
   the font.
3. Strings used as byte buffers. The tool sees types, not intent. P2 removes the known sites first,
   and the `span` category catches a site that writes through a span.
4. Null strings. Rule 3, rule 8, the struct fixes and the strict gate (2.2). A null that reaches
   only a `string ==` between two variables stays invisible, so the sources are closed.
5. Host text. Every entry point is listed (`host-entry`) and classified, text normalizes exactly as
   the shim did after P1b, and paths keep their chars on purpose.
6. A long freeze. P5 compiles the rewritten tree before the freeze and records a fix for every
   error, so 9.5-1 starts from a tree that compiles. The record is keyed by member, so the
   cleanups and a late merge do not void it.
7. The private lockstep. Product and private repository change in the same run, and `check.py`
   reads both. The report pairs the product branch with the private commit.
8. Performance. `Encode` allocates at every stream write and at every `Rtl.StringBytes` walk P2
   leaves, and `Byte` costs a lookup per glyph. The pacing layer measures, and the later cleanup
   optimizes where it points.
9. Gate time. The `--check` step binds the whole product tree (about three minutes for the dry run
   today). The 9.5-1 report records the product layer's wall time before and after.

## 7. Second opinion disposition

Every amendment of `9.5-second-opinion.md` is applied. None is rejected. Where the spec departs from
the amendment's wording, the reason follows.

| amendment | disposition |
|---|---|
| A1 nulls | applied (2.2, rule 8). The switch is set through `AOW_STRICT_STRINGS` and throws its own `NullStringException`, so the census counts strict failures apart from other null references. The spec names a preferred struct fix (a null-reading getter), which P5 confirms per site |
| A2 host text | applied (2.5, `host-entry`). Settings and registry values are classified per key, because one store holds player names and paths alike |
| A3 totality | applied (2.1, 2.6, P1b) |
| A4 implicit comparers | applied (rule 6, the dictionary order test in P6) |
| A5 characters | applied (2.3). Key handlers stay `ref byte Key`, and the key-handler residue is gone |
| A6 enforcement | applied (3.4). The step joins the `product` layer in 9.5-1, because before the pass every file names `AnsiString` |
| A7 one encoder | applied (P1b, P4, 2.1, 2.4) |
| A8 9.7 | applied (2.7, the binary-value row of 2.4) |
| A9 order | applied (3.5), with CF-1 to CF-4 of `construction-form.md` in place of 9.4-7 and C6 in the chain, as that document and the review order them |
| A10 freeze | applied (3.2, P5, 5.1), with one change. The tool skips only the test files that pin `AnsiString` itself, not all of the test project, because the other test files use strings as data and their rewrite is as mechanical as the Units'. P5 is done at zero errors with the patch applied, which is the measurable form of "each error class has a recorded fix" |
| A11 Layout | applied (3.3). The cross-check runs first in P4 and again in 9.5-1 |
| A12 emitted names | applied (3.1, 5.1) |
| A13 proofs | applied (section 4). The glyph test, the 128 to 159 fixture, the input round trip, the dictionary order test and the culture guard land in P6 on the base, so they prove an unchanged result instead of a passing one. The culture guard runs at host start as well as in a test |
| A14 corrections | applied (sections 1, 2.6 and 5) |

## 8. Result

Brief 9.5-1 merged on 2026-11-05 (PR #95, main `c55a631`, with the review fixes of `d4d0e44`). The
gate of the merged pass passed all 13 layers:

- The product layer runs 3,337 tests (`Rtl.Tests` 153, `Units.Tests` 2,914, `Formats.Tests` 270),
  3,334 on the base. The three new tests are the host path test, the static table test and
  `HeadlessServicesTests.MissingStringValueReadsAsEmpty`. The summary carries
  `Strings check: 0 findings` and zero build warnings.
- `git grep -n AnsiString -- src` prints nothing. `src/Rtl/AnsiString.cs` and `Rtl.StringBytes` are
  deleted.
- The exception census holds every count at 0 in every run, as the base does, `NullStringException`
  included. The oracle layer has one run more than the base, the Headless run of the typed-name save
  of 9.7-2.
- The sort dump equals `data/rewrite/9.5-sorts-base.txt` byte for byte.
- The other layers pass: transpiler 328 tests, forms 172, localization 181,410
  assertions in L1 and L2, oracle 177 of 177 saves equal in byte and model modes, frames 100 of 100,
  scenarios 92 states and 92 frames equal.

The tool read 545 files and changed 303 of them, with 7,127 rewrites and 164 residue sites. The 32
`struct-field` sites are fixed by entries of the 57-entry patch `data/rewrite/strings-patch.json`,
the 117 `host-entry` sites are classified, and the 15 `as-ptr` sites are zero-terminated copies
that nothing writes through. The patch entries and the hand edits outside it are recorded by member
in the private `data/rewrite/strings.md`. The product makes 121 `Cp1252` calls outside the tests
and `Rtl/Cp1252.cs`, and that record lists each one under its boundary of 2.4.
