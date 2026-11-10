# RTL shim specification

State: draft, 2026-10-24. Sections 1.2, 2.3 and 3 revised on 2026-11-05 for `string` (brief
9.5-2). The transpiler specification (`transpiler.md`) defines what generated code
expects from the shim. This document defines the shim: the C# library `src/Rtl/Aow.Rtl.csproj`
that stands in for the Delphi 3 runtime library (System, SysUtils), the parts of the VCL the
transpiled code uses (Classes, Windows, Graphics, Controls, Forms, Dialogs, Registry), the core
types the transpiler emits (`Set<E>`, `Ptr<T>`, `TObject` with its metaclass) and the code page
1252 table behind the port's `string` text (`Cp1252`). The
reference for every behavior is the Delphi 3.02 source under `C:\Tools\Delphi3\SOURCE` and, where
that source is assembly, the probe compiled with the oracle compiler.

Scope rule. The shim implements what the shim demand list from the transpiler names, and nothing
else. This document covers the symbols the survey of 2026-10-24 found (`transpiler.md` section 10).
A symbol the demand list adds later gets its entry here before it is implemented.

## Layout

The library is three groups, each in its own folder. Groups 1 and 2 stay in the namespace
`Aow.Rtl`. The group 3 services are in `Aow.Rtl.Host`. `RtlUnit` stays one partial static class in
`Aow.Rtl`, and each partial file holds one topic, so `using static Aow.Rtl.RtlUnit` is unchanged.
The `RtlUnit` members of group 3 (the console output, `Files`, `Clock`, `InstallHost`) are
therefore in `Aow.Rtl` too. The localization runtime is under `src/Rtl/Host/Localize/`. Its
dictionary and runtime types are in `Aow.Rtl.Localize`, and the components that forms name
(`IvTranslator` (Delphi `TIvTranslator`), `IvDictionary` (Delphi `TIvDictionary`), `IvBinaryDictionary` (Delphi `TIvBinaryDictionary`), `IvCustomTranslator` (Delphi `TIvCustomTranslator`),
`TranslatorProperties`, `TranslationTarget`) stay in `Aow.Rtl`.

`src/Rtl/Delphi/` (group 1) is Delphi behavior the game's results depend on. It holds `Random` and
the seed, banker's rounding and the other arithmetic, the code page 1252 table and the string
routines, `CompareText` and `AnsiCompareText`, `Format` and the conversions, `Set<E>`, the
streams, file dates and the `FindFirst` order, and the exceptions the game catches by class.
`Catalog` and `ConstructionContext` sit here too. Both are port machinery the product keeps, outside
the retirement backlog.

`src/Rtl/Legacy/` (group 2) is what the literal translation left behind. Later passes retire it.
The RT-2 inventory counts 88 types and 22 `RtlUnit` members in this group, after the four no-op
registrations and their call sites went. 90 of those 110 symbols occur in product code outside
`src/Rtl` and the test projects. The other 20 have no such occurrence and stay, because a test pins them, Rtl still calls them, or the product names them
under another spelling. The per-symbol counts are the retirement backlog. `StringList` (Delphi `TStringList`)
sorting is in this group with `StringCollection` (Delphi `TStrings`). The order it sorts in is Delphi behavior, and the
comparers that define it stay in group 1.

`src/Rtl/Host/` (group 3) is host services that are not Delphi. It holds the file system, the
clock, the installed host, process safety, the gate switches, console output, the exception census
and the localization runtime. `Aow.Rtl.Host` is the namespace because these types are the host
seam, and `Aow.Rtl.Localize` already names the localization subset.

No implementation moved to `src/Platform`. `RtlUnit` constructs `SystemIoFileSystem` and
`SystemClock` as its defaults, and tests construct them too. The interfaces the units call stay in
this project, which sits below Units.

## 1. Core types for generated code

### 1.1 TObject and the metaclass

CF-3 retires the metaclass shim from the product. `TObject` retains instance identity, class-name
lookup, destruction and freeing. Engine class identity and creation use `EClass`, and family
descriptors use `Catalog<TKey, TValue>`. The historical code below documents the transpiler's
reference output, which still emits `Meta` and diverges from the product source.

```csharp
public class TObject
{
    public int InstanceId { get; }            // from a global counter, assigned in the constructor
    public Meta ClassType => MetaRegistry.Of(GetType());
    public static string ClassNameOf(Type t) { ... }   // the Delphi spelling of a generated type's name
    public virtual void Destroy() { }
    public class Meta
    {
        public Type Type { get; internal set; }
        public string ClassName => ClassNameOf(Type);
        public Meta ClassParent => MetaRegistry.Of(Type.BaseType);   // defined down to the closure root
        public int InstanceSize { get; internal set; }                // from the layout model
        public virtual TObject NewInstance() => Factory();
        internal Func<TObject> Factory;
        public bool InheritsFrom(Type t) => t.IsAssignableFrom(Type);
    }
}
public static class MetaRegistry
{
    public static void Register(Type t, TObject.Meta meta, Func<TObject> factory, int instanceSize) { ... }
    public static TObject.Meta Of(Type t) { ... }    // one instance per type; throws outside the closure
}
// AbstractErrorException (section 1.7) is raised by the body of every abstract method
public static class TObjectExtensions
{
    public static void Free(this TObject o) { if (o != null) o.Destroy(); }
}
```

