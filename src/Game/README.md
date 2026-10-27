# Aow.Game

The game executable: starts the SDL host (`src/Platform`) with the game install as the data root.
Until milestone 5.3 it shows a test pattern instead of the title scene.

```
dotnet run --project src/Game -- --game-root "<Age of Wonders install>" [--user-data <dir>] [--scale <n>] [--fullscreen]
dotnet run --project src/Game -- --verify-sdl        # SDL lifecycle self-check
dotnet run --project src/Game -- --smoke-test        # present three frames and close
```

The settings and the files the game writes go to the user data directory, never into the install.
