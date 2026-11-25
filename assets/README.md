# Assets

Resources made for OpenAoW itself, not taken from the original game. The original game's data
stays in the user's installation and is never copied here.

- `icon/openaow.webp`: the application icon picture, a red dragon on a transparent background,
  1024 by 1024. It is the source.
- `icon/openaow.ico`: the Windows icon of `Aow.exe` (`ApplicationIcon` in `src/Aow/Aow.csproj`),
  with the sizes 16, 20, 24, 32, 40, 48, 64, 96, 128 and 256. It is generated from the source by
  `tools/icon/make_icon.py` in the private repository. Regenerate it after a change to the source.
