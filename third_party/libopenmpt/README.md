# libopenmpt 0.8.9

The module player the port uses for the game's `.IT` music (`docs/specs/platform.md` section 5).

`win-x64/` holds the amd64 runtime DLLs of the official development package, unchanged:

| archive | from | SHA-256 |
|---|---|---|
| `libopenmpt-0.8.9+release.dev.windows.vs2022.zip` | https://lib.openmpt.org/files/libopenmpt/dev/ | `a23e375e576e3a6997ffeeb6cf34488e9020e2a5d8226856b02628b6a87c5e8e` |

Downloaded on 2026-10-27 over HTTPS from the official site, with the user's approval. openmpt.org
publishes no checksums for these archives and the DLLs carry no Authenticode signature, so the
hash above records the copy used here, for checking any later copy against it.

On Linux the port loads the distribution's `libopenmpt.so.0`, because openmpt.org ships no Linux
binaries.

libopenmpt is BSD-3-Clause licensed, and the bundled codec libraries carry their own licenses, all
in `licenses/`.
