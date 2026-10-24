# How OpenAoW is built

This document describes who does what, where the truth lives, and how a task goes from a
specification to verified code. Read `docs/design.md` first for the design itself.

## 1. Roles

Three parties work on the port:

- The design session. A Claude session in `D:\git\open-aow` owns the architecture, the design
  document, the specifications in `docs/specs/` and the task briefs. In the design stage it runs on
  the strongest model and writes everything down. In the implementation stage the same session,
  with its context kept, runs on a cheaper model and mostly coordinates: it briefs the implementer,
  reads its reports, verifies the result against the oracle and commits.
- The implementer. GPT Sol-6.1 at high reasoning effort, driven through the `codex` command line
  in non-interactive, resumable sessions. One session per task. It writes the code and the tests
  for one brief and reports back. It does not commit and does not change branches.
- The recovery session. The "decompile" session of the `aow` project in `C:\git\aow` refines the
  recovered Delphi source. `D:\git\aow` is a pull of it. Ask that session when the recovered source
  is unclear.

The user decides scope, settles open questions and approves anything irreversible or outward facing.

## 2. Where the truth lives

- `docs/design.md`: the decisions. When a decision changes, this file changes first.
- `docs/specs/`: one specification per subsystem, with the algorithms, formats and mappings an
  implementer needs, and a done measure. `docs/specs/README.md` lists them and their state.
- `docs/briefs/`: one brief per implementation task, written from a specification. A brief is
  self-contained: the implementer gets nothing else.
- `D:\git\aow`: the recovered Delphi source (`src/delphi3/`), the recovery documents (`doc/`), its data files (`data/`) and
  the tools (`tools/`). The port reads from it and never writes to it.
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

Continue a task with context kept:

```bash
codex exec resume --json <thread_id> "<follow-up>" < /dev/null
```

Fork a task to try an alternative without losing the first line of work:

```bash
codex exec fork <thread_id> "<new direction>" < /dev/null
```

Run long tasks in the background and read the report file when they finish. A task that needs
the Delphi compiler or the game files says so in its brief with the exact paths.

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
5. When the done measure holds, the design session commits with a message that names the brief.
   Commit messages end with the attribution line for the model in use.

## 5. Standing rules for the implementer

Every brief carries these:

- Do not commit, do not switch branches, do not push.
- Generated code under `src/Generated/` is never edited by hand. Change the transpiler or add an
  override under `src/Overrides/`.
- Do not change `D:\git\aow`.
- Tests run with one command named in the brief, and the brief's done measure is checked by a
  command, not by reading.
- Report what was done, what was not done and why, and anything found that the brief did not
  anticipate. A report that claims completion without the done measure's output is incomplete.

## 6. Verification ladder

In order of cost. A task uses the cheapest rung that proves its claim.

1. The .NET build compiles with no warnings in the changed projects.
2. Unit tests pass. Fixtures that encode Delphi behavior (random sequences, string routine results,
   stream bytes) come from small Delphi programs compiled with the oracle compiler and are stored
   under `tests/fixtures/` with the program that made them.
3. The persistence round trip: a save read and written back is byte-identical.
4. The headless oracle: a seeded computer-only game produces the same saves as the Delphi build,
   turn for turn, with the floating point allowance described in `docs/design.md`.
5. Frame comparison: a scene rendered by the port equals the Delphi build's frame.
6. A person plays it.