`InstanceId` starts at 1 and increments per construction on one thread. It replaces object
addresses where the original used them as values (`transpiler.md` rule N7). A `Meta` exists only
for the classes in the metaclass closure of `transpiler.md` 4.4, registered by the generated
`MetaRegistry.cs`. `ClassType` on a class outside the closure throws, and the transpiler never emits
it there. `Component` (Delphi `TComponent`) (section 4.4 of this document) carries the one VCL virtual constructor the
set calls through a class reference, so its `Meta` has the `Create(Component owner)` factory.

### 1.2 Text: `string` and `Cp1252`

Text is the .NET `string`. Until milestone 9.5 the shim held text in a byte string struct,
`AnsiString`, and brief 9.5-1 (merged 2026-11-05) deleted it and moved the whole tree to `string`
in one pass (`strings.md`). Every `char` of a game string is the code page 1252 decoding of exactly
one byte, so the length and the positions of a string equal those of the Delphi bytes. Host paths
are the one kind of text outside that image (section 3.4, `strings.md` 2.5).

`Cp1252` (`src/Rtl/Delphi/Cp1252.cs`) owns the map. The table is frozen, and from 9.7 on it is a format
contract (`strings.md` 2.1 and 2.7).

| member | semantics |
|---|---|
| `Char(byte)` | the 256-entry table. Bytes 0 to 127 and 160 to 255 map to the char of the same code, 128 to 159 to the Windows-1252 chars (0x80 to U+20AC and so on), and the five undefined bytes 129, 141, 143, 144 and 157 to U+0081, U+008D, U+008F, U+0090 and U+009D. It is a bijection onto 256 chars |
| `Byte(char)` | the inverse of `Char`. Any other char gives 63 (`?`) |
| `IsMapped(char)` | true exactly for the 256 chars of `Char` |
| `Text(byte)` | the one-char string of `Char(b)`, from a cache |
| `Decode(ReadOnlySpan<byte>)` | `Char` per byte, zero bytes included |
| `Encode(string)` | `Byte` per char into a new `byte[]` |
| `Normalize(string)` | `Char(Byte(c))` per char, the entry rule for host text that becomes game text |
| `Compare(string, string)` | per position the byte of each char, then the lengths, as `_LStrCmp` in `SYSTEM.PAS` does. A char outside the table orders after every mapped char, by its code. `Compare(a, b) == 0` holds exactly when `a == b` |
| `Comparer` | an `IComparer<string>` over `Compare`, for the sorts, searches and sorted collections over game text |

- Value semantics. A `string` is immutable, `==` is ordinal, and inside the image ordinal equality
  is byte equality because the map is a bijection. A string is a dictionary key as it is. Its hash
  code varies per process, and no game logic depends on it (`strings.md` 2.1).
- Indexing is zero-based: the Delphi `S[I]` reads `S[I - 1]`. Out of range throws
  `IndexOutOfRangeException`, which `Rtl.Wrap` maps to `ERangeError` (section 1.7). Delphi 3 with
  `{$R-}` reads garbage there, and a correct program does not do it.
- An element is a `char`. A Delphi `Char` stays `byte` in variables, fields, parameters, returns
  and sets, and it meets a string element through `Cp1252.Byte`, `Cp1252.Char` and `Cp1252.Text`
  (`strings.md` 2.3).
- `Rtl.WithChar(s, i, c)` returns a copy with the char at 1-based `i` replaced and throws
  `ERangeError` out of range. `Rtl.Resized(s, n)` returns a copy of length `n`, padded with `'\0'`
  when longer, and throws `ERangeError` for a negative `n`. Delphi leaves the new bytes undefined.
  `Rtl.Strings(n)` returns a string array filled with `""`.
- Nulls. A game string is never null, and the sources of a null are closed (`strings.md` 2.2). The
  switch `Rtl.StrictStrings` decides what a null argument does in the routines that read it:
  `Rtl.Length`, `Rtl.IsEmpty`, the text routines of sections 2.3 and 3.1, the file name routines of
  section 3.4 (the `Extract*` family, `ChangeFileExt`, `ExpandFileName`), `Cp1252.Encode`,
  `Normalize` and `Compare`, and the string conversion of `FormatArg` (1.6). Off, a null reads as
  `""`, as Delphi reads nil, and that is the game. On, it throws `NullStringException` (an
  `InvalidOperationException`). `s.Length` on a null throws `NullReferenceException` in both modes.
  `GateSwitches.Enter` turns the switch on when the environment variable `AOW_STRICT_STRINGS` is
  `1`. Game, Headless and the private gate hosts HostChecks and Localization.Tests call it at entry,
  and the gate sets the variable for every command it starts (`testing.md` section 6). The
  `Rtl.Tests` and `Units.Tests` projects turn it on in a module initializer.
- `PChar`. `Ptr<byte>.Unbox(s)` returns a pointer to a zero-terminated encoded copy of `s`, so a
  write through it does not reach the string. `Rtl.StrPas(p)` and `Rtl.FromPChar(p)` decode up to
  the first zero byte, and a null pointer reads `""`. `Rtl.StrPasBuffer(span)` does the same over a
  byte buffer. `Rtl.NullTerminatedText(s)` cuts a string at its first `'\0'`.
- Bytes exist only at the boundaries of `strings.md` 2.4: a stream writer encodes, a stream reader
  decodes, `Rtl.ReadShortString` decodes a Pascal length-prefixed image and `Rtl.WriteShortString`
  encodes and truncates to the image size. A font draws the glyph `Cp1252.Byte(c)` of each char.
- Generated literals are C# string literals. The sources of `src/Units` are pure ASCII.

### 1.3 `Set` (Delphi `TSet`)<E>

