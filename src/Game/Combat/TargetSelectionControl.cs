#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P1. }
public sealed class TargetSelectionControl : IDisposable
{
    // Structur.PAS:3785
    // images, HNs, status, mode, callbacks, range, source coordinates, source, change events,
    // change, target, text, title, cancel text, images
    public TargetSelectionControl()
    {
        OnChangeStore = new();
        HexNumbers = new SByteList();
        Text = new DelphiStringList();
    }

    public CustomImageLibrary ImageLibrary { get; private set; }
    public SByteList HexNumbers { get; private set; }
    public Set<TargetSelectionStatusElement> Status { get; set; }
    public Set<TargetSelectionModeElement> TargetMode { get; set; }

    public event ShowTargetSelectionEvent OnShow;

    internal void SetShowHandler(ShowTargetSelectionEvent handler) => OnShow = handler;

    public event ValidateTargetEvent OnValidateTarget;

    internal void SetValidateTargetHandler(ValidateTargetEvent handler) => OnValidateTarget = handler;

    public int Range { get; set; }
    public MapCoordinate SourceLocation { get; set; }
    public object Source { get; set; }

    internal EventStore<TargetSelectionControl> OnChangeStore { get; }

    public event NotifyEvent<TargetSelectionControl> Changed
    {
        add => OnChangeStore.Add(value);
        remove => OnChangeStore.Remove(value);
    }

    public TargetSelectionChange Change { get; private set; }
    public MapCoordinate Target { get; private set; }
    public DelphiStringCollection Text { get; }
    public string Title { get; set; } = "";
    public string CancelText { get; set; } = "";
    public int SourceUnitId { get; set; }
    public LibraryImage SourceImage { get; set; }
    public LibraryImage ValidTargetImage { get; set; }
    public LibraryImage InvalidTargetImage { get; set; }

