# Graphics specification

State: draft, 2026-10-25. Covers the pixel pipeline of the game: the surfaces, blitters and blend
tables of `GFXE` (Gfxepack.dpl), the image libraries, sprites, fonts and animation sequences of
`ImageLib` (Ilpack.dpl), the display controller and frame loop of `DisplayC` (Dcpack.dpl), the
frame procedure and cursor of the main form (`AoWUnit.pas`), and the presenter that puts the
finished frame on the screen. The UI toolkit that draws into these surfaces (aowInt) is
`ui-layout.md`, the window, input and audio hosting is `platform.md`, and the intro video player
(VideoP) is `platform.md` too. Facts come from `src/delphi3` of the `aow` repository as read on
2026-10-25 and from the main form's component properties in `data/forms/AoW/MainForm.txt`.

## 1. Decisions

1. Software rendering, unchanged. The original draws every frame into a 16-bit back surface in
   system memory with its own blitters and then copies it to the screen. The port keeps all of
   that: the drawing code is transpiled, the 39 assembler blitters are rewritten in C# with the
   same results, and only the last copy, back surface to window, is new (section 7).
2. The canonical pixel format is `pfRGB555`. The main form's windowed controller `WinDC` is a
   `TWindows16DC` with `PixelFormat = pf555`, so the rebuilt game renders 555 frames in windowed
   mode, and those frames are the oracle for pixel comparison (section 9). The 565 code paths are
   kept, transpiled or rewritten like the 555 ones, and selectable by a setting, but nothing is
   verified against the oracle in 565.
3. No DirectDraw, no GDI. `TDDrawSurface`, `TDIB16Surface` and `TDIBSurface` become memory
   surfaces with the same class names and the same methods, so that the game code that creates
   them (the map viewer's scene cache, window back surfaces, the structure preview) transpiles
   unchanged and takes its system memory branches. A surface is never lost. Methods that need a
   Windows handle (`BltDC`, `Handle`, `ShowDC`) raise `ENotSupported` and are reached only by
   editor and design-time code.
4. Pixel exactness is the measure. Where the original's assembler produces results that depend on
   its implementation (the paired-pixel blend paths of section 5), the port reproduces the
   implementation, not an idealized formula, and a conformance probe proves each routine against
   the oracle compiler's binary.
5. The frame loop stays the original's synchronous loop (`UpdateFrameMode = umSynchrone`,
   `FrameRate` 30). The presenter adds nothing of its own: no interpolation, no extra buffering,
   and the frame limiter is the original's algorithm on the host clock.

## 2. Unit map