`readonly struct Set<E> where E : unmanaged, Enum` with a `ulong Mask`. Members: `Of(params E[])`,
`Range(E lo, E hi)`, `Contains(E)`, `With(E)`, `Without(E)`, `IsSubsetOf(Set<E>)`, `IsEmpty`,
`Count`, operators `+` (union), `-` (difference), `&` (intersection), `==`, `!=`, and
`FromMask(ulong)`. The ordinal of `E` is the bit number. `Set<byte>` is the same struct for a set
over an integer subrange. A constructor argument above 63 throws.

### 1.4 Ptr<T>

`struct Ptr<T>` with fields `T[] Array` and `int Index`. Members: `ref T Value`, `this[int]` (a
`ref T` at `Index + i`), `IsNull`, `AsSpan(int n)` and `AsSpan()`, `Alloc(int n)`, `Realloc(int n)`
(copies the old elements into a new array), `To(T[] array, int index)`, operators `+ int`,
`- int`, `- Ptr<T>` (index difference), `==`, `!=`, `Copy(Ptr<T> src, Ptr<T> dst, int n)` (span
copy, which handles overlap like `Move`) and `Fill(Ptr<T> dst, int n, T value)`. `Reinterpret<U>`
gives a view of the same storage as another unmanaged type, which never nests (a view addresses
the original array at a byte offset).

A `PtrBox<T> : TObject` holds a pointer. Tests construct it directly. `Ptr<T>.Unbox(object)`
gives that pointer back and throws on any other object. `Ptr<byte>.Unbox(string)` is the
zero-terminated copy of section 1.2. `Bytes.Of(ref T)` is the `Span<byte>` over an unmanaged
struct for the untyped buffer routines (`GetCRC32`, `Move`, `FillChar`, `CompareMem` with
`SizeOf`).

What uses `Ptr<T>` is the boundary of the RTL alone: the `PChar` routines of section 2.3, the
typed `Move`, `FillChar`, `GetMem`, `ReallocMem`, `FreeMem`, `New` and `Dispose` of section 2.4,
`TStream.Read`, `Write`, `ReadBuffer` and `WriteBuffer` on a `Ptr<byte>`, `ReadLegacyRecord` (a
record address passed as `TObject`), and the box cases of `BufferBytes` and `DelphiList.IndexOf` (Delphi `TList`). The
units hold no `Ptr<T>` since the graphics slices GP-1 to GP-4 (`graphics.md` 3.5): blocks of
records are arrays, pixels are a `byte[]` with a byte index, optional rectangles are `Rect?`.
They do not convert a pointer to a span and they do not read a boxed pointer as bytes.
`ReadLegacyRecord` recovers a record through `Unbox(object)`. Records grown and shifted as arrays
use `RtlUnit.ReallocArray`, `MoveElements` and `ClearElements` (section 2.4).

### 1.5 IntOrRef and MsgArg

`struct IntOrRef { int Value; object Ref; }` (`src/Rtl/Legacy/IntOrRef.cs`) with implicit conversions from `int`,
from `TObject` (a `PtrBox<T>` among them) and from `string`, and `T As<T>()`, which reads `""` for
a string slot that holds no string. `Value` of a reference is its `InstanceId`, or for a string an
id from the same counter. `Ref` of an integer is `null`. The first stage used it wherever the
source overlays a reference and an integer (`transpiler.md` 4.3). Since milestone 9.6 it remains
only in `HashItem.Key` (Delphi `THashItem`) (`Engine.cs` and `HashTabU.cs`), where `HashList` (Delphi `THashList`) keeps an integer key
and `StringHashItem` (Delphi `TStringHashItem`) a string in the same slot, until the collections pass gives each hash list
a typed key. The message slots took `MsgArg` and the blend
slot of `ShowInfo` (Delphi `TShowInfo`) took the union `BlendValue` of the units (`unions-and-kinds.md` 3.3).

`MsgArg` (`src/Rtl/Legacy/MsgArg.cs`, brief 9.6-4) is the slot of an engine message, a 32-bit word of the
original that holds an integer or an object reference. It is a custom union, a `[Union] readonly
struct` implementing `IUnion`, with two cases:

| case | conversion | read |
|---|---|---|
| `int` | implicit from `int`, so a `byte` or a `short` widens to it | `TryGetValue(out int)`, and `Int` |
| `TObject` | implicit from `TObject?`, so every class derived from it converts | `TryGetValue(out TObject)`, and `As<T>()` |

A nil reference is the union's null, not an object case holding null, since the original slot
holds 0 for it, and `default(MsgArg)` is the same empty slot. `Int` reads the integer, 0 for the
empty slot, and throws `EInvalidCast` on a reference: a reference has no integer view, where
`IntOrRef` offered its `InstanceId`. `As<T>()` reads the reference as `T`, and `null` for an
integer, the empty slot or another type. `Value` boxes and serves reflection only, patterns read
the cases through `TryGetValue`. Equality (`==`, `Equals`) is the equality of the original word:
an integer equals the same integer, a reference the same object, the empty slot, the nil
reference and the integer 0 are all the word 0, and a nonzero integer never equals a reference.
`MapField.SendMapFieldMsg` (Delphi `TMapField`) compares the answer slot that way to tell whether a handler answered.

The parameter arrays are inline arrays of `MsgArg` named by their role, with the arity of the
message protocol as a named constant: `EMsgParams` (Delphi `TEMsgParams`) (`Engine.cs`, `Arity = 3`) for `EMsg.Param` (Delphi `TEMsg`)
and `MapFieldMsgParams` (Delphi `TMapFieldMsgParams`) (`HSEngine.cs`, `Arity = 4`) for `MapFieldMsg.Param` (Delphi `TMapFieldMsg`). `EMsg.Payload`
and `MapFieldMsg.Result` are single `MsgArg` fields. The `Engine`, `Obj` and `Value` accessors of
the first stage's `EMsg` are gone. No message record reaches a stream or a packet.

