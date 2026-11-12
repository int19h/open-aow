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
        _onChangeStore = new();
        HexNumbers = new SByteList();
        Text = new StringList();
    }

    public CustomImageLibrary ImageLibrary;
    public SByteList HexNumbers;
    public Set<TargetSelectionStatusElement> Status;
    public Set<TargetSelectionModeElement> TargetMode;
    public ShowTargetSelectionEvent OnShow;
    public ValidateTargetEvent OnValidateTarget;
    public int Range;
    public int SourceX;
    public int SourceY;
    public int SourceL;
    public object Source;

    internal EventStore<TargetSelectionControl> _onChangeStore;

    public event NotifyEvent<TargetSelectionControl> FOnChange
    {
        add => _onChangeStore.Add(value);
        remove => _onChangeStore.Remove(value);
    }

    public TargetSelectionChange Change;
    public sbyte TargetX;
    public sbyte TargetY;
    public sbyte TargetL;
    public StringCollection Text;
    public string Title = "";
    public string CancelText = "";
    public int SourceUnitId;
    public LibraryImage SourceImage;
    public LibraryImage ValidTargetImage;
    public LibraryImage InvalidTargetImage;

    // Structur.PAS:3793
    public virtual void Dispose()
    {
        Text?.Dispose();
        _onChangeStore.Clear();
        HexNumbers?.Dispose();
        HexNumbers = null;
        if (ImageLibrary != null)
        {
            // { Original links the image library again when destroying the control. }
            AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TARGHEX.ILB");
            ImageLibrary = null;
        }
    }

    // Structur.PAS:3809
    public void TriggerOnChangeEvent(TargetSelectionChange change)
    {
        Change = change;
        _onChangeStore.Raise(this);
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
            if (AoWHSMap.Current.Mouse.Capture != this || AoWHSMap.Current.Mouse.Button == MouseButton.Right)
            {
                Deactivate();
                AoWHSMap.Current.SelectedArmy.IgnoreMouseDown = true;
            }
            else
            {
                mouse = AoWHSMap.Current.Mouse;
                if (mouse.Capture == this && mouse.Button == MouseButton.Left)
                {
                    if (ValidTarget(TargetX, TargetY, TargetL))
                    {
                        TriggerOnChangeEvent(TargetSelectionChange.Selected);
                        if (!Status.Contains(TargetSelectionStatusElement.Activated))
                        {
                            AoWHSMap.Current.SelectedArmy.IgnoreMouseDown = true;
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

            mapField = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
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
                AoWHSMap.Current.Mouse.SetCursor(110 + (int)(((uint)AoWHSMap.Current.Frame) >> 2) % 10);
                AoWHSMap.Current.Mouse.CursorVisible = true;
            }
            else
            {
                AoWHSMap.Current.Mouse.SetCursor(120);
                AoWHSMap.Current.Mouse.CursorVisible = false;
            }
        }
    }

    // Structur.PAS:3877
    public void MouseMoveEvent(object sender)
    {
        if (Status.Contains(TargetSelectionStatusElement.Activated))
        {
            TargetX = (sbyte)AoWHSMap.Current.Mouse.GetHexX();
            TargetY = (sbyte)AoWHSMap.Current.Mouse.GetHexY();
            TargetL = (sbyte)AoWHSMap.Current.ViewL;
            bool condition = AoWHSMap.Current.Mouse.GetHexX() != -1 && AoWHSMap.Current.Mouse.GetHexY() != -1;
            if (condition)
            {
                var hexY = AoWHSMap.Current.Mouse.GetHexY();
                condition = ValidTarget((sbyte)AoWHSMap.Current.Mouse.GetHexX(), (sbyte)hexY, (sbyte)AoWHSMap.Current.ViewL);
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
            ImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TARGHEX.ILB");
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
            AoWHSMap.Current.Mouse.SetCapture(this);
            AoWHSMap.Current.Mouse.OnDown += MouseDownEvent;
            AoWHSMap.Current.Mouse.OnMove += MouseMoveEvent;
            AoWHSMap.Current.NewFrameEvents += NewFrameEvent;
            AoWHSMap.Current.Selection.OnDeselect += DeselectHexSprite;
            AoWHSMap.Current.Mouse.CursorMode = 256;
            AoWHSMap.Current.Keyboard.Lock();
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
            AoWHSMap.Current.Mouse.OnDown -= MouseDownEvent;
            AoWHSMap.Current.Mouse.OnMove -= MouseMoveEvent;
            AoWHSMap.Current.NewFrameEvents -= NewFrameEvent;
            AoWHSMap.Current.Selection.OnDeselect -= DeselectHexSprite;
            if (AoWHSMap.Current.Mouse.Capture == this)
            {
                if (AoWHSMap.Current.SelectedArmy.GetArmyHexSprite() != null)
                {
                    armyHexSprite = AoWHSMap.Current.SelectedArmy.GetArmyHexSprite();
                    AoWHSMap.Current.Mouse.SetCapture(null);
                    AoWHSMap.Current.Selection.UnselectLocation();
                    AoWHSMap.Current.Selection.SelectLocationHexSprite(armyHexSprite);
                }
                else
                {
                    AoWHSMap.Current.Mouse.SetCapture(null);
                }
            }

            AoWHSMap.Current.Keyboard.Unlock();
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
