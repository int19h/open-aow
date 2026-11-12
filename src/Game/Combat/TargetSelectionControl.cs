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
public partial class TargetSelectionControl : IDisposable
{
    // Structur.PAS:3785
    // images, HNs, status, mode, callbacks, range, source coordinates, source, change events,
    // change, target, text, title, cancel text, images
    public TargetSelectionControl()
    {
        OnChangeStore = new();
        HexNumbers = new SByteList();
        Text = new StringList();
    }

    public CustomImageLibrary ImageLibrary { get; private set; }
    public SByteList HexNumbers { get; private set; }
    public Set<TargetSelectionStatusElement> Status { get; set; }
    public Set<TargetSelectionModeElement> TargetMode { get; set; }
    public ShowTargetSelectionEvent OnShow { get; set; }
    public ValidateTargetEvent OnValidateTarget { get; set; }
    public int Range { get; set; }
    public int SourceX { get; set; }
    public int SourceY { get; set; }
    public int SourceL { get; set; }
    public object Source { get; set; }

    internal EventStore<TargetSelectionControl> OnChangeStore { get; }

    public event NotifyEvent<TargetSelectionControl> FOnChange
    {
        add => OnChangeStore.Add(value);
        remove => OnChangeStore.Remove(value);
    }

    public TargetSelectionChange Change { get; private set; }
    public sbyte TargetX { get; private set; }
    public sbyte TargetY { get; private set; }
    public sbyte TargetL { get; private set; }
    public StringCollection Text { get; }
    public string Title { get; set; } = "";
    public string CancelText { get; set; } = "";
    public int SourceUnitId { get; set; }
    public LibraryImage SourceImage { get; set; }
    public LibraryImage ValidTargetImage { get; set; }
    public LibraryImage InvalidTargetImage { get; set; }

    // Structur.PAS:3793
    public virtual void Dispose()
    {
        Text?.Dispose();
        OnChangeStore.Clear();
        HexNumbers?.Dispose();
        HexNumbers = null;
        if (ImageLibrary != null)
        {
            // { Original links the image library again when destroying the control. }
            GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TARGHEX.ILB");
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
        HexSpriteMapMouse mouse;
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            if (StrategicMap.Current.Mouse.Capture != this || StrategicMap.Current.Mouse.Button == MouseButton.Right)
            {
                Deactivate();
                StrategicMap.Current.SelectedArmy.IgnoreMouseDown = true;
            }
            else
            {
                mouse = StrategicMap.Current.Mouse;
                if (mouse.Capture == this && mouse.Button == MouseButton.Left)
                {
                    if (ValidTarget(TargetX, TargetY, TargetL))
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
        bool result = default;
        MapField mapField;
        if (X != -1 && Y != -1)
        {
            if (TargetMode.Contains(TargetSelectionModeElement.ValidateDistance))
            {
                result = HexGeometry.DeltaHexToRadius(SourceX, SourceY, X, Y) <= Range;
            }
            else
            {
                result = true;
            }

            if (TargetMode.Contains(TargetSelectionModeElement.FixedLevel))
            {
                result = result && L == SourceL;
            }

            mapField = StrategicMap.Current.MapContainer.GetField(X, Y, L);
            result = result && mapField.TerrainType != 15;
            if (result && OnValidateTarget != null)
            {
                OnValidateTarget(this, X, Y, L, ref result);
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
                StrategicMap.Current.Mouse.SetCursor(110 + (int)(((uint)StrategicMap.Current.Frame) >> 2) % 10);
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
            TargetX = (sbyte)StrategicMap.Current.Mouse.GetHexX();
            TargetY = (sbyte)StrategicMap.Current.Mouse.GetHexY();
            TargetL = (sbyte)StrategicMap.Current.ViewL;
            bool condition = StrategicMap.Current.Mouse.GetHexX() != -1 && StrategicMap.Current.Mouse.GetHexY() != -1;
            if (condition)
            {
                var hexY = StrategicMap.Current.Mouse.GetHexY();
                condition = ValidTarget((sbyte)StrategicMap.Current.Mouse.GetHexX(), (sbyte)hexY, (sbyte)StrategicMap.Current.ViewL);
            }

            if (condition)
            {
                Status = Status + Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Visible);
            }
            else
            {
                Status = Status - Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Visible);
            }
        }
    }

    // Structur.PAS:3891
    public void ShowSelection(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int deltaX = default;
        int deltaY = default;
        int X = default;
        int Y = default;
        int xs;
        int ys;
        if (Status.Contains(TargetSelectionStatusElement.Activated) && TargetX != -1 && Status.Contains(TargetSelectionStatusElement.Visible))
        {
            HexGeometry.HexToPixel(TargetX, TargetY, ref X, ref Y);
            X -= pixelXSurface;
            Y -= pixelYSurface;
            for (int I = 0, last = HexNumbers.Count - 1; I <= last; I++)
            {
                if (HexNumbers[I] >= 0)
                {
                    HexGeometry.HexNumberToPixel(I, ref deltaX, ref deltaY);
                    xs = X + deltaX;
                    ys = Y + deltaY;
                    if (TargetMode.Contains(TargetSelectionModeElement.OwnerDraw))
                    {
                        if (OnShow != null)
                        {
                            OnShow(this, I, xs, ys, lockedSurface);
                        }
                    }
                    else
                    {
                        var item = HexNumbers[I];
                        ImageLibrary.Get(item).Show(xs - 8, ys, lockedSurface);
                    }
                }
            }
        }
    }

    // Structur.PAS:3915
    public void Initialize(object source)
    {
        if (ImageLibrary == null)
        {
            ImageLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TARGHEX.ILB");
        }

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
            Status = Status + Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
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
        ArmyHexSprite armyHexSprite = default;
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            Status = Status - Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
            StrategicMap.Current.Mouse.OnDown -= MouseDownEvent;
            StrategicMap.Current.Mouse.OnMove -= MouseMoveEvent;
            StrategicMap.Current.NewFrameEvents -= NewFrameEvent;
            StrategicMap.Current.Selection.OnDeselect -= DeselectHexSprite;
            if (StrategicMap.Current.Mouse.Capture == this)
            {
                if (StrategicMap.Current.SelectedArmy.GetArmyHexSprite() != null)
                {
                    armyHexSprite = StrategicMap.Current.SelectedArmy.GetArmyHexSprite();
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
        if (Status.Contains(TargetSelectionStatusElement.Activated) && Source != null && unit == Source && ((Troop)unit).UnitStatus.Contains(UnitStatusElement.Killed))
        {
            Deactivate();
        }
    }

    // intf_P1_05.inc:31

}
