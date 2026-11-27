#nullable disable warnings
// Translated from AoWE.pas.

using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P1. }
// Structur.PAS:3785
// images, HNs, status, mode, callbacks, range, source coordinates, source, change events,
// change, target, text, title, cancel text, images
public sealed class TargetSelectionControl(EngineObject creator) : IDisposable
{
    // The strategic map that owns this control. Its engine holds the map and the pack the control reads.
    public EngineObject Creator { get; } = creator;

    public CustomImageLibrary? ImageLibrary { get; private set; }
    public SByteList? HexNumbers { get; private set; } = [];
    public TargetSelectionStatuses Status { get; set; }
    public TargetSelectionModes TargetMode { get; set; }

    private ShowTargetSelectionEvent? _show;

    internal void SetShowHandler(ShowTargetSelectionEvent handler) => _show = handler;

    private ValidateTargetEvent? _validateTarget;

    internal void SetValidateTargetHandler(ValidateTargetEvent handler) => _validateTarget = handler;

    public int Range { get; set; }
    public MapCoordinate SourceLocation { get; set; }
    public object? Source { get; set; }

    public event EventHandler<TargetSelectionChangedEventArgs>? Changed;

    // CityTargetSpell refuses a second listener, as the original read the count of the change list.
    internal int ChangedHandlerCount => Changed?.GetInvocationList().Length ?? 0;
    public MapCoordinate Target { get; private set; }
    public DelphiStringCollection Text { get; } = new DelphiStringList();
    public string Title { get; set; } = "";
    public string CancelText { get; set; } = "";
    public int SourceUnitId { get; set; }
    public LibraryImage? SourceImage { get; set; }
    public LibraryImage? ValidTargetImage { get; set; }
    public LibraryImage? InvalidTargetImage { get; set; }

