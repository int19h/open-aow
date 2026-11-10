# How OpenAoW is built

This document describes who does what, where the truth lives, and how a task goes from a
specification to verified code. Read `docs/design.md` first for the design itself.

## 1. Roles

Four parties work on the port:

- The design session. A Claude session in `D:\git\open-aow` owns the architecture, the design
  document, the specifications in `docs/specs/` and the task briefs. In the design stage it runs on
  the strongest model and writes everything down. In the implementation stage the same session,
  with its context kept, runs on a cheaper model and mostly coordinates: it briefs the implementer,
  reads its reports, verifies the result against the oracle and commits.
- The implementer. GPT Sol-6.1 at high reasoning effort, driven through the `codex` command line
  in non-interactive, resumable sessions. One session per task. It writes the code and the tests
  for one brief and reports back. It does not commit and does not change branches.
- The researcher. GPT-6-Astra at high reasoning effort, through the same command line. It does the
  grunt work the design session must not spend its own capacity on: reading the recovered source
  for patterns and counts, building catalogues under `data/` of the private repository with their
  scripts under `tools/survey/`, preparing reports, and giving a second opinion on a design point. Its design
  advice is advice: the design session reviews it and decides. The Codex quota is much larger than
  the design session's, so anything that does not need the design session's reasoning goes here.
- The recovery session. The "decompile" session of the `aow` project in `C:\git\int19h\aow`
  refines the recovered Delphi source. `D:\git\aow` is the user's working copy, which the user
  updates (the upstream history was rewritten on 2026-10-24, so the copy follows upstream by reset,
  not merge). Since the evening of 2026-10-25 the two sessions run in the same subscription and
  message each other directly (the recovery session is named `decompile`; this one is `open-aow`).
  Earlier that day they could not, and requests went through files in `D:\msg` relayed by the user
  (`open-aow-requests-<date>.md`, `aow-replies-<date>.md`); those exchanges are kept under
  `correspondence/` in the private repository. Requests stay front-loaded and ordered by priority,
  each item standing alone, and a copy of every request and reply still goes to `correspondence/`.

The user decides scope, settles open questions and approves anything irreversible or outward facing.

## 2. Where the truth lives

- `docs/design.md`: the decisions. When a decision changes, this file changes first.
- `docs/specs/`: one specification per subsystem, with the algorithms, formats and mappings an
  implementer needs, and a done measure. `docs/specs/README.md` lists them and their state.
- `docs/milestones.md`: the order in which the specifications are implemented, what each step
  depends on and which done measure closes it. Briefs are cut from its steps.
- `docs/briefs/`: one brief per implementation task, written from a specification. A brief is
  self-contained: the implementer gets nothing else.
- `D:\git\aow`: the recovered Delphi source (`src/delphi3/`), the recovery documents (`doc/`), its
  data files (`data/`, including the four files generated for the port at commit 8fffb88) and the
  tools (`tools/`). The port reads from it and never writes to it.
- `D:\Build\oracle`: the seeded inputs and the original's per-day saves the oracle comparison uses
  (`specs/testing.md` 3.1). Derived from game data, so never committed.
- `D:\git\open-aow-private`: the second repository of the port (decided 2026-10-26, layout in
  `docs/design.md` section 8). The product repository holds only what ships and the documents
  that specify it. Everything built on the way goes here: the transpiler, the catalogues the port
  built from the recovered source (`data/`, with a Markdown summary next to each file) and the
  scripts that produced them (`tools/survey/`, so that a catalogue can be rebuilt after the
  recovery changes the source), the probes and the oracle tooling, and `correspondence/`, every
  request to and reply from the recovery project, dated, so that the provenance of a fact in a
  specification can be traced. The product repository has `origin` at
  `github.com/int19h/open-aow` (default branch `main`) and takes changes through pull requests
  (section 4). The private repository has no remote and is committed directly. A change that
  touches both gets one commit in each with the same first line.

Surveys of the recovered source or the binaries are the port's own work, done by a researcher run
(section 1) from this session. The recovery session is asked only what its coordinator knows
from running the recovery: decisions, tooling and data locations, why something was done, what is
missing. It answers investigative asks by spawning its own subagents, which only adds a relay hop
and spends its quota.
- The oracle: the game rebuilt from the recovered source with Delphi 3.02 at `C:\Tools\Delphi3`,
  and the original GOG install at `D:\Games\GOG\Age of Wonders`. Behavior questions are answered by
  running it, not by reasoning about it.

## 3. The Codex command line

Facts as checked on 2026-10-24 with codex-cli 0.163.0. `~/.codex/config.toml` sets the model to
`gpt-6.1-sol` at medium effort, so every run passes the effort explicitly.

Start a task:

