# Transpiler specification: Delphi 3 to C#

State: draft, 2026-10-24. Decisions in `../design.md` section 3 apply.

The transpiler reads the recovered Delphi 3 units of the transpile set and writes C# that compiles
against the RTL shim (`rtl-shim.md`). It is regenerable: the same input gives the same output, and
hand-written code lives only in overrides. This document fixes the pipeline, the mapping of every
construct the source uses, the normalization of the byte-matching idioms, the semantic traps, the
override mechanism, the report and the done measures.

The numbers in this document come from a survey of `D:\git\aow\src\delphi3` on 2026-10-24 over the
units in the transpile set (section 3 of `../design.md`). They size the work and show which
constructs matter. Section 10 lists them.

## 1. Inputs and outputs

Inputs:

- The transpile set: the units listed in `../design.md` section 3, under `D:\git\aow\src\delphi3`,
  with their include files. The set is a list in a configuration file, `tools/transpile/units.toml`,
  one entry per unit with its path and its treatment (`transpile`, `skip`).
- The Delphi 3 RTL and VCL sources under `C:\Tools\Delphi3\SOURCE` (`RTL\SYS`, `RTL\WIN`, `VCL`,
  `TOOLSAPI`) and the third-party interface units under `D:\git\aow\src\delphi3\imports`. The
  transpiler parses only their interface sections, to resolve names and types. It never emits code
  for them.
- The class database `D:\git\aow\notes\classes.json`: for every class of the original binaries, its
  parent, instance size, virtual method table and fields by byte offset. Section 6 uses it.
- The override manifest `src/Overrides/overrides.toml` (section 8).

Outputs:

- One C# file per unit, `src/Generated/<Unit>.cs`, in the project `src/Generated/Generated.csproj`.
- The report `src/Generated/report.md` (section 9).
- The shim demand list `src/Generated/shim-demand.md`: every RTL, VCL and third-party symbol the
  generated code references, with its declaring unit and the number of references. It is the work
  list for the RTL shim.

The transpiler is a Python 3.11 program under `tools/transpile/` with no dependencies outside the
standard library. One command runs it:

```bash
python tools/transpile/transpile.py --source D:\git\aow\src\delphi3 --rtl C:\Tools\Delphi3\SOURCE --classes D:\git\aow\notes\classes.json --out src\Generated
```

Source files are read as Windows code page 1252. String and character literals keep their byte
values (section 5.6).

## 2. Pipeline

1. Preprocess. Expand `{$I file}` includes textually. Evaluate `{$IFDEF}`, `{$IFNDEF}`, `{$ELSE}`,
   `{$ENDIF}` and `{$IFOPT}` with the symbols `WIN32`, `VER100` and `CPU386` defined, and record
   the other directives (`{$O-}`, `{$I-}`, `{$L}`, `{$R}`) without acting on them. Keep a map from
   every output line to its source file and line.
2. Lex. Case-insensitive identifiers, `$` hex literals, `#` character codes, `'` strings with `''`
   escapes, comments in `{ }`, `(* *)` and `//`.
3. Parse each unit to a syntax tree. The grammar is Delphi 3 Object Pascal (the Delphi 3 Object
   Pascal Language Guide). Section 10 lists the constructs the source uses. Constructs that Delphi 3
   does not have are not parsed: dynamic arrays, `Int64`, `LongWord`, `out` parameters, default
   parameters, overloading, `implements`, `WideString` use, `automated` sections.
4. Resolve. Build the symbol table of every unit in the transpile set and of every interface section
   of the RTL, VCL and import units they use. Resolve every identifier to its declaration, with
   Delphi scoping: the current block, the enclosing routines, `with` expressions innermost first,
   the class and its ancestors, the unit, then the units of the `uses` clauses from last to first.
   Type every expression. An unresolved identifier is an error that stops the run, except inside a
   member the manifest overrides.
5. Normalize. Rewrite the tree with the rules of section 6 (byte-matching idioms) and section 7
   (semantic traps). Each rewrite is recorded in the report with its rule number and location.
6. Emit C#. Section 4 and section 5 define the mapping.
7. Apply overrides. Omit the members and units the manifest names and record them in the report.
8. Write the report and the shim demand list.

Every stage is a module with its own tests. The syntax tree and the typed tree are plain data
classes, so that a test can build one by hand.

## 3. Units, names and visibility

A Delphi unit becomes one C# file with one namespace, `Aow.Units.<Unit>`. The unit's types become
types in that namespace. The unit's routines, variables, typed constants and true constants become
members of `public static partial class <Unit>` in the same namespace. A file that uses another
unit gets `using Aow.Units.<Other>;` and `using static Aow.Units.<Other>.<Other>;`.

Name resolution across units. Delphi resolves an unqualified name through the `uses` list from the
last unit to the first. C# reports an ambiguity instead. The resolver knows which declaration Delphi
picks, so when two visible units declare the same name, the emitter writes the fully qualified name
at that use. A name that Delphi qualifies (`AoWE.AoWHSMap`) stays qualified.

Spelling. Every identifier is emitted with the spelling of its declaration, not of the use. C#
keywords and contextual keywords used as identifiers get the `@` prefix. A member whose name equals
its enclosing type's name gets the suffix `_`. Two identifiers in one scope that differ only in
letter case are an error listed in the report.

