# OpenAoW analyzers

Two Roslyn analyzers with code fixes check rules of AGENTS.md and of the design that the
.editorconfig style rules do not cover. `Aow.Analyzers.csproj` builds them. `Tests/` holds their
tests, which run with the product's `dotnet test OpenAow.slnx`.

| Id | Rule | Severity | Fix |
| --- | --- | --- | --- |
| AOW0001 | Named arguments (issue 325) | warning | names the arguments of the call |
| AOW0002 | Integer-to-enum conversions only in read and write code (issues 333 and 357) | warning | a constant cast becomes the enum member of that value |

## Where the analyzers live and how builds load them

The analyzers are part of the product build, so they live in the product repository, beside
`src/` and outside it. The product's `Directory.Build.props` imports `Aow.Analyzers.props`, which
gives every product project a project reference to `Aow.Analyzers.csproj` as an analyzer. A clone
of the product alone builds and enforces the rules, with no path into the private repository. The
folder stays outside `src/` because the private rewrite tools load every project under `src/` as
game code.

A project of the private repository loads the same analyzers with
`<Import Project="$(ProductRoot)/analyzers/Aow.Analyzers.props" />`, the direction the private
repository already depends on the product. A private generator that writes calls without a
semantic model references `Aow.Analyzers.csproj` and asks `NamedArgumentRules.NeedsNames` which
arguments to name (the forms generator does this).

The project compiles against the Roslyn assemblies of the SDK that `global.json` pins (the
compiler's `bincore` and the workspace layer of its `dotnet-format`), so the analyzers bind to the
compiler that runs them and need no package feed. The command line build and `dotnet format`
enforce them. An IDE that runs analyzers on .NET Framework skips them with a load warning, since
they target the product's .NET.

A project sets `UseAowAnalyzers` to `false` to leave the import out. The analyzer project does,
since it cannot load its own output, and so do its tests, which reference the analyzers as an
assembly and as an analyzer in one reference.

## AOW0001: named arguments

AGENTS.md "How new code is written": a call with one argument needs no name. From the second
argument on, arguments are named, except where a strong positional convention carries the
meaning. A boolean argument is always named unless it is the first argument.

`NamedArgumentRules.Classify` gives each argument of a call one verdict, in this order:

1. The first argument of the list needs no name. A call with one argument has nothing to check.
2. An argument that carries a name passes.
3. An element of a `params` list in expanded form cannot take a name and passes.
4. A call inside an expression tree (a lambda converted to `Expression<T>`) passes, since an
   expression tree takes no named argument.
5. An argument that is a name equal to the parameter name passes: a local, a parameter, a field
   (`_count` and `s_count` for `count`), the last name of a member access (`other.Width` for
   `width`, `a?.Width` too), an `out var value` declaration, each with `ref`, `in`, `out`, `!` and
   parentheses looked through. The comparison ignores case. A boolean argument passes this way
   too: `Frob(x, reportProgress)` reads as `Frob(x, reportProgress: reportProgress)`.
6. A lambda or an anonymous method in the last place passes.
7. A boolean argument (a `bool` or `bool?` parameter) needs a name. Rules 8 and 9 do not apply to
   it.
8. A convention group passes: a run of consecutive parameters whose names are one of the groups
   below, in that order, with every argument of the run present, positional and in order. A name
   matches a word of the group when the name is the word after a prefix and a suffix common to the
   run are taken off (`sourceX, sourceY`, `x1, y1`), when it starts with the word followed by an
   upper case letter (`expectedSubstring`, `actualString`), or when it ends with the word after a
   lower case letter (`mapLevel`, `hexLevel`). Case is ignored. A longer group is tried first.
9. A parameter of a numbered series passes: a name that ends in digits, where another parameter
   of the method has the same stem with other digits (`val1, val2`, `path1, path2`, `arg1, arg2`).
10. Every other argument needs a name.

The convention groups, from the parameter names of the code base's calls (the census of issue 325
counted the arguments each group keeps positional on 2026-11-19):

| Group | Arguments | Where |
| --- | --- | --- |
| `expected, actual` | 3524 | xUnit `Assert` and the test helpers |
| `x, y` | 3070 | screen, pixel and hex positions |
| `x, y, level` | 951 | map positions (`MapCoordinate`, `GetField`) |
| `left, top, right, bottom` | 668 | `Rect` |
| `left, top, width, height` | 442 | bounds |
| `width, height` | 316 | sizes |
| `a, b` | 259 | symmetric comparisons (`string.Equals`) |
| `key, value` | 209 | dictionaries |
| `low, high` and `lo, hi` | 209 | ranges |
| `name, value` | 148 | members of documents and settings |
| `year, month, day, hour, minute, second`, `year, month, day`, `hour, minute, second` | 127 | dates and times |
| `x, y, z`, `x, y, width, height`, `left, top`, `right, bottom` | 34 | positions and bounds |
| `min, max`, `minimum, maximum`, `row, column`, `column, row`, `left, right`, `old, new`, `sender, e` | under 20 each | ranges, cells, pairs, replacements, events |