```bash
codex exec --json -C D:\git\open-aow -s workspace-write -m gpt-6.1-sol -c model_reasoning_effort='"high"' --add-dir D:\git\aow -o <report file> "<the brief text>" < /dev/null
```

The first JSONL event, `thread.started`, carries the `thread_id`. Record it in the brief file.
The `-o` file receives the implementer's final message. Redirect stdin from the null device, or the
command waits for piped input. `--add-dir D:\git\aow` gives the implementer read access to the
recovered source. `workspace-write` lets it edit the repository and run builds and tests, and
makes it ask before anything outside.

Continue a task with context kept. `resume` has no `-m` option and falls back to the config's
model and effort, so both are passed as `-c` overrides, for the implementer and the researcher
alike (checked on 2026-10-25: a resume without them ran on the default model):

```bash
codex exec resume --json -c 'model="gpt-6.1-sol"' -c 'model_reasoning_effort="high"' <thread_id> "<follow-up>" < /dev/null
```

Start a research run (the prompt is a file, read from standard input with `-`). A research run
writes catalogues and scripts, so it starts in the private repository:

```bash
codex exec --json -C D:\git\open-aow-private -s workspace-write -m gpt-6-astra -c model_reasoning_effort='"high"' --add-dir D:\git\aow -o <report file> - < <prompt file>
```

The prompt names the files the run is allowed to write (a script under `tools/survey/`, its output
under `data/<topic>/`) and nothing else, so that a commit from this session never sweeps in a
half-written file.

The sandbox needs ownership. The sandbox setup adds a write grant for `CodexSandboxUsers` to every
writable root and a deny entry to its `.git`, which needs the right to change the ACL. A directory
or a `.git` owned by the Administrators group (one made from an elevated shell) fails with
`helper_unknown_error: setup refresh had errors`, and the cause is in
`~/.codex/.sandbox/sandbox.<date>.log`. This happened to `D:\git\open-aow-private` on 2026-10-26
and was fixed by giving the directory to the user and replacing its `.git` with a copy the user
owns. Every run keeps the sandbox. A task that must run without it (`-s danger-full-access`, or
`-c 'sandbox_mode="danger-full-access"'` on `resume`) states the directory rule in its prompt, and
a task that rewrites history makes a backup first (a `git bundle` of every ref).