Visibility. Delphi `private` and `protected` members are visible to the whole declaring unit, which
the source uses freely (unit routines read private fields of the unit's classes). The mapping is:

| Delphi | C# |
|---|---|
| private | internal |
| protected | protected internal |
| public | public |
| published | public, plus the attribute `[Published]` |

The generated project is one assembly, so `internal` gives the unit-level access that Delphi gives.

Generated file header. Every file starts with a comment that names the source unit, the
transpiler version and the date, and says that the file is generated and never edited by hand.
Every method gets a comment `// <file>.pas:<line>` with the location of its Delphi declaration.
Delphi comments are kept at their statements, including the recovery project's notes in braces.

## 4. Declarations

### 4.1 Simple types

| Delphi | C# | note |
|---|---|---|
| Byte | byte | |
| ShortInt | sbyte | |
| Word | ushort | |
| SmallInt | short | |
| Integer, LongInt | int | |
| Cardinal | uint | Delphi 3 limits it to 0..2147483647. 9 uses, each reviewed |
| Comp | long | 7 uses, all in method pointer comparisons, removed by rule N5 |
| Single | float | |
| Double, Real, Extended | double | Real is the 6-byte legacy type. `../design.md` accepts the precision change |
| Boolean, ByteBool, WordBool, LongBool | bool | a cast from an integer becomes `!= 0` |
| Char | byte | Delphi 3 `Char` is one byte. The alias `AnsiChar` names it in generated code |
| string, AnsiString | AnsiString | the shim's byte string struct, section 5.6 |
| ShortString, string[n] | AnsiString | 4 uses. The length limit is lost and reported |
| PChar | Ptr<byte> | section 5.8 |
| Pointer | object | section 5.8 |
| TClass, class of T | T.Meta | section 4.4 |

A subrange type (`0..7`, `Low..High`) becomes its base integer type. The range is lost. A named
subrange becomes a `using` alias of that type.

### 4.2 Enumerations and sets

An enumeration becomes `public enum Name : byte` with the members numbered from 0 in declaration
order. Delphi 3 stores an enumeration of up to 256 values in one byte (`{$Z1}` is the default and
the source sets nothing else). `Ord(e)` becomes `(int)e`, `TEnum(i)` becomes `(TEnum)i`, `Succ`,
`Pred`, `Inc` and `Dec` become casts around integer arithmetic, and `Low` and `High` become the
first and last member.

A set type becomes `TSet<E>` where `E` is the element enumeration, or `TSet<byte>` for a set over an
integer subrange. `TSet<E>` is a `readonly struct` of the shim with a `ulong` mask. The survey
found no set over a base type with more than 64 values (the largest is `set of 0..31`), so one
`ulong` is enough and a larger base is an error. Set operations map to the struct's operators:

| Delphi | C# |
|---|---|
| `[a, b, c..d]` | `TSet<E>.Of(a, b).Range(c, d)` |
| `x in S` | `S.Contains(x)` |
| `A + B`, `A - B`, `A * B` | `A + B`, `A - B`, `A & B` (operators of the struct) |
| `A = B`, `A <> B` | `A == B`, `A != B` |
| `A <= B`, `A >= B` | `A.IsSubsetOf(B)`, `B.IsSubsetOf(A)` |
| `Include(S, x)`, `Exclude(S, x)` | `S = S.With(x)`, `S = S.Without(x)` |
| `Byte(S)`, `Word(S)`, `Integer(S)` | `(byte)S.Mask` and so on |

### 4.3 Records and arrays

A record becomes a `public struct` with public fields. Assignment copies, as in Delphi. A `const`
record parameter becomes an `in` parameter. A `var` record parameter becomes `ref`.

Layout. Every struct whose fields are all unmanaged gets `[StructLayout(LayoutKind.Explicit)]` with
a `[FieldOffset]` per field, at the offsets Delphi 3 gives the record. The transpiler has a layout
model for Delphi 3 records: `packed` records place fields back to back, and unpacked records align
each field to the smaller of its size and 4 bytes. The model is not trusted on its own. The probe
`records-layout` (section 11) prints `SizeOf` and field offsets for a battery of records compiled
with the oracle compiler, and the transpiler's model must reproduce every one before it is used.
With the layout fixed, `rwData(Rec, SizeOf(Rec), id)` and `Move` on records can use the struct's
bytes directly. `SizeOf(T)` is a compile-time constant from the same model.

Variant records. The transpile set has two: `TEMsg` in Engine and `TXYL` in HSEngine. `TXYL`
overlays four `Shortint` fields with a `Word` and an `Integer`, all unmanaged, and becomes an
explicit-layout struct. `TEMsg` overlays an object reference (`Obj`, `Engine`) with an `Integer`
(`Value`) and with an integer array (`Data`), and a C# struct cannot overlay a reference with an
integer. Its mapping is hand-written in the shim as a struct with separate storage per variant and
properties that alias the fields of the same category, see `rtl-shim.md`. The transpiler treats a
variant record whose variants mix references and values as an error unless an override provides it.

Static arrays. `array[L..H] of T` becomes a generated struct `FixedArray_<T>_<n>` with the
attribute `[InlineArray(n)]` (C# 12), a one-field struct that the runtime lays out as `n` elements
of `T`. Element access `A[i]` becomes `A[i - L]`. Multi-dimensional arrays become nested inline
arrays. Array constants become static fields initialized by a collection expression. `Low(A)`,
`High(A)` and `Length(A)` are constants.

Open array parameters. `array of T` becomes `ReadOnlySpan<T>` for value and `const` parameters and
`Span<T>` for `var` parameters. `High(A)` becomes `A.Length - 1`. A call passes an array or an
open array constructor `[a, b]` as a collection expression. `array of const` becomes
`params TVarRec[]`, where `TVarRec` is the shim's tagged value (section 5.6, `Format`).

### 4.4 Classes and metaclasses

A class becomes `public partial class Name : Parent`. A class with no parent derives from the
shim's `TObject`. Fields keep their names and become fields. Methods become methods. The partial
keyword lets an override file add or replace members (section 8).

Metaclass. Delphi's class references (`class of T`, 15 types in the set) with virtual class methods
(30 declarations, 1,391 class method declarations in all) become a parallel object per class. For
every class `TFoo` the emitter writes a nested class `TFoo.Meta` that derives from the parent's
`Meta` and a static property `TFoo.Class` that holds its one instance:

```csharp
public partial class TFoo : TBar
{
    public new class Meta : TBar.Meta
    {
        public override string ClassName => "TFoo";
        public override TObject NewInstance() => new TFoo();
        public override int ClassID() { ... }      // a virtual class method
        public int Helper() { ... }                // a non-virtual class method
    }
    public static new Meta Class { get; } = new Meta();
    public override TObject.Meta ClassType => Class;
}
```

The rules:

- Every class method, virtual or not, becomes an instance method of `Meta`. A virtual class method
  is `virtual` or `override`. `Self` inside a class method is the `Meta` instance.
- A call `TFoo.Bar(...)` becomes `TFoo.Class.Bar(...)`. A call through a class reference variable
  `C.Bar(...)` becomes `C.Bar(...)`. A call `Bar(...)` from an instance method becomes
  `ClassType.Bar(...)`, which dispatches on the dynamic class as Delphi does.
- A variable of type `TFooClass` becomes `TFoo.Meta`. `TFooClass(X)` is a cast between `Meta`
  types. `C = TFoo` becomes `C == TFoo.Class`. `obj.ClassType = TFoo` becomes
  `obj.ClassType == TFoo.Class`. `C.InheritsFrom(TFoo)` and `obj is TFoo` are the shim's
  `InheritsFrom` and C# `is`. `ClassName`, `ClassParent`, `InstanceSize` (5 uses, reviewed one by
  one) are `Meta` members.
- A class reference stored in a `TList` (the engine's `TEObjectClassList`) is a `Meta` object.

Constructors. A Delphi constructor is a method that runs on a fresh zeroed instance, with virtual
dispatch when declared `virtual` (10 in the set), and it destroys the instance when it raises. The
mapping splits each constructor `Create(args)` into an instance method `Create__init(args)` with the
body, and a factory:

- The body method is `public virtual void Create__init(args)` for a virtual constructor and
  `public void Create__init_TFoo(args)` for a static one, so that a descendant's static constructor
  of the same name hides nothing. `inherited Create(args)` in a body becomes the parent's body
  method. A constructor overridden with `override` overrides the body method.
- The static factory `public static TFoo Create(args)` does `var o = new TFoo(); try {
  o.Create__init(args); } catch { o.Destroy(); throw; } return o;`.
- A virtual constructor also gets a `Meta` factory, `public virtual TFoo Create(args)`, that calls
  `NewInstance()` and the virtual body method. `TEObjectClass(ClassType).Create(nil)` becomes
  `((TEObject.Meta)ClassType).Create(null)` and creates the dynamic class, as Delphi does.
- A static constructor called through a class reference (`AClass.Create`, Delphi allocates the
  referenced class and runs the static body) becomes `(TFoo)C.NewInstance()` followed by the body
  method.
- Delphi 3 has no `AfterConstruction`. Nothing else runs.

The C# constructor of every generated class is parameterless and empty. Field initializers are not
used, because Delphi zeroes the instance and C# does the same.

Destructors. `destructor Destroy; override` becomes `public override void Destroy()`. `inherited
Destroy` becomes `base.Destroy()`. `obj.Free` becomes the shim's null-safe `obj.Free()` extension,
which calls `Destroy()` on a non-null reference. Nothing reclaims memory. The garbage collector does,
once no reference remains. Code that reads an object after `Free` behaves as if the object were
alive, where Delphi gives undefined behavior. The report lists every class whose destructor does
anything other than free owned objects and call `inherited`, for review.

Methods. `virtual`, `override` and `abstract` map one to one. `dynamic` (9 uses, in units outside
the set except zlib) maps to `virtual`. A `message` method (none outside the rewritten units and
the design-time units) is an error. `inherited;` without a name calls the parent's method of the
same name with the same arguments, and emits nothing when no parent implements it, as in Delphi.
A method body that is only inline assembly is an error that an override must resolve. The 25 such
methods in the set are listed in `../design.md` section 3.

Accessor classes. `TENodeAccess = class(TENode);` and `TDrawPanelAccess` exist to reach protected
members through a cast. Rule N4 removes them.

### 4.5 Properties

| Delphi | C# |
|---|---|
| `property P: T read FP write FP` | `public T P { get => FP; set => FP = value; }` |
| `property P: T read GetP write SetP` | `public T P { get => GetP(); set => SetP(value); }` |
| `property P: T read FP` | getter only |
| `property P: T read GetP index 3` | `get => GetP(3)` |
| `property Items[I: Integer]: T read Get write Put; default;` | a C# indexer |
| `property Items[I: Integer]: T read Get write Put;` (not default) | a readonly property `Items` returning the struct `ItemsAccessor` with an indexer that calls `Get` and `Put` |
| `default 0`, `nodefault` | `[DefaultValue(0)]` on the property, for the layout loader |
| `published` | `[Published]` |

Event properties (`property OnX: TNotifyEvent read FOnX write FOnX`) are plain properties of a
delegate type, not C# events, because the source assigns and compares them.

The field and the property both exist in C#, because the source uses both.

### 4.6 Procedural types and events

`TNotifyEvent = procedure(Sender: TObject) of object` becomes `public delegate void
TNotifyEvent(TObject Sender)`. A plain procedural type without `of object` becomes the same kind of
delegate. `@Routine` and `Method` as a value become method groups. `Assigned(F)` becomes `F !=
null`. A call `F(args)` stays a call. `TMethod(F).Code` and `.Data` become `F.Method` and `F.Target`.
A `nil` assignment becomes `null`.

### 4.7 Variables, constants and resource strings

A unit variable becomes a `public static` field of the unit class. A typed constant (583 in the set)
is writable in Delphi 3 and becomes a `public static` field with an initializer. A true constant
becomes `public const` for integer, floating point, character and Boolean values and `public static
readonly` for strings, sets, records and arrays. Constant expressions are folded by the transpiler.

`absolute` (67 declarations, 51 of them in AoW.exe aliasing one global under another name) becomes
a static property that reads and writes the aliased variable. An `absolute` over a variable of a
different type is an error.

A `resourcestring` (60) becomes a `public static readonly AnsiString` field and an entry in a
generated table `ResourceStrings` keyed by `Unit.Name`, so that the localization layer can replace
the value at startup (`localization.md`).

### 4.8 Initialization and finalization

The `initialization` section becomes `public static void Initialize()` and `finalization` becomes
`public static void Finalize()` on the unit class. The emitter writes `Aow.Units.Startup` with
`InitializeAll()` and `FinalizeAll()` that call them in order.

Order. The Delphi compiler writes a unit initialization table into every executable and package,
and the runtime runs it in table order, each unit once, with a counter per unit. A package's table
starts with calls into the packages it requires and continues with its own units. The original
binaries therefore hold the exact order, and the recovery's build tool reads it:
`D:\git\aow\tools\buildgame.py` has `initialization_table`, which reads the table from a `.dpl`
through its package info and from `AoW.exe` from the entry point, and lines 347 to 376 write
`initialization-order.json`. The recovery session reported on 2026-10-24 that `AoWEPACK.dpl` has
173 entries (71 into required packages in the order VCL30, EngineP, ILPACK, GFXEPACK, SoundP,
HSEPack, Network, Localize, aowFXpck, vclx30, then its 92 own units from Pad, Animation, EventLog
to FileSecurity, RandomNode) and `AoW.exe` has 327, whose own entries are resolved against
`notes/signatures`. The transpiler takes that JSON as an input (`--init-order`) and emits
`InitializeAll` in that order, skipping units outside the transpile set. Finalization runs in the
reverse order. The file does not exist yet, see open point 2.

## 5. Statements and expressions

### 5.1 Blocks and control flow

| Delphi | C# |
|---|---|
| `begin ... end` | `{ ... }` |
| `if c then a else b` | `if (c) a else b` |
| `case x of 1, 3: a; 5..9: b; else c end` | `switch (x) { case 1: case 3: a; break; case >= 5 and <= 9: b; break; default: c; break; }` |
| `for I := A to B do S` | `for (int I = A, I__end = B; I <= I__end; I++) S` when `I` is `int`. For a narrower type or an enumeration, the counter is an `int` and the loop body starts with `var I = (T)I__;` |
| `for I := A downto B do S` | the same with `>=` and `I--` |
| `while c do S` | `while (c) S` |
| `repeat S until c` | `do { S } while (!c);` |
| `Exit` | `return;` or `return Result;` |
| `Break`, `Continue` | the same |
| `goto L`, `L:` | the same (2 uses, in AoWTC) |
| `with E do S` | section 5.2 |
| `try S finally F end` | `try { S } finally { F }` |
| `try S except H end` | section 5.5 |
| `raise E` | `throw E;` |
| `raise` | `throw;` |
| `inherited` | section 4.4 |
| `asm ... end` | error, override required |

For loops evaluate both bounds once before the loop, as Delphi does. The loop variable is not
read after the loop by any correct Delphi program, and the emitter does not preserve its value.

Every local variable is declared with `= default` at the top of the method, because Delphi locals
are uninitialized and C# requires definite assignment. A function's `Result` is a local named
`Result`, initialized to `default`, and every exit returns it.

Nested routines (122) become C# local functions. Both capture the enclosing locals by reference.

### 5.2 With statements

`with E do S` (672 uses) binds a scope, not a variable. The resolver resolves every identifier in
`S`, so the emitter knows which names refer to members of `E`. The emitter writes a block with a
local for `E` and qualifies those names:

- `E` of class type: `{ var w__1 = E; ... w__1.Member ... }`.
- `E` a record that is an assignable location (a variable, a field, an array element, a
  dereference): `{ ref var w__1 = ref E; ... }`, so that writes reach the original.
- `E` a record from a function or a property: `{ var w__1 = E; ... }`. Writes go to the copy, as
  in Delphi.
- `with A, B do` nests two blocks.

### 5.3 Operators and arithmetic

| Delphi | C# | note |
|---|---|---|
| `+ - *` on integers | `+ - *` | 32-bit wrap. The project sets `CheckForOverflowUnderflow` false |
| `/` | `(double)a / b` | always floating point in Delphi |
| `div`, `mod` | `/`, `%` on integers | both truncate toward zero and `%` takes the dividend's sign in both languages |
| `shl` | `<<` | both mask the count to 5 bits on 32-bit operands |
| `shr` | `(int)((uint)a >> n)` | Delphi `shr` is a logical shift on signed types. This is the one shift that differs |
| `and or xor not` on integers | `& \| ^ ~` | |
| `and or xor not` on Booleans | `&& \|\| ^ !` | Delphi 3 defaults to `{$B-}`, short circuit, as C# |
| `= <>` | `== !=` | |
| `< > <= >=` | the same | |
| `in` | `TSet.Contains` | section 4.2 |
| `is` | `is` | |
| `as` | `Rtl.As<T>(x)` | returns `null` for `null` and raises `EInvalidCast` otherwise, as Delphi |
| `TFoo(x)` on a class | `(TFoo)x` | section 5.7 |
| `Integer(x)`, `Byte(x)`, `Word(x)` on integers | `(int)x`, `(byte)x`, `(ushort)x` | unchecked truncation |
| `Round(x)` | `Rtl.Round(x)` | rounds half to even, as the x87 default. Returns `int` |
| `Trunc(x)` | `(int)x` | |
| `Int(x)`, `Frac(x)` | `Math.Truncate`, `x - Math.Truncate(x)` | |
| `Abs Sqr Sqrt Sin Cos ArcTan Ln Exp` | `Math.*` | `Sqr(x)` is `x * x` |
| `Random` | `Rtl.Random()`, `Rtl.Random(n)` | the shim's Delphi generator, `rtl-shim.md` |
| `Inc(x)`, `Dec(x)`, `Inc(x, n)` | `x++`, `x--`, `x += n` | on a pointer, section 5.8 |
| `Ord(x)` | `(int)x` | |
| `Chr(x)`, `#65` | `(byte)x`, `(byte)65` | |
| `High(T)`, `Low(T)` | constants | |
| `SizeOf(T)` | a constant from the layout model | |

Integer promotion. Delphi evaluates arithmetic on `Byte`, `ShortInt`, `Word` and `SmallInt`
operands as `Integer` and C# does the same, so no cast is needed inside an expression. A result
assigned to a narrower variable gets an unchecked cast. Mixed signed and unsigned operands follow
Delphi's rule that `Cardinal` with `Integer` is evaluated as `Integer` in Delphi 3, because
`Cardinal` is a 31-bit range there. The resolver types every expression and emits a cast wherever
the C# type of the expression differs from the Delphi type.

### 5.4 Calls and evaluation order

Arguments. `var` parameters become `ref`. `const` parameters of record type become `in`. `const`
parameters of other types are plain parameters. An untyped `var` or `const` parameter (`var Buf`)
is an error unless the callee is a shim routine with a known mapping (`Move`, `FillChar`, `rwData`,
`Stream.Read`, `Stream.Write`), section 5.8.

Evaluation order. Delphi 3 does not evaluate the arguments of a call left to right. The recovery
notes (`D:\git\aow\notes\match.md`, "Register-convention arguments") observed that simple arguments
are loaded right to left and that argument calls happen in an order that depends on the calling
convention. The order matters only when two or more argument expressions have side effects, and
`Random` is the side effect that matters for the oracle (511 calls, some inside argument lists).
The rule: when two or more arguments of one call contain a call to a routine that is not known to
be pure, the emitter evaluates them into temporaries in Delphi's order before the call. Delphi's
order is taken from the probe `evalorder` (section 11), which calls a tracing function in argument
positions, as operands of binary operators, as array indices and on both sides of an assignment,
and prints the sequence under the oracle compiler. The emitter has one table for these orders, and
the probe is the proof of the table. Until the probe runs, the table says right to left for call
arguments and left to right for operator operands.

Pure routines for this rule: the shim's string functions, `Ord`, `Chr`, `Length`, `High`, `Low`,
`SizeOf`, `Abs`, `Round`, `Trunc`, field reads and property getters that the resolver can see
return a field.

### 5.5 Exceptions

The shim's `Exception` class derives from `System.Exception`, and every Delphi exception class
derives from it. Generated files alias `Exception` to the shim class.

| Delphi | C# |
|---|---|
| `try S except on E: EFoo do H1; on E: EBar do H2; else H3; end` | `try { S } catch (EFoo E) { H1 } catch (EBar E) { H2 } catch (System.Exception e__) { H3 }` |
| `try S except H end` | `try { S } catch (System.Exception) { H }` |
| `on E: Exception do H` | `catch (System.Exception e__) { var E = Rtl.Wrap(e__); H }` |
| `raise EFoo.Create(msg)` | `throw EFoo.Create(msg);` |
| `raise;` inside a handler | `throw;` |
| `Abort` | `throw EAbort.Create()` |

`Rtl.Wrap` returns a shim exception as it is, and wraps a .NET runtime exception in the matching
Delphi class: `NullReferenceException` as `EAccessViolation`, `DivideByZeroException` as
`EDivByZero`, `IndexOutOfRangeException` as `ERangeError`, `InvalidCastException` as
`EInvalidCast`, anything else as `Exception`. The catch-all forms catch .NET exceptions too,
because the original code relies on `except` to swallow access violations in a few places.

`Assert(cond, msg)` becomes `Rtl.Assert(cond, msg, file, line)` and raises `EAssertionFailed`.
The shipped game kept assertions on (the recovery matched their line numbers), so the port keeps
them on.

### 5.6 Strings and characters

Delphi 3 strings are byte strings with 1-based indexing, value semantics and copy on write. The
game depends on the bytes: file formats store them, and the bitmap fonts index glyphs by byte. The
mapping is the shim's `AnsiString` struct, `rtl-shim.md` section on strings. The emitter writes:

| Delphi | C# |
|---|---|
| `'text'` | `(AnsiString)"text"` with bytes above 127 as `\xNN` escapes, decoded as code page 1252 by the implicit conversion |
| `#13#10`, `'a'#0` | `AnsiString.FromBytes(13, 10)`, concatenation |
| `S + T` | `S + T` |
| `S = T`, `S < T` and so on | the struct's operators, byte-wise ordinal comparison, shorter prefix first |
| `S[i]` read | `S[i]` (1-based indexer) |
| `S[i] := c` | `S = S.WithChar(i, c)` (copy on write) |
| `Length(S)` | `S.Length` |
| `SetLength(S, n)` | `S = S.Resized(n)` |
| `Copy`, `Pos`, `Delete`, `Insert`, `UpperCase`, `LowerCase`, `Trim`, `CompareText`, `IntToStr`, `StrToInt`, `Format`, `StrPas`, `StrLComp` | `Rtl.*` functions with Delphi semantics |
| `PChar(S)` | `S.AsPtr()`, a `Ptr<byte>` over the string's bytes with a terminating zero |
| `string(P)` from `PChar` | `AnsiString.FromPChar(P)` |
| `Char` comparisons and ranges | byte comparisons |

A `Format` call passes its `array of const` as `params TVarRec[]`. The shim's `Format` follows the
Delphi 3 format string grammar (`rtl-shim.md`).

`Str`, `Val`, `Write` and `WriteLn` (console only, in test hosts) are errors in generated code.

### 5.7 Casts between classes

A Delphi hard cast `TFoo(x)` checks nothing. The emitter writes a checked C# cast `(TFoo)x`. When
the oracle run throws `InvalidCastException` at a generated cast, that cast either hides a type
confusion the port must understand or is a layout-compatible cast the original relied on. The
manifest can mark a cast location `unchecked`, and the emitter then writes
`Unsafe.As<TFoo>(x)`. Every such mark is a reviewed decision with a comment.

`TObject(P)` from a `Pointer` and `Pointer(obj)` to a `Pointer` are reference conversions to and
from `object`. `Integer(obj)` and `Integer(Self)` are section 6, rule N7.

### 5.8 Pointers, memory and untyped parameters

Pointer code is concentrated in the units that are rewritten. The transpile set still has pointer
code in Engine (lists and streams), ImageLib (the format core), HSEngine (`PHNList`, 20 fields),
AoWEPACK (`PIntegerArray`, `PRect`, `PByteArray`) and a few others. The mapping covers the forms the
set uses and reports everything else:

- A pointer to an element type, `^T` or `PT`, becomes the shim struct `Ptr<T>`: an array reference
  and an index. `P^` is `P.Value` (a `ref T` property), `P^.F` is `P.Value.F`, `Inc(P)` and
  `Inc(P, n)` advance the index, `P + n` and `P - n` return new pointers, `P1 - P2` is the index
  difference, `P = nil` is `P.IsNull`, and `P^[i]` on a pointer to an array type is `P[i]`.
- `GetMem(P, n * SizeOf(T))`, `AllocMem` and `ReallocMem(P, n * SizeOf(T))` on a `Ptr<T>` become
  `P = Ptr<T>.Alloc(n)` and `P = P.Realloc(n)`. The size expression must be a constant or a product
  with `SizeOf(T)`, else the construct is an error. `FreeMem(P)` becomes `P = default`.
- `New(P)` and `Dispose(P)` (14 and 10 uses) become `P = Ptr<T>.Alloc(1)` and `P = default`.
- `@V` where `V` is an array element or a variable of element type becomes `Ptr<T>.To(array,
  index)` or a pointer to a one-element box, which the resolver can only build when `V` is an array
  element or a field of a boxed record. `@V` for a `var` argument to a shim routine with a known
  mapping is handled by that mapping. Other `@V` uses are errors.
- `Move(Src, Dst, n * SizeOf(T))` where both are elements of `Ptr<T>` or arrays of `T` becomes
  `Ptr<T>.Copy(src, dst, n)`. `Move` on two records of the same type with `SizeOf` becomes an
  assignment. `FillChar(X, SizeOf(X), 0)` becomes `X = default` for a record or `Array.Fill`
  for an array. Other `Move` and `FillChar` forms are errors.
- `TList.List^[I]` (224 uses) becomes `List.List[I]`, the shim's raw array of the list.
- `PChar` becomes `Ptr<byte>` as above.
- `Pointer` (untyped) becomes `object`. A cast from `object` to a `Ptr<T>` is an error.

The `rwData(var Buf; Size; id)` call of the storage stream takes `ref T` overloads for unmanaged
`T` and a `Ptr<byte>` overload. The emitter picks the overload from the argument's type. The 146
`rwData` calls in the set are listed in the report with the overload chosen.

## 6. Normalization of byte-matching idioms

The recovered source was written to reproduce the original machine code byte for byte. Where the
original code reached a field through a pointer in a register, or where a recovered declaration had
no name for a field, the recovery wrote constructs that express the machine code rather than the
program. They concentrate in AoWEPACK. The transpiler folds them into plain code. Every fold is
recorded in the report with its rule number, so that the folds can be compared against a later
cleanup of the recovery source, if the user asks the recovery session for one.

| rule | pattern (count in the set) | result |
|---|---|---|
| N1 | A typed constant or variable of pointer type initialized once with the address of a global, `WorldMap: PAoWHSMap = @AoWE.AoWHSMap` (727 dereferences of such views) | every `View^` becomes the global. A use of `View` as a value is an error |
| N2 | A pointer variable assigned the address of one global in one place and dereferenced elsewhere, `RandomSeed: ^Integer` pointing at `RandSeed` | the same fold. The assignment is removed |
| N3 | A layout view: `TRec(Pointer(Obj)^).Field`, `PRec(Pointer(Obj))^.Field`, `PRec(Obj)^.Field` where `Obj` is a class instance and `TRec` is a record of padding and named fields, or `TRawObject(Pointer(Obj)^).Fields[n div 4]` (168 `Pointer(x)^` casts, 115 `TRawObject` uses, 108 view record types) | compute the byte offset of `Field` in `TRec` with the layout model, compute the instance layout of the class of `Obj` from its recovered declaration (4 bytes for the VMT pointer, then the fields of each ancestor in order with the record alignment rule), find the field at that offset, and emit `Obj.<FieldName>` with the field made `internal`. The recovered declarations carry offset comments (`{ +$18 FMapContainer }`) and `classes.json` lists offsets for a subset of fields. Both are cross-checks of the computed layout, and a mismatch is an error. The recovery session confirmed on 2026-10-24 that 147 of 162 `Fields[]` views and 33 of 62 record views resolve to named fields, 17 of them private fields of another unit, 6 need a cast of a `THexagonSprite` variable to `TArmyHS`, and the rest have bases the recovery could not type mechanically, which the resolver types from context or reports |
| N4 | An accessor class `TXAccess = class(TX);` used only in casts | the casts become plain references to the `TX` object, and the members reached become `internal`. An empty class used anywhere else than a cast stays a class |
| N5 | `Comp(FList^[I]) <> Comp(Item)` and `TMethod(X).Code = TMethod(Y).Code` on method pointers | delegate equality, `X == Y` |
| N6 | `Integer(Pointer(@FX)^)` and other reinterpretations of a field group as one integer (11 uses) | error, override required. The report lists them |
| N7 | `Integer(Self)`, `Integer(Owner)`, `Integer(obj)` as a value | `obj.InstanceId`, a 32-bit id the shim assigns to every object at construction from a counter. In `Msg.Param[i] := Integer(obj)` and `TFoo(Msg.Param[i])` the parameter type is the shim's `IntOrRef` and the conversions use its `Ref` and `Value` members. Address arithmetic such as `(Integer(Self) shr 8) and 3` (shore and animation variants) uses `InstanceId` and is deterministic in the port where the original depended on the heap |
| N8 | `with PRec(P)^ do` and `with TRec(Pointer(P)^) do` | the `with` is resolved after N3 |
| N9 | `Pointer(obj)` passed where a class reference is expected, `TFoo(Pointer(x))` | the reference conversion of section 5.7 |
| N10 | A unit-qualified name `Unit.Name` | kept |

The recovery session established on 2026-10-24 which folds are safe at the source level. The raw
layout views (N3) and accessor classes (N4) exist because a field had no recovered name, the wrong
type or private visibility in another unit, and replacing them with plain field access compiles
to identical code. The pointer views to globals (N1, N2) are different: with `{$G+}` Delphi reads
another unit's variable through an import cell before every use, and the typed pointer constant
keeps the cell in a register, so the view reproduces the original code and removing it costs
matches. The same holds for the shape casts `TObj(Pointer(Sender))` and `T(Pointer(@g)^)`. So
N1, N2 and N9 stay transpiler rules for good, and N3 and N4 are candidates for a source cleanup
that the report's normalization section lists for the recovery session.

## 7. Semantic traps

Each entry names the rule and the probe (section 11) that proves it.

1. `shr` is a logical shift on `Integer`. Probe `integers`.
2. `Random` and `RandSeed` reproduce the Delphi 3 generator bit for bit: `RandSeed := RandSeed *
   $08088405 + 1`, `Random(n) = (UInt64(Cardinal(RandSeed)) * n) shr 32`, and the floating point
   `Random` is `Cardinal(RandSeed) * 2^-32`. The algorithm is in `rtl-shim.md`. Probe `random`.
3. `Round` rounds half to even. `Trunc` truncates toward zero. Probe `floats`.
4. Integer arithmetic wraps at 32 bits, `div` and `mod` truncate toward zero. Probe `integers`.
5. Strings compare byte-wise and a shorter prefix sorts first. `CompareText` and `UpperCase`
   handle only the ASCII letters. `AnsiCompareText` (4 uses) uses the Windows locale, which the
   shim replaces with the invariant comparison of code page 1252 and the report flags. Probe
   `strings`.
6. `S[i] := c` copies on write, so a second variable that referred to the same string keeps the old
   value. Probe `strings`.
7. For loop bounds are evaluated once. A loop over a `Byte` variable to 255 terminates. Probe
   `loops`.
8. Argument evaluation order, section 5.4. Probe `evalorder`.
9. Delphi `Boolean` from an integer cast is `value != 0`, and `Ord(True)` is 1. Probe `integers`.
10. A hard cast between classes checks nothing in Delphi. Section 5.7.
11. Enumerations are one byte, and a set is a bit per ordinal. `Byte(Set)` reads the mask. Probe
    `sets`.
12. Record layout, section 4.3. Probe `records-layout`.
13. Object addresses as values, rule N7. The port is deterministic where the original was not.
14. Floating point precision: the port uses `double` where Delphi used x87 extended intermediates.
    Accepted in `../design.md`.
15. `Assert` is active. Section 5.5.
16. Unit initialization order, section 4.8. Probe `init-order` with three units.
17. `TList` growth, `TStringList` sorting and duplicate handling, and `IndexOf` semantics follow
    the Delphi 3 `CLASSES.PAS` source. `rtl-shim.md`. Probe `lists`.
18. Exceptions raised in a constructor destroy the instance. Probe `classes`.
19. `inherited;` with no parent implementation is a no-op. Probe `classes`.
20. Virtual class methods called from an instance method dispatch on the dynamic class. Probe
    `classes`.

## 8. Overrides

`src/Overrides/overrides.toml` lists what the transpiler must not emit:

```toml
[[skip]]
member = "Engine.TBitList.Get"        # Unit.Class.Member, or Unit.Routine, or Unit.Class
reason = "inline assembly"
file = "src/Overrides/Engine/TBitList.cs"
```

The generated class is `partial`, so the override file declares `partial class TBitList` in the
same namespace and supplies the member. For a whole class the generated file omits the class and
the override supplies it with the same public shape. For a whole unit the entry is `Unit.*`. A
skipped member that the generated code calls must exist in the override, or the build fails, which
is the intended check. The report lists every skip and whether its file exists.

Overrides never change generated files. A fix to a mapping goes into the transpiler.

## 9. Report

`src/Generated/report.md` has these sections, each with counts and locations (`file:line`):

1. Units processed, with lines in and lines out.
2. Errors: constructs the transpiler does not map, by rule, with the Delphi source line. Each error
   leaves a `/* TODO(T-<rule>) <reason> */ throw new NotImplementedException();` in the output
   where a statement was expected, so that the project still compiles. The count of TODOs is the
   first number of the report.
3. Normalizations applied, by rule of section 6, with before and after text.
4. Overrides applied.
5. Casts marked unchecked.
6. `rwData` calls and the overload chosen.
7. Destructors with logic beyond freeing, for review.
8. Identifiers that collide across units and were qualified.
9. The shim demand list, written to its own file as well.

## 10. Survey of the source

Counts over the transpile set on 2026-10-24, to size the mapping work. Counts are of source lines
that contain the construct, so they are approximate.

| construct | count | mapping |
|---|---|---|
| classes | 1,409 declarations | 4.4 |
| `override` | 5,267 | 4.4 |
| `virtual` | 1,668 | 4.4 |
| `abstract` | 450 | 4.4 |
| `inherited` | 2,766 | 4.4 |
| class methods | 1,391 declarations, 30 virtual | 4.4, metaclass |
| `class of` types | 15 | 4.4 |
| virtual constructors | 10 | 4.4 |
| properties | 1,300, 46 indexed, 16 default array | 4.5 |
| `published` | 1,114 | 3 |
| method pointer types | 89 | 4.6 |
| enumerations | about 900 | 4.2 |
| set types | 140, largest base 0..31 | 4.2 |
| records | 173, 56 packed, 2 variant in the set | 4.3 |
| static array types | 40 | 4.3 |
| open array parameters | 33, 7 `array of const` | 4.3 |
| pointer types | 169 | 5.8 |
| `with` | 672 | 5.2 |
| `case` | 640 | 5.1 |
| `try` | 392, 103 `finally`, 286 `except`, 10 `on E:` | 5.5 |
| `raise` | 220 | 5.5 |
| `goto` | 2 | 5.1 |
| nested routines | 122 | 5.1 |
| typed constants | 583 | 4.7 |
| `absolute` | 67 | 4.7 |
| `resourcestring` | 60 | 4.7 |
| `initialization` sections | 138 | 4.8 |
| `shr` | 527 | 5.3 |
| `div`, `mod` | 669, 93 | 5.3 |
| `Random` | 511 | 7 |
| `Round` | 284 | 5.3 |
| `Format` | 279 | 5.6 |
| `Inc`, `Dec` | 1,543, 730 | 5.3 |
| `GetMem`, `FreeMem`, `ReallocMem` | 123 | 5.8 |
| `Move`, `FillChar` | 132, 103 | 5.8 |
| `TList.List^` | 224 | 5.8 |
| `rwData` | 146 | 5.8 |
| inline assembly | 25 routines in the set | 4.4, override |
| byte-match idioms | about 1,000 | 6 |
| `Variant`, `WideString`, interfaces, `message` methods, `dynamic`, `automated`, `object` types | none in the set, except interfaces in rewritten units | rejected |

RTL and VCL units the set imports most: Classes, SysUtils, Windows, Messages, Graphics, Controls,
Forms, Dialogs, StdCtrls, ExtCtrls, Menus, Grids, ComCtrls, Registry. The shim demand list
replaces this guess with the exact symbol list on the first run. The most used routines are
`IntToStr` (586), `Format` (311), `Delete` (289), `Copy` (194), `Pos` (147), `Insert` (70),
`CompareText` (47), `UpperCase` (46), and the classes `TList` (329), `TStream` (262),
`TStringList` (224), `TComponent` (200), `TStrings` (175), `TMemoryStream` (101),
`TFileStream` (44). The `TComponent` members used are `Owner` (820), `Name` (459), `Tag` (226),
`Assign` (136), `Loaded` (64), `ComponentState` (33), `Notification` (3), `GetChildren` (2),
`Components` (1).

## 11. Testing

Three layers, cheapest first.

1. Unit tests of the transpiler in `tools/transpile/tests/`: the lexer, the parser on every
   construct of section 10, the resolver on scoping cases (`with`, unit order, class scope), the
   layout model against the `records-layout` probe output, and golden tests that map a Delphi
   snippet to the expected C# text.
2. Probes in `tests/probes/<name>.pas`. A probe is a Delphi 3 console program that prints lines.
   `tests/probes/run.py` compiles each probe with `C:\Tools\Delphi3\BIN\DCC32.EXE -CC`, runs it
   and stores the output in `tests/probes/expected/<name>.txt` (committed), then transpiles the
   probe with the transpiler, builds it against the shim and runs it, and compares the outputs
   line by line. The probe set covers section 7: `integers`, `floats`, `random`, `strings`,
   `format`, `sets`, `enums`, `records-layout`, `arrays`, `with`, `case`, `loops`, `exceptions`,
   `classes`, `properties`, `evalorder`, `lists`, `streams`, `typed-constants`, `init-order`.
   A probe fails when a line differs. A probe is the proof of a rule, so a rule without a probe is
   a rule under suspicion.
3. The transpile set itself: the transpiler runs on all units and the generated project builds.

Done measures, in order:

1. T1, parse: every unit of the transpile set and every needed RTL, VCL and import interface
   section parses with no error. Measured by the transpiler's exit code and the report's unit
   count.
2. T2, resolve: the report lists zero unresolved identifiers outside overridden members.
3. T3, emit for Engine: `Engine`, `EList`, `FMemStr`, `PlugInM` and `inc/HashTab` emit, and the
   generated project builds with zero errors against the shim. The TODO count is reported.
4. T4, probes: every probe in `tests/probes` passes.
5. T5, emit for the set: the whole transpile set builds with zero errors. The TODO count is the
   measure of what the overrides still owe, and it goes down to zero as overrides land.

The persistence round trip and the headless oracle are the done measures of `persistence.md` and
`testing.md`, not of this document.

## 12. Open points of this specification

1. Whether the recovery source gets a cleanup pass for the raw layout views and accessor classes
   (rules N3 and N4), which the recovery session confirmed is match-neutral. The pointer views
   (N1, N2) stay in the source either way. Decision with the user.
2. `initialization-order.json` from `D:\git\aow\tools\buildgame.py` has to be produced once and
   kept in the `aow` repository or copied here. Request to the recovery session, through the user.
3. The argument evaluation order table, to be filled from the `evalorder` probe.
4. The Delphi 3 record layout rule, to be confirmed by the `records-layout` probe before the
   layout model is used for `rwData` and `Move`.