| unit | lines | treatment |
|---|---:|---|
| `GFXE.pas` | 12,903 | Transpile the Pascal parts: palette, `TSurface`, `TDataSurface`, pixel format ids and masks, `Color2PixelInfo` and its inverses, the clipping blitters (`BltSDRectClipped`, `BltSprite16`, `BltSpriteClipped16`), `ColorFillSD`, `DrawLine16`, `CreateSDfromSD`, `GetPixelInfo`, `MinimizeTransparentRect`, `GetSurfaceRectangles`, `RemapSurfacePart`, the blend table classes and generators, `TDisplayModeList`. Rewrite the 28 assembler routines (section 5). Replace `TDIBSurface`, `TDIB08Surface`, `TDIB16Surface`, `TDIB24Surface`, `TDIB32Surface`, `TDDrawSurface` with memory surfaces (section 3.4). Drop `TGDISurface`, `TDrawPanel`, the DirectDraw interfaces and `DD`, `GetOptimalGFXESettings` with `GFXDiag.pas` (a timing dialog that picks a `BltRect` variant), `RegisterBltProc`, the settings stream and registry routines, `SaveSurfaceDescToBMP24` (kept as a test helper), `LoadBMP` and the `TImageFile` classes (used by the editor's grabbing tools). `GetElapsedMilliSeconds` and `GetElapsedMicroSeconds` read `IClock`. `RDTSC`, `CPUIDSupported` and `MMXsupport` return constants |
| `ImageLib.pas` | 10,752 | Transpile the classes of section 6. Rewrite the 11 assembler routines. Drop the design-time and editor parts: `TCustomImageDrawGrid` and descendants, `TImageLibraryPanel`, `TILManagerComboBox`, `TILManagerListBox`, the property editors, clipboard, `GrabImage` and `CreateGrabForm` of every class (image capture from screenshots, used to build libraries), `ShowDC`, `BltLibraryImage`, `BltImageSequence`, `BltImageSequenceEx` (GDI canvas drawing). The rules packages reference some of these from editor-support code (`BltLibraryImage` 8 times in AoWEPACK, 3 in Engine, 1 in HSEPack; `TCustomImageDrawGrid` 7 times in AoWEPACK); those units are reviewed when the transpile set is finalized, and a referenced routine becomes a stub that raises `ENotSupported` rather than a drop |
| `DisplayC.pas` | 3,191 | Transpile `TCustomDisplay`'s logic (frame loop, surface index, pause and resume, controller activation, events) with its `TWinControl` base replaced by the shim's control stub, and `TDisplayCtrl` (frame counter, frame limiter, invalidation matrix). Replace the five Windows controllers and the three DirectDraw controllers with one controller, `TSdlDC` (section 7.3). Drop `TDDrawFSWinControl`, `TUpdateFrameThread` (asynchronous mode, editor only), `TMatrix` stays (used by `BltInvalidatedSurface`, which the game does not use but the code references) |
| `AoWUnit.pas` (`TMainForm`) | 463 | Rewritten as the platform host (`../design.md` section 3). Its frame procedure `DisplayUpdateFrame`, `GetCursorRect`, `DrawCursor` and `ScrollMap` are transcribed as they are, because they are the game loop and the software cursor (section 7.5) |
| `Video.pas` (VideoP) | 665 | Rewrite, `platform.md`: AVI through the Video for Windows API, one file, `IMAGES\SCENES\INTRO.AVI` (94 MB) |

The transpiler meets these units with the pointer idioms of section 3.5. `tools/transpile/units.toml`
lists the dropped routines and classes per unit, so that a reference to a dropped member from the
transpile set is an error, not a silent stub.

## 3. Surfaces and pixel formats

### 3.1 The surface description

Every drawing routine of the game takes a `const SD: TSurfaceDesc`, the description of a locked
surface, and the counts of such parameters show where drawing lives: AoWEPACK 59, AoW 28, AoWTCPCK
3; aowInt and AOWTools lock surfaces themselves (12 and 10 `Lock` calls) and pass descriptors down.
The record (60 bytes, offsets from `GFXE.pas`):

| offset | field | meaning |
|---:|---|---|
| 0 | `Width`, `Height: Integer` | pixels |
| 8 | `BitCount: Integer` | 8, 16, 24 or 32 |
| 12 | `Pitch: Integer` | bytes per row, positive |
| 16 | `ClipRect: TRect` | the writable rectangle in surface coordinates, `Left`/`Top` inclusive, `Right`/`Bottom` exclusive. `Lock` sets it to the whole surface |
| 32 | `Data: Pointer` | the first byte of row 0 |
| 36 | `Palette: TGFXEPalette` | for 8-bit surfaces |
| 40 | `PixelFormatID: Integer` | section 3.2 |
| 44 | `RMask`, `GMask`, `BMask`, `AMask: Integer` | channel masks, filled by the 16-bit surfaces |

`CreateSDfromSD(Src, R, Dest)` makes a sub-descriptor: `Dest.Data` points at `(R.Left, R.Top)`,
`Dest.ClipRect` is `Src.ClipRect` intersected with `R` and translated by `(-R.Left, -R.Top)`, and
everything else is copied. The UI toolkit draws every window through such sub-descriptors (`AoW`
uses it 5 times, aowInt through its own equivalents). The assembler blitters read `ClipRect`,
`Pitch` and `Data` at their byte offsets (`[edi+$10]`, `[edi+$0C]`, `[edi+$20]`), which is why the
record's layout is part of the contract and why the shim's `TSurfaceDesc` is an explicit-layout
struct with the offsets above.

### 3.2 Pixel formats

A pixel format id packs the bit count in bits 0 to 7, the channel order in bits 8 to 15 (two bits
per channel in the order A, B, G, R) and the bits per channel in the high nibbles (`MASKtoPFID`
and `PFIDtoMASK` are transpiled). The constants:

| id | value | layout |
|---|---|---|
| `pfPAL8` | `$00000008` | 8-bit palette index |
| `pfRGB555` | `$55509310` | `xRRRRRGG GGGBBBBB` |
| `pfARGB1555` | `$55519310` | as 555 with the top bit as alpha |
| `pfRGB565` | `$56509310` | `RRRRRGGG GGGBBBBB` |
| `pfRGB24`, `pfBGR24` | `$88809318`, `$88801B18` | 24-bit |
| `pfRGB32`, `pfXRGB32` | `$88889320`, `$88809320` | 32-bit |

`Color2PixelInfo(Color, pfid, Palette)` converts a `TColor` (`$00BBGGRR`): 565 gives
`(R and $F8) shl 8 + (G and $FC) shl 3 + (B and $F8) shr 3`, 555 gives `(R and $F8) shl 7 + (G and
$F8) shl 2 + (B and $F8) shr 3`, and `pfPAL8` gives the palette's nearest entry by the sum of
absolute channel differences, lowest index on a tie. `TSurface.GetColor` reverses 555 as
`(W and $7C00) shr 7 + (W and $03E0) shl 6 + (W and $1F) shl 19` (the low 3 bits of each channel
are zero). The port's `TSdlDC` back surfaces are `pfRGB555` (decision 2).

### 3.3 Clipping and the simple blitters

The Pascal routines are transpiled and need no specification beyond their semantics, recorded here
because the frame oracle depends on them:

- `BltSDRectClipped(Src, SrcRect, Dest, X, Y)`: clips `SrcRect` to `Src.ClipRect` (moving `X`,
  `Y` along when the left or top is cut), then to `Dest.ClipRect`, and copies `W` by `H` pixels
  with `BltRect` when both formats are equal and `W > 0`, `H > 0`. Different formats return
  `GFXE_PIXELFORMATNOTSUPPORTED` and draw nothing.
- `BltSpriteClipped16`: the same clipping, then `BltColorKey16` with the key from
  `Color2PixelInfo(ColorKey, Src.PixelFormatID)`.
- `ColorFillSD(R, Color, SD)`: clips `R` to `ClipRect` and fills rows with `FillWord` for the three
  16-bit formats, `GFXE_NOTSUPPORTED` otherwise.
- `DrawLine16(X1, Y1, X2, Y2, Skip, Color, SD)`: Bresenham from the leftmost endpoint, one pixel
  every `Skip + 1` steps, with a fast path when both endpoints are inside `ClipRect` and a
  per-pixel bounds test otherwise. The four octant variants and the final pixel rule (`if Count =
  0 then P^ := Color` after the loop) are kept exactly, including the detail that the fast path
  advances `P` without tracking `X` and the clipped path tracks both.
- `TSurface.Blt`, `BltClipped`, `BltColorKey`, `BltColorKeyClipped`: lock both surfaces and call
  `BltRect` or `BltColorKey16`; `BltStretch` supports only equal sizes and returns
  `GFXE_NOTSUPPORTED` otherwise. `TSurface.Cls` fills every row with the converted color.

### 3.4 Surface classes in the port

| class | original | port |
|---|---|---|
| `TSurface` | abstract base with the VMT `GetColor, GetPixelInfo, GetWidth, GetHeight, GetPixelFormatID, Blt, BltClipped, BltColorKey, BltColorKeyClipped, BltDC, BltStretch, Lock, Unlock, LockEx, UnlockEx, UpdatePalette, Resize, Cls` | transpiled. `BltDC` raises `ENotSupported` |
| `TDataSurface` | memory surface: `Create(W, H, pfid, Palette)`, `Resize` allocates `Pitch = (((BitCount + 7) shr 3) * W + 7) and not 7` bytes per row with the data 8-byte aligned, `Lock` fills the descriptor with `ClipRect` = whole surface, `CaptureSurface` copies a rectangle of another surface | transpiled. The memory is a `byte[]`, `Data` is a `Ptr<byte>` at offset 0 (the 8-byte alignment of the original is a performance choice with no effect on pixels, the pitch formula is kept so that `Pitch` values match) |
| `TDIB16Surface` | a 16-bit Windows DIB section, bottom-up, used by `TWindows16DC` for the back surfaces and by `Structur.PAS:1696` for a structure preview; `Lock` gives top-down `Data` with a negative pitch | a memory surface with the same constructor `Create(W, H, pfid)`, top-down positive pitch `(W * 2 + 3) and not 3`, `Cls`, `Resize`, `Blt`, `Lock`, `PixelFormatID` read-write before the first lock. `Handle` and `BltDC` raise |
| `TDIBSurface`, `TDIB08Surface`, `TDIB24Surface`, `TDIB32Surface` | DIB sections of other depths, used by `TWindows08DC`, `TWindows24DC`, the video player and the editor | the same memory stand-in, generic in bit count. Only the video player (rewritten) and editor code reach them |
| `TDDrawSurface` | a DirectDraw surface: `CreateSurface(W, H, pfid, Flags)`, `Lost`, `Restore`, `Status` (`ssVideoMemory`, `ssSystemMemory`), `AsyncCls`, `OnSurfaceLost` | a memory surface. `CreateSurface` allocates, `Status` is `[ssSystemMemory]`, `Lost` is `False`, `Restore` returns `DD_OK`, `AsyncCls` is `Cls`. The game creates these only when the back surface it draws into `is TDDrawSurface` (`HSEngine.pas:4813`, `AoWWinControl.pas:189`, `AoWBookWindow.pas:139`), which is never true in the port, so the class exists for the type checks and the editor |
| `TGFXEPalette` | 256 entries of 4 bytes with `PixelFormatID`, `LoadFromStream`, `Remap` to 555 or 565, `GetNearestColor` | transpiled |

### 3.5 Pointer idioms the transpiler meets here

The graphics code addresses pixels through integer arithmetic on pointers, and these forms are
the ones the transpiler's `Ptr<T>` mapping (`transpiler.md` 5.8) has to cover for GFXE, ImageLib,
aowInt and the drawing code of AoWEPACK:

| Delphi | C# |
|---|---|
| `Pointer(Integer(P) + n)`, `PWord(Integer(P) + n)`, `PChar(Integer(P) + n)` | `P + n` on `Ptr<byte>`, reinterpreted as `Ptr<ushort>` where the target type says so |
| `PWord(X + X + Integer(SD.Data) + Y * SD.Pitch)^` | `SD.Data.U16At(X + X + Y * SD.Pitch)`, a typed read at a byte offset |
| `Inc(Integer(P), SD.Pitch)`, `Dec(Integer(P), n)` | `P += n`, `P -= n` |
| `Inc(P)` on `PWord` | `P += 2` in bytes, the element size |
| `PIntArray(Integer(RowTable) + H * 4)` | `RowTable.Slice(H * 4)` as `Ptr<int>` |
| `Integer(FData)` stored as a value in an integer array (`TPicture16.ShowOpaqueStretched`, `RowTable^[I] := C * FPitch + Integer(FData)`) | not expressible: the four stretched `Show` methods of `TPicture16` (`ShowOpaqueStretched`, `ShowBlendedStretched`) are overrides that build the row table as an `int[]` of byte offsets and pass `FData` to the rewritten `BltStretched16` as the base |
| `GetMem`, `ReallocMem`, `FreeMem` of pixel buffers | `Ptr<byte>` over a `byte[]` of the size, reallocation copies |
| `FillChar(P^, n, v)`, `Move(S^, D^, n)` | `Ptr` spans |

A probe `pointers` (section 10) exercises each row on the oracle compiler before the graphics units
are transpiled.

## 4. Blend tables

A blend table is 32 by 32 bytes indexed by two 5-bit channel values. The 16-bit blenders look up
every channel of a source pixel (`I`, the first index) and the destination pixel (`J`) and write
the table value, so a table is a function of two 5-bit inputs. Five collections exist as globals of
`GFXE`, each a `TLookupBlendTableCollection100` with 31 tables and a `Lookup[0..100]` array that
maps a percentage to table `(Percentage * 31) div 100`, capped at 30. `InitBlendTables(5, 31)` runs
in the unit's initialization. Table `k` of a collection is generated with `V = k / 30`:

| collection | `Table[I, J]` |
|---|---|
| `AlphaBlendTable` | `Round(I * V + (1 - V) * J)` |
| `LinearAlphaBlendTable` | `F = I / (31 * V)` for `V > 0` (else 1), capped at 1; `Round(I * F + (1 - F) * J)` |
| `IntensityBlendTable` | `F = 0` for `I = 0`, else `I * V * 1.75 / (I + J)`; `min(31, Round(I * F) + J)` |
| `ShadowBlendTable` | `F = (31 - I) * (1 / 31) * V`; `V2 = Round((1 - F) * J)`, clamped to `0..J` |
| `BrightenBlendTable` | `max(J, Round(I * V + (1 - V) * J))` |

The originals compute in `Single` variables with x87 extended intermediates and Delphi's `Round`
(banker's rounding). The port computes in `double` with `Math.Round(x, MidpointRounding.ToEven)`
and proves equality with the probe `blendtables`, which prints a hash of every table (5 by 31 by
1,024 bytes) from the oracle compiler. If a table differs at a rounding boundary, the port keeps
the oracle's bytes for that table as a constant, since the tables are derived data, not game data.

The game selects tables in three ways: `TShowInfo.BlendMode` with `BlendPercentage` through
`Lookup`, a `bmUser` table passed directly, and the cursor's pulsing alpha in `DrawCursor`,
`AlphaBlendTable.LookupTables[Abs((Frame shl 2) mod 100 - 50) + 30]`, an index into the 31 tables
through `GetLookupTables` (the `Lookup` array, so the same mapping).

## 5. Assembler routines

### 5.1 Inventory

The recovery counted 330 `asm` routines in the transpile set when a trailing comment after the
`asm` keyword is allowed: GFXE 28, ImageLib 11, Engine 14, HSEngine 8, Video 2 and 267 in
AoWEPACK. The 39 of GFXE and ImageLib are the graphics blitters and are rewritten here. The Engine,
HSEngine and AoWEPACK ones are mostly stubs the recovery wrote to reproduce the original code
(thunks that `JMP` to another routine, functions whose body is `XOR EAX,EAX`, a custom allocator, a
few bit tests); `transpiler.md` section 8.2 covers them.

### 5.2 The blitters

Every routine below becomes a static C# method on `Aow.Graphics.Blit` with the same name and the
same parameter list, pointers as `Ptr<byte>` and `const SD: TSurfaceDesc` as `in TSurfaceDesc`. The
semantics column is the contract. Where the semantics say "paired", the assembler processes two
pixels per step when the destination is dword aligned and the run is long enough, with a different
mask on the high pixel, and the result differs from the single-pixel formula in the low bit of one
channel. The port reproduces the pairing (it transcribes the control flow of the assembler) and the
probe of section 5.3 is the proof.

GFXE (all `pascal` calling convention in the original, irrelevant in C#):

| routine | semantics |
|---|---|
| `BltRect`, `RegisterBltRect`, `FPBltRect`, `RepMovsdBltRect`, `MMXBltRect` | copy `Width` bytes of `Height` rows from `Src` (pitch `SrcPitch`) to `Dst` (pitch `DstPitch`). One implementation, `BltRect`; the variants were a speed choice of `GetOptimalGFXESettings` and produce the same bytes |
| `BltColorKey16(Src, Dst, SrcPitch, DstPitch, Width, Height, ColorKey)` | copy `Width` 16-bit pixels per row except pixels equal to `ColorKey` |
| `BltTransparent555`, `BltTransparent565` | 50 percent blend of source over destination without a color key: `((s and M) + (d and M)) shr 1` per pixel, with `M = $7BDF` (555) and `$F7DF` (565) for single pixels; paired dwords use `$7BDE7BDF` and `$F7DEF7DF`. Used by `TPicture16.ShowTransparent` and `TSprite16` |
| `BltStretched16(RowTable, ColTable, Dst, DstPitch, Width, Height)` | nearest neighbour: `Dst[y][x] = Row(y)[Col(x)]` where `RowTable[y]` is the source row (an address in the original, a byte offset from the base in the port) and `ColTable[x]` the source column |
| `FillInt(Dest, Count, Value)`, `FillWord`, `FillTrippleByte` | fill `Count` elements of 4, 2 or 3 bytes |
| `RemapSurface16to16(Src, Dst, Table, ...)`, `RemapSurface16to08`, `RemapSurface16to32` | per pixel table lookup, `Dst = Table[Src]` with a 65,536-entry table of the destination size |
| `RemapSurface555to32`, `RemapSurface565to32`, `RemapSurface555to565`, `RemapSurface565to555` | channel expansion and repacking per pixel. 555 to 565: `(p and $7FE0) shl 1 + (p and $1F)` with the green low bit 0; 565 to 555: `(p and $FFC0) shr 1 + (p and $1F)`; to 32: each 5-bit channel shifted into an 8-bit channel (low bits 0) |
| `BlendSurface555(Src, Dst, SrcPitch, DstPitch, Width, Height, Table)`, `BlendSurface565` | per pixel, per channel: `Dst.ch = Table[Src.ch5, Dst.ch5]` where `ch5` is the channel's top 5 bits; for 565 the green low bit of source and destination is dropped and the result green is written with its low bit 0. Paired |
| `BlendColorKey555`, `BlendColorKey565` | as `BlendSurface`, skipping source pixels equal to `ColorKey` |
| `BlendStretched555`, `BlendStretched565` | `BltStretched16` with the blend of `BlendSurface` instead of the copy |
| `RDTSC`, `CPUIDSupported`, `MMXsupport` | `0`, `False`, `False` |

ImageLib:

| routine | semantics |
|---|---|
| `BltRLE16(X, Y, SD, Data, Width, Height, ColorKey)` | draws an RLE sprite (section 6.3) at `(X, Y)` with clipping to `SD.ClipRect`. Rows above the clip top are skipped by their stored size, rows below are not drawn, a row that lies entirely inside horizontally takes the fast path (dword reads, two pixels per step), a row that crosses the left or right edge is walked pixel by pixel with a bounds test. A sprite whose rectangle misses the clip rectangle entirely draws nothing |
| `BlendRLE555`, `BlendRLE565` | the same traversal, every opaque pixel written as the 50 percent blend of `BltTransparent555`/`565` (same masks, paired in the fast path). `TRLESprite16.ShowTransparent` calls these, so a "transparent" RLE sprite is a half-blended one |
| `RemapRLE555to565(Data, Height, ColorKey)`, `RemapRLE565to555` | convert every pixel word of every row in place, leaving the color key words and the skip counts that follow them untouched. `FColorKey` is not converted, so a converted pixel that happens to equal the key becomes transparent; this is the original's behavior |
| `ShowBitMask16(X, Y, SD, Data, Width, Height)` | writes `$FFFF` (white) at every set bit of the mask (section 6.4), clipped |
| `ShowShadow555`, `ShowShadow565` | at every set bit, darkens the destination to three quarters: `(d shr 1 and $3DEF) + (d shr 2 and $739C)` for 555 and `(d shr 1 and $7BEF) + (d shr 2 and $E79C)` for 565, paired with the doubled masks |
| `ShowRLE08(X, Y, SD, Data, Width, Height, ColorKey: Byte)` | draws an 8-bit RLE sprite (rows of a length dword, opaque indices, and for a transparent run the transparent index followed by the run length) to an 8-bit surface |
| `ShowRLE08to16(..., Palette)` | the same through a palette of 16-bit entries |
| `TFrameTableList.Error` | raises `EListError` with the message and index, as `TList.Error` does |

The exact bit handling of the paired paths is read from the assembler, not from a formula, and the
C# transcription is checked instruction group by instruction group against the listing in
`GFXE.pas` and `ImageLib.pas` (the recovery transcribed the assembler from the binary, so the
listing is the original's code).

### 5.3 Conformance probe

`tests/probes/blitters.pas` is a DCC32 program that uses `GFXE` and `ImageLib` directly (the
units compile outside the game: they need only VCL units), fills source and destination buffers
from a seeded `Random`, calls every routine of section 5.2 on a battery of cases (both formats,
widths 1 to 70 so that every alignment and tail length occurs, clip rectangles that cut each edge
and that miss the sprite, skips of 0 to 3, RLE rows with runs at every position, bit masks of
widths across dword boundaries) and prints a 32-bit hash of the destination buffer after each
call. The C# side runs the same cases through `Aow.Graphics.Blit` and prints the same hashes. The
expected output is committed; a differing line names the routine and case. The RLE and bit mask
inputs are built in the probe by the ImageLib encoders (`TRLESprite16.GrabImage`,
`TBitMask.GrabImage`), which are transpiled for the test even though the game never grabs.

## 6. Image libraries

The format description here is what the transpiled loader reads; the port does not implement a
second reader. It is recorded so that test tools can read libraries and so that a divergence has a
vocabulary. The GOG install holds 538 `.ILB` files, 109 MB, under `Images\`, `Int\` and
`EDIMAGES\`; the largest are `Int\Scenes\PyreFrm.ILB` (5.1 MB) and the storm effects (4 MB each).

### 6.1 File layout

`TILBHeader` (24 bytes by the layout rules of `transpiler.md` 4.3): `ID: string[4]` (`'ILB'#0`,
5 bytes, 3 bytes padding), `Version: Single` at offset 8 (shipped files are version 4),
`HeaderSize` 12, `IndexEnd` 16, `Size` 20. From version 4 the file is header, palette manager,
image index up to `IndexEnd`, then the discardable data (pixels) up to `Size`. The index is read
into memory in one piece; an entry is `Integer index`, the image record (`ReadImageFromStreamEx`:
class id, then `ReadImageData`), `Integer reference`, terminated by index `-1`. A reference entry
has no image of its own and points at another index (fonts use this for shared glyphs). The palette
manager holds the 8-bit palettes (`TLibraryImagePalette`: `TGFXEPalette` plus a remapped copy made
on demand for the surface's format).

`TLibraryImage.ReadImageData` (version byte first): `FileName` (an `Integer` length and the
bytes), `DataWidth`, `DataHeight`, `Xshift`, `Yshift`, `FileIndex`, `LoadMode` (1 byte),
`DataSize`, `DataPosition` (version 2 and up), `Width`, `Height`, `ShowInfo` (8 bytes, version 3 and
up). `TImage16` adds `StoredPixelFormatID`. The class-specific trailers:

| class | id | trailer after the common fields | pixel data |
|---|---|---|---|
| `TPicture08` | 1 | palette link | rows of indices, pitch `(W + 3) and not 3` below 32 pixels, `(W + 7) and not 7` from 32 |
| `TRLESprite08` | 2 | transparent index, reserved, `RLELeft`, `RLETop`, `RLEWidth`, `RLEHeight` | 8-bit RLE rows |
| `TPicture16` | `$10` | none | rows of 16-bit pixels, pitch `(W * 2 + 3) and not 3` below 32 pixels, `(W * 2 + 7) and not 7` from 32 |
| `TRLESprite16` | `$11` | `SpriteWidth`, `SpriteHeight`, `SpriteX`, `SpriteY`, `ColorKey`, reserved | section 6.3 |
| `TTransparentRLESprite16` | `$12` | as `$11` | as `$11`, default `ShowMode` `smTransparent` |
| `TBitMask` | `$13` | `MaskWidth`, `MaskHeight`, `MaskLeft`, `MaskTop` | section 6.4 |
| `TShadow` | `$14` | as `$13` | as `$13` |
| `TTransparentPicture16` | `$15` | none | as `$10`, default `ShowMode` `smTransparent` |
| `TSprite16` | `$16` | `SpriteWidth`, `SpriteHeight`, `SpriteX`, `SpriteY`, `TransparentColor` | as `$10`, the pixels of the sprite rectangle only |
| `TImageNode` | `$100` | a count and nested images | the images' data |

An image's visual rectangle is `(Xshift, Yshift)` to `(Width, Height)`; the pixel data covers
`DataWidth` by `DataHeight` from the shift, and sprites and masks add their own offset
(`SpriteX`, `MaskLeft`) inside the data rectangle. `GetVisualDimensions` returns the rectangle
actually drawn.

### 6.2 Loading on demand

`FLoadOnDemand` libraries (`TCustomImageLibrary.LoadFromFile`) keep only the index; an image loads
its pixels on first `Show` through `LoadDiscardableImageData`, which opens the library file again
(`TFastLoadFileStream`, a read-only sequential handle), seeks to `DataOffset + DataPosition` and
reads `DataSize` bytes. `lmLoadFreeOnDemand` images release the pixels after each show. The port
routes the file through `IRtlHost.Files`; it is free to keep one handle per library open, since the
behavior is the same. The headless harness (`testing.md`) loads libraries too: `TImageSequence`
and the map object resources query image dimensions and frame counts in rules code.

### 6.3 The 16-bit RLE sprite

`FData` holds `SpriteHeight` rows. A row is a dword with the row's length in bytes including the
dword itself (written as a word length and a zero word), then a sequence of words: a pixel, or the
color key followed by a word with a skip length in bytes (twice the number of transparent pixels).
The row is padded to a dword boundary with one color key word when its length is not a multiple
of 4. The encoder is `TRLESprite16.GrabImage`: it first shrinks the grabbed rectangle to the
smallest one holding non-key pixels (`MinimizeTransparentRect`), records it as `SpriteX`,
`SpriteY`, `SpriteWidth`, `SpriteHeight` relative to the data rectangle, and encodes the rows.
`ColorKey` is in the stored pixel format and is remapped with the data when the surface format
differs (`RemapToPixelFormat`).

### 6.4 The bit mask

`FMaskData` holds `MaskHeight` rows of `Pitch = ((W + 31) shr 5) shl 2` bytes, each row whole
dwords, the first pixel of a row in the most significant bit of its first dword (`AddBit` shifts
left and flushes a dword every 32 bits, the last dword of a row is shifted up to fill). A `TBitMask`
draws white, a `TShadow` darkens (section 5.2).

### 6.5 Show dispatch

`TLibraryImage.Show(X, Y, SD)` dispatches on the image's `ShowInfo`: `smOpaque` calls
`ShowOpaque`, `smTransparent` calls `ShowTransparent`, `smBlended` calls `ShowBlended` with the
table selected by `BlendMode` and `BlendPercentage` (section 4), or the user table. `ShowEx` takes
the show info as a parameter but reads the blend mode from the image's own `FShowInfo`, a quirk
of the original kept by transpilation. `ShowStretched` exists for pictures only. `ShowClipped` is
`Show`: every blitter clips. Per class:

| class | opaque | transparent | blended |
|---|---|---|---|
| `TPicture16` | `BltRect` of the clipped rectangle | `BltTransparent555/565` | `BlendSurface555/565`; stretched forms through `BltStretched16` and `BlendStretched` |
| `TSprite16` | `BltColorKey16` | `BltTransparent` | `BlendColorKey` |
| `TRLESprite16` | `BltRLE16` | `BlendRLE555/565` | not supported |
| `TBitMask` | `ShowBitMask16` | not supported | not supported |
| `TShadow` | `ShowShadow555/565` | not supported | not supported |
| `TPicture08` | `RemapSurface08to555/565` through the remapped palette | | |
| `TRLESprite08` | `ShowRLE08to16` | | |
| `TImageNode` | each child in order | | |

A picture whose pixel format differs from the surface's is remapped in place on first use
(`RemapToPixelFormat`), so a 565 library drawn on a 555 surface is converted once per image and the
converted pixels stay. Shipped libraries are stored in one 16-bit format; the frame oracle runs in
555, and the `blitters` probe covers `RemapRLE555to565` and the reverse so that a 565 run is also
exact.

### 6.6 Fonts

`TImageLibraryFont` is a library whose image `N` is the glyph of character code `N`.
`CharacterDistance` (2 by default) is added after every glyph, a character without an image
advances 5 pixels plus the distance, `Height` is the tallest glyph, `CharCase` lower- or
upper-cases the text first, `SetTextCentered` subtracts `TextWidth shr 1`, and `SetColor` recolors
8-bit glyph libraries by writing `Color2PixelInfo(Color)` into palette entry
`FontColorPaletteIndex` of the first palette and of its remapped copy. All transpiled.

### 6.7 Image sequences

`TImageSequence` animates a library: `SequenceMode` is one of `smNone` (one image), `smUp`,
`smUpDown`, `smRandom`, `smFrameTable`, `smRndFrameTable`; `SequenceSpeed` is frames per image minus
one; a chain of layers is linked through `Next`. `GetFrameIndex(Frame, Offset)` advances
`FFrameCounter` when `Frame` differs from the last frame seen and the speed counter has run out,
and maps the counter into the library or the frame table (`smUpDown` reflects with
`Abs(Result - (Count - 2))`). The random modes call `System.Random`, and `LoadFromStream` of a
frame-table sequence calls `Random(FFrameTable.Count)` while loading. These are the drawing-side
uses of the global generator that `testing.md` 3.3 notes: they do not touch game logic because
`TAoWHSMap.Random` reloads its own seed before every draw, and the frame oracle (section 9) fixes
`RandSeed` as the playtest harness does so that random animations repeat.

## 7. Display controller and frame loop

### 7.1 The display control

`TCustomDisplay` is the VCL control that owns the controllers and runs the loop. The main form's
instance (`MainForm.txt`): `Align = alClient`, `Color = clBlack`, `Options = [doAutoResizeBackSurface,
doAutoSwapDC, doAutoStartUpdateFrameLoop, doAutoActivatePrimaryDC, doRestoreSurfacesOnSwap,
doBltOnWMPAINT]`, `UpdateFrameMode = umSynchrone`, `FrameRate = 30`, and the constructor's defaults
`BufferingScheme = bsDouble`, `BackSurfaceControlMode = bmBltSurface`, back surface 640 by 480.
`SetFrameRate` stores `FFrameTime = Round(1000 / FrameRate)`, 33 ms. `PrimaryDC` is `WinDC` in
windowed mode; `FormCreate` makes it `DXDC` and `SecondaryDC` `WinDC` when the registry says full
screen. In the port both are the one `TSdlDC`, and `SwapDC` toggles its fullscreen flag.

### 7.2 One frame

`StartUpdateFrameLoop` in synchronous mode runs, until the loop is stopped or the application
terminates: `if not paused then UpdateFrame; Application.ProcessMessages`. `UpdateFrame`:

1. In triple buffering, toggle `FSurfaceIndex` between 1 and 2 (double buffering keeps 1).
2. Track whether the mouse is above the control; when it leaves with no button down, deliver a
   `MouseMove([], -1, -1)` so that the toolkit sees the pointer leave.
3. Call `OnUpdateFrame(Self, FActiveDC, FSurfaceIndex)`, which is `TMainForm.DisplayUpdateFrame`
   (`testing.md` 3.2 lists its order: deferred setup actions, DirectPlay messages, `ProcessSound`,
   `AoWHSMap.NewFrame`, then if a controller with a back surface exists: `GetMousePos`,
   `GetCursorRect`, `ScrollMap`, `General.Manager.DrawSurface(DC.GetSurface(1))`, the software
   cursor drawn into the locked back surface, the frame rate label, then `ReturnToTitle` on exit
   pending and `ApplyVideoSettings`). Exceptions in the two draw calls are caught and shown as
   messages, as in the original.
4. `FActiveDC.NewFrame`: every 5 frames, `CurrentFrameRate = Round(5000 / (now - last + 1))`.
5. `FActiveDC.WaitForVerticalBlank`: the frame limiter. `Target = LastVBTime + FrameTime`; if the
   clock is at or past `Target - FrameTime`, spin until `Target` with `Sleep(0)` while more than 5
   ms remain; then `LastVBTime = now`. The port sleeps with the host's precise wait for the part of
   `D` above 2 ms and spins the rest, which gives the same frame times without burning a core.
6. `ExecuteBltSurfaceEvent`: `OnBltSurface` (unassigned in the game), then `BltSurface(FSurfaceIndex)`
   for `bmBltSurface`, then `ShowFrameRate` when the option is on (debug mode only).

`Application.ProcessMessages` becomes the host's event pump: it drains the window's events and
delivers mouse and key events to the display control's handlers (`DisplayMouseDown`, `DisplayMouseMove`,
`DisplayMouseUp`, `DisplayKeyDown`, `DisplayKeyPress`, `DisplayDblClick`), which hand them to the
window manager (`ui-layout.md`). Pausing (`PauseFrameUpdateLoop`, a counter) is used around
controller swaps and modal VCL dialogs.

### 7.3 `TSdlDC`, the one controller

`TSdlDC` implements `TAbstractDC` for the port. Of the 35 virtual methods the game reaches these:

| method | behavior |
|---|---|
| `GetSurface(0)` | the screen stand-in: a `TSurface` whose `Width` and `Height` are the window's client size in back-surface pixels (section 7.4), with no pixel storage. `ScrollMap` reads its size to find the screen edges, `TCustomDisplay.Resize` reads it to size the back surfaces when `doAutoResizeBackSurface` is on |
| `GetSurface(1)`, `GetSurface(2)` | the back surfaces, `TDIB16Surface` stand-ins of `BackSurfaceWidth` by `BackSurfaceHeight` in `pfRGB555`, cleared to the display's color on creation, as `TWindows16DC.Activate` does |
| `GetSurfaceCount` | `Ord(BufferingScheme)` |
| `Activate(Display)`, `Deactivate` | create and free the back surfaces, set `dsActivated` and `dsWindowed` (the latter so that windowed-only branches of the game run), call `Display.ResizeBackSurfaces` with the client size when `doAutoResizeBackSurface` is on, as `TWindowsDC.Activate` does |
| `BltSurface(Index)`, `FlipSurface(Index)` | present `GetSurface(Index)` (section 7.4). `FlipSurface` returns `DCERR_NOFLIP` after presenting, as `TWindowsDC` does, so that the display falls back to blitting |
| `ResizeBackSurfaces(W, H)` | resize both back surfaces |
| `GetMousePos(X, Y)` | the pointer position in back-surface pixels, or `(-1, -1)` when the pointer is outside the client area, when another window has captured the mouse, or when the window is not focused. The original also reports `-1` when the cursor leaves the parent's client rectangle |
| `NewFrame`, `WaitForVerticalBlank`, `GetCurrentFrameRate`, `ShowFrameRate` | transpiled from `TDisplayCtrl`, on `IClock` |
| `SetPrimarySurfaceRgn`, `GetPrimarySurfaceRgn`, `GetStatus`, `GetDisplay`, `Handle`, `OutputWnd`, `Hide`, `Show`, `SupportDisplayMode` | transpiled from `TDisplayCtrl`, with `Handle` and `OutputWnd` returning the shim's window stub |
| `GetPalette`, `GetHPalette`, `SetPalet`, `RealizePaletNow`, `UpdatePalette` | no palette: `nil`, `0`, `DD_OK` |
| `Invalidate`, `InvalidateAll`, `InitInvMatrix`, `GetInvBlockSize`, `GetPixPosofBlock`, `GetBlockPosofPix`, `BltInvalidatedSurface` | transpiled from `TDisplayCtrl` and `TAbstractDC`; unused in `bmBltSurface` mode |
| `WndProc`, `WindowRecreated`, `ShowError` | `False`, nothing, `IDialogService.ShowMessage` |

### 7.4 Presenting, scaling and modes

Presenting copies the back surface's 555 pixels to the window. The host (`platform.md`) owns an
SDL3 window, a renderer and a streaming texture of `SDL_PIXELFORMAT_XRGB1555` the size of the back
surface; `Present(surface)` uploads the rows (`SDL_UpdateTexture` with the surface's pitch) and
draws the texture into the window with `SDL_RenderTexture`, then `SDL_RenderPresent`. The texture
is recreated when the back surface size changes.

Scaling policy: the back surface is the game's logical screen. Windowed, the window's client area
is the back surface size times an integer scale (1 by default, settable), and a user resize snaps to
the nearest integer scale. Fullscreen uses the desktop mode (`SDL_WINDOW_FULLSCREEN` with the
borderless desktop size), draws the back surface centered with the largest integer scale that fits,
and letterboxes the rest in black. There is no mode switching. The game's own resolution logic
(`VideoDialog`: `FDisplayWidth`, `FDisplayHeight` for play, 640 by 480 for the title scene when
`AllowResolutionSwitches` is set, `ChangeResolution` in `ApplyVideoSettings`) is kept and acts on
the back surface size: `ChangeResolution(W, H)` resizes the back surfaces, `ChangeResolution(0, 0)`
(windowed) sizes them to the window. `ListDisplayModes` offers 640 by 480, 800 by 600, 1024 by 768
and 1280 by 1024 filtered by the desktop size, the modes the original offered on a typical adapter.
The registry's `DisplayMode` (0 windowed, 1 full screen) maps to the fullscreen flag. The game
clamps the toolkit's layout to at least 640 by 480 (`DisplayResizeBackSurface`).

Mouse coordinates are mapped back through the same scale and offset to back-surface pixels, so
`GetMousePos` returns what the original returned on a 1:1 window, and positions over the letterbox
are outside.

### 7.5 Cursor

The game draws its own cursor into the back surface at the end of the frame (`DrawCursor`): the
map cursor image at `(X - 25, Y - 25)`, blended with a pulsing alpha when the seated player is
busy, a secondary cursor image at `(X - 15, Y - 20)`, and the interface cursor from `IntGfxMod.Cursors`
outside a game. The host hides the system cursor while the pointer is over the window
(`SDL_HideCursor`) and shows it over the letterbox. `GetCursorRect` and `FLastCursorRect` only drive
`FCursorChanged`, a hint the toolkit uses to redraw the cursor's old place; they are transcribed.

### 7.6 Headless

With no controller (`DC = nil`), `DisplayUpdateFrame` skips the mouse, scroll, draw and cursor
block and runs only the game logic, which is the path `testing.md` 3.2 drives. The headless host
therefore has no `TSdlDC` and no back surfaces at all, and `TCustomDisplay` is not created. Image
libraries still load (section 6.2).

## 8. What the graphics layer needs from the host

`IRtlHost.Display` (the SDL implementation is `platform.md`):

```csharp
interface IDisplayHost {
    (int Width, int Height) ClientSize { get; }        // in back-surface pixels after scaling
    bool Fullscreen { get; set; }
    void Present(in TSurfaceDesc backSurface);          // 555 rows, pitch in bytes
    (int X, int Y, bool Inside) PointerPosition();      // back-surface pixels, Inside false outside the image
    bool HasFocus { get; }
    void PumpEvents(IDisplayEvents sink);               // mouse, key, resize, close, focus
    void ShowSystemCursor(bool show);
}
```

`IClock.ElapsedMilliseconds` and `IClock.WaitMilliseconds(int)` serve the frame limiter and
`GetElapsedMilliSeconds`.

## 9. Frame oracle

The oracle of this layer is the rebuilt game in windowed mode, which draws in `pfRGB555` through
`TWindows16DC`. `tools/oracle/frames.py` (planned in `testing.md` 4) captures reference frames:

1. Capture point: a debugger breakpoint on `TWindowsDC.BltSurface` (or `TDIB16Surface.BltDC`), where
   the back surface is complete and the cursor is drawn. The script reads `FBack1`'s bits
   (`Width`, `Height`, `Pitch`, `FBits` with its bottom-up layout) and writes `frame-<n>.raw` (555
   words, top-down) with a small JSON of size and frame number. The frame number is
   `AoWHSMap.Frame` in a game and the display's frame count otherwise.
2. Fixtures: a list in `tests/frames/fixtures.toml`, each with a save or scene, a frame number to
   capture, the window size (640 by 480 unless stated), the registry settings (windowed, detail
   level, no intro), the mouse position (outside the window unless the fixture states one, so that
   no cursor is drawn), and `RandSeed` fixed as the playtest harness does. The first set: the title
   scene at frame 60; `small` (Mountain Clash) day 1 at frames 1, 30 and 90 after load; the same
   with the map scrolled by a scripted mouse position; one city window, one unit info window, one
   spellbook page opened by scripted clicks; `heroes` at the first tactical combat frame.
3. The port renders the same fixture with the same script through `TSdlDC` in headless-presenter
   mode (a `Present` that writes the frame to a file instead of a window) and
   `tools/oracle/compare_frames.py` compares the two 555 images pixel for pixel, writes the count of
   differing pixels, the first differing pixel, and a PNG with the differences highlighted, and
   applies the fixture's masks (rectangles excluded from the comparison; the first known one is the
   frame rate label, drawn only in debug mode, so none is expected at first).

Divergences come from four sources, checked in this order: a blitter (section 5.3 catches it
first), a blend table (section 4), the toolkit's layout (`ui-layout.md`, the form files), and the
game logic (which `testing.md`'s save comparison catches first). Floating point in drawing code
(`XScale := W / FDataWidth` and the `Round` calls of the stretched blits, the blend table
generation) is the one place where `double` versus `Single` with extended intermediates can move a
pixel; the stretched blit's table construction is therefore part of the `blitters` probe too.

## 10. Done measures

- G1, blitters: the `blitters` probe passes for every routine of section 5.2 in both formats.
- G2, blend tables: the `blendtables` probe passes.
- G3, libraries: every `.ILB` of the GOG install loads through the transpiled `TCustomImageLibrary`
  with no exception, and a `libraries` probe that renders every image of every library onto a
  black 555 surface at `(0, 0)` with `Show` and prints a hash per image gives the same hashes from
  the oracle compiler's build and the port. This proves the transpiled loader and the show dispatch
  on real data, including the 8-bit libraries and fonts.
- G4, frames: the fixtures of section 9 compare equal with no masks except those listed in the
  fixture file.
- G5, pacing: on a 30 frame rate the port's frame times measured over 300 frames are within 1 ms
  of 33 ms, and a frame that takes longer than 33 ms is not followed by a catch-up burst (the
  original's limiter has none either).

## 11. Open points

1. Which editor-support routines of ImageLib and GFXE the rules packages reference from code that
   the game executes (`BltLibraryImage`, `TCustomImageDrawGrid` in AoWEPACK). Settled when the
   transpile set is finalized by reading those references.
2. Integer-only scaling, or a filtered fractional scale as an option for small screens. Integer
   scaling is the default because it keeps pixel art exact; a fractional option is a later platform
   decision.
3. The 565 path is kept but unverified. A `blitters` probe run in 565 covers the routines, not the
   whole frame.