### 1.6 FormatArg

The argument of `Format` and `CreateFmt`, the Delphi `array of const` element (`src/Rtl/Legacy/FormatArg.cs`,
brief 9.6-1, in place of the first stage's `TVarRec`). `Format(string? format, params
ReadOnlySpan<FormatArg> args)` and every `CreateFmt` of section 1.7 take the arguments as a span.
It is a custom union, a `[Union] readonly struct` implementing `IUnion` over one `long`, one
reference and a case byte, so a value case does not box. The cases are the Delphi `vt` kinds that
`SysUtils.Format` reads, closed by `Format` itself:

| case | Delphi kind | implicit conversions |
|---|---|---|
| `int` | `vtInteger` | `int`, so a `short` or a constant widens to it |
| `bool` | `vtBoolean` | `bool` |
| `char` | `vtChar` | `char`, and `byte`, a Delphi `Char` that enters as `Cp1252.Char` of the byte (`strings.md` 2.3) |
| `double` | `vtExtended` | `double`, so a `uint` or a `long` widens to it |
| `string` | `vtAnsiString` | `string`, which reads the null switch of section 1.2 |
| `TObject` | `vtObject` | `TObject?` |
| `PointerArg` | `vtPointer` | `PointerArg`, a `record struct` over the `uint` address that `%p` prints |

A nil `TObject` is the union's null, not an object case holding null, so `default(FormatArg)` and
`(TObject?)null` are the same empty argument, which `Format` rejects like any argument of the
wrong kind. The conversion operators exist one per source type because the compiler prefers a
user-defined conversion to a union conversion: one operator per case keeps the case each argument
had under `TVarRec`, so a constant 1 stays an integer and never becomes a char. The PChar case and
the class case of `TVarRec` are gone. No caller passes a `Ptr<byte>`, which the build proves, and
the class case went with the metaclass (`metaclass-retirement.md`). `Format` reads each argument
with one switch expression over the cases with no discard arm, so a new case fails the build until
`Format` handles it (3.3).

### 1.7 Exceptions

```
System.Exception
  Aow.Rtl.Exception            Message, HelpContext, Create(msg), CreateFmt(fmt, args)
    AbortException                     Abort() raises it, hosts swallow it silently
    EConvertError, EListError, EStreamError, EReadError, EWriteError, EFOpenError, EFCreateError,
    EInvalidCast, EAccessViolation, EDivByZero, ERangeError, EIntOverflow, EInvalidOp,
    AssertionFailedException, EComponentError, EInvalidOperation, EOutOfMemory, EInOutError,
    AbstractErrorException             raised by the generated body of an abstract method (transpiler.md 4.4)
