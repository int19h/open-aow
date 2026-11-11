// Translated from AoWComp.pas.
using Aow.Game.Draw;

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

public delegate void AoWOnDrawItemEvent<in TSender>(TSender Sender, in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc);
