# RTL shim specification

State: draft, 2026-10-24. The transpiler specification (`transpiler.md`) defines what generated code
expects from the shim. This document defines the shim: the C# library `src/Rtl/Aow.Rtl.csproj`
that stands in for the Delphi 3 runtime library (System, SysUtils), the parts of the VCL the
transpiled code uses (Classes, Windows, Graphics, Controls, Forms, Dialogs, Registry) and the core
types the transpiler emits (`AnsiString`, `TSet<E>`, `Ptr<T>`, `TObject` with its metaclass). The
reference for every behavior is the Delphi 3.02 source under `C:\Tools\Delphi3\SOURCE` and, where
that source is assembly, the probe compiled with the oracle compiler.

Scope rule. The shim implements what the shim demand list from the transpiler names, and nothing
else. This document covers the symbols the survey of 2026-10-24 found (`transpiler.md` section 10).
A symbol the demand list adds later gets its entry here before it is implemented.

## 1. Core types for generated code

### 1.1 TObject and the metaclass

```csharp
public class TObject
{
    public int InstanceId { get; }            // from a global counter, assigned in the constructor
    public virtual Meta ClassType => Class;
    public static Meta Class { get; } = new Meta();
    public virtual void Destroy() { }
    public class Meta
    {
        public virtual string ClassName => "TObject";
        public virtual Meta ClassParent => null;
        public virtual TObject NewInstance() => new TObject();
        public bool InheritsFrom(Meta other) { ... }   // walks ClassParent
    }
}
public static class TObjectExtensions
{
    public static void Free(this TObject o) { if (o != null) o.Destroy(); }
}
```

`InstanceId` starts at 1 and increments per construction on one thread. It replaces object
addresses where the original used them as values (`transpiler.md` rule N7). `ClassName` returns the
Delphi spelling. Generated classes override `ClassType`, `Class` and `Meta` as `transpiler.md`
section 4.4 describes.

### 1.2 AnsiString

A `readonly struct` that holds a `byte[]` and behaves like a Delphi 3 long string:

- Value semantics. Assignment shares the array. Every mutating operation returns a new struct with a
  new array (`WithChar`, `Resized`, concatenation). This is copy on write without reference counts.
- `Length` is the array length. The empty string is the default struct (`null` array), and every
  member treats a `null` array as empty.
- `this[int i]` returns the byte at 1-based `i`. Out of range throws `ERangeError`. Delphi 3 with
  `{$R-}` reads garbage there, and a correct program does not do it.
- `WithChar(int i, byte c)` returns a copy with the byte replaced.
- `Resized(int n)` returns a copy of length `n`, padded with zero bytes when longer. Delphi leaves
  the new bytes undefined.
- Operators `+`, `==`, `!=`, `<`, `<=`, `>`, `>=`. Comparison is unsigned byte-wise over the common
  length, then by length. `_LStrCmp` in `SYSTEM.PAS` does exactly that.
- `implicit operator AnsiString(string)` encodes with code page 1252. Generated literals use it.
  The transpiler writes bytes above 127 as `\xNN` escapes, so the conversion is exact.
- `ToString()` decodes with code page 1252, for logs and for the host's file APIs.
- `FromBytes(params byte[])`, `FromPChar(Ptr<byte>)` (up to the first zero byte), `AsPtr()` (a
  `Ptr<byte>` over a copy with one trailing zero byte).
- `GetHashCode` and `Equals` are byte-wise, so an `AnsiString` is a dictionary key.

### 1.3 TSet<E>

`readonly struct TSet<E> where E : unmanaged, Enum` with a `ulong Mask`. Members: `Of(params E[])`,
`Range(E lo, E hi)`, `Contains(E)`, `With(E)`, `Without(E)`, `IsSubsetOf(TSet<E>)`, `IsEmpty`,
`Count`, operators `+` (union), `-` (difference), `&` (intersection), `==`, `!=`, and
`FromMask(ulong)`. The ordinal of `E` is the bit number. `TSet<byte>` is the same struct for a set
over an integer subrange. A constructor argument above 63 throws.

### 1.4 Ptr<T>

`struct Ptr<T>` with fields `T[] Array` and `int Index`. Members: `ref T Value`, `this[int]` (a
`ref T` at `Index + i`), `IsNull`, `Alloc(int n)`, `Realloc(int n)` (copies the old elements into a
new array), `To(T[] array, int index)`, operators `+ int`, `- int`, `- Ptr<T>` (index difference),
`==`, `!=`, `Copy(Ptr<T> src, Ptr<T> dst, int n)` (`Array.Copy`, which handles overlap like
`Move`), and `Fill(Ptr<T> dst, int n, T value)`.

