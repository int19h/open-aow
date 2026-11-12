// Translated from AoWUnitSelDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// unit selector. retained event log
public partial class UnitSelectionDialog : DataModule
{
    public UnitSelectionDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Panel UnitPnl;
    public Panel SourcePnl;
    public Label SourceNameLbl;
    public CheckBox U1;
    public CheckBox U2;
    public CheckBox U3;
    public CheckBox U4;
    public CheckBox U5;
    public CheckBox U6;
    public CheckBox U7;
    public CheckBox U8;
    public Image SourceFace;
    public Label SelectionInfoLbl;
    public Label SelectionLbl;

    public UnitSelector Event { get; set; }

    private UnitSelectionActivation _eventLog;

    // AoWUnitSelDlg.pas:66
    public void DrawUnit(int index, int X, int Y, ref LockedSurface lockedSurface)
    {
        Rect saveRectangle;
        UnitShowInfo info;
        LibraryImage image;
        Units.Alignment alignment;
        if (index < Event.Units.GetCount())
        {
            saveRectangle = lockedSurface.ClipRectangle;
            var rectangle = new Rect(X + 5, Y + 4, X + 46, Y + 56);
            lockedSurface.ClipRectangle = rectangle;
            alignment = Event.Units.GetUnit(index).GetAlignment();
            if ((int)alignment < DialogHosts.UIGraphics.UnitBGs.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                image = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            Event.Units.GetUnit(index).Show(X - 8, Y - 5, lockedSurface, info);
            if ((((int)(((uint)Event.Selectable) >> index)) & 1) == 0)
            {
                image = DialogHosts.UIGraphics.UnitBGs[7];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            lockedSurface.ClipRectangle = saveRectangle;
        }
    }

    // AoWUnitSelDlg.pas:177
    public void ResetSelection()
    {
        if (Event != null)
        {
            Event.Release();
            Event = null;
        }

        if (_eventLog != null)
        {
            _eventLog.Release();
            _eventLog = null;
        }

        U1.Checked = false;
        U2.Checked = false;
        U3.Checked = false;
        U4.Checked = false;
        U5.Checked = false;
        U6.Checked = false;
        U7.Checked = false;
        U8.Checked = false;
        SelectionInfoLbl.Text = "";
    }

    // AoWUnitSelDlg.pas:186
    public void Execute(UnitSelectionActivation request)
    {
        Troop U;
        ResetSelection();
        _eventLog = request;
        _eventLog.AddRef();
        Event = request.UnitSelector;
        Event.AddRef();
        U = AoWHSMap.Current.UnitControl.FindUnit(Event.SourceUnitId);
        if (U != null)
        {
            SourcePnl.Visible = true;
            var name = U.GetName();
            SourceNameLbl.Text = name;
        }
        else
        {
            SourcePnl.Visible = false;
        }

        SelectionLbl.Text = Event.Caption;
        Dlg.Visible = true;
    }

    // AoWUnitSelDlg.pas:95
    public void ClearEvent()
    {
        if (Event != null)
        {
            Event.Release();
            Event = null;
        }

        if (_eventLog != null)
        {
            _eventLog.Release();
            _eventLog = null;
        }
    }

    // AoWUnitSelDlg.pas:101
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWUnitSelDlg.pas:107
    public void SelectBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (Event.CanSelect(ref reason))
        {
            Event.Select();
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.Current.ShowWarning(reason);
        }
    }

    // AoWUnitSelDlg.pas:120
    public void U1Change(CheckBox sender, int slot)
    {
        string reason = "";
        if (sender.Checked)
        {
            if (!Event.Canceled)
            {
                Event.SetSelection((byte)(1 << slot));
            }
            else
            {
                Event.SetSelection((byte)(Event.Selection | (1 << slot)));
            }
        }
        else
        {
            Event.SetSelection((byte)(Event.Selection & ~(1 << slot)));
        }

        U1.Checked = (Event.Selection & 1) != 0;
        U2.Checked = (Event.Selection & 2) != 0;
        U3.Checked = (Event.Selection & 4) != 0;
        U4.Checked = (Event.Selection & 8) != 0;
        U5.Checked = (Event.Selection & 16) != 0;
        U6.Checked = (Event.Selection & 32) != 0;
        U7.Checked = (Event.Selection & 64) != 0;
        U8.Checked = (Event.Selection & 128) != 0;
        if ((Event.Selectable & (1 << slot)) != 0)
        {
            if (sender.Checked)
            {
                DialogHosts.General.PlayButtonSound(1);
            }
            else
            {
                DialogHosts.General.PlayButtonSound(2);
            }
        }

        if (Event.Selection != 0)
        {
            if (Event.CanSelect(ref reason))
            {
                SelectionInfoLbl.TextColor = 0;
            }
            else
            {
                SelectionInfoLbl.TextColor = 149;
            }
        }
        else
        {
            SelectionInfoLbl.Text = "";
        }
    }

    // AoWUnitSelDlg.pas:146
    public void U1PreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        DrawUnit(slot, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref lockedSurface);
    }

    // AoWUnitSelDlg.pas:151
    public void SourceFacePreDraw(LockedSurface lockedSurface)
    {
        Rect R = default;
        Troop U;
        if (Event != null)
        {
            U = AoWHSMap.Current.UnitControl.FindUnit(Event.SourceUnitId);
            if (U != null)
            {
                R.Left = this.SourceFace.WinLeft + SourcePnl.WinLeft;
                R.Right = this.SourceFace.WinWidth + R.Left;
                R.Top = this.SourceFace.WinTop + SourcePnl.WinTop;
                R.Bottom = this.SourceFace.WinHeight + R.Top;

                if (U.GetFace() != null)
                {
                    U.GetFace().ShowStretched(R, lockedSurface);
                }
            }
        }
    }

    // AoWUnitSelDlg.pas:171
    public void DlgHide()
    {
        ClearEvent();
    }
}