    // Structur.PAS:3793
    public void Dispose()
    {
        Changed = null;
        HexNumbers = null;
        if (ImageLibrary is not null)
        {
            // { Original links the image library again when destroying the control. }
            _ = Creator.GetEngine().GamePack.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "TARGHEX.ILB");
            ImageLibrary = null;
        }
    }

    // Structur.PAS:3809
    public void TriggerOnChangeEvent(TargetSelectionChange change) => Changed?.Invoke(this, new TargetSelectionChangedEventArgs { Change = change });

    // Structur.PAS:3816
    public void DeselectHexSprite(object sender)
    {
        if (Status.Contains(TargetSelectionStatuses.Activated))
        {
            Deactivate();
        }
    }

    // Structur.PAS:3821
    public void MouseDownEvent(object sender)
    {
        GameEngine engine = Creator.GetEngine();
        if (Status.Contains(TargetSelectionStatuses.Activated))
        {
            StrategicMap strategicMap = engine.StrategicMap;
            if (strategicMap.Mouse.Capture != this || strategicMap.Mouse.Button == MouseButton.Right)
            {
                Deactivate();
                engine.StrategicMap.SelectedArmy.IgnoreMouseDown = true;
            }
            else
            {
                HexSpriteMapMouse mouse = engine.StrategicMap.Mouse;
                if (mouse.Capture == this && mouse.Button == MouseButton.Left)
                {
                    if (ValidTarget(Target.X, Target.Y, L: Target.MapLevel))
                    {
                        TriggerOnChangeEvent(TargetSelectionChange.Selected);
                        if (!Status.Contains(TargetSelectionStatuses.Activated))
                        {
                            engine.StrategicMap.SelectedArmy.IgnoreMouseDown = true;
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:3845
    public bool ValidTarget(int X, int Y, int L)
    {
        bool result;
        if (X != -1 && Y != -1)
        {
            result = !TargetMode.Contains(TargetSelectionModes.ValidateDistance) || HexGeometry.DeltaHexToRadius(SourceLocation.X, SourceLocation.Y, X, Y) <= Range;

            if (TargetMode.Contains(TargetSelectionModes.FixedLevel))
            {
                result = result && L == SourceLocation.MapLevel;
            }

            MapField mapField = Creator.GetEngine().StrategicMap.MapContainer.GetField(X, Y, L);
            result = result && mapField.Terrain != TerrainType.Border;
            if (result && _validateTarget is not null)
            {
                _validateTarget(this, (sbyte)X, (sbyte)Y, Lt: (sbyte)L, Valid: ref result);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:3862
    public void NewFrameEvent(object sender)
    {
        GameEngine engine = Creator.GetEngine();
        if (Status.Contains(TargetSelectionStatuses.Activated))
        {
            if (Status.Contains(TargetSelectionStatuses.Visible))
            {
                StrategicMap strategicMap = engine.StrategicMap;
                strategicMap.Mouse.SetCursor(110 + ((strategicMap.Frame >>> 2) % 10));
                strategicMap.Mouse.CursorVisible = true;
            }
            else
            {
                StrategicMap engineStrategicMap = engine.StrategicMap;
                engineStrategicMap.Mouse.SetCursor(120);
                engineStrategicMap.Mouse.CursorVisible = false;
            }
        }
    }

    // Structur.PAS:3877
    public void MouseMoveEvent(object sender)
    {
        if (Status.Contains(TargetSelectionStatuses.Activated))
        {
            StrategicMap strategicMap = Creator.GetEngine().StrategicMap;
            Target = new MapCoordinate((sbyte)strategicMap.Mouse.HexX, (sbyte)strategicMap.Mouse.HexY, (sbyte)strategicMap.ViewMapLevel);
            bool condition = strategicMap.Mouse.HexX != -1 && strategicMap.Mouse.HexY != -1;
            if (condition)
            {
                int hexY = strategicMap.Mouse.HexY;
                condition = ValidTarget(strategicMap.Mouse.HexX.TruncateToSByte(), hexY.TruncateToSByte(), L: strategicMap.ViewMapLevel.TruncateToSByte());
            }

            Status = condition
                ? Status | TargetSelectionStatuses.Visible
                : Status & ~TargetSelectionStatuses.Visible;
        }
    }

    // Structur.PAS:3891
    public void ShowSelection(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (Status.Contains(TargetSelectionStatuses.Activated) && Target.X != -1 && Status.Contains(TargetSelectionStatuses.Visible))
        {
            HexGeometry.HexToPixel(Target.X, Target.Y, out int x, out int y);
            x -= pixelXSurface;
            y -= pixelYSurface;
            int deltaX = default;
            int deltaY = default;
            for (int i = 0, last = HexNumbers.Count - 1; i <= last; i++)
            {
                if (HexNumbers[i] >= 0)
                {
                    HexGeometry.HexNumberToPixel(i, ref deltaX, ref deltaY);
                    int xs = x + deltaX;
                    int ys = y + deltaY;
                    if (TargetMode.Contains(TargetSelectionModes.OwnerDraw))
                    {
                        _show?.Invoke(this, HN: i, xs, ys, SurfaceDesc: lockedSurface);
                    }
                    else
                    {
                        int item = HexNumbers[i];
                        ImageLibrary.Get(item).Show(xs - 8, ys, lockedSurface);
                    }
                }
            }
        }
    }

    // Structur.PAS:3915
    public void Initialize(object source)
    {
        ImageLibrary ??= Creator.GetEngine().GamePack.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "TARGHEX.ILB");

        if (!Status.Contains(TargetSelectionStatuses.Activated))
        {
            HexNumbers.Clear();
            Source = source;
            TargetMode = TargetSelectionModes.FixedLevel;
        }
        else
        {
            throw new InvalidOperationException("please deactivate before initializing (TTargetSelectionControl)");
        }
    }

    // Structur.PAS:3928
    public void Activate()
    {
        GameEngine engine = Creator.GetEngine();
        if (!Status.Contains(TargetSelectionStatuses.Activated))
        {
            Status |= TargetSelectionStatuses.Activated;
            engine.StrategicMap.Mouse.SetCapture(this);
            StrategicMap strategicMap = engine.StrategicMap;
            strategicMap.Mouse.ButtonPressed += MouseDownEvent;
            strategicMap.Mouse.Moved += MouseMoveEvent;
            strategicMap.FrameStarted += NewFrameEvent;
            strategicMap.Selection.Deselected += DeselectHexSprite;
            strategicMap.Mouse.CursorMode = 256;
            strategicMap.Keyboard.Lock();
            TriggerOnChangeEvent(TargetSelectionChange.Activated);
            MouseMoveEvent(this);
        }
        else
        {
            throw new InvalidOperationException("Already activated");
        }
    }

    // Structur.PAS:3946
    public void Deactivate()
    {
        GameEngine engine = Creator.GetEngine();
        if (Status.Contains(TargetSelectionStatuses.Activated))
        {
            Status &= ~TargetSelectionStatuses.Activated;
            StrategicMap strategicMap = engine.StrategicMap;
            strategicMap.Mouse.ButtonPressed -= MouseDownEvent;
            strategicMap.Mouse.Moved -= MouseMoveEvent;
            strategicMap.FrameStarted -= NewFrameEvent;
            strategicMap.Selection.Deselected -= DeselectHexSprite;
            if (strategicMap.Mouse.Capture == this)
            {
                if (strategicMap.SelectedArmy.ArmyHexSprite is not null)
                {
                    ArmyHexSprite armyHexSprite = strategicMap.SelectedArmy.ArmyHexSprite;
                    strategicMap.Mouse.SetCapture(null);
                    engine.StrategicMap.Selection.UnselectLocation();
                    engine.StrategicMap.Selection.SelectLocationHexSprite(armyHexSprite);
                }
                else
                {
                    engine.StrategicMap.Mouse.SetCapture(null);
                }
            }

            engine.StrategicMap.Keyboard.Unlock();
            TriggerOnChangeEvent(TargetSelectionChange.Deactivated);
            Source = null;
            _show = null;
            _validateTarget = null;
        }
    }

    // Structur.PAS:3974
    public void UnitChanged(object unit)
    {
        // UnitChanged receives the move payload as object. Typing that payload is issue 150.
        if (Status.Contains(TargetSelectionStatuses.Activated) && Source is not null && unit == Source && ((Troop)unit).UnitStatus.Contains(UnitStatuses.Killed))
        {
            Deactivate();
        }
    }
}