### 1.5 IntOrRef

`struct IntOrRef { int Value; object Ref; }` with implicit conversions from `int` and from
`TObject`, and `T As<T>()`. `TEMsg.Param` and the message `Data` array use it. `Value` of a
reference is its `InstanceId`. `Ref` of an integer is `null`.

### 1.6 TVarRec

The tagged value of `array of const`: `struct TVarRec { VarKind Kind; long Int; double Ext; AnsiString Str; object Obj; bool Bool; byte Chr; }`
with implicit conversions from `int`, `byte`, `bool`, `double`, `AnsiString`, `TObject` and
`TObject.Meta`. `Format` reads it.

### 1.7 Exceptions

```
System.Exception
  Aow.Rtl.Exception            Message, HelpContext, Create(msg), CreateFmt(fmt, args)
    EAbort                     Abort() raises it, hosts swallow it silently
    EConvertError, EListError, EStreamError, EReadError, EWriteError, EFOpenError, EFCreateError,
    EInvalidCast, EAccessViolation, EDivByZero, ERangeError, EIntOverflow, EInvalidOp,
    EAssertionFailed, EComponentError, EInvalidOperation, EOutOfMemory, EInOutError
```

`Rtl.Wrap(System.Exception)` maps .NET runtime exceptions as `transpiler.md` section 5.5 lists and
returns shim exceptions unchanged. `Exception.Message` is an `AnsiString`. Generated exception
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

Semantics of the string intrinsics, from `SYSTEM.PAS`:

- `Copy(S, Index, Count)`: `Index` below 1 becomes 1, `Index` past the end gives empty, `Count`
  below 0 becomes 0, `Count` past the end is clipped.
- `Delete(S, Index, Count)`: nothing happens for `Index` below 1 or past the end or `Count` at
  most 0. `Count` is clipped to the end.
- `Insert(Source, S, Index)`: `Index` below 1 becomes 1, past `Length + 1` becomes `Length + 1`.
- `Pos(Sub, S)`: 0 for an empty `Sub`, else the 1-based position of the first match or 0.
- `SetLength(S, N)`: see `Resized`.
- `Length`, `Chr`, `Ord`, `UpCase` (ASCII only), string concatenation.
- `SetString(S, P, Len)` builds from a `Ptr<byte>` and a length.
- `StrLen`, `StrPas`, `StrCopy`, `StrLComp`, `StrScan` on `Ptr<byte>` with zero termination.

### 2.4 Memory and misc

`Move`, `FillChar`, `GetMem`, `FreeMem`, `ReallocMem`, `New`, `Dispose` exist only in the typed
forms the transpiler emits (`transpiler.md` section 5.8). `Assert(bool, AnsiString, string file,
int line)` throws `EAssertionFailed` with the Delphi message format `"<msg> (<file>, line <n>)"`.
`Halt` throws a host-handled `EHalt`. `ParamStr`, `ParamCount` come from the host.

## 3. SysUtils

### 3.1 Strings

| routine | semantics |
|---|---|
| `IntToStr(int)` | decimal, minus sign for negatives |
| `IntToHex(int, digits)` | uppercase hex, left padded with zeros to `digits` |
| `StrToInt(S)` | `Val` semantics: leading spaces skipped, optional `+` or `-`, `$` prefix for hex, digits to the end, anything else raises `EConvertError` with the message `'%s' is not a valid integer value` |
| `StrToIntDef(S, def)` | the same, returns `def` on error |
| `UpperCase`, `LowerCase` | ASCII letters only |
| `AnsiUpperCase`, `AnsiLowerCase` | code page 1252 letters, including the accented ones, by a fixed table |
| `CompareStr(A, B)` | byte-wise, returns the difference of the first differing bytes or of the lengths |
| `CompareText(A, B)` | `CompareStr` after ASCII upper-casing |
| `AnsiCompareStr`, `AnsiCompareText` | section 3.2 |
| `Trim`, `TrimLeft`, `TrimRight` | remove bytes at most 32 at the ends |
| `Format(fmt, args)` | section 3.3 |
| `QuotedStr`, `StringOfChar`, `AnsiPos` | as Delphi |
| `FloatToStr` | not in the demand list, not implemented until it is |

### 3.2 Locale comparison

Delphi's `AnsiCompareText` and `AnsiCompareStr` call the Windows `CompareString` with the user
locale. `TStrings.IndexOf` and the sorted `TStringList` use `AnsiCompareText`. In the transpile set
`IndexOf` is called 173 times, for lookups by name, and the sorted string list sorts only the file
dialog's lists. Equality is what the game logic needs, and Windows equality under
`NORM_IGNORECASE` means equal after case folding. The shim implements:

- `AnsiCompareText(A, B)`: fold both with the code page 1252 case table (ASCII letters and the
  accented letters 192 to 222 fold to 224 to 254, except 215 and 247), then compare with a fixed
  collation: letters and digits by their folded value, every other byte by its value, shorter
  prefix first.
- `AnsiCompareStr(A, B)`: the same collation without folding.

This gives the same equality as Windows for code page 1252 and a deterministic order on every
platform. The order differs from Windows word sort for punctuation and accents, which affects
only the file dialog. Probe `strings` checks equality against Windows on all 65,536 byte pairs.

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
zeros. Width pads with spaces on the left, or on the right with `-`. `%s` of a `TVarRec` holding
an integer raises `EConvertError`, as Delphi does.

### 3.4 Files and paths

| routine | semantics |
|---|---|
| `ExtractFileName`, `ExtractFilePath`, `ExtractFileExt`, `ChangeFileExt`, `ExtractFileDir` | split at the last `\` or `:`. The port also accepts `/`, because the host converts paths (section 7) |
| `ExpandFileName` | absolute path with `.` and `..` removed, relative to the host's working directory |
| `FileExists`, `DirectoryExists`, `DeleteFile`, `RenameFile`, `CreateDir`, `GetCurrentDir` | through the host file system |
| `FindFirst`, `FindNext`, `FindClose` with `TSearchRec` (`Name`, `Size`, `Attr`, `Time`) | section 3.5 |
| `FileAge`, `FileDateToDateTime` | the file's modification time as `TDateTime` |

Attributes: `faDirectory`, `faAnyFile`, `faReadOnly`, `faHidden`, `faArchive`, with the Delphi
rule that a search with `Attr` returns files whose attributes are a subset of `Attr` plus the
normal files, and `faDirectory` not in `Attr` excludes directories.

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

### 4.1 TList

Fields `List` (a `object[]`, exposed because the generated code reads `FList^[I]` 224 times),
`Count`, `Capacity`. Methods `Add`, `Insert`, `Delete`, `Remove`, `IndexOf`, `Exchange`, `Move`,
`First`, `Last`, `Clear`, `Pack`, `Expand`, `Sort(TListSortCompare)`, `Error`. `Grow` adds 4 below
8 entries, 16 up to 64, and a quarter above. `Sort` is the `QuickSort` of `CLASSES.PAS`: pivot at
`(L + R) shr 1`, scan from both ends with the comparator, swap while `I <= J`, recurse on the left
part and loop on the right. The AI sorts candidate lists with its own comparators
(`CompareAutoProductions`, `CompareWanderFields`, `CompareAIPlayerActionClasses`), and the order of
equal elements after this exact algorithm decides which candidate wins, so the algorithm is ported
as written and probe `lists` checks it on arrays with many equal keys.

`TList` of the shim is also the base of the engine's `TEObjectList` and `TEObjectClassList`
(generated code).

### 4.2 TStrings and TStringList

`TStrings`: `Count`, `Strings[i]`, `Objects[i]`, `Add`, `AddObject`, `Insert`, `Delete`, `Clear`,
`IndexOf` (linear, `AnsiCompareText`), `IndexOfObject`, `IndexOfName`, `Names`, `Values`,
`Text` (`CR LF` after every line, including the last, and a reader that accepts `CR`, `LF` or
both), `CommaText`, `LoadFromFile`, `LoadFromStream`, `SaveToFile`, `SaveToStream`, `Assign`,
`AddStrings`, `BeginUpdate`, `EndUpdate`, `Exchange`, `Move`.

`TStringList`: the above plus `Sorted`, `Duplicates` (`dupIgnore`, `dupAccept`, `dupError`),
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

`THandleStream` is not needed. The engine's `TFastMemoryStream` is generated code over `TStream`.

### 4.4 TPersistent and TComponent

`TPersistent`: `Assign(source)` calls `source.AssignTo(this)`, whose default raises `EConvertError`
with `Cannot assign a %s to a %s`. `GetNamePath`, `GetOwner`.

`TComponent`: `Owner`, `Name` (setter validates the identifier, checks duplicates in the owner
with `CompareText`, raises `EComponentError`), `Tag`, `ComponentCount`, `Components[i]`,
`ComponentIndex`, `ComponentState` (`csLoading`, `csDestroying`, `csDesigning` never set),
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

`TRect` (`Left, Top, Right, Bottom`, plus `Rect()`, `Bounds()`, `OffsetRect`, `InflateRect`,
`UnionRect`, `IntersectRect`, `PtInRect`, `IsRectEmpty`, `EqualRect`), `TPoint` (`Point()`),
`HWND`, `HDC`, `DWORD`, `BOOL`, `HResult`. `TColor` is `int` with the `cl*` constants the code uses
(`clBtnFace`, `clRed`, `clBlack`, `clWhite`, `clNone`, `clDefault`, `clBtnShadow`,
`clBtnHighlight`, `clAppWorkSpace`) and `ColorToRGB` mapping the system colors to the Windows
default theme values. `TShiftState` (a `TSet` of `ssShift`, `ssAlt`, `ssCtrl`, `ssLeft`,
`ssRight`, `ssMiddle`, `ssDouble`), `TMouseButton`, `TCursor`, `TNotifyEvent`, `TKeyEvent`,
`TMouseEvent`, `TMouseMoveEvent`, `TKeyPressEvent`, the `VK_*` and `WM_*` constants the code
names, `MulDiv`, `HiWord`, `LoWord`, `MakeLong`, `GetSystemMetrics` (screen size from the host).

### 5.2 Dialogs

`ShowMessage(msg)`, `MessageDlg(msg, type, buttons, helpCtx)`, `MessageBox(hwnd, text, caption,
flags)` and `Application.MessageBox` go to the host's `IDialogService`. The game compares the
results (`mrYes`, `mrNo`, `mrCancel`, `mrOk`, 20 comparisons), so the calls are modal: the service
shows the dialog and runs a nested frame loop until the player answers, then returns the result.
The headless harness answers from a script (`testing.md`). The constants `mt*`, `mb*`, `mr*`,
`MB_*`, `id*` keep their Delphi values.

### 5.3 Application, Screen, Clipboard

`Application.ExeName` (12 uses) returns the host's executable path or, for the game root, the
configured game directory. `Application.Handle`, `MainForm`, `Title`, `Active`, `Terminate` and
`CreateForm` are host hooks. `Screen.ActiveForm` returns `null`. `Clipboard.AsText`,
`HasFormat`, `GetComponent`, `SetComponent` are stubs that hold one value in memory.

### 5.4 Registry and ini files

`TRegistry` (`OpenKey`, `CreateKey`, `ReadString`, `WriteString`, `ReadInteger`, `WriteInteger`,
`ReadBool`, `WriteBool`, `ValueExists`, `KeyExists`, `DeleteValue`, `RootKey`, `CloseKey`) is
backed by the host's settings store, a JSON file in the user data directory, with the key path as
the section. The game's `TAoWRegistry` (generated) derives from it. `TIniFile` (`ReadString`,
`WriteString`, `ReadInteger`, `WriteInteger`, `ReadBool`, `ReadSection`, `ReadSections`,
`EraseSection`) reads and writes a real ini file, because `TImageSequence` stores frame tables in
ini files next to the image libraries.

## 6. Shim behaviors that affect the oracle

These are the places where a careless shim makes the headless comparison fail. Each has a probe.

1. `Random` and `RandSeed` (2.1). Probe `random`.
2. `TList.Sort` (4.1). Probe `lists`.
3. `TStringList` sorted insertion and `Find` (4.2). Probe `lists`.
4. `AnsiCompareText` equality (3.2). Probe `strings`.
5. `FindFirst` order (3.5). Probe `files`.
6. `Round` (2.2). Probe `floats`.
7. `TStrings.Text` line endings and the reader (4.2). Probe `strings`.
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
    ISettingsStore Settings { get; }  // TRegistry backing store
    string ExeName { get; }
    string GameRoot { get; }
}
```

Paths. The game builds paths with `\` and the original root directory. `IFileSystem` accepts both
separators, maps `\` to the platform separator, and resolves case-insensitively on case-sensitive
file systems by directory listing, because the data files use mixed case (`Images/`, `IMAGES/`,
`.ILB`, `.ilb`).

## 8. Testing

Unit tests in `src/Rtl.Tests/` for every member in this document, with fixtures from the probes
where a probe exists. The probe outputs come from `tests/probes/expected/` (`transpiler.md`
section 11): the shim test loads the expected text and compares the shim's output for the same
calls. Where the probe cannot run (host-dependent members), the test states the expected behavior
from `CLASSES.PAS` or `SYSUTILS.PAS` with the source line in a comment.

Done measures:

1. R1: the core types of section 1 exist, with tests, and a hand-written C# file in the style of
   the transpiler's output compiles against them.
2. R2: every member the shim demand list names after the first transpiler run over Engine exists.
3. R3: the probes `random`, `strings`, `format`, `lists`, `streams`, `files`, `floats` pass
   against the shim.
4. R4: every member the demand list names after the run over the whole transpile set exists.