```

`Rtl.Wrap(System.Exception)` maps .NET runtime exceptions as `transpiler.md` section 5.5 lists and
returns shim exceptions unchanged. `Exception.Message` is a `string`. Generated exception
classes derive from these.

### 1.8 FixedArray

The transpiler emits `[InlineArray(n)] struct FixedArray_<T>_<n> { T element0; }` per distinct
(`T`, `n`) in a shared generated file. The shim has nothing to add.

## 2. System unit

### 2.1 Random

```csharp
public static int RandSeed;                       // the global, read and written by the game (123 uses)
public static int Random(int range)
{
    RandSeed = unchecked(RandSeed * 0x08088405 + 1);
    return (int)(((ulong)(uint)RandSeed * (uint)range) >> 32);
}
public static double Random()
{
    RandSeed = unchecked(RandSeed * 0x08088405 + 1);
    return (uint)RandSeed * (1.0 / 4294967296.0);
}
public static void Randomize()                     // hours*60+minutes, *60+seconds, *1000+ms of the system time
```

This is `_RandInt`, `_RandExt` and `Randomize` from `SYSTEM.PAS` line by line. The `double` result
is exact: a 32-bit integer scaled by a power of two. The game sets `RandSeed` itself from map
seeds, and the oracle harness sets it on both builds. Probe `random` prints the first 1,000 values
of both forms after a fixed seed.

### 2.2 Rounding and arithmetic

`Round(double)` rounds half to even and returns `int` (`_ROUND` uses `FISTP` under the default
control word). `Trunc` is a cast. `Int`, `Frac`, `Abs`, `Sqr`, `Sqrt`, `Sin`, `Cos`, `ArcTan`,
`Ln`, `Exp`, `Power` map to `System.Math`. `Power` is `Math.Pow`.

### 2.3 Strings

Semantics of the string intrinsics, from `SYSTEM.PAS`, over `string` (`src/Rtl/Delphi/StringRoutines.cs`).
The routines keep their Delphi names and 1-based positions, so the transpiled arithmetic at their
call sites is unchanged. Only the direct indexer is zero-based (section 1.2). Every routine is
total over chars outside the table: such a char passes through unchanged and keeps two strings
that differ apart.

- `Copy(S, Index, Count)`: `Index` below 1 becomes 1, `Index` past the end gives empty, `Count`
  at most 0 gives empty, `Count` past the end is clipped.
- `Delete(S, Index, Count)`: nothing happens for `Index` below 1 or past the end or `Count` at
  most 0. `Count` is clipped to the end. A `ref string` overload assigns the result.
- `Insert(Source, S, Index)`: `Index` below 1 becomes 1, past `Length + 1` becomes `Length + 1`.
  A `ref string` overload assigns the result.
- `Pos(Sub, S)`: 0 for an empty `Sub`, else the 1-based position of the first ordinal match or 0.
  `AnsiPos` is `Pos`.
- `SetLength(ref S, N)`: assigns `Rtl.Resized(S, N)` (section 1.2).
- `Length(S)`: the char count, with a null read by the switch of section 1.2.
- `Chr(n)` returns the `byte` `n`, because its result lands in Delphi `Char` bytes. `Ord(c)` of a
  string element is `Cp1252.Byte(c)`, and `Ord` of a `byte` is the byte.
- `UpCase`: ASCII letters only, with a `char` and a `byte` overload.
- Concatenation is `string +`.
- `SetString(ref S, P, Len)` decodes `Len` bytes from a `Ptr<byte>`, and the overload
  `SetString(P, Len)` returns the same string. A null pointer gives `Len` chars `'\0'`, and
  `Len` at most 0 gives `""`.
- `StrLen`, `StrPas`, `StrCopy`, `StrLComp`, `StrScan` on `Ptr<byte>` with zero termination.
  `StrPas` and `FromPChar` decode up to the first zero byte (section 1.2).

### 2.4 Memory and misc

`Move`, `FillChar`, `GetMem`, `FreeMem`, `ReallocMem`, `New`, `Dispose` exist only in the typed
forms the transpiler emits (`transpiler.md` section 5.8). For a block of records held as an
array, `ReallocArray(A, N)` is `ReallocMem` (a new array that keeps the elements both sizes
share, `null` for 0), `MoveElements(Src, I, Dst, J, N)` is `Move` of `N` records between array
indexes, overlapping or not, and `ClearElements(A, I, N)` is `FillChar` of `N` records with zero.
A count of 0 or less does nothing. `Assert(bool, string, string file,
int line)` throws `AssertionFailedException` (Delphi `EAssertionFailed`) with the Delphi message format `"<msg> (<file>, line <n>)"`, and
`Assertion failed` stands for an empty message.
`Halt` throws a host-handled `HaltException` (Delphi `EHalt`). `ParamStr`, `ParamCount` come from the host.

## 3. SysUtils

### 3.1 Strings

The routines work on `string`, char for char as Delphi works byte for byte. Every routine that
takes a string reads a null argument through the switch of section 1.2. A char outside the table
passes through unchanged.

| routine | semantics |
|---|---|
| `IntToStr(int)` | decimal, minus sign for negatives, invariant culture |
| `IntToHex(int, digits)` | uppercase hex of the unsigned value, left padded with zeros to `digits` when `digits` is 1 to 15, as `FormatBuf` ignores another precision |
| `StrToInt(S)` | `Val` semantics: leading spaces skipped, optional `+` or `-`, `$` prefix for hex (not after a sign), digits to the end or to the first `'\0'`, anything else raises `EConvertError` with the message `'%s' is not a valid integer value` |
| `StrToIntDef(S, def)` | the same, returns `def` on error |
| `UpperCase`, `LowerCase` | ASCII letters only. Every other char stays |
| `AnsiUpperCase`, `AnsiLowerCase` | the code page 1252 case tables of `src/Rtl/Delphi/TextCompare.cs`, applied through `Cp1252.Byte` and `Cp1252.Char` to mapped chars, never `ToUpperInvariant`. Lower case adds 32 to 65 to 90, 192 to 214 and 216 to 222 and maps 138, 140, 142 and 159 to 154, 156, 158 and 255. Upper case is the inverse. µ (181) has no case pair in the table and stays. A char outside the table stays |
| `CompareStr(A, B)` | `Cp1252.Compare`: the difference of the first differing bytes or of the lengths |
| `CompareText(A, B)` | `CompareStr` after ASCII upper-casing |
| `AnsiCompareStr`, `AnsiCompareText` | section 3.2 |
| `Trim`, `TrimLeft`, `TrimRight` | remove chars at most 32 at the ends. U+00A0 (byte 0xA0) stays, as in Delphi |
| `Format(fmt, args)` | section 3.3 |
| `QuotedStr` | doubles every `'` and encloses the result in `'` |
| `StringOfChar(c, n)` | `n` copies of `c`, `""` for `n` at most 0. A `byte` overload takes a Delphi `Char` through `Cp1252.Char` |
| `AnsiPos` | `Pos` (section 2.3) |
| `FloatToStr` | not in the demand list, not implemented until it is |

### 3.2 Locale comparison

Delphi's `AnsiCompareText` and `AnsiCompareStr` call the Windows `CompareString` with the user
locale. `StringCollection.IndexOf` and the sorted `StringList` use `AnsiCompareText`. In the transpile set
`IndexOf` is called 173 times, for lookups by name, and the sorted string list sorts only the file
dialog's lists. Equality is what the game logic needs, and Windows equality under
`NORM_IGNORECASE` means equal after case folding. The shim implements:

- `AnsiCompareText(A, B)`: fold the byte of each mapped char with the lower-case table of
  section 3.1, then compare with a fixed collation: every byte by its value, except the soft
  hyphen 173, which collates as 0, and 136 (ˆ), which collates as 94 (`^`). Shorter prefix first.
- `AnsiCompareStr(A, B)`: the same collation without folding.
- A char outside the table orders after every mapped char, by its code, as in `Cp1252.Compare`, so
  two host names that differ stay apart (`HostPathTests`).

This gives the same equality as Windows for code page 1252 and a deterministic order on every
platform. The order differs from Windows word sort for punctuation and accents, which affects
only the file dialog. Probe `strings` checks equality against Windows on all 65,536 byte pairs,
and `SysUtilsProbeTests` pins the 15,568 pairs whose order differs from Windows in
`src/Rtl.Tests/AnsiCompareText-order-exceptions.txt`.

### 3.3 Format

The Delphi 3 format string grammar, from `FormatBuf` in `SYSUTILS.PAS`:

```
'%' [ index ':' ] [ '-' ] [ width ] [ '.' precision ] type
index, width, precision: decimal digits or '*' (taken from the next argument)
type: 'd' 'u' 'x' (integers), 's' (string, char, PChar, object as ClassName is not supported), 'p' (pointer), 'e' 'f' 'g' 'n' 'm' (floating point)
'%%' is a literal percent
```

Letters are case-insensitive. The transpile set uses only `%s` (95) and `%d` (6), with no width or
precision. The shim implements the whole integer and string part and raises `EConvertError` for
the floating point types until the demand list asks for them. `%d` with a precision pads with
zeros. Width pads with spaces on the left, or on the right with `-`. `%s` takes the string case
or the char case (1.6), the char with no precision cut. `%p` takes the pointer case and prints its
eight uppercase hex digits. Any other case for a conversion, the empty argument included, raises
`EConvertError`, as Delphi does for `%s` of an integer. A string or a char argument keeps a char
outside the table. The implementation is the shim's own `FormatBuf` port in
`src/Rtl/Delphi/Format.cs`, never `string.Format`.

### 3.4 Files and paths

| routine | semantics |
|---|---|
| `ExtractFileName`, `ExtractFilePath`, `ExtractFileExt`, `ChangeFileExt`, `ExtractFileDir` | split at the last `\` or `:`, as Delphi 3 does, never at `/` (probe `files`). The host's file service accepts both separators (section 7) |
| `ExpandFileName` | absolute path with `.` and `..` removed, relative to the host's working directory |
| `FileExists`, `DirectoryExists`, `DeleteFile`, `RenameFile`, `CreateDir`, `GetCurrentDir` | through the host file system |
| `FindFirst`, `FindNext`, `FindClose` with `SearchRec` (Delphi `TSearchRec`) (`Name`, `Size`, `Attr`, `Time`) | section 3.5 |
| `FileAge`, `FileDateToDateTime` | the file's modification time as `TDateTime` |

Attributes: `faDirectory`, `faAnyFile`, `faReadOnly`, `faHidden`, `faArchive`. Delphi 3 filters
on the hidden, system, volume and directory bits only: an entry is returned unless it has one of
those bits and `Attr` does not, so read-only and archive files come back whatever the mask, and a
plain file reports attribute 128. A search that includes directories also returns `.` and `..`.
The end of an enumeration is code 18, and a pattern with no match returns code 2 (probe `files`,
2026-10-26). `ExtractFileName` and `ExtractFilePath` of Delphi 3 do not split at `/`, and the shim keeps that.

Paths are `string` with the host's chars (`strings.md` 2.5). `ParamStr`, `GetCurrentDir`,
`ExpandFileName` and the `FindFirst` names keep a char outside the table, and so does a settings value
under a path key (`strings.md` 2.5). A path meets the table only at the font (`Cp1252.Byte`, glyph 63)
and at a stream writer (`Cp1252.Encode`, byte 63). `SearchRec.Name` is a property that reads a
null as `""`, because `SearchRec search = default` skips every initializer.

### 3.5 Directory enumeration order

`FindFirst` and `FindNext` on NTFS return entries in the order of the directory index, which is
the ordinal order of the upper-cased file name. The game enumerates image libraries, sounds,
plug-ins and user files with `FindFirst` (13 call sites), and the order can decide resource
indices. The shim therefore sorts every enumeration by the upper-cased name in ordinal order of
UTF-16 code units before returning it, on every platform. Probe `files` compares against a real
NTFS listing of a prepared directory.

### 3.6 Time

`Now`, `Time`, `Date` return `TDateTime` (a `double`, days since 1899-12-30, fraction of day).
`DecodeTime`, `DecodeDate`, `EncodeTime`, `EncodeDate`, `DateTimeToStr` (with the Delphi default
format `dd.mm.yy hh:nn:ss` replaced by the host locale's short format, used once for display).
`GetTickCount` and `timeGetTime` return milliseconds from the host clock. The headless harness
replaces the clock with a fixed one (`testing.md`).

## 4. Classes

The shim ports the Delphi 3 `CLASSES.PAS` classes the set uses, method by method, keeping the
algorithms. The reasons are in the notes per class.

### 4.1 DelphiList

Fields `List` (a `object[]`, exposed because the generated code reads `FList^[I]` 224 times),
`Count`, `Capacity`. Methods `Add`, `Insert`, `Delete`, `Remove`, `IndexOf`, `Exchange`, `Move`,
`First`, `Last`, `Clear`, `Pack`, `Expand`, `Sort(ListSortCompare)`, `Error`. `Grow` adds a quarter of
`Capacity` when `Capacity > 64`, 16 when `Capacity > 8`, and 4 otherwise, so 8 grows to 12 and 64
to 80 (probe `lists`). `Sort` is the `QuickSort` of `CLASSES.PAS`: pivot at
`(L + R) shr 1`, scan from both ends with the comparator, swap while `I <= J`, recurse on the left
part and loop on the right. The AI sorts candidate lists with its own comparators
(`CompareAutoProductions`, `CompareWanderFields`, `CompareAIPlayerActionClasses`), and the order of
equal elements after this exact algorithm decides which candidate wins, so the algorithm is ported
as written and probe `lists` checks it on arrays with many equal keys.

`DelphiList` of the shim is also the base of the engine's `EObjectList` (Delphi `TEObjectList`) and `EObjectClassList` (Delphi `TEObjectClassList`)
(generated code).

### 4.2 StringCollection and StringList

`StringCollection`: `Count`, `Strings[i]`, `Objects[i]`, `Add`, `AddObject`, `Insert`, `Delete`, `Clear`,
`IndexOf` (linear, `AnsiCompareText`), `IndexOfObject`, `IndexOfName`, `Names`, `Values`,
`Text` (`CR LF` after every line, including the last, and a reader that accepts `CR`, `LF` or
both), `CommaText`, `LoadFromFile`, `LoadFromStream`, `SaveToFile`, `SaveToStream`, `Assign`,
`AddStrings`, `BeginUpdate`, `EndUpdate`, `Exchange`, `Move`.

`StringList`: the above plus `Sorted`, `Duplicates` (`dupIgnore`, `dupAccept`, `dupError`),
`Sort`, `Find` (binary search with `AnsiCompareText`, moving to the first equal entry unless
`dupAccept`), `OnChange`, `OnChanging`. `Add` on a sorted list uses `Find` and the `Duplicates`
rule. `Sort` is the `QuickSort` of `CLASSES.PAS` with `AnsiCompareText`. `IndexOf` on a sorted list
uses `Find`.

### 4.3 Streams

`TStream`: `Read`, `Write` (abstract, on `Span<byte>`), `ReadBuffer`, `WriteBuffer` (raise
`EReadError`, `EWriteError` on a short transfer), `Seek(offset, origin)`, `Position`, `Size`,
`CopyFrom(source, count)` (count 0 copies all from position 0, in blocks of 61,440 bytes),
`ReadComponent` and `WriteComponent` are not implemented (the layout loader replaces them).
Overloads `Read(ref T)` and `Write(in T)` for unmanaged `T` serve the generated `Stream.Read(X,
SizeOf(X))` calls and use the struct's bytes in its explicit layout.

`TMemoryStream`: `Memory` (a `byte[]`, exposed because the code reads `.Memory^`), `Capacity`,
`SetSize`, `Clear`, `LoadFromStream`, `LoadFromFile`, `SaveToStream`, `SaveToFile`. Capacity grows
to the next multiple of 8,192, as `Realloc` does. `Write` past the end grows the stream.

`TFileStream(name, mode)` with `fmCreate`, `fmOpenRead`, `fmOpenWrite`, `fmOpenReadWrite` and the
share flags ignored except `fmShareDenyWrite`, which the host maps to a read share. A failed open
raises `EFOpenError` or `EFCreateError` with the Delphi message.

`THandleStream` is not needed. The engine's `FastMemoryStream` (Delphi `TFastMemoryStream`) is generated code over `TStream`.

### 4.4 `Persistent` (Delphi `TPersistent`) and Component

`Persistent`: `Assign(source)` calls `source.AssignTo(this)`, whose default raises `EConvertError`
with `Cannot assign a %s to a %s`. `GetNamePath`, `GetOwner`.

`Component`: `Owner`, `Name` (setter validates the identifier, checks duplicates in the owner
with `CompareText`, raises `EComponentError`), `Tag`, `ComponentCount`, `Components[i]`,
`ComponentIndex`, `ComponentState` (`csLoading`, `csDestroying` set recursively by `Destroying` as in `CLASSES.PAS`, `csDesigning` never set),
`InsertComponent`, `RemoveComponent`, `FindComponent` (`CompareText`), `DestroyComponents`
(destroys from the last to the first), `Notification` (recursive to owned components),
`FreeNotification`, `Loaded`, `Destroying`, `GetChildren`, `GetParentComponent`, `HasParent`,
`SetChildOrder`, `DefineProperties` (no-op), `Destroy` in the `CLASSES.PAS` order: free
notifications, `Destroying`, `DestroyComponents`, removal from the owner, `base.Destroy()`.

The layout loader (`ui-layout.md`) creates components and sets their published properties by
reflection on the `[Published]` attribute. The shim provides `Rtti.PublishedProperties(Type)`
with the property name, type, default value and setter, cached per type.

### 4.5 Others

`TThread` is used only by rewritten and dropped units and is not implemented. `TBits`,
`TCollection`, `TReader`, `TWriter`, `TParser` are not in the set.

## 5. Windows, Graphics, Controls, Forms, Dialogs

### 5.1 Types and helpers

`Rect` (Delphi `TRect`) (`Left, Top, Right, Bottom`, plus `Rect()`, `Bounds()`, `OffsetRect`, `InflateRect`,
`UnionRect`, `IntersectRect`, `PtInRect`, `IsRectEmpty`, `EqualRect`), `TPoint` (`Point()`),
`HWND`, `HDC`, `DWORD`, `BOOL`, `HResult`. `TColor` is `int` with the `cl*` constants the code uses
(`clBtnFace`, `clRed`, `clBlack`, `clWhite`, `clNone`, `clDefault`, `clBtnShadow`,
`clBtnHighlight`, `clAppWorkSpace`) and `ColorToRGB` mapping the system colors to the Windows
default theme values. `ShiftState` (Delphi `TShiftState`) (a `Set` of `ssShift`, `ssAlt`, `ssCtrl`, `ssLeft`,
`ssRight`, `ssMiddle`, `ssDouble`), `MouseButton` (Delphi `TMouseButton`), `TCursor`, `NotifyEvent` (Delphi `TNotifyEvent`), `KeyEvent` (Delphi `TKeyEvent`),
`MouseEvent` (Delphi `TMouseEvent`), `MouseMoveEvent` (Delphi `TMouseMoveEvent`), `KeyPressEvent` (Delphi `TKeyPressEvent`), the `VK_*` and `WM_*` constants the code
names, `MulDiv`, `HiWord`, `LoWord`, `MakeLong`, `GetSystemMetrics` (screen size from the host).

### 5.2 Dialogs

`ShowMessage(msg)`, `MessageDlg(msg, type, buttons, helpCtx)`, `MessageBox(hwnd, text, caption,
flags)` and `Application.MessageBox` go to the host's `IDialogService`. The game compares the
results (`mrYes`, `mrNo`, `mrCancel`, `mrOk`, 20 comparisons), so the calls are modal: the service
shows the dialog and runs a nested frame loop until the player answers, then returns the result.
The headless harness answers from a script (`testing.md`). The constants `mt*`, `mb*`, `mr*`,
`MB_*`, `id*` keep their Delphi values.

`OpenUrl(url)` goes to the host's `IShellService` (`IRtlHost.Shell`), the port's form of the
original's `ShellExecute` of a web address (`platform.md` section 9).

### 5.3 Application, Screen, Clipboard

`Application.ExeName` (12 uses) returns the host's executable path or, for the game root, the
configured game directory. `Application.Handle`, `MainForm`, `Title`, `Active`, `Terminate` and
`CreateForm` are host hooks. `Screen.ActiveForm` returns `null`. `Clipboard.AsText`,
`HasFormat`, `GetComponent`, `SetComponent` are stubs that hold one value in memory.

### 5.4 Registry and ini files

`Registry` (Delphi `TRegistry`) (`OpenKey`, `CreateKey`, `ReadString`, `WriteString`, `ReadInteger`, `WriteInteger`,
`ReadBool`, `WriteBool`, `ValueExists`, `KeyExists`, `DeleteValue`, `RootKey`, `CloseKey`) is
backed by the host's settings store, a JSON file in the user data directory, with the key path as
the section. The game's `AoWRegistry` (Delphi `TAoWRegistry`) (generated) derives from it. `IniFile` (Delphi `TIniFile`) (`ReadString`,
`WriteString`, `ReadInteger`, `WriteInteger`, `ReadBool`, `ReadSection`, `ReadSections`,
`EraseSection`) reads and writes a real ini file, because `ImageSequence` (Delphi `TImageSequence`) stores frame tables in
ini files next to the image libraries.

## 6. Shim behaviors that affect the oracle

These are the places where a careless shim makes the headless comparison fail. Each has a probe.

1. `Random` and `RandSeed` (2.1). Probe `random`.
2. `DelphiList.Sort` (4.1). Probe `lists`.
3. `StringList` sorted insertion and `Find` (4.2). Probe `lists`.
4. `AnsiCompareText` equality (3.2). Probe `strings`.
5. `FindFirst` order (3.5). Probe `files`.
6. `Round` (2.2). Probe `floats`.
7. `StringCollection.Text` line endings and the reader (4.2). Probe `strings`.
8. `Copy`, `Delete`, `Insert`, `Pos` edge cases (2.3). Probe `strings`.
9. `StrToInt` acceptance (3.1). Probe `strings`.
10. `TMemoryStream` growth and `Position` after writes (4.3). Probe `streams`.

## 7. Host interface

The shim depends on the host through one interface, so that the headless harness, the SDL host
and a test can each provide it:

```csharp
public interface IRtlHost
{
    IFileSystem Files { get; }        // open, exists, enumerate (sorted as 3.5), delete, rename, directories
    IDialogService Dialogs { get; }   // MessageDlg, MessageBox, ShowMessage, modal
    IClock Clock { get; }             // Now, GetTickCount, timeGetTime
    ISettingsStore Settings { get; }  // Registry backing store
    string ExeName { get; }
    string GameRoot { get; }
}
```

Paths. The game builds paths with `\` and the original root directory. `IFileSystem` accepts both
separators, maps `\` to the platform separator, and resolves case-insensitively on case-sensitive
file systems by directory listing, because the data files use mixed case (`Images/`, `IMAGES/`,
`.ILB`, `.ilb`).

## 8. Testing

Unit tests in `src/Rtl.Tests` cover the members in this document. Where a Delphi probe recorded
library behavior, the recording ships in `src/Rtl.Tests/Probes` and the test reads that file.
The tests do not look up a private checkout. Where the probe cannot run (host-dependent members),
the test states the expected behavior from `CLASSES.PAS` or `SYSUTILS.PAS` with the source line
in a comment.

The gate's probes layer runs the hand-written graphics probes `blitters`, `libraries` and
`blendtables` against their recordings. It does not translate Delphi probes. The transpiler is
not a gate layer. The forms model still builds `Transpiler.slnx`, because `Forms.Model`
references that assembly and reads `tools/transpile/reference/overrides.toml`.

The early done measures named the probe runner as the check:

1. R1: the core types of section 1 exist, with tests.
2. R2: every member the shim demand list names after the first transpiler run over Engine exists.
3. R3: the probes `random`, `strings`, `format`, `lists`, `streams`, `files` and `floats` were
   the gate check for those routines. The same facts now live in `src/Rtl.Tests` and read the
   copied recordings, together with `sets`, `integers`, `integer-list-slots`, `record-move` and
   `reference-fill`.
4. R4: every member the demand list names after the run over the whole transpile set exists.
