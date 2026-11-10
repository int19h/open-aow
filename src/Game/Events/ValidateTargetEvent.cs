// Translated from AoWE.pas.
using Aow.Game.Combat;

namespace Aow.Game.Events;

public delegate void ValidateTargetEvent(TargetSelectionControl TargetSelection, sbyte Xt, sbyte Yt, sbyte Lt, ref bool Valid);