The sandbox has no network, so a run cannot restore NuGet packages. The design session restores
before the run (`dotnet restore` in the solution's directory, outside the sandbox) and the brief
says that no new package can be added, or the run builds against the local package cache and the
design session restores and tests after it.

`codex exec resume` takes no `--add-dir`, and a resumed turn keeps only the working directory
writable: pass the extra roots again as `-c "sandbox_workspace_write.writable_roots=['D:\git\open-aow']"`
(seen on 2026-10-26, when a resumed emitter run could not write `src/Generated`). A turn that fails
with `Selected model is at capacity` is resumed later, in the same thread.

A task in the private repository that edits product source adds `--add-dir D:\git\open-aow`.
The transpiler no longer writes into the product. Its `emit --out <dir>` writes reference output
under the private repository for porting upstream changes by hand. The gate does not build or
test the transpiler as its own layer. The forms model still builds `Transpiler.slnx`, because
`Forms.Model` references that assembly and reads `tools/transpile/reference/overrides.toml`.

Fork a task to try an alternative without losing the first line of work:

```bash
codex exec fork <thread_id> "<new direction>" < /dev/null
```

Run long tasks in the background and read the report file when they finish. A task that needs
the Delphi compiler or the game files says so in its brief with the exact paths.

### 3.1 What the recovery project learned running Codex

The recovery project ran its byte-matching workers as Codex sessions through scripts now copied to
`D:\Build\agents\scripts\` (its own git repository, with a README) with briefs in
`D:\Build\agents\briefs\`. What carries over to the port:

- The Codex CLI is not on `PATH`, and its directory changes with every desktop app update.
  `findcodex.py` there reads `CODEX_CLI_PATH` from `~/.codex/config.toml`.
- Workers ran with `--approve-for-me --worktree --add-dir <scratch>` and never committed: the
  sandbox of such a session cannot write the repository's `.git`. The coordinator merged each
  worktree's diff onto main with a three-way apply, built, measured, and committed only when no
  measure dropped. The port's rule that the implementer never commits comes from the same place.
- Every session needs a waiter. The end of a session (`turn.completed` or `turn.failed` in the
  JSONL events) goes unnoticed otherwise. `codex exec resume` works in the current directory, so a
  session is resumed from its own worktree.
- Briefs must say that lists of examples are not exhaustive, or the worker handles the examples
  only.
- Models: GPT Sol-6.1 at xhigh effort for most workers, GPT Astra-6 at high for a rewrite round,
  Sol-6.1 at medium for small mechanical tasks. The port starts at high and raises to xhigh for a
  task whose first attempt falls short.
- Never commit on main while a merge queue runs: the merge stages the worker's changes in the
  index.

## 4. From specification to code

1. The design session writes or updates the specification in `docs/specs/`. A specification has
   the inputs, the outputs, the algorithm or mapping in enough detail that two implementers write
   the same behavior, the edge cases, and the done measure.
2. The design session writes a brief in `docs/briefs/<task>.md`. The brief names the
   specification, the files to create or change, the files to read first, what is out of scope,
   how to build and test, and the done measure copied from the specification. It repeats the
   standing rules: no commits, no branch changes, CRLF or LF as the repository sets, report what
   was not done.
3. The implementer runs. Its report goes to the file named in `-o`. The design session reads it,
   checks the diff, runs the tests and the oracle comparison where the task touches behavior.
4. Findings go back to the same Codex session by `resume`, so the implementer keeps its context.
   Three rounds without progress means the brief or the specification is wrong. Fix that, not the
   code.
5. When the done measure holds, the design session commits on the brief's branch
   (`brief/<task>`, cut from `main`), pushes it and opens a pull request whose description
   carries the done measure's command output. The design session merges when the measure holds
   (the user delegated merges on 2026-10-26).
   Commit messages end with the attribution line for the model in use. Changes in the private
   repository go straight to its `master`.
6. The repositories carry an `AGENTS.md` with the standing rules of section 5, which Codex reads
   on every run, so a brief repeats only the rules it tightens.

## 5. Standing rules for the implementer

Every brief carries these:

- Do not commit, do not switch branches, do not push.
- Product code goes to `D:\git\open-aow`, everything else to `D:\git\open-aow-private`
  (`docs/design.md` section 8). The brief names the repository the task works in.
- The game library under `src/Game/` and `src/Engine/` is source and is edited by hand. Every edit keeps
  `python tools/check.py` passing from the private repository. The hand-over took place on
  2026-10-27. The private transpiler emits reference diffs outside the product repository.
  The gate does not build or test the transpiler as its own layer.
- Do not change `D:\git\aow`.
- Tests run with one command named in the brief, and the brief's done measure is checked by a
  command, not by reading.
- Report what was done, what was not done and why, and anything found that the brief did not
  anticipate. A report that claims completion without the done measure's output is incomplete.

## 6. Verification ladder

In order of cost. A task uses the cheapest rung that proves its claim.

1. The .NET build compiles with no warnings in the changed projects.
2. Unit tests pass. Recordings of Delphi library behavior ship in `src/Rtl.Tests/Probes` and the
   tests read them there. The gate's probes layer runs `blitters`, `libraries` and `blendtables`
   against their recordings. It does not translate Delphi probes.
3. The persistence round trip: a save read and written back is byte-identical in the first stage,
   and equal under the comparator in the second (`docs/design.md` sections 5 and 9).
4. The headless oracle: a seeded computer-only game produces the same saves as the Delphi build,
   turn for turn, with the floating point allowance described in `docs/design.md`.
5. Frame comparison: a scene rendered by the port equals the Delphi build's frame.
6. A person plays it.

## 7. Code format

The C# sources under `src/` follow one format, and the gate's product layer checks it.

- `.editorconfig` at the repository root sets the whitespace (four spaces, braces on their own
  lines, the spacing around operators and commas), LF line endings, UTF-8 without a byte order
  mark, and the order of using directives (plain directives with the System namespaces first,
  then static directives, then aliases). `dotnet format` applies these. The style preferences in the file (file-scoped
  namespaces, `var`, expression bodies) are silent: they guide new code, and neither the build nor
  `dotnet format` enforces them.
- The blank lines between members are kept by MemberSpacing, a Roslyn tool in the private
  repository (`tools/format`). Every member of a type, a namespace or a file is separated from the
  next by exactly one blank line. Consecutive single-line members of one kind (fields, properties,
  expression-bodied or bodiless methods, enum members, delegates) keep zero or one blank line as
  written. The blank line goes above the comments and attributes of the member below. Everywhere
  else a run of blank lines collapses to one. Lines inside string literals, multi-line comments and
  inactive preprocessor regions never change.
- The reformat that introduced this format is listed in `.git-blame-ignore-revs`. Run
  `git config blame.ignoreRevsFile .git-blame-ignore-revs` once in a clone to have `git blame`
  skip it.

Before a pull request, format the tree and check it, from the product repository:

```bash
dotnet format OpenAow.slnx
dotnet run --project D:\git\open-aow-private\tools\format\Format.csproj -- src
dotnet format OpenAow.slnx --verify-no-changes
dotnet run --project D:\git\open-aow-private\tools\format\Format.csproj -- --check src
```

The first two commands rewrite files. The last two change nothing and list the files that differ.
The gate runs the two checks after the product tests and fails the product layer with the names of
the files that differ.