Edge cases, decided:

- `params`: the expanded elements take no name. An array passed in normal form is an ordinary
  argument.
- Lambdas: only the last argument is exempt. A lambda before other arguments is named.
- Operators have no argument list. Indexers and element access (`grid[x, y]`, `{ [key] = value }`)
  are exempt: their arguments are subscripts.
- Attribute arguments are exempt. An attribute is a declaration, its positional arguments follow
  the attribute's constructor, and `InlineData` and its kind are `params` lists.
- .NET and third-party methods follow the rule like product methods. Their parameter names are
  part of their documented API, and the numbered-series and convention rules keep the calls whose
  names say nothing (`Math.Max(val1, val2)`, `Path.Combine(path1, path2)`) positional.
- `nameof` is an operator, not a call, and is not checked.
- Object and collection initializers: member initializers carry names by syntax, and the implicit
  `Add` calls of a collection initializer take no names.
- `base(...)`, `this(...)` and a primary constructor's base call (`class A(int x) : B(x, 1)`) are
  calls and follow the rule, as do `new T(...)`, `new(...)`, local functions and delegate
  invocations.
- Deconstruction (`var (a, b) = value`) and tuples have no argument list to check.
- A call through `dynamic` and a function pointer call are not checked. An argument that binds to
  no parameter (a compile error) is not reported.
- An extension method called on its receiver counts from its argument list: the receiver is not
  an argument.
- A parameter name that is a keyword is written with `@` (`@event: 2`).

The fix inserts `name: ` before each argument the rule asks to name, after the argument's leading
trivia, so a line break before the argument stays where it is. It names every reported argument of
a call at once. It never reorders arguments, so a positional argument after a named one stays in
its place, which C# 7.2 and later accept.

## AOW0002: integer-to-enum conversions

An integer becomes an enum value only in read and write code or in a member the allow list
names (issues 333 and 357). The analyzer reports these conversions elsewhere:

- a cast of an integer (`(TerrainType)index`), and of a constant (`(TerrainType)6`), apart from
  the implicit conversion of constant zero,
- a cast of one enum type to another, which goes through the integer,
- an unboxing cast of a boxed integer (`(TerrainType)(object)index`),
- a pointer cast to a pointer to an enum,
- `Enum.ToObject`, and `Enum.Parse` and `Enum.TryParse`, which accept a number as well as a name,
- a reinterpretation through `Unsafe` or `MemoryMarshal`, where the result holds an enum that no
  parameter holds (`Unsafe.As<byte, TerrainType>`, `MemoryMarshal.Cast<byte, TerrainType>`).

Read and write code is, walking out from the conversion through local functions, lambdas, members,
types and namespaces:

1. a member of a type that carries `[StoredLayout]` or `[StoredRecord]` (the transient I/O images
   of design.md section 3),
2. a member that takes a persister (`IPersister` or a type that implements it) and a member of a
   persister type,
3. a member that takes a `System.IO.Stream`, a `BinaryReader` or a `BinaryWriter` (or a derived
   type),
4. a namespace that the `aow_read_write_namespaces` option of .editorconfig names, with every
   namespace inside it. The product names `Aow.Formats` (the codecs of the original and own
   formats) and `Aow.Platform` (the SDL host, which reads native records).

Tests are opted out in .editorconfig: a test builds enum values from numbers on purpose.

The allow list is `EnumCastAllowList.txt` beside the analyzer project. The product build passes
it to the analyzer as an additional file (see `Aow.Analyzers.props`). Each line names one member
that keeps a conversion, as a documentation id
(`M:Aow.Game.Units.Army.GetPartyMoves(System.Int32)~Aow.Game.World.Movement.MoveTypes`),
with blank lines and `#` comments left out. The census of issue 357 lists the members the file
names today: the game-logic conversions the enum work has not removed yet, one member per line.
A new kept conversion adds its member to the file. A removed conversion deletes its line with
the conversion.

The rule is at warning, so a new conversion outside read and write code fails the build until
its member enters the allow list or the conversion goes. The fix replaces a cast of a constant
with the one member of that value. A value that no member or several members have keeps its
cast.
