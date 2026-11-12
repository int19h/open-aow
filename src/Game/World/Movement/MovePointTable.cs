// The move points of each terrain and overlay (Delphi TMovePointTable, array[0..15, -1..14] of
// Shortint). A table is a value: the code copies whole tables and writes them through byte views.
// The overlay index starts at -1, so an index subtracts MovementTables.FirstOverlay.
global using MovePointTable = Aow.Engine.Buffers.Array16<Aow.Engine.Buffers.Array16<sbyte>>;
