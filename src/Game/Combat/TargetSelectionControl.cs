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
        FOnChangeStore = new();
        FHNs = new SByteList();
        FText = new StringList();
    }

    public CustomImageLibrary FImageLibrary;
    public SByteList FHNs;
    public Set<TargetSelectionStatusElement> FStatus;
    public Set<TargetSelectionModeElement> FTargetMode;
    public ShowTargetSelectionEvent FOnShow;
    public ValidateTargetEvent FOnValidateTarget;
    public int FRange;
    public int FSourceX;
    public int FSourceY;
    public int FSourceL;
    public object FSource;

    internal EventStore<TargetSelectionControl> FOnChangeStore;

    public event NotifyEvent<TargetSelectionControl> FOnChange
    {
        add => FOnChangeStore.Add(value);
        remove => FOnChangeStore.Remove(value);
    }

    public TargetSelectionChange FChange;
    public sbyte FTargetX;
    public sbyte FTargetY;
    public sbyte FTargetL;
    public StringCollection FText;
    public string FTitle = "";
    public string FCancelText = "";
    public int FSourceUnitID;
    public LibraryImage FSourceImage;
    public LibraryImage FValidTargetImage;
    public LibraryImage FInvalidTargetImage;

    // Structur.PAS:3793
    public virtual void Dispose()
    {
        FText?.Dispose();
        FOnChangeStore.Clear();
        FHNs?.Dispose();
        FHNs = null;
        if (FImageLibrary != null)
        {
            // { Original links the image library again when destroying the control. }
            AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TARGHEX.ILB");
            FImageLibrary = null;
        }
    }

    // Structur.PAS:3809
    public void TriggerOnChangeEvent(TargetSelectionChange Change)
    {
        FChange = Change;
        FOnChangeStore.Raise(this);
        FChange = TargetSelectionChange.None;
    }

    // Structur.PAS:3816
    public void DeselectHS(object Sender)
    {
        if (FStatus.Contains(TargetSelectionStatusElement.Activated))
        {
            Deactivate();
        }
    }

    // Structur.PAS:3821
    public void MouseDownEvent(object Sender)
    {
        HexSpriteMapMouse Mouse;
        if (FStatus.Contains(TargetSelectionStatusElement.Activated))
        {
            if (AoWHSMap.Current.Mouse.Capture != this || AoWHSMap.Current.Mouse.Button == MouseButton.Right)
            {
                Deactivate();
                AoWHSMap.Current.FSelectedArmy.FIgnoreMouseDown = true;
            }
            else
            {
                Mouse = AoWHSMap.Current.Mouse;
                if (Mouse.Capture == this && Mouse.Button == MouseButton.Left)
                {
                    if (ValidTarget(FTargetX, FTargetY, FTargetL))
                    {
                        TriggerOnChangeEvent(TargetSelectionChange.Selected);
                        if (!FStatus.Contains(TargetSelectionStatusElement.Activated))
                        {
                            AoWHSMap.Current.FSelectedArmy.FIgnoreMouseDown = true;
                        }
                    }
                }
            }
        }
    }

    // Structur.PAS:3845
    public bool ValidTarget(sbyte X, sbyte Y, sbyte L)
    {
        bool Result = default;
        MapField MapField;
        if (X != -1 && Y != -1)
        {
            if (FTargetMode.Contains(TargetSelectionModeElement.ValidateDistance))
            {
                Result = HexGeometry.dHXtoRad(FSourceX, FSourceY, X, Y) <= FRange;
            }
            else
            {
                Result = true;
            }

            if (FTargetMode.Contains(TargetSelectionModeElement.FixedLevel))
            {
                Result = Result && L == FSourceL;
            }

            MapField = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
            Result = Result && MapField.TerrainType != 15;
            if (Result && FOnValidateTarget != null)
            {
                FOnValidateTarget(this, X, Y, L, ref Result);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:3862
    public void NewFrameEvent(object Sender)
    {
        if (FStatus.Contains(TargetSelectionStatusElement.Activated))
        {
            if (FStatus.Contains(TargetSelectionStatusElement.Visible))
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
    public void MouseMoveEvent(object Sender)
    {
        if (FStatus.Contains(TargetSelectionStatusElement.Activated))
        {
            FTargetX = (sbyte)AoWHSMap.Current.Mouse.GetXhx();
            FTargetY = (sbyte)AoWHSMap.Current.Mouse.GetYhx();
            FTargetL = (sbyte)AoWHSMap.Current.ViewL;
            bool condition = AoWHSMap.Current.Mouse.GetXhx() != -1 && AoWHSMap.Current.Mouse.GetYhx() != -1;
            if (condition)
            {
                var yhx = AoWHSMap.Current.Mouse.GetYhx();
                condition = ValidTarget((sbyte)AoWHSMap.Current.Mouse.GetXhx(), (sbyte)yhx, (sbyte)AoWHSMap.Current.ViewL);
            }

            if (condition)
            {
                FStatus = FStatus + Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Visible);
            }
            else
            {
                FStatus = FStatus - Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Visible);
            }
        }
    }

    // Structur.PAS:3891
    public void ShowSelection(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int DX = default;
        int DY = default;
        int X = default;
        int Y = default;
        int XS;
        int YS;
        if (FStatus.Contains(TargetSelectionStatusElement.Activated) && FTargetX != -1 && FStatus.Contains(TargetSelectionStatusElement.Visible))
        {
            HexGeometry.HXtoHP(FTargetX, FTargetY, ref X, ref Y);
            X -= XhpSurf;
            Y -= YhpSurf;
            for (int I = 0, last = FHNs.Count - 1; I <= last; I++)
            {
                if (FHNs[I] >= 0)
                {
                    HexGeometry.HNtoHP(I, ref DX, ref DY);
                    XS = X + DX;
                    YS = Y + DY;
                    if (FTargetMode.Contains(TargetSelectionModeElement.OwnerDraw))
                    {
                        if (FOnShow != null)
                        {
                            FOnShow(this, I, XS, YS, SurfaceDesc);
                        }
                    }
                    else
                    {
                        var item = FHNs[I];
                        FImageLibrary.Get(item).Show(XS - 8, YS, SurfaceDesc);
                    }
                }
            }
        }
    }

    // Structur.PAS:3915
    public void Initialize(object Source)
    {
        if (FImageLibrary == null)
        {
            FImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "TARGHEX.ILB");
        }

        if (!FStatus.Contains(TargetSelectionStatusElement.Activated))
        {
            FHNs.Clear();
            FSource = Source;
            FTargetMode = Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel);
        }
        else
        {
            throw new InvalidOperationException("please deactivate before initializing (TTargetSelectionControl)");
        }
    }

    // Structur.PAS:3928
    public void Activate()
    {
        if (!FStatus.Contains(TargetSelectionStatusElement.Activated))
        {
            FStatus = FStatus + Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
            AoWHSMap.Current.Mouse.SetCapture(this);
            AoWHSMap.Current.Mouse.OnDown += MouseDownEvent;
            AoWHSMap.Current.Mouse.OnMove += MouseMoveEvent;
            AoWHSMap.Current.NewFrameEvents += NewFrameEvent;
            AoWHSMap.Current.Selection.OnDeselect += DeselectHS;
            AoWHSMap.Current.Mouse.CursorMode = 256;
            AoWHSMap.Current.FKeyboard.Lock();
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
        ArmyHexSprite ArmyHS = default;
        if (FStatus.Contains(TargetSelectionStatusElement.Activated))
        {
            FStatus = FStatus - Set<TargetSelectionStatusElement>.Of(TargetSelectionStatusElement.Activated);
            AoWHSMap.Current.Mouse.OnDown -= MouseDownEvent;
            AoWHSMap.Current.Mouse.OnMove -= MouseMoveEvent;
            AoWHSMap.Current.NewFrameEvents -= NewFrameEvent;
            AoWHSMap.Current.Selection.OnDeselect -= DeselectHS;
            if (AoWHSMap.Current.Mouse.Capture == this)
            {
                if (AoWHSMap.Current.FSelectedArmy.GetArmyHS() != null)
                {
                    ArmyHS = AoWHSMap.Current.FSelectedArmy.GetArmyHS();
                    AoWHSMap.Current.Mouse.SetCapture(null);
                    AoWHSMap.Current.Selection.UnselectLocation();
                    AoWHSMap.Current.Selection.SelectLocationHS(ArmyHS);
                }
                else
                {
                    AoWHSMap.Current.Mouse.SetCapture(null);
                }
            }

            AoWHSMap.Current.FKeyboard.Unlock();
            TriggerOnChangeEvent(TargetSelectionChange.Deactivated);
            FSource = null;
            FOnShow = null;
            FOnValidateTarget = null;
        }
    }

    // Structur.PAS:3974
    public void UnitChanged(object AUnit)
    {
        if (FStatus.Contains(TargetSelectionStatusElement.Activated) && FSource != null && AUnit == FSource && ((Troop)AUnit).FUnitStatus.Contains(UnitStatusElement.Killed))
        {
            Deactivate();
        }
    }

    // intf_P1_05.inc:31

}
