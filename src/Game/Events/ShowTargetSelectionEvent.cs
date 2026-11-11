// Translated from AoWE.pas.
using Aow.Game.Combat;

using Aow.Game.Draw;

namespace Aow.Game.Events;

public delegate void ShowTargetSelectionEvent(TargetSelectionControl TargetSelection, int HN, int Xs, int Ys, in LockedSurface SurfaceDesc);
