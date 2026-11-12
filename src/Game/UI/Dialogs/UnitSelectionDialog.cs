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

    public Window Window;
    public Panel UnitPanel;
    public Panel SourcePanel;
    public Label SourceNameLabel;
    public CheckBox Unit1CheckBox;
    public CheckBox Unit2CheckBox;
    public CheckBox Unit3CheckBox;
    public CheckBox Unit4CheckBox;
    public CheckBox Unit5CheckBox;
    public CheckBox Unit6CheckBox;
    public CheckBox Unit7CheckBox;
    public CheckBox Unit8CheckBox;
    public Image SourceFace;
    public Label SelectionInfoLabel;
    public Label SelectionLabel;

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
            if ((int)alignment < DialogHosts.UIGraphics.UnitBackgrounds.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                image = DialogHosts.UIGraphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X + 25 - image.Width / 2, Y + 30 - image.Height / 2, lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            Event.Units.GetUnit(index).Show(X - 8, Y - 5, lockedSurface, info);
            if ((((int)(((uint)Event.Selectable) >> index)) & 1) == 0)
            {
                image = DialogHosts.UIGraphics.UnitBackgrounds[7];
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

        Unit1CheckBox.Checked = false;
        Unit2CheckBox.Checked = false;
        Unit3CheckBox.Checked = false;
        Unit4CheckBox.Checked = false;
        Unit5CheckBox.Checked = false;
        Unit6CheckBox.Checked = false;
        Unit7CheckBox.Checked = false;
        Unit8CheckBox.Checked = false;
        SelectionInfoLabel.Text = "";
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
            SourcePanel.Visible = true;
            var name = U.GetName();
            SourceNameLabel.Text = name;
        }
        else
        {
            SourcePanel.Visible = false;
        }

        SelectionLabel.Text = Event.Caption;
        Window.Visible = true;
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
        Window.Visible = false;
    }

    // AoWUnitSelDlg.pas:107
    public void SelectBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (Event.CanSelect(ref reason))
        {
            Event.Select();
            Window.Visible = false;
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

        Unit1CheckBox.Checked = (Event.Selection & 1) != 0;
        Unit2CheckBox.Checked = (Event.Selection & 2) != 0;
        Unit3CheckBox.Checked = (Event.Selection & 4) != 0;
        Unit4CheckBox.Checked = (Event.Selection & 8) != 0;
        Unit5CheckBox.Checked = (Event.Selection & 16) != 0;
        Unit6CheckBox.Checked = (Event.Selection & 32) != 0;
        Unit7CheckBox.Checked = (Event.Selection & 64) != 0;
        Unit8CheckBox.Checked = (Event.Selection & 128) != 0;
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
                SelectionInfoLabel.TextColor = 0;
            }
            else
            {
                SelectionInfoLabel.TextColor = 149;
            }
        }
        else
        {
            SelectionInfoLabel.Text = "";
        }
    }

    // AoWUnitSelDlg.pas:146
    public void U1PreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        DrawUnit(slot, UnitPanel.WinLeft + sender.WinLeft, UnitPanel.WinTop + sender.WinTop, ref lockedSurface);
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
                R.Left = this.SourceFace.WinLeft + SourcePanel.WinLeft;
                R.Right = this.SourceFace.WinWidth + R.Left;
                R.Top = this.SourceFace.WinTop + SourcePanel.WinTop;
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