    // Structur.PAS:3793
    public void Dispose()
    {
        Text?.Dispose();
        OnChangeStore.Clear();
        HexNumbers?.Dispose();
        HexNumbers = null;
        if (ImageLibrary is not null)
        {
            // { Original links the image library again when destroying the control. }
            _ = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), fileName: "TARGHEX.ILB");
            ImageLibrary = null;
        }
    }

    // Structur.PAS:3809
    public void TriggerOnChangeEvent(TargetSelectionChange change)
    {
        Change = change;
        OnChangeStore.Raise(this);
        Change = TargetSelectionChange.None;
    }

    // Structur.PAS:3816
    public void DeselectHexSprite(object sender)
    {
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            Deactivate();
        }
    }

    // Structur.PAS:3821
    public void MouseDownEvent(object sender)
    {
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            if (StrategicMap.Current.Mouse.Capture != this || StrategicMap.Current.Mouse.Button == MouseButton.Right)
            {
                Deactivate();
                StrategicMap.Current.SelectedArmy.IgnoreMouseDown = true;
            }
            else
            {
                HexSpriteMapMouse mouse = StrategicMap.Current.Mouse;
                if (mouse.Capture == this && mouse.Button == MouseButton.Left)
                {
                    if (ValidTarget(Target.X, Target.Y, L: Target.MapLevel))
                    {
                        TriggerOnChangeEvent(TargetSelectionChange.Selected);
                        if (!Status.Contains(TargetSelectionStatusElement.Activated))
                        {
                            StrategicMap.Current.SelectedArmy.IgnoreMouseDown = true;
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:3845
    public bool ValidTarget(sbyte X, sbyte Y, sbyte L)
    {
        bool result;
        if (X != -1 && Y != -1)
        {
            result = !TargetMode.Contains(TargetSelectionModeElement.ValidateDistance) || HexGeometry.DeltaHexToRadius(SourceLocation.X, SourceLocation.Y, X, Y) <= Range;

            if (TargetMode.Contains(TargetSelectionModeElement.FixedLevel))
            {
                result = result && L == SourceLocation.MapLevel;
            }

            MapField mapField = StrategicMap.Current.MapContainer.GetField(X, Y, L);
            result = result && mapField.TerrainType != 15;
            if (result && OnValidateTarget is not null)
            {
                OnValidateTarget(this, X, Y, Lt: L, Valid: ref result);
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
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            if (Status.Contains(TargetSelectionStatusElement.Visible))
            {
                StrategicMap.Current.Mouse.SetCursor(110 + ((StrategicMap.Current.Frame >>> 2) % 10));
                StrategicMap.Current.Mouse.CursorVisible = true;
            }
            else
            {
                StrategicMap.Current.Mouse.SetCursor(120);
                StrategicMap.Current.Mouse.CursorVisible = false;
            }
        }
    }

    // Structur.PAS:3877
    public void MouseMoveEvent(object sender)
    {
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            Target = new MapCoordinate((sbyte)StrategicMap.Current.Mouse.HexX, (sbyte)StrategicMap.Current.Mouse.HexY, (sbyte)StrategicMap.Current.ViewMapLevel);
            bool condition = StrategicMap.Current.Mouse.HexX != -1 && StrategicMap.Current.Mouse.HexY != -1;
            if (condition)
            {
                int hexY = StrategicMap.Current.Mouse.HexY;
                condition = ValidTarget((sbyte)StrategicMap.Current.Mouse.HexX, (sbyte)hexY, L: (sbyte)StrategicMap.Current.ViewMapLevel);
            }

            Status = condition
                ? Status + Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Visible)
                : Status - Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Visible);
        }
    }

    // Structur.PAS:3891
    public void ShowSelection(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (Status.Contains(TargetSelectionStatusElement.Activated) && Target.X != -1 && Status.Contains(TargetSelectionStatusElement.Visible))
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
                    if (TargetMode.Contains(TargetSelectionModeElement.OwnerDraw))
                    {
                        OnShow?.Invoke(this, HN: i, xs, ys, SurfaceDesc: lockedSurface);
                    }
                    else
                    {
                        sbyte item = HexNumbers[i];
                        ImageLibrary.Get(item).Show(xs - 8, ys, lockedSurface);
                    }
                }
            }
        }
    }

    // Structur.PAS:3915
    public void Initialize(object source)
    {
        ImageLibrary ??= GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), fileName: "TARGHEX.ILB");

        if (!Status.Contains(TargetSelectionStatusElement.Activated))
        {
            HexNumbers.Clear();
            Source = source;
            TargetMode = Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel);
        }
        else
        {
            throw new InvalidOperationException("please deactivate before initializing (TTargetSelectionControl)");
        }
    }

    // Structur.PAS:3928
    public void Activate()
    {
        if (!Status.Contains(TargetSelectionStatusElement.Activated))
        {
            Status += Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
            StrategicMap.Current.Mouse.SetCapture(this);
            StrategicMap.Current.Mouse.OnDown += MouseDownEvent;
            StrategicMap.Current.Mouse.OnMove += MouseMoveEvent;
            StrategicMap.Current.NewFrameEvents += NewFrameEvent;
            StrategicMap.Current.Selection.OnDeselect += DeselectHexSprite;
            StrategicMap.Current.Mouse.CursorMode = 256;
            StrategicMap.Current.Keyboard.Lock();
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
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            Status -= Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
            StrategicMap.Current.Mouse.OnDown -= MouseDownEvent;
            StrategicMap.Current.Mouse.OnMove -= MouseMoveEvent;
            StrategicMap.Current.NewFrameEvents -= NewFrameEvent;
            StrategicMap.Current.Selection.OnDeselect -= DeselectHexSprite;
            if (StrategicMap.Current.Mouse.Capture == this)
            {
                if (StrategicMap.Current.SelectedArmy.ArmyHexSprite is not null)
                {
                    ArmyHexSprite armyHexSprite = StrategicMap.Current.SelectedArmy.ArmyHexSprite;
                    StrategicMap.Current.Mouse.SetCapture(null);
                    StrategicMap.Current.Selection.UnselectLocation();
                    StrategicMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                }
                else
                {
                    StrategicMap.Current.Mouse.SetCapture(null);
                }
            }

            StrategicMap.Current.Keyboard.Unlock();
            TriggerOnChangeEvent(TargetSelectionChange.Deactivated);
            Source = null;
            OnShow = null;
            OnValidateTarget = null;
        }
    }

    // Structur.PAS:3974
    public void UnitChanged(object unit)
    {
        // UnitChanged receives the move payload as object. Typing that payload is issue 150.
        if (Status.Contains(TargetSelectionStatusElement.Activated) && Source is not null && unit == Source && ((Troop)unit).UnitStatus.Contains(UnitStatusElement.Killed))
        {
            Deactivate();
        }
    }
}
